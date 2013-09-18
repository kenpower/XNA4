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
                    "╔═╦═╦════╗",
                    "║f║3║1111║",
                    "║2║4╠════╣",
                    "╠═╩═╣7777║",
                    "║555║7777║",
                    "╠═╦═╬════╣",
                    "║f║9║1111║",
                    "║3╚═╬═╗66║",
                    "║34f╠═╝99║",
                    "╚═══╩════╝"          
        };

        String[] NPCmap ={  
                    "tctctctctc",
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "

        };

        List<Building> buildings;
        List<Street> streets;
        List<ParticleSystem> flares;

        List<NPC> npcs;

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
            buildings = new List<Building>();

            BuildingFactory.Init(this,blockSize);

            for(int z=0;z<map.Length;z++){
                for(int x=0;x<map[z].Length;x++){
                    char c=map[z][x];
                    if (BuildingFactory.buildingSymbols.Contains(c))
                    {
                        Building b = BuildingFactory.makeBuilding(map[z][x], new Vector2(x, z) );
                        buildings.Add(b);
                    }
                }
            }

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

            flares = new List<ParticleSystem>();

            ParticleSystemFactory.Init(this, blockSize);

            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (ParticleSystemFactory.particleSymbols.Contains(c))
                    {
                        ParticleSystem ps = ParticleSystemFactory.makeParticleSystem(map[z][x], new Vector2(x, z));
                        flares.Add(ps);
                    }
                }
            }

            npcs = new List<NPC>();

            NPCFactory.Init(this, blockSize);

            for (int z = 0; z < NPCmap.Length; z++)
            {
                for (int x = 0; x < NPCmap[z].Length; x++)
                {
                    char c = NPCmap[z][x];
                    if (NPCFactory.NPCSymbols.Contains(c))
                    {
                        NPC npc = NPCFactory.makeNPC(c, new Vector2(x, z));
                        npcs.Add(npc);
                    }
                }
            }

            MissileFactory.Init(this, blockSize);
            MissileFactory.makeMissile(new Vector2(0, 0));

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

            foreach(Building b in buildings){
                CollisionManager.DetectCameraBuildingCollisions(camera, b);
            }

            foreach (ParticleSystem b in flares)
            {
                b.Update(gameTime,camera);
            }

            MissileFactory.Update(gameTime);

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




            foreach (Building b in buildings)
            {
                b.Draw(gameTime, camera);
            }

            foreach (Street s in streets)
            {
                s.Draw(gameTime, camera);
            }

            foreach (ParticleSystem ps in flares)
            {
                ps.Draw(gameTime, camera);
            }

            foreach (NPC npc in npcs)
            {
                npc.Draw(gameTime, camera);
            }


            MissileFactory.Draw(gameTime, camera);


            base.Draw(gameTime);
        }
    }
}
