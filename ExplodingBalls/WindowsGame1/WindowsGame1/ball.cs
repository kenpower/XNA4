using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Ball
    {
        static Texture2D image;
        static int num=0;
        static float radius;
        static float scale=0.1f;

        public bool dead=false;
        Vector2 position;
        Vector2 velocity;
        Explosion explosion=new Explosion();


        public void Init(Rectangle rect)
        {
            int speed = 100;
            Random r = new Random(num++);

            position.X = r.Next(0, rect.Width);
            position.Y = r.Next(0, rect.Height);

            velocity.X = r.Next(-speed,speed);
            velocity.Y = r.Next(-speed,speed);

        }

        public static void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("ball");
            radius=(image.Width/2)*scale;

        }

      

        public void Draw(SpriteBatch spriteBatch)
        {
            if (explosion.started)
            {
                explosion.Draw(spriteBatch);
            }
            if (dead) return;
            //SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.Draw(image, position, null, Color.White, 0, new Vector2(image.Width/2,image.Height/2), scale, SpriteEffects.None, 0.5f);
            //spriteBatch.End();
        }

        public void Update(GameTime time, Rectangle rect, List<Ball> balls)
        {
            if (explosion.started)
            {
                explosion.Update();
            }
            if (dead) return;
            position += velocity * time.ElapsedGameTime.Milliseconds / 1000;

            if (position.X < 0)
            {
                position.X = 0;
                velocity.X *= -1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                velocity.Y *= -1;
            }
            if (position.X >rect.Width)
            {
                position.X = rect.Width;
                velocity.X *= -1;
            }
            if (position.Y > rect.Height)
            {
                position.Y = rect.Height;
                velocity.Y *= -1;
            }
          
            //check for collisions
            foreach(Ball b in balls)
            {
                if (b == this || b.dead==true)
                {
                    continue; //skip this ball
                }

                if(Vector2.Distance(position,b.position) < radius *2){
                    this.dead = true;
                    b.dead=true; //kill both balls
                    explosion.Start((position + b.position) / 2);
                   
                }
            }
        }

    }


}
