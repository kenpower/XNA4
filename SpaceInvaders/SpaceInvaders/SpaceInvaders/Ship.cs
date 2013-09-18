using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Ship
    {

        Model ship;
        Model bulletModel;
        bool spawning = false;
        bool visible = true;
        public Vector3 position=Vector3.Zero;
        Vector3 direction = Vector3.Backward;
        float maxSpeed=10;
        float yaw=0;
        float maxYawSpeed=0.01f;// radiansperframe
        float maxHorzSpeed = 50;
        Game game;
        Camera camera;
        Matrix world=Matrix.Identity;
        DateTime timeOfLastBullet;
        DateTime timeOfLastDeath=DateTime.MinValue;

        public int radius = 1000;

        List<Bullet> bullets = new List<Bullet>();

        public List<Bullet> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }

        public Ship(Game theGame)
        {
            game = theGame;
        }

        public void Init()
        {
            ContentManager Content = (ContentManager)game.Services.GetService(typeof(ContentManager));
            camera = (Camera)game.Services.GetService(typeof(Camera));
            ship = Content.Load<Model>("p1_wedge");
            bulletModel = Content.Load<Model>("cone");
            timeOfLastBullet = DateTime.Now;
           
        }

        public void Update(GameTime gt)
        {

            KeyboardState ks = Keyboard.GetState();

            if(ks.IsKeyDown(Keys.Up)){
                yaw+=maxYawSpeed;
            }
            if(ks.IsKeyDown(Keys.Down)){
                yaw-=maxYawSpeed;
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += maxHorzSpeed;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= maxHorzSpeed;

            }


            Matrix rot=Matrix.CreateRotationY(yaw);
            direction = Vector3.Transform(Vector3.Forward, rot);

            //only spawn bullet if its been 100ms since last bullet was made
            if (ks.IsKeyDown(Keys.Space)  
                && 
                DateTime.Now.Subtract(timeOfLastBullet).TotalMilliseconds
                > 100)
            {
                Bullet b = new Bullet(game);
                b.Initialize(bulletModel,direction,position);
                bullets.Add(b);
                timeOfLastBullet = DateTime.Now;
            }
            world = Matrix.CreateRotationY(yaw) * Matrix.CreateTranslation(position) ;

            foreach (Bullet b in bullets)
            {
                  b.Update(gt);
            }
            bullets.RemoveAll(Bullet.IsDead);

            long millsSinceLastDeath = (long)DateTime.Now.Subtract(timeOfLastDeath).TotalMilliseconds;
            if (millsSinceLastDeath < 3000)
            {//flicker for 3 seconds after death
                spawning = true;
                long tenthsSinceLastDeath = millsSinceLastDeath / 100;
                visible=true;
                if (tenthsSinceLastDeath % 2 == 0)
                {
                    visible = false;
                }
            }
            else
            {
                spawning = false;
                visible = true;

            }
           
        }

        public void Draw(GameTime gt)
        {

            if (visible)
            {

                foreach (ModelMesh mesh in ship.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        //Effect Settings Goes here
                        effect.LightingEnabled = false;
                        effect.World = world;
                        effect.Projection = camera.Proj;
                        effect.View = camera.View;
                    }
                    mesh.Draw();
                }
            }
            foreach (Bullet b in bullets)
            {
                 b.Draw(gt);
            }

        }

        public void Killed()
        {
            if(!spawning)
                timeOfLastDeath = DateTime.Now;
        }
    }
}
