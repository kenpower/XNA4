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

namespace SolarSystem
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Body sun, earth, moon, mars, marsMoon, marsMoon2;

        Single simulationSpeed;// times faster than real-time


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1000;
            this.graphics.PreferredBackBufferHeight = 750;
            //this.graphics.IsFullScreen = true;
            IsMouseVisible = true;
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
            sun = new Body();
            sun.Init(0, 0, 300);

            earth = new Body();
            earth.Init(200, 365, 75);

            moon = new Body();
            moon.Init(75, 28, 30);

            mars = new Body();
            mars.Init(350, 450, 60);
            
            marsMoon = new Body();
            marsMoon.Init(50, 15, 15);

            marsMoon2 = new Body();
            marsMoon2.Init(80, 30, 20);

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

            sun.Image = Content.Load<Texture2D>("sun");

            earth.Image = Content.Load<Texture2D>("earth");
            moon.Image = Content.Load<Texture2D>("moon");
            mars.Image = Content.Load<Texture2D>("mars");

            marsMoon.Image = Content.Load<Texture2D>("moon");
            marsMoon2.Image = Content.Load<Texture2D>("moon");


            

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

            Single maxSimulationSpeed = 10000;
            Single minSimulationSpeed = 100;
            int maxX = this.graphics.GraphicsDevice.Viewport.Width;

            int x=Mouse.GetState().X;


            simulationSpeed = minSimulationSpeed+ maxSimulationSpeed * Math.Abs((float)x / (float)maxX);


            Matrix screenCentre = Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2,0);

            sun.Update(gameTime, screenCentre, simulationSpeed);

            earth.Update(gameTime, sun.Transform, simulationSpeed);

            moon.Update(gameTime, earth.Transform, simulationSpeed);

            mars.Update(gameTime, sun.Transform, simulationSpeed);
            
            marsMoon.Update(gameTime, mars.Transform, simulationSpeed);
            marsMoon2.Update(gameTime, mars.Transform, simulationSpeed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here


            sun.Draw(spriteBatch, gameTime);
            earth.Draw(spriteBatch, gameTime);
            moon.Draw(spriteBatch, gameTime);
            
            mars.Draw(spriteBatch, gameTime);
            marsMoon.Draw(spriteBatch, gameTime);
            marsMoon2.Draw(spriteBatch, gameTime);
         




            base.Draw(gameTime);
        }

 
    }
}
