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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model cone,sphere;
        Texture2D moon;

        Matrix worldSphere,worldCone, view, projection;
        Vector3 lightDir;
        float angle = 0;
       

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
            cone = Content.Load<Model>("Cone");
            sphere = Content.Load<Model>("Sphere");
            worldCone = Matrix.Identity;
           
            view = Matrix.CreateLookAt(new Vector3(0, 10, 10), Vector3.Zero, Vector3.Up);
  
            projection = Matrix.CreatePerspectiveFieldOfView(0.5f, GraphicsDevice.Viewport.AspectRatio, 1, 100);

          

            lightDir = new Vector3(+1, 0, 0);

            moon = Content.Load<Texture2D>("moon_map");

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

            worldSphere = Matrix.CreateTranslation(3, 0, 0) *Matrix.CreateRotationY(angle) ;
            angle += 0.02f;
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

            // TODO: Add your drawing code here

            foreach (ModelMesh mesh in cone.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //Effect Settings Goes here                      
                    effect.EnableDefaultLighting();
                    effect.World = worldCone;
                    effect.Projection = projection;
                    effect.View = view;
                    //effect.DirectionalLight0.Direction = new Vector3(-1, 0, 0);
                }
                mesh.Draw();

            }
            foreach (ModelMesh mesh in sphere.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.FogEnabled = true;
                    effect.FogColor = Color.CornflowerBlue.ToVector3(); // For best results, make this color whatever your background is.   
                    effect.FogStart = 10f;
                    effect.FogEnd = 15.5f;  
                    effect.EnableDefaultLighting();
                    //effect.DiffuseColor = Color.Red.ToVector3();
                    effect.DirectionalLight0.Direction = lightDir;
                    effect.World = worldSphere;
                    effect.Projection = projection;
                    effect.View = view;
        
                }
                mesh.Draw();

            }

            base.Draw(gameTime);
        }
    }
}
