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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SAT_Collision_Demo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Shape> shapes=new List<Shape>();
        bool go = false;
        

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

            Shape.Texture = Content.Load<Texture2D>("Pixel");

            //graphics.PreferredBackBufferWidth = 400;
            //graphics.PreferredBackBufferHeight = 300;
            //graphics.ApplyChanges();

            Random r = new Random();
            Rectangle screenRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            float maxspeed = 10;
            int maxsize = 30, minsize = 15;
            for (int i = 0; i < 3; i++) { 
                shapes.Add(new Box( minsize+r.Next(maxsize),
                                    minsize + r.Next(maxsize), 
                                    new Vector2(100+r.Next(screenRect.Right-200), 100+r.Next(screenRect.Right-200)), 
                                    (float)(r.NextDouble()-0.5)/5,
                                    new Vector2((float)(r.NextDouble() - 0.5) * maxspeed, (float)(r.NextDouble() - 0.5) * maxspeed)
                                ));

                shapes.Add(new Triangle(minsize + r.Next(maxsize), 
                                    new Vector2(100 + r.Next(screenRect.Right - 200), 100 + r.Next(screenRect.Right - 200)),
                                    (float)(r.NextDouble() - 0.5) / 5,
                                    new Vector2((float)(r.NextDouble() - 0.5) * maxspeed, (float)(r.NextDouble() - 0.5) * maxspeed)
                                    ));

                shapes.Add(new Polygon(minsize + r.Next(maxsize), 5,
                                    new Vector2(100 + r.Next(screenRect.Right - 200), 100 + r.Next(screenRect.Right - 200)),
                                    (float)(r.NextDouble() - 0.5) / 5,
                                    new Vector2((float)(r.NextDouble() - 0.5) * maxspeed, (float)(r.NextDouble() - 0.5) * maxspeed)
                                    )); 
                shapes.Add(new Polygon(minsize + r.Next(maxsize), 10,
                     new Vector2(100 + r.Next(screenRect.Right - 200), 100 + r.Next(screenRect.Right - 200)),
                     (float)(r.NextDouble() - 0.5) / 5,
                     new Vector2((float)(r.NextDouble() - 0.5) * maxspeed, (float)(r.NextDouble() - 0.5) * maxspeed)
                     ));
                                   
            } 
           
            
           
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

            //if(Keyboard.GetState().GetPressedKeys().Length > 0){
            //    go=true;
            //}
            go = true;
            

            Rectangle screenRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

           

            if(go){ 
                //check for collisions
                for(int i=0;i<shapes.Count;i++){
                    for(int j=i+1;j<shapes.Count;j++){
                        if (Shape.CheckCollision(shapes[i], shapes[j]))
                        {
                            //repond to collision
                            Shape.bounce(shapes[i], shapes[j]);
                        }

                        }
                    }


                foreach (Shape s in shapes)
                        {
                            s.Update(screenRect);
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
            spriteBatch.Begin();
            foreach (Shape s in shapes)
            {
                s.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
