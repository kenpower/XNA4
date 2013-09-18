using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CityShooter
{
    class NPCFactory
    {
        public static String NPCSymbols = "tc";
        public static String tankSymbols = "t";
        public static String civilianSymbols = "c";

        static Model tankModel;
        static Model civilianModel;
        static Rectangle blockSize;

        public static NPC makeNPC(char c,Vector2 position)
        {
            
            NPC n=null; 
            
            if(tankSymbols.Contains(c)){
                n=new Tank(theGame);
                n.Model = tankModel;
            }

            if(civilianSymbols.Contains(c)){
                n=new Civilian(theGame);
                n.Model = civilianModel;
            }

            if (n!=null)
            {
                n.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);

                n.Init();
            }
            return n;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadNPCModels();
            blockSize=bS;
            
        }

        static void LoadNPCModels()
        {

            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            tankModel = contentManger.Load<Model>("tank");
            civilianModel = contentManger.Load<Model>("tiny");



        }

    }
    abstract public class NPC: GameObject
    {

        protected Model model;

        public Model Model
        {
          get { return model; }
          set { model = value; }
        }

        Rectangle size;

        public Rectangle Size
        {
            get { return size; }
            set { size = value; }
        }

        BoundingBox boundingBox;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }


        public NPC(Game game):base(game)
        {
        }

        public abstract override void Init();


        public abstract override void Update(GameTime gametime);

        public abstract override void Draw(GameTime gametime, Camera camera);


    }

    public class Tank:  NPC
    {
        public Tank(Game game):base(game)
        {
        }
        
        public override void Draw(GameTime gametime, Camera camera)
        {
            Matrix[] bones = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(bones);
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.EnableDefaultLighting();
                    e.World = bones[m.ParentBone.Index] * Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(position) * Matrix.CreateTranslation(6,0, 6);
                    e.Projection = camera.Projection;
                    e.View = camera.View;
                }

                m.Draw();
            }
        }

        public override void Update(GameTime gametime) { }

        public override void Init() { }


    }

    public class Civilian : NPC
    {
        public Civilian(Game game):base(game)
        {
        }

        public override void Draw(GameTime gametime, Camera camera)
        {
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {

                    e.World =   Matrix.CreateScale(0.01f)*Matrix.CreateTranslation(position)* Matrix.CreateTranslation(6, 3.1f, 6);
                    e.Projection = camera.Projection;
                    e.View = camera.View;
                }

                m.Draw();
            }
        }
        public override void Update(GameTime gametime) { }

        public override void Init() { }
    }
}
