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

        Texture2D sun, earth, moon, mars;
        Vector2 sunOrigin, moonOrigin, earthOrigin, marsOrigin;
        Single earthOrbit, moonOrbit, marsOrbit;
        Single earthYear, moonYear, marsYear;
        Single sunSize, earthSize, moonSize, marsSize;
        Single sunScale, earthScale, moonScale, marsScale;
        Single simulationSpeed = 10;//milliseconds per day


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 1280;
            this.graphics.IsFullScreen = true;
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
            earthOrbit=200;  //radius of the orbit
            moonOrbit =75;
            marsOrbit=350;

            sunSize=300; //sprite size on screen
            earthSize=75;
            moonSize = 30;
            marsSize=60;

            earthYear=365;
            moonYear=28;
            marsYear=450;

            
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

                sun = Content.Load<Texture2D>("sun");

            earth = Content.Load<Texture2D>("earth");
            moon = Content.Load<Texture2D>("moon");
            mars = Content.Load<Texture2D>("mars");

            Point center;

            center=sun.Bounds.Center;
            sunOrigin = new Vector2(center.X,center.Y);
            sunScale = sunSize/ sun.Width;

            center=earth.Bounds.Center;
            earthOrigin = new Vector2(center.X,center.Y);
            earthScale =  earthSize/ earth.Width;
 
            center=mars.Bounds.Center;
            marsOrigin = new Vector2(center.X,center.Y);
            marsScale =  marsSize/ mars.Width;

            center=moon.Bounds.Center;
            moonOrigin = new Vector2(center.X,center.Y);
            moonScale = moonSize/ moon.Width ;

            

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

            Single maxSimulationSpeed = 1000;
            Single minSimulationSpeed = 5;
            int maxX = this.graphics.GraphicsDevice.Viewport.Width;

            int x=Mouse.GetState().X;


            simulationSpeed = minSimulationSpeed+ maxSimulationSpeed * (float)x / (float)maxX;
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

            Double days = gameTime.TotalGameTime.TotalMilliseconds / simulationSpeed;

            Single earthRotation = (Single)(days / earthYear) * MathHelper.TwoPi;
            Single moonRotation = (Single)(days / moonYear) * MathHelper.TwoPi;
            Single marsRotation = (Single)(days / marsYear) * MathHelper.TwoPi;



            Matrix sunTransform = Matrix.CreateTranslation(GraphicsDevice.Viewport.Bounds.Center.X, GraphicsDevice.Viewport.Bounds.Center.Y, 0);
            Matrix earthTransform = Matrix.CreateTranslation(earthOrbit,0,0)*Matrix.CreateRotationZ(earthRotation)*sunTransform  ;
            Matrix moonTransform = Matrix.CreateTranslation(moonOrbit, 0, 0)*Matrix.CreateRotationZ(moonRotation)*earthTransform ;
            Matrix marsTransform = Matrix.CreateTranslation(marsOrbit, 0, 0) * Matrix.CreateRotationZ(marsRotation) * sunTransform;

            spriteBatch.Begin(SpriteSortMode.BackToFront,null,null,null,null,null,sunTransform);
            spriteBatch.Draw(sun, Vector2.Zero, null, Color.White, 0, sunOrigin, sunScale, SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, earthTransform);
            spriteBatch.Draw(earth, Vector2.Zero, null, Color.White, 0, earthOrigin, earthScale, SpriteEffects.None, 0);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, moonTransform);
            spriteBatch.Draw(moon, Vector2.Zero, null, Color.White, 0, moonOrigin, moonScale, SpriteEffects.None, 0);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, marsTransform);
            spriteBatch.Draw(mars, Vector2.Zero, null, Color.White, 0, marsOrigin, marsScale, SpriteEffects.None, 0);
            spriteBatch.End();




            base.Draw(gameTime);
        }

 
    }
}
