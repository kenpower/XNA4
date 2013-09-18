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

namespace CityShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
       
        

        String[] map ={  
                    "╬═╦═╦════╗",
                    "║1║3║1111║",
                    "║2║4╠════╣",
                    "╠═╩═╣7777║",
                    "║555║7777║",
                    "╠═╦═╬════╣",
                    "║3║9║1111║",
                    "║3╚═╬═╗66║",
                    "║343╠═╝99║",
                    "╚═══╩════╝"          
        };

        
        List<Street> streets;
        Rectangle blockSize;

        Camera camera;
        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);
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
            blockSize = new Rectangle(0, 0, 15, 15);
           

            streets = new List<Street>();

            StreetFactory.Init(this, blockSize);

            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (StreetFactory.streetSymbols.Contains(c))
                    {
                        Street s = StreetFactory.makeStreet(map[z][x], new Vector2(x, z));
                        streets.Add(s);
                    }
                }
            }
            camera = new Camera();
            camera.Init(new Vector3(0, 10, 0), new Vector3(50, 10, 50), Vector3.Up,0.6f,graphics.GraphicsDevice.Viewport.AspectRatio,1,1000);

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
            camera.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp; // need to do this on reach devices to allow non 2^n textures
            RasterizerState rs = RasterizerState.CullNone;
            
            GraphicsDevice.RasterizerState = rs;
            // TODO: Add your drawing code here
            
            
            foreach (Street s in streets)
            {
                s.Draw(gameTime, camera);
            }
            base.Draw(gameTime);
        }
    }
}
