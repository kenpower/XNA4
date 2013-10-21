using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankDemo2D_tranformations
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tank, turret,terrain;

        Vector3 terrain_origin;
   
        Vector2 tank_pos=new Vector2();
        Vector2 tank_origin = new Vector2();
        int tank_radius;
        
        Vector2 turret_origin = new Vector2();
        Vector2 screen_centre;

        Vector2 camera_pos = new Vector2();
        float camera_zoom = 1.0f;
        
        
        float speed = 3;

        float turn_speed = 0.03f;
        float tank_rotation = 0.0f;
        
        float turret_rotation = 0.0f;

        float camera_speed = 1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tank = Content.Load<Texture2D>("tank_body");
            turret = Content.Load<Texture2D>("turret");
            terrain = Content.Load<Texture2D>("terrain");


            tank_origin = new Vector2(-tank.Bounds.Center.X, -tank.Bounds.Center.Y);
            turret_origin = new Vector2(-18, -turret.Bounds.Center.Y);
            screen_centre = new Vector2(GraphicsDevice.Viewport.Bounds.Center.X,GraphicsDevice.Viewport.Bounds.Center.Y);
            terrain_origin = new Vector3(-terrain.Bounds.Center.X, -terrain.Bounds.Center.Y,0);

            tank_radius = tank.Bounds.Width / 2;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState kbs=Keyboard.GetState();

            Vector2 tank_dir = new Vector2(1, 0);
            Matrix rot = Matrix.CreateRotationZ(tank_rotation);
            tank_dir=Vector2.Transform(tank_dir, rot);
            //Vector2 old_pos = tank_pos;

            if (kbs.IsKeyDown(Keys.Up))
            {
                tank_pos = tank_pos + tank_dir * speed;
            }

            if (kbs.IsKeyDown(Keys.Down))
            {
                tank_pos = tank_pos - tank_dir * speed;
            }
            Rectangle game_bounds=terrain.Bounds;
            game_bounds.Offset((int)terrain_origin.X,(int)terrain_origin.Y);
            game_bounds.Inflate(-tank_radius, -tank_radius);
            //Point pos=new Point((int)tank_pos.X,(int)tank_pos.Y);
            
            
            //if (!game_bounds.Contains(pos))
            //{
            //    tank_pos = old_pos;
            //}

            tank_pos.X = MathHelper.Clamp(tank_pos.X, game_bounds.Left, game_bounds.Right);
            tank_pos.Y = MathHelper.Clamp(tank_pos.Y, game_bounds.Top, game_bounds.Bottom);

            //camera_pos = new Vector2(GraphicsDevice.Viewport.Bounds.Center.X, GraphicsDevice.Viewport.Bounds.Center.Y) - tank_pos;

            //if(camera_pos.X<
            if (kbs.IsKeyDown(Keys.Left))
            {
                tank_rotation -= turn_speed*2;
            }

            if (kbs.IsKeyDown(Keys.Right))
            {
                tank_rotation += turn_speed * 2;
            }

 
            if (kbs.IsKeyDown(Keys.X))
            {
                turret_rotation += turn_speed;
            }

            if (kbs.IsKeyDown(Keys.Z))
            {
                turret_rotation -= turn_speed;
            }

            if (kbs.IsKeyDown(Keys.W))
            {
                camera_pos.Y += camera_speed;
            }

            if (kbs.IsKeyDown(Keys.S))
            {
                camera_pos.Y -= camera_speed;

            }


            if (kbs.IsKeyDown(Keys.D))
            {
                camera_pos.X += camera_speed;
            }

            if (kbs.IsKeyDown(Keys.A))
            {
                camera_pos.X -= camera_speed;
            }

            if (kbs.IsKeyDown(Keys.Q))
            {
                camera_zoom*=1.02f;
            }

            if (kbs.IsKeyDown(Keys.E))
            {
                camera_zoom *= 0.98f;
            }

            //camera_pos = -tank_pos * camera_zoom + screen_centre;
            camera_pos = -tank_pos;

            Vector2 camera_bounds = new Vector2();
            camera_bounds.X = terrain.Bounds.Center.X - (GraphicsDevice.Viewport.Width/2) / camera_zoom;
            camera_bounds.Y = terrain.Bounds.Center.Y - (GraphicsDevice.Viewport.Height/2) / camera_zoom;

           
            camera_pos.X = MathHelper.Clamp(camera_pos.X, -camera_bounds.X, +camera_bounds.X);
            camera_pos.Y = MathHelper.Clamp(camera_pos.Y, -camera_bounds.Y, +camera_bounds.Y);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix cameraMatrix = Matrix.CreateTranslation(camera_pos.X, camera_pos.Y, 0) * Matrix.CreateScale(camera_zoom) * Matrix.CreateTranslation(screen_centre.X, screen_centre.Y, 0);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(terrain_origin) * cameraMatrix);
            //spriteBatch.Draw(tank, Vector2.Zero, Color.White);
            spriteBatch.Draw(terrain, Vector2.Zero, Color.White);
            spriteBatch.End();


            Matrix tankMatrix = Matrix.CreateScale(1) * Matrix.CreateRotationZ(tank_rotation) * Matrix.CreateTranslation(tank_pos.X, tank_pos.Y, 0) * cameraMatrix;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(tank_origin.X, tank_origin.Y, 0) * tankMatrix);
            //spriteBatch.Draw(tank, Vector2.Zero, Color.White);
            spriteBatch.Draw(tank, Vector2.Zero, Color.White);
            spriteBatch.End();


            Matrix turretMatrix = Matrix.CreateTranslation(turret_origin.X, turret_origin.Y, 0) * Matrix.CreateRotationZ(turret_rotation) * tankMatrix;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, turretMatrix);
            spriteBatch.Draw(turret, Vector2.Zero, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);

        }
 
    }
}
