using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CityShooter
{
    class MissileFactory
    {

        static Model missileModel;

        static Rectangle blockSize;

        public static List<Missile> missiles = new List<Missile>();

        public static Missile makeMissile(Vector2 position)
        {
            
           Missile m=new Missile(theGame);
           m.Model = missileModel;
            
           m.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);


           m.Direction = new Vector3(1, 0.0f, 1);
           m.Init();

           missiles.Add(m);
            
           return m;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadMissileModels();
            blockSize=bS;
            
        }

        static void  LoadMissileModels()
        {

            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            missileModel = contentManger.Load<Model>("missile");

        }

        public static void Update(GameTime gameTime)
        {
            foreach (Missile m in missiles)
            {
                m.Update(gameTime);
            }
        }

        internal static void Draw(GameTime gameTime, Camera camera)
        {
            foreach (Missile m in missiles) {
                m.Draw(gameTime, camera);
            }
        }
    }

        class Missile : GameObject
        {

            Model model;

            public Model Model
            {
                get { return model; }
                set { model = value; }
            }

            Vector3 direction;

            public Vector3 Direction
            {
                get { return direction; }
                set { direction = value; }
            }

            float yawAngle;

          
            float pitchAngle;

           

            float speed = 0.05f;
            public Missile(Game game)
                : base(game)
            {


            }

            public override void Update(GameTime gametime)
            {
                Vector3 velocity;
                velocity = direction;
                velocity.Normalize();
                velocity *= speed;
                position += velocity;

                //calculate direction orientation
                direction.Normalize();
                Vector2 yawVector = new Vector2(direction.X, direction.Z);
                float horizLength = yawVector.Length();
                yawVector.Normalize();
                yawAngle = (float)Math.Acos(Vector2.Dot(yawVector, Vector2.UnitX)) + MathHelper.Pi;

                Vector2 pitchVector = new Vector2(horizLength, direction.Y);

                pitchVector.Normalize();


                pitchAngle = (float)Math.Acos(Vector2.Dot(pitchVector, Vector2.UnitX));
                if (pitchVector.Y < 0)
                    pitchAngle = -pitchAngle;

                KeyboardState ks = Keyboard.GetState();

                Vector3 pitchAxis = Vector3.Cross(direction,Vector3.Up);
                pitchAxis.Normalize();
                Matrix increasePitch = Matrix.CreateFromAxisAngle(pitchAxis, 0.01f);
                Matrix decreasePitch = Matrix.CreateFromAxisAngle(pitchAxis, -0.01f);
                if (ks.IsKeyDown(Keys.W))
                {
                    direction = Vector3.Transform(direction, increasePitch);
                }
                if (ks.IsKeyDown(Keys.S))
                {
                    direction = Vector3.Transform(direction, decreasePitch);
                }


                base.Update(gametime);
            }

            public override void Draw(GameTime gametime, Camera camera)
            {

                Matrix orientation = Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, 0);
                Matrix world = orientation * Matrix.CreateTranslation(position);

                foreach (ModelMesh m in model.Meshes)
                {
                    foreach (BasicEffect e in m.Effects)
                    {
                        e.EnableDefaultLighting();
                        e.World = world;
                        e.Projection = camera.Projection;
                        e.View = camera.View;
                    }

                    m.Draw();
                }
                base.Draw(gametime, camera);
            }
        }
}
