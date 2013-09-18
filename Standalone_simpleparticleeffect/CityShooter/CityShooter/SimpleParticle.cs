/*Simple particle system*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CityShooter
{
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

        
        public void Init()
        {
            float s = 0.5f; ; // initial size of a particle
            verts[0].Position = new Vector3(-s, -s, 0);
            verts[1].Position = new Vector3(+s, -s, 0);
            verts[2].Position =  new Vector3(-s, +s, 0);
            verts[3].Position =  new Vector3(+s, +s, 0);


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

        }

        public void Draw(Camera camera,GraphicsDevice g)
        {
           
            Effect.View = camera.View;
            Effect.Projection = camera.Projection;
            Effect.World = Matrix.CreateBillboard(position, camera.Position, camera.Up, camera.Direction);
            Effect.Texture = texture;

            
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    g.DrawUserPrimitives(PrimitiveType.TriangleStrip, verts, 0, 2);
                }
            
        }
  
    }
}
