using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CityShooter
{
    class SimpleParticleSystemFactory
    {
        public static String particleSymbols = "f";
        public static String flareSymbols = "f";

        static Texture2D flareTexture;


        static Rectangle blockSize;

        public static SimpleParticleSystem makeParticleSystem(char c, Vector2 position)
        {
            SimpleParticleSystem ps = new SimpleParticleSystem(theGame);
       
            if (flareSymbols.Contains(c))
            {
                ps.Texture = flareTexture;
        
            }

            ps.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);
            ps.Position += new Vector3(blockSize.Width / 2, 0, blockSize.Height / 2); //centre on tile

            ps.Init();
            return ps;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadParticleSystemTextures();
            blockSize = bS;

        }

        static void LoadParticleSystemTextures()
        {

            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));

            flareTexture = contentManger.Load<Texture2D>("flare");

        }

    }
    class SimpleParticleSystem:GameObject
    {
        public static int MaxParticles = 1000;
        Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


       public SimpleParticleSystem(Game game)
            : base(game)
        {
        

        }

       

        SimpleParticleEmmiter emmiter=new SimpleParticleEmmiter();

        List<SimpleParticle> particles = new List<SimpleParticle>();

        public override void Init()
        {

 
            emmiter.Init();
            emmiter.Position = position;

            Effect = new BasicEffect(graphicsDevice);
            Effect.VertexColorEnabled = true;
            Effect.TextureEnabled = true;

 
        }

        public void Update(GameTime gametime, Camera camera)
        {
            emmiter.Update(gametime, particles,this);

            foreach (SimpleParticle p in particles)
            {
                p.Update(gametime,camera);
            }


        }

        override public  void Draw(GameTime gametime, Camera camera)
        { 

            foreach(SimpleParticle sp in particles){
                sp.Draw(camera,graphicsDevice);
            }
        }
    }


    class SimpleParticleEmmiter{

        Vector3 position;
        Random random;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public SimpleParticleEmmiter()
        {
        }
        public void Init()
        {
            random = new Random();
        }

        public void Update(GameTime gameTime, List<SimpleParticle> particleList,SimpleParticleSystem sps)
        {

            if (SimpleParticleSystem.MaxParticles > particleList.Count)
            {
                float maxVel = 3.6f;
                SimpleParticle p=new SimpleParticle();
                p.Init();
                p.Position = position;
                p.velocity = new Vector3((float)random.NextDouble()* maxVel, Math.Abs((float)random.NextDouble()) * 3.0f, (float)random.NextDouble() * maxVel);
                p.Texture=sps.Texture;
                p.Effect=sps.Effect;
                particleList.Add(p);
            }
        
        }
    }
    
    

    class SimpleParticle
    {

        BasicEffect effect;

        public BasicEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }
        public Vector3 velocity;
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        Texture2D texture;

        public Texture2D Texture
        {
          get { return texture; }
          set { texture = value; }
        }
        
        

        VertexPositionColorTexture[] verts = new VertexPositionColorTexture[4];
        Vector3[] initVertsPos = new Vector3[4];

        
        public void Init()
        {
            float s = 0.5f; ; // initial size of a particle
            initVertsPos[0] = new Vector3(-s, -s, 0);
            initVertsPos[1] = new Vector3(+s, -s, 0);
            initVertsPos[2] = new Vector3(-s, +s, 0);
            initVertsPos[3] = new Vector3(+s, +s, 0);


            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(1, 0);
            verts[2].TextureCoordinate = new Vector2(0, 1);
            verts[3].TextureCoordinate = new Vector2(1, 1);

            verts[0].Color = Color.White;
            verts[1].Color = Color.White;
            verts[2].Color = Color.White;
            verts[3].Color = Color.White;
 


            
        }


        public void Update(GameTime gametime, Camera camera)
        {
            float time=(float)gametime.ElapsedGameTime.TotalMilliseconds/1000.0f;

           
            position += velocity * time;

            Matrix billboardM = Matrix.CreateBillboard(position, camera.Position, camera.Up, camera.Direction);
  
            Matrix transform = billboardM;//* scaleM;//*rotationM*scaleM


            for (int i = 0; i < 4; i++)
            {
                verts[i].Position = Vector3.Transform(initVertsPos[i], transform);

            }

      
        }

        public void Draw(Camera camera,GraphicsDevice g)
        {

            Effect.View = camera.View;
            Effect.Projection = camera.Projection;
            Effect.World = Matrix.Identity;
            Effect.Texture = texture;

            
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    g.DrawUserPrimitives(PrimitiveType.TriangleStrip, verts, 0, 2);
                }
            
        }
  
    }
}
