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

        Texture2D terrain;
        Texture2D bullet;

        Vector2 screen_centre;

        Vector3 camera_pos = new Vector3();
        float camera_zoom = 1.0f;
        KeyboardState oldkbs;

        Vector3 terrain_origin;
       

        float camera_speed = 1;

        Rectangle game_bounds;
        Tank tank = new Tank();
        Turret turret = new Turret();



        static int num_enemies=10;
        Enemy[] enemies = new Enemy[num_enemies];

        List<Bullet> bulletList = new List<Bullet>();
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
            
            Random r = new Random();
            foreach (Enemy e in enemies)
            {
                e.SpriteBatch = spriteBatch;
                e.Init(r, game_bounds);

            }
            
            oldkbs = Keyboard.GetState();
           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            terrain = Content.Load<Texture2D>("terrain");


            screen_centre = new Vector2(GraphicsDevice.Viewport.Bounds.Center.X,GraphicsDevice.Viewport.Bounds.Center.Y);
            terrain_origin = new Vector3(-terrain.Bounds.Center.X, -terrain.Bounds.Center.Y,0);

            tank.Image = Content.Load<Texture2D>("tank_body");
            tank.SpriteBatch = spriteBatch;
            turret.Image = Content.Load<Texture2D>("turret");
            turret.SpriteBatch = spriteBatch;

            bullet= Content.Load<Texture2D>("bullet");
           

            tank.turret = turret;
            turret.tank = tank;

            game_bounds = terrain.Bounds;
            game_bounds.Offset((int)terrain_origin.X,(int)terrain_origin.Y);

            Texture2D enemyB = Content.Load<Texture2D>("enemy_body");
            for (int i = 0; i < num_enemies; i++)
            {
                enemies[i] = new Enemy();
                enemies[i].SpriteBatch = spriteBatch;
                enemies[i].Image = enemyB;

            }

            //Bullet.sprite = Content.Load<Texture2D>("bullet");

            //Bullet.spriteOrigin = new Vector2(-Bullet.sprite.Bounds.Center.X, -Bullet.sprite.Bounds.Center.Y);
            
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





            tank.Update(gameTime, game_bounds);

            turret.Update(gameTime, game_bounds);

            foreach (Enemy e in enemies)
            {
                e.Update(gameTime, game_bounds,tank.position);

            }

            foreach (Bullet b in bulletList)
            {
                b.Update(gameTime, game_bounds);
            
            }

            if (kbs.IsKeyDown(Keys.Space))
            {
                camera_pos.Y += camera_speed;
            }

            //camera_pos = new Vector2(GraphicsDevice.Viewport.Bounds.Center.X, GraphicsDevice.Viewport.Bounds.Center.Y) - tank_pos;

            //if(camera_pos.X<

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

            if (kbs.IsKeyDown(Keys.Space))
            {
                if(oldkbs.IsKeyUp(Keys.Space)){
                    Bullet b= new Bullet();
                    b.SpriteBatch = spriteBatch;
                    b.Image = bullet;
                   
                    b.position = tank.position;
                    b.rotation = tank.rotation+turret.rotation;
                    bulletList.Add(b);
                }

               
            }

            //camera_pos = -tank_pos * camera_zoom + screen_centre;
            camera_pos = -tank.position;

            Vector2 camera_bounds = new Vector2();
            //centre of camera (camera position should stay within these +/- bounds)
            camera_bounds.X = terrain.Bounds.Width/2 - (GraphicsDevice.Viewport.Width/2) / camera_zoom;
            camera_bounds.Y = terrain.Bounds.Height/2 - (GraphicsDevice.Viewport.Height/2) / camera_zoom;

           
            camera_pos.X = MathHelper.Clamp(camera_pos.X, -camera_bounds.X, +camera_bounds.X);
            camera_pos.Y = MathHelper.Clamp(camera_pos.Y, -camera_bounds.Y, +camera_bounds.Y);


            oldkbs = kbs;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix cameraMatrix = 
                Matrix.CreateTranslation(camera_pos.X, camera_pos.Y, 0) * 
                Matrix.CreateScale(camera_zoom) * 
                Matrix.CreateTranslation(screen_centre.X, screen_centre.Y, 0);





            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, 
                Matrix.CreateTranslation(terrain_origin) * cameraMatrix);
            
            //spriteBatch.Draw(tank, Vector2.Zero, Color.White);
            spriteBatch.Draw(terrain, Vector2.Zero, Color.White);
            spriteBatch.End();


            tank.Draw(cameraMatrix);

            
            turret.Draw(cameraMatrix);

            foreach (Enemy e in enemies)
            {
                e.Draw(cameraMatrix);

            } 
            
            foreach (Bullet b in bulletList)
            {
                b.Draw(cameraMatrix);

            }
            

            base.Draw(gameTime);

        }

      
 
    }
}
