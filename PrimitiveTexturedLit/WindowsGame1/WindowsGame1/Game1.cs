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
        Texture2D image1;
        Texture2D image2;
        Texture2D image3;


        Matrix world;
        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[24];
        BasicEffect basicEffect;
        float angle;
       

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

            basicEffect = new BasicEffect(graphics.GraphicsDevice);

            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 200), new Vector3(0, 0, 0), Vector3.Up);

            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f,
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                0.1f,
                100000);



            image1 = Content.Load<Texture2D>("charlize_sml");
            image2 = Content.Load<Texture2D>("charlize_b");
            image3 = Content.Load<Texture2D>("ae");

            // create our quad
            //give each vertex texture coordinates
            vertices[0].Position = new Vector3(0, 100, 0);
            vertices[0].TextureCoordinate = new Vector2(0, 1);

            vertices[1].Position = new Vector3(0, 0, 0);
            vertices[1].TextureCoordinate = new Vector2(0, 0);

            vertices[2].Position = new Vector3(100, 100, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 1);

            vertices[3].Position = new Vector3(100, 0, 0);
            vertices[3].TextureCoordinate = new Vector2(1, 0);

            vertices[0].Normal =vertices[1].Normal=vertices[2].Normal=vertices[3].Normal= new Vector3(0, 0, -1);


            vertices[4].Position = new Vector3(0, 100, 0);
            vertices[4].TextureCoordinate = new Vector2(0, 1);

            vertices[5].Position = new Vector3(0, 0, 0);
            vertices[5].TextureCoordinate = new Vector2(0, 0);

            vertices[6].Position = new Vector3(0, 100,100);
            vertices[6].TextureCoordinate = new Vector2(1, 1);

            vertices[7].Position = new Vector3(0, 0, 100);
            vertices[7].TextureCoordinate = new Vector2(1, 0);

            vertices[4].Normal = vertices[5].Normal = vertices[6].Normal = vertices[7].Normal = new Vector3(-1, 0, 0);

            vertices[8].Position = new Vector3(100,0, 0);
            vertices[8].TextureCoordinate = new Vector2(0, 1);

            vertices[9].Position = new Vector3(0, 0, 0);
            vertices[9].TextureCoordinate = new Vector2(0, 0);

            vertices[10].Position = new Vector3(100, 0, 100);
            vertices[10].TextureCoordinate = new Vector2(1, 1);

            vertices[11].Position = new Vector3(0, 0, 100);
            vertices[11].TextureCoordinate = new Vector2(1, 0);

            vertices[8].Normal = vertices[9].Normal = vertices[10].Normal = vertices[11].Normal = new Vector3(0, -1, 0);

           
            vertices[12].Position = new Vector3(100, 100, 0);
            vertices[12].TextureCoordinate = new Vector2(0, 1);

            vertices[13].Position = new Vector3(100, 0, 0);
            vertices[13].TextureCoordinate = new Vector2(0, 0);

            vertices[14].Position = new Vector3(100, 100, 100);
            vertices[14].TextureCoordinate = new Vector2(1, 1);

            vertices[15].Position = new Vector3(100, 0, 100);
            vertices[15].TextureCoordinate = new Vector2(1, 0);

            vertices[13].Normal = vertices[14].Normal = vertices[15].Normal = vertices[12].Normal = new Vector3(1, 0, 0);

            vertices[16].Position = new Vector3(100, 100, 0);
            vertices[16].TextureCoordinate = new Vector2(0, 1);

            vertices[17].Position = new Vector3(0, 100, 0);
            vertices[17].TextureCoordinate = new Vector2(0, 0);

            vertices[18].Position = new Vector3(100, 100, 100);
            vertices[18].TextureCoordinate = new Vector2(1, 1);

            vertices[19].Position = new Vector3(0, 100, 100);
            vertices[19].TextureCoordinate = new Vector2(1, 0);

            vertices[17].Normal = vertices[18].Normal = vertices[19].Normal = vertices[16].Normal = new Vector3(0, 1, 0);

            vertices[20].Position = new Vector3(0, 100, 100);
            vertices[20].TextureCoordinate = new Vector2(0, 1);

            vertices[21].Position = new Vector3(0, 0, 100);
            vertices[21].TextureCoordinate = new Vector2(0, 0);

            vertices[22].Position = new Vector3(100, 100, 100);
            vertices[22].TextureCoordinate = new Vector2(1, 1);

            vertices[23].Position = new Vector3(100, 0, 100);
            vertices[23].TextureCoordinate = new Vector2(1, 0);

            vertices[20].Normal = vertices[21].Normal = vertices[22].Normal = vertices[23].Normal = new Vector3(0, 0, 1);


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

            world = Matrix.CreateTranslation(-50,-50,-50)*Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle * 2.5f) * Matrix.CreateRotationX(angle * 1.5f)*Matrix.CreateTranslation(0,0,-200);
            angle += 0.005f;
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
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState=rs;
            // TODO: Add your drawing code here


            basicEffect.World = world;

            basicEffect.TextureEnabled = true;

            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();
            basicEffect.DirectionalLight1.Enabled = false;
            basicEffect.DirectionalLight2.Enabled = false;



            basicEffect.Texture = image1;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 4, 2);
            }
            basicEffect.Texture = image2;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 8, 2);
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 12, 2);
            }
            basicEffect.Texture = image3;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 16, 2);
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 20, 2);
            }
 

            base.Draw(gameTime);
        }
    }
}
