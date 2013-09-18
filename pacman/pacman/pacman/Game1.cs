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

namespace pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D pac, pacClosed;
        Vector2 position;
        Vector2 origin;
        float speed;
        bool flip=false;
        
        Vector2 velocity;
        float direction;


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

            pac = Content.Load<Texture2D>("pac_open");
            origin = new Vector2(pac.Width / 2, pac.Height / 2);
            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            direction = 0;
            speed = 3;
            velocity = new Vector2(0, 0);
            
            

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
            KeyboardState ks = Keyboard.GetState();
           

            if(ks.IsKeyDown(Keys.A)){
                direction=0;
                flip = true;
                velocity = new Vector2(-speed, 0);
            }
           if(ks.IsKeyDown(Keys.D)){
                direction=0;
                flip = false;
                velocity = new Vector2(speed, 0);
            }
           if(ks.IsKeyDown(Keys.S)){
                direction=MathHelper.PiOver2;
                velocity = new Vector2(0, speed);
                flip = false;
            }
           if(ks.IsKeyDown(Keys.W)){
                direction=MathHelper.PiOver2 *3;
                velocity = new Vector2(0, -speed);
                flip = false;
            }

           Rectangle bounds=graphics.GraphicsDevice.Viewport.Bounds;
           bounds.Inflate(-pac.Width / 2, -pac.Height / 2);

           if (position.X < bounds.Left)
           {
               velocity.X = 0;
               position.X = bounds.Left;
           }

           if (position.X > bounds.Right)
           {
               velocity.X = 0;
               position.X = bounds.Right;
           }

           if (position.Y < bounds.Top)
           {
               velocity.Y = 0;
               position.Y = bounds.Top;
           }

           if (position.Y > bounds.Bottom)
           {
               velocity.Y = 0;
               position.Y = bounds.Bottom;
           }

 
 
           position += velocity;




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
            SpriteEffects se = SpriteEffects.None;
            if (flip == true)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(pac, position, null, Color.White, direction, origin,1, se, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
