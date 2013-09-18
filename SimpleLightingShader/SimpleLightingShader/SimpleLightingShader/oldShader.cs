
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace XNA_4_TexturedLitTeapot
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        VertexPositionNormalTexture[] toruspoints;
        Matrix view, proj, world1, world2;
        Effect effect;
        Vector3 eye =new Vector3( 0, 0, 50 );
        float angle = 0.0f;
        Texture2D texture;
        Model teapot;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
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
            InitializeTransform();
            FillTourusPoints();
            InitializeTransform();
            
            effect = content.Load<Effect>("shader_5");





            base.Initialize();
        }

        private void InitializeTransform()
        {

            view = Matrix.CreateLookAt(
                eye,
                Vector3.Zero,
                Vector3.Up
                );

            proj = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 1000.0f);




        }

        private void FillTourusPoints()
        {

            int layers = 30;
            int slices = 30;
            float u, du = 1.0f / layers;
            float v, dv = 1.0f / slices;
            int point = 0;

            toruspoints = new VertexPositionNormalTexture[layers * slices * 3 * 2];

            for (u = 0; u < 1.0f; u += du)
            {
                for (v = 0; v < 1.0f; v += dv)
                {
                    //uppertriangle
                    toruspoints[point].Position = XNATorusPoint(u, v);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u, v);
                    toruspoints[point].TextureCoordinate = new Vector2(u, v);
                    point++;
                    toruspoints[point].Position = XNATorusPoint(u + du, v);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u + du, v);
                    toruspoints[point].TextureCoordinate = new Vector2(u + du, v);
                    point++;
                    toruspoints[point].Position = XNATorusPoint(u, v + dv);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u, v + dv);
                    toruspoints[point].TextureCoordinate = new Vector2(u, v + dv);
                    point++;

                    //lowertriangle
                    toruspoints[point].Position = XNATorusPoint(u + du, v);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u + du, v);
                    toruspoints[point].TextureCoordinate = new Vector2(u + du, v);
                    point++;
                    toruspoints[point].Position = XNATorusPoint(u + du, v + dv);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u + du, v + dv);
                    toruspoints[point].TextureCoordinate = new Vector2(u + du, v + dv);
                    point++;
                    toruspoints[point].Position = XNATorusPoint(u, v + dv);
                    toruspoints[point].Normal = XNATexturedTorusNormal(u, v + dv);
                    toruspoints[point].TextureCoordinate = new Vector2(u, v + dv);
                    point++;

                }

            }
        }

        private Vector3 XNATexturedTorusNormal(float u, float v)
        {
            u *= (float)Math.PI * 2.0f;
            v *= (float)Math.PI * 2.0f;


            return new Vector3(
                (float)Math.Cos(u) * ((float)Math.Cos(v)),
                (float)Math.Sin(v),
                (float)Math.Sin(u) * ((float)Math.Cos(v))
                );
        }

        private Vector3 XNATorusPoint(float u, float v)
        {
            u *= (float)Math.PI * 2.0f;
            v *= (float)Math.PI * 2.0f;

            float R = 10.5f, r = 3.5f;
            return new Vector3(
                (float)Math.Cos(u) * (R + r * (float)Math.Cos(v)),
                r * (float)Math.Sin(v),
                (float)Math.Sin(u) * (R + r * (float)Math.Cos(v))
                );
        }
        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected

 override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // TODO: Load any ResourceManagementMode.Automatic content
            }

            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            angle += 0.01f;
            // Allows the default game to exit on Xbox 360 and Windows
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            world1 = Matrix.CreateRotationX(angle) * Matrix.CreateRotationY(angle * 1.5f) * Matrix.CreateTranslation(-10, 0, 0);
            world2 = Matrix.CreateRotationX(angle) * Matrix.CreateRotationY(angle * 1.5f) * Matrix.CreateTranslation(+10, 0, 0);
            
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

            // TODO: Add your drawing code here
            graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice,
                VertexPositionNormalTexture.VertexElements);

            effect.Parameters["view_proj_matrix"].SetValue(world1 * view * proj);
            effect.Parameters["inv_world_matrix"].SetValue(Matrix.Invert(world1)); 
            effect.Parameters["Light1_Position"].SetValue(new Vector4(-10,50,0,1));
            effect.Parameters["Light1_Color"].SetValue(new Vector4(1, 1, 1, 1));
            effect.Parameters["view_position"].SetValue(eye);
            effect.Parameters["Light_Ambient"].SetValue(new Vector4(0.5f, 0.5f, 0.5f, 1));
            effect.CurrentTechnique = effect.Techniques["Gouraud"];

           



            effect.Begin();
             
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(
                PrimitiveType.TriangleList,
                toruspoints,
                0,
                toruspoints.Length / 3);
                
                pass.End();
               
            }


            effect.End();

            // draw the per pixel lit torus
            effect.Parameters["view_proj_matrix"].SetValue(world2 * view * proj);
            effect.Parameters["inv_world_matrix"].SetValue(Matrix.Invert(world2));
            effect.CurrentTechnique = effect.Techniques["TransformPerPixel"];

            effect.Begin();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(
                PrimitiveType.TriangleList,
                toruspoints,
                0,
                toruspoints.Length / 3);

                pass.End();

            }


            effect.End();
            base.Draw(gameTime);
        }
    }
}