using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CityShooter
{
    class ParticleSystemFactory
    {
        public static String particleSymbols = "fr";
        public static String flareSymbols = "f";

        static Texture2D flareTexture;


        static Rectangle blockSize;

        public static ParticleSystem makeParticleSystem(char c, Vector2 position)
        {
            ParticleSystem ps = new ParticleSystem(theGame);
       
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

            //flareTexture = contentManger.Load<Texture2D>("fireball2");
           flareTexture = contentManger.Load<Texture2D>("mypart");

        }

    }
    class ParticleSystem:GameObject
    {
        public static int MaxParticles = 1000;
        Texture2D texture;

 
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


       public ParticleSystem(Game game)
            : base(game)
        {
        

        }

        VertexPositionColorTexture[] vertices;
        
        int numTriangles;
       

        ParticleEmmiter emmiter=new ParticleEmmiter();

        List<Particle> particles = new List<Particle>();

        public override void Init()
        {
           
            Effect = new BasicEffect(graphicsDevice);
            Effect.VertexColorEnabled = true;
            Effect.TextureEnabled = true;
            

            vertices = new VertexPositionColorTexture[ParticleSystem.MaxParticles * 6];


            emmiter.Init();
            emmiter.Position = position;

 
        }

        public void Update(GameTime gametime, Camera camera)
        {
            emmiter.Update(gametime, particles);

            particles.RemoveAll(delegate(Particle p) { return p.age >= p.maxAge; });

            foreach (Particle p in particles)
            {
                p.Update(gametime,camera);
            }


        }

        public void Draw(GameTime gametime, Camera camera)
        { 
           
            Effect.View = camera.View;
            Effect.Projection = camera.Projection;
            Effect.World = Matrix.Identity;
            Effect.Texture = texture;

            int i = 0;
            for(int j=particles.Count-1;j>=0;j--)
            {
                particles[j].getVertices(vertices, i * 6);
                i++;
            }

            BlendState bs = new BlendState();
            BlendState oldbs = graphicsDevice.BlendState;


            bs.ColorBlendFunction = BlendFunction.Add;


            bs.AlphaSourceBlend = Blend.SourceAlpha;
            bs.ColorSourceBlend = Blend.SourceAlpha; 
            
            //bs.AlphaSourceBlend = Blend.One;
            //bs.ColorSourceBlend = Blend.One;



            bs.AlphaDestinationBlend = Blend.One;// InverseSourceAlpha;
            bs.ColorDestinationBlend = Blend.One;//;Blend.InverseSourceAlpha;

            //bs.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            //bs.ColorDestinationBlend = Blend.InverseSourceAlpha;
         
            //bs.AlphaDestinationBlend = Blend.Zero;
            //bs.ColorDestinationBlend = Blend.Zero;
           
            graphicsDevice.BlendState = bs;
           
            DepthStencilState oldDss = graphicsDevice.DepthStencilState;
            DepthStencilState newDss = new DepthStencilState();
            newDss.DepthBufferWriteEnable = false;

            graphicsDevice.DepthStencilState = newDss;
          
            numTriangles = i * 2;
            if (numTriangles > 0)
            {
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, numTriangles);

                }
            }

            graphicsDevice.DepthStencilState = oldDss;
            graphicsDevice.BlendState = oldbs;

        }
    }


    class ParticleEmmiter{

        Vector3 position;
        Random random;
        float timeBetweenParticles = 0.01f; //seconds
        DateTime timeOfLastParticle; 

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ParticleEmmiter()
        {
        }
        public void Init()
        {
            timeOfLastParticle = DateTime.MinValue;
            random = new Random();
        }
        public void Update(GameTime gameTime, List<Particle> particleList)
        {
            if ((DateTime.Now - timeOfLastParticle).TotalSeconds < timeBetweenParticles)
            {
                return;
            }

            if (ParticleSystem.MaxParticles > particleList.Count)
            {
                float maxVel = 1.7f;
                Particle p=new Particle();
                p.Init();
                p.position = position;
                p.velocity = new Vector3((float)random.NextDouble()* maxVel, Math.Abs((float)random.NextDouble()) * 3.0f, (float)random.NextDouble() * maxVel);
                p.angularVelocity = (float)random.NextDouble() - 0.5f;
                p.force = new Vector3(0.01f, 0.1f, 0.01f);
                particleList.Add(p);
            }

           
            timeOfLastParticle = DateTime.Now;
            
        }
    }
    
    

    class Particle
    {
        float rotation;
        public Vector3 position;
        public Vector3 velocity;
        //Color color;
        public Vector3 force;
        //float maxHorzSpeed;
        //float maxVertSpeed;
        Vector2 size; 
        Vector2 maxSize;
        public float angularVelocity;
        public float age;
        public float maxAge = 5;

        VertexPositionColorTexture[] verts = new VertexPositionColorTexture[4];
        Vector3[] initVertsPos = new Vector3[4];

        public void Init()
        {
            float s=1; // initial size of a particle
            initVertsPos[0] = new Vector3(-s, -s, 0);
            initVertsPos[1] = new Vector3(+s, -s, 0);
            initVertsPos[2] = new Vector3(+s, +s, 0);
            initVertsPos[3] = new Vector3(-s, +s, 0);

            for (int i = 0; i < 4; i++)
            {
                verts[i].Position = initVertsPos[i];
            }

            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(1, 0);
            verts[2].TextureCoordinate = new Vector2(1, 1);
            verts[3].TextureCoordinate = new Vector2(0, 1);

            verts[0].Color = Color.White;
            verts[1].Color = Color.White;
            verts[2].Color = Color.White;
            verts[3].Color = Color.White;
 


            maxSize = new Vector2(2, 2);
            size = new Vector2(0.1f, 0.1f);

            
        }


        public void Update(GameTime gametime, Camera camera)
        {
            float time=(float)gametime.ElapsedGameTime.TotalMilliseconds/1000.0f;

            rotation += angularVelocity * time;
            position += velocity * time;
            velocity += force * time;

           

            if (size.X < maxSize.X)
            {
                size *= 1.035f;
            }

 
            Matrix billboardM = Matrix.CreateBillboard(position, camera.Position, camera.Up, camera.Direction);
            Matrix rotationM = Matrix.CreateRotationZ(rotation);
            Matrix scaleM = Matrix.CreateScale(size.X, size.Y, 1);
            //Matrix translateM = Matrix.CreateTranslation(position);

            Matrix transform = scaleM * rotationM * billboardM;//* scaleM;//*rotationM*scaleM


            for (int i = 0; i < 4; i++)
            {
                verts[i].Position = Vector3.Transform(initVertsPos[i], transform);
                verts[i].Color.A = (byte)( 255.0*(1 -(age / maxAge)));

                if ((1 - (age / maxAge)) < 0.0f)
                {
                    ;
                }
            }

           age += time;

      
        }

        public void getVertices(VertexPositionColorTexture[] vertices,int offset)
        {

            vertices[offset++] = verts[0];
            vertices[offset++] = verts[1];
            vertices[offset++] = verts[3];

            vertices[offset++] = verts[1];
            vertices[offset++] = verts[3];
            vertices[offset++] = verts[2];

        }
  
    }
}
