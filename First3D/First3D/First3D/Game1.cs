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

namespace First3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model ship;
        Matrix view, world, proj;
        Vector3 position = new Vector3();
        float rotationX;
        float rotationY;
        int projType = 1;

        Texture2D space;

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
            world = Matrix.CreateTranslation(Vector3.Zero);
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
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


            ship = Content.Load<Model>("p1_wedge");
            space = Content.Load<Texture2D>("space");

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

            // this code is normally found in Update(...)   


        KeyboardState ks=Keyboard.GetState();

        projType = 1;
        if(ks.IsKeyDown(Keys.Space))
            projType=2;
        if (ks.IsKeyDown(Keys.Left)){// check if left key is pressed
           position.X += 100;
        
       }
       if (ks.IsKeyDown(Keys.Right))
       {// check if left key is pressed
           position.X -= 100;

       } 
        
       if (ks.IsKeyDown(Keys.Up))
       {// check if left key is pressed
           position.Z -= 100;

       }
       if (ks.IsKeyDown(Keys.Down))
       {// check if left key is pressed
           position.Z += 100;

       }


       if (ks.IsKeyDown(Keys.Z))
       {// check if left key is pressed
           rotationX += 0.1f;

       }
       if (ks.IsKeyDown(Keys.X))
       {// check if left key is pressed
           rotationX -= 0.1f;

       } if (ks.IsKeyDown(Keys.V))
       {// check if left key is pressed
           rotationY += 0.1f;

       }
       if (ks.IsKeyDown(Keys.C))
       {// check if left key is pressed
           rotationY -= 0.1f;

       }




            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(space, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();


            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            // TODO: Add your drawing code here
            view = Matrix.CreateLookAt(new Vector3(0, 10, 5000), Vector3.Zero, Vector3.Up);
            float aspect = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            if (projType == 1)
                proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f,
                   aspect,
                    0.1f,
                    100000);
            else
                proj = Matrix.CreateOrthographic(4000*aspect, 4000, 1, 100000);
            world =Matrix.CreateRotationY(rotationY)*Matrix.CreateRotationX(rotationX) * Matrix.CreateTranslation(position)  ;


            foreach (ModelMesh mesh in ship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //Effect Settings Goes here
                    effect.LightingEnabled = false;
                    effect.World = world;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }

          


            base.Draw(gameTime);
        }
    }
}
