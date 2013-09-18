using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
        class Explosion
        {
            static Texture2D image;
            public bool started;
            int curentFrame;
            static int numFrames;
            static int frameWidth;
            static int frameHeight;
            static int frameRows;
            static int frameColumns;
            static int framesPerSec;
            DateTime timeOfLastFrame;
            Vector2 position;


            public void Start(Vector2 pos)
            {
                started = true;
                curentFrame = 0;
                position = pos;
                timeOfLastFrame = DateTime.Now;
            }

            public static void LoadContent(ContentManager content)
            {
                image = content.Load<Texture2D>("explosion");
                frameRows = 5;
                frameColumns = 5;
                frameWidth = image.Width / frameColumns;
                frameHeight = image.Height / frameRows;
                framesPerSec = 10;
                numFrames = frameRows * frameHeight;
            }

            Rectangle GetFrame(int i)
            {
                int column = i % frameColumns;
                int row = (i - column) / frameColumns;

                return new Rectangle(column * frameWidth, // xpos
                                    row * frameHeight,//ypos
                                    frameWidth-60,// width
                                    frameHeight);//height
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                //SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                Rectangle r = GetFrame(curentFrame);
                spriteBatch.Draw(image, position, GetFrame(curentFrame), Color.White, 0,
                    new Vector2(r.Width / 2, r.Height / 2), 0.5f, SpriteEffects.None, 0.9f);
                //spriteBatch.End();
            }

            public void Update()
            {
                long millsSinceLastFrame = (DateTime.Now.Ticks - timeOfLastFrame.Ticks) / 10000;

                int millsPerFrame = 1000 / framesPerSec;

                if (millsSinceLastFrame > millsPerFrame)
                {
                    curentFrame++;
                    timeOfLastFrame = DateTime.Now;
                }

                if (curentFrame >= numFrames)
                {
                    started = false;
                }


            }

        }
    

}
