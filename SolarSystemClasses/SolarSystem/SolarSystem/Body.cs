using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SolarSystem
{
    class Body
    {

        Texture2D image;

        public Texture2D Image
        {
            get { return image; }
            set { image = value;
                Point center = image.Bounds.Center;
                imageOrigin = new Vector2(center.X, center.Y);
                screenScale = screenSize / image.Width;
            }
        }

        Vector2 imageOrigin;
        Single orbitRadius;
        Single yearLength;
        Single screenScale;
        Single screenSize;

        Matrix transform;

        public Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        public void Init( Single radius, Single year, Single size)
        {
            orbitRadius = radius;
            yearLength = year;
            screenSize = size;

           
        
        }

        public void Update(GameTime gameTime, Matrix parentTransform, float simSpeed)
        {
            Double secondsElapsed = gameTime.TotalGameTime.TotalSeconds;

            Single rotation=0;
            Single secondsPerYear = yearLength * 365 * 24 * 60 * 60;
            if (yearLength > 0)
            {
                rotation = (Single)(secondsElapsed / secondsPerYear) * MathHelper.TwoPi;
                rotation *= simSpeed*1000000;
            }


            Transform = Matrix.CreateTranslation(orbitRadius, 0, 0) * 
                Matrix.CreateRotationZ(rotation) * 
                parentTransform;

        }
       
        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            sb.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Transform);
            sb.Draw(image, Vector2.Zero, null, Color.White, 0, imageOrigin, screenScale, SpriteEffects.None, 0);
            sb.End();
      
        }
       


    }
}
