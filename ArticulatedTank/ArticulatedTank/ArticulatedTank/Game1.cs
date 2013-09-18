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

namespace ArticulatedTank
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model tank;
        BasicEffect basicEffect;
        float angle = 0.0f;
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
            tank = Content.Load<Model>("tank");
            basicEffect = new BasicEffect(GraphicsDevice);

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

            angle += 0.01f;
            base.Update(gameTime);
        }

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateLookAt(new Vector3(500, 500, 500), Vector3.Zero, Vector3.Up);
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(1, GraphicsDevice.Viewport.AspectRatio, 1, 10000);

            Quaternion q=new Quaternion(Vecor3.Cross
            Matrix[] transforms = new Matrix[tank.Bones.Count]; 
            tank.CopyAbsoluteBoneTransformsTo(transforms); 
            foreach (ModelMesh mesh in tank.Meshes) { 
                foreach (BasicEffect effect in mesh.Effects) {
                    Matrix meshWorld  = transforms[mesh.ParentBone.Index] * world;
                    
                    if (mesh.Name == "hatch_geo" || mesh.Name == "canon_geo")
                    {
                        meshWorld =  Matrix.CreateTranslation(0, 105, 100)*Matrix.CreateRotationY(angle * 2) * transforms[9] ;
                    }

                    if (mesh.Name == "turret_geo")
                    {
                        meshWorld = Matrix.CreateRotationY(angle * 2) * transforms[mesh.ParentBone.Index];
                    }
                    if ( mesh.Name == "canon_geo" )
                    {
                        //meshWorld =  Matrix.CreateRotationX(angle /2)*transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(angle * 2);
                    }

                    if (mesh.Name.Contains("wheel"))
                    {
                        meshWorld =  Matrix.CreateRotationX(angle * 2)*transforms[mesh.ParentBone.Index] ;
                    }
                    

                    effect.World = meshWorld;
                   
 
                    effect.View = view; 
                    effect.Projection = proj; 
                } 
                mesh.Draw(); 
            }

            base.Draw(gameTime);
        }
    }
}
