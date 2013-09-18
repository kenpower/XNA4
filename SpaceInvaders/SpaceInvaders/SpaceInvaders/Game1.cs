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

namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Ship ship;
        List<Invader> invaders = new List<Invader>();
        Model invaderModel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ship = new Ship(this);
            camera  = new Camera(this);

            
            Services.AddService(typeof(ContentManager), Content);
            Services.AddService(typeof(Camera), camera);
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


            camera.Init();
            ship.Init();

            invaderModel = Content.Load<Model>("cube");
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    int x = -4500 + i * 1000;
                    int y = -5000 - j * 1000;
                    Invader inv = new Invader(this);
                    inv.Initialize(invaderModel, Vector3.Forward, new Vector3(x, 0, y));
                    invaders.Add(inv);
                }
            }

            Invader.Bullets = new List<Bullet>();
            Invader.BulletModel = Content.Load<Model>("Cylinder");
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
            camera.Update();
            ship.Update(gameTime);
            foreach (Invader i in invaders)
            {
                i.Update(gameTime);
            }

            foreach (Bullet b in Invader.Bullets)
            {
                b.Update(gameTime);
            }

            CheckForCollisions();

            invaders.RemoveAll(Invader.IsDead);
            Invader.Bullets.RemoveAll(delegate(Bullet b){return b.position.Z>1000;});


            CheckForShipCollisions();

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

            ship.Draw(gameTime);
            foreach (Invader i in invaders)
            {
                i.Draw(gameTime);
            }
            foreach (Bullet b in Invader.Bullets)
            {
                b.Draw(gameTime);
            }


            base.Draw(gameTime);
        }

        void CheckForCollisions()
        {
            foreach (Invader i in invaders)
            {
                foreach (Bullet b in ship.Bullets)
                {
                    int touchingDistance=b.radius+i.radius;

                    float distance = Vector3.Distance(b.position, i.position);
                    if (distance < touchingDistance)
                    {
                        b.alive = false;
                        i.alive = false;
                    }
                }
            }
        }

        void CheckForShipCollisions()
        {

                foreach (Bullet b in Invader.Bullets)
                {
                    int touchingDistance = b.radius + ship.radius;

                    float distance = Vector3.Distance(b.position, ship.position);
                    if (distance < touchingDistance)
                    {
                        b.alive = false;
                        ship.Killed();
                    }
                }
            
        }
    }
}
