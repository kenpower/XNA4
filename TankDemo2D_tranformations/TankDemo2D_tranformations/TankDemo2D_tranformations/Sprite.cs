using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TankDemo2D_tranformations
{
    class Sprite
    {
        private Sprite parent;

        public Sprite Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected Texture2D image;


        public Texture2D Image
        {
            get { return image; }
            set {   image = value;
                    imageOrigin = new Vector3(-image.Bounds.Center.X, -image.Bounds.Center.Y,0);
                    boundingRadius =image.Bounds.Width / 2;
            }
        }
        protected SpriteBatch spriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }


        public Vector3 position;
        public Vector3 velocity;

        public float rotation;
        protected Vector3 imageOrigin;
        protected float scale=1;
        protected int boundingRadius;
        private float maxSpeed=1;

        protected float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        private float maxTurnSpeed=0.01f;

        protected float MaxTurnSpeed
        {
            get { return maxTurnSpeed; }
            set { maxTurnSpeed = value; }
        }

        private Matrix world;

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        public Vector3 Direction
        {
            get
            {
                Vector3 dir = new Vector3(1, 0,0);
                Matrix rot = Matrix.CreateRotationZ(rotation);
                return  Vector3.Transform(dir, rot);
            }

            
            
        }

        public void Update(GameTime gameTime, Rectangle game_bounds)
        {
            position += velocity;

            game_bounds.Inflate((int)(-boundingRadius * scale), (int)(-boundingRadius * scale));

            position.X = MathHelper.Clamp(position.X, game_bounds.Left, game_bounds.Right);
            position.Y = MathHelper.Clamp(position.Y, game_bounds.Top, game_bounds.Bottom);

            world = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(position);
        
        }

        public void Draw(Matrix cameraMatrix)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(imageOrigin) * world * cameraMatrix);
            SpriteBatch.Draw(image, Vector2.Zero, Color.White);
            SpriteBatch.End();
        }


    }
}
