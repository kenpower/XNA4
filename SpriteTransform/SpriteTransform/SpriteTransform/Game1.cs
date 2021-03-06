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

namespace SpriteTransform
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D invader;
        Vector2 pos, position2;
        //Vector2 origin;
        bool redEye;



        float rotation = 0;

        Matrix alienTransform=Matrix.Identity;
        
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
            IsMouseVisible = true;

            invader = Content.Load<Texture2D>("space-invader");
            //origin = new Vector2(invader.Width / 2, invader.Height / 2);
            

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

            Vector2 m=new Vector2(Mouse.GetState().X,Mouse.GetState().Y);

            m = Vector2.Transform(m, Matrix.Invert(alienTransform));
            if (Mouse.GetState().LeftButton==ButtonState.Pressed ){
                if( Vector2.Distance(pos, m) < 50)
                {
                    redEye = true;
                }
                else{
                    redEye = false;
                }
            }

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

            rotation -= 0.03f;
            Vector2 origin = new Vector2(invader.Width / 2, invader.Height / 2);

            //spin in place
            Matrix alienTransform =
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(200, 200, 0);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, 
                null, null, null, null, null, 
                alienTransform);


            spriteBatch.Draw(invader, 
                Vector2.Zero, //position
                null,Color.White, 0, 
                origin,// 
                1, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

//Vector3 centre=new Vector3(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height/ 2,0);
//alienTransform =Matrix.CreateTranslation(50, 0, 0) *
//                Matrix.CreateRotationZ(4*rotation) * 

//                Matrix.CreateRotationZ(-rotation)* 
//                Matrix.CreateTranslation(100, 0, 0) *
//                Matrix.CreateRotationZ(rotation)*
//                Matrix.CreateTranslation(centre);
