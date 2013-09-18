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

namespace SideScrollling
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, foreground, sky;
        int pos = 0;

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

           background= Content.Load<Texture2D>("Mountains");
           foreground = Content.Load<Texture2D>("nyc3");
           sky = Content.Load<Texture2D>("sunset");
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

            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                pos+=10;
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                pos-=10;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            int backgroundWidth = 2048;
            int backgroundHeight = GraphicsDevice.Viewport.Height;

            //if (pos< -backgroundWidth)
            //    pos = 0;
            //if (pos > backgroundWidth)
            //    pos = 0;
            spriteBatch.Begin();
            spriteBatch.Draw(sky, GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();


            DrawBackground(backgroundWidth, backgroundHeight, pos / 2, background);
            DrawBackground(backgroundWidth, backgroundHeight, pos, foreground);



            base.Draw(gameTime);
        }

        private void  DrawBackground(int backgroundWidth, int backgroundHeight, int position, Texture2D texture)
        {
            position = position % backgroundWidth;
            Matrix scrollMatrix = Matrix.CreateTranslation(position, 0, 0);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, scrollMatrix);
            spriteBatch.Draw(texture, new Rectangle(0, 0, backgroundWidth, backgroundHeight), Color.White);
            spriteBatch.End();

            Matrix scrollMatrix2;
            if (position >= 0)
                scrollMatrix2 = Matrix.CreateTranslation(position - backgroundWidth, 0, 0);
            else
                scrollMatrix2 = Matrix.CreateTranslation(position + backgroundWidth, 0, 0);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, scrollMatrix2);
            spriteBatch.Draw(texture, new Rectangle(0, 0, backgroundWidth, backgroundHeight), Color.White);
            spriteBatch.End();

  
        }
    }
}
