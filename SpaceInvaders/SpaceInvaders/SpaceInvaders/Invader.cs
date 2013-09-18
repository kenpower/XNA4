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


namespace SpaceInvaders
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Invader : Microsoft.Xna.Framework.GameComponent
    {
        static List<Bullet> bullets;// one list for all invaders
        static Random random = new Random();
        public static List<Bullet> Bullets
        {
            get { return Invader.bullets; }
            set { Invader.bullets = value; }
        }
        static Model bulletModel;

        public static Model BulletModel
        {
          get { return Invader.bulletModel; }
          set { Invader.bulletModel = value; }
        }


        Game theGame;
        Model invaderModel;
        
        public Vector3 position = Vector3.Zero;
        float maxSpeed = 10;
        Vector3 velocity;
        Camera camera;
        Matrix world = Matrix.Identity;
        public bool alive;
       
        
        public int radius = 500;

        public Invader(Game game)
            : base(game)
        {
            theGame = game;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public  void Initialize(Model m, Vector3 direction, Vector3 pos)
        {
            // TODO: Add your initialization code here
            position=pos;
            velocity=direction*maxSpeed;
            ContentManager Content = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            camera = (Camera)theGame.Services.GetService(typeof(Camera));
            invaderModel = m;
            
            //timeOfBirth= DateTime.Now;
            alive = true;
            base.Initialize();

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //position += velocity;
           

            world =Matrix.CreateScale(radius)*Matrix.CreateTranslation(position) ;

//            if (DateTime.Now.Subtract(timeOfBirth).TotalMilliseconds > 2000)
//            {
//                alive = false;
//            }
            int i=random.Next()%40;
            if (i == 0 && bullets.Count<40)
            {
                Bullet b = new Bullet(theGame);
                b.Initialize(bulletModel, Vector3.Backward, position);
                bullets.Add(b);
            }

            base.Update(gameTime);
        }


        public void Draw(GameTime gt)
        {

            foreach (ModelMesh mesh in invaderModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //Effect Settings Goes here
                    effect.LightingEnabled = true;
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.Projection = camera.Proj;
                    effect.View = camera.View;
                }
                mesh.Draw();
            }

        }

        public static bool IsDead(Invader b)
        {
           return !b.alive;
        }

    
    }
}
