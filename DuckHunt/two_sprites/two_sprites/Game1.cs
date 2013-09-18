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

namespace two_sprites
{

    public class MySprite
    {

        Texture2D myTexture;

        // Set the coordinates to draw the sprite at.  
        public Vector2 spritePosition = Vector2.Zero;
        Vector2 spriteSpeed = new Vector2(100, 100); // pixels per second
        Vector2 spriteOrigin;
        float radius;
        float angle;
        SpriteEffects flip;
        
     
        public Vector2 SpriteSpeed
        {
            get { return spriteSpeed; }
            set { spriteSpeed = value; }
        }

        bool alive;

        public bool Alive
        {
          get { return alive; }
          set { alive = value; }
        }


       public void LoadContent(ContentManager c,String imageName)
        {

            myTexture = c.Load<Texture2D>(imageName);
            radius = myTexture.Height / 2;
            spriteOrigin = new Vector2(myTexture.Width / 2, myTexture.Height / 2);
            Alive=true;


        }

        public void UpdateSprite(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if(!alive) return;
            // Move the sprite by speed, scaled by elapsed time.      
            spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            int MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width/2;
            int MinX = myTexture.Width / 2;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height/2;
            int MinY =  myTexture.Height/2;
            // Check for bounce.      
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }
            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }

            //calculate angle

            //double slope;
            //if(spriteSpeed.X==0){
            //    if(spriteSpeed.Y>0){
            //           slope=double.PositiveInfinity;
            //    }
            //    slope=double.NegativeInfinity;
            //}
            //else{
            //    slope=spriteSpeed.Y/spriteSpeed.X;
            //}


            angle = (float)Math.Atan2(spriteSpeed.Y , spriteSpeed.X);
            flip = SpriteEffects.FlipHorizontally;
            //flip = SpriteEffects.None;
            if (spriteSpeed.X < 0)
            {
                flip = flip|SpriteEffects.FlipVertically;
            
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!alive) return;

            Matrix matrix = Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(spritePosition.X, spritePosition.Y, 0);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, matrix);
            spriteBatch.Draw(myTexture, Vector2.Zero, null, Color.White, 0, spriteOrigin, 1, flip, 1);
            spriteBatch.End();
        }

        public bool PointInSprite(Vector2 point)
        {
            if((spritePosition-point).Length()<radius){
                return true;
            }
            return false;
        }

    }

    public class Explosion
    {

        public static Texture2D myTexture;

        // Set the coordinates to draw the sprite at.  
        public Vector2 spritePosition = Vector2.Zero;
      
        Vector2 spriteOrigin;
       
        bool alive;
        TimeSpan timeToLive;
        float blend;
        float scale;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }


        public void Init(Vector2 pos,TimeSpan tTL)
        {
            DateTime a = DateTime.Now;

            spritePosition = pos;
            timeToLive = tTL;

            spriteOrigin = new Vector2(myTexture.Width / 2, myTexture.Height / 2);
            Alive = true;


        }

        public void UpdateSprite(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (!alive) return;
            // Move the sprite by speed, scaled by elapsed time.    
            DateTime a = DateTime.Now;

            timeToLive=timeToLive.Subtract(gameTime.ElapsedGameTime);
            if (timeToLive.CompareTo(TimeSpan.Zero) < 0)
            {
                alive = false;
            }

            blend = (float)timeToLive.TotalMilliseconds / 500;
            scale = (1000-(float)timeToLive.TotalMilliseconds)/1000;

            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!alive) return;
            Color c = new Color(blend, blend, blend, blend);
            Matrix matrix =  Matrix.CreateScale(scale)*Matrix.CreateTranslation(spritePosition.X, spritePosition.Y, 0);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,null,null,null,null,matrix);
            spriteBatch.Draw(myTexture, Vector2.Zero, null, c, 0, spriteOrigin,1, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }

       

    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // This is a texture we can render.  

        MySprite[] sp=new MySprite[100];


        MouseState oldMouseState;
        int aliveCount;

        SpriteFont font;
        double theTime;
        Texture2D crossHairs;
        List<Explosion> explosions=new List<Explosion>();

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.      
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Random r = new Random();
            for(int i=0;i<100;i++){
                sp[i]=new MySprite();
                sp[i].LoadContent(Content,"duck");
                int MaxX = graphics.GraphicsDevice.Viewport.Width;
                int MaxY = graphics.GraphicsDevice.Viewport.Height;
                sp[i].spritePosition = new Vector2(r.Next() % MaxX, r.Next() % MaxY);
                sp[i].SpriteSpeed = new Vector2(r.Next() % 300-150, r.Next() % 200-100);
            }

            font = Content.Load<SpriteFont>("SpriteFont1");
            crossHairs = Content.Load<Texture2D>("crosshairs");
            Explosion.myTexture = Content.Load<Texture2D>("explosion");
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
            oldMouseState = Mouse.GetState();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
       

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            for (int i = 0; i < 100; i++)
            {
                sp[i].UpdateSprite(gameTime, graphics);
            }

            foreach (Explosion e in explosions)
            {
                e.UpdateSprite(gameTime, graphics);
            }

            //is this a mouse click?
            if (oldMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //check for collisions

                Vector2 mp = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                for (int i = 0; i < 100; i++)
                {
                    if (sp[i].PointInSprite(mp) && sp[i].Alive)
                    {
                        sp[i].Alive = false;
                        Explosion e = new Explosion();
                        e.Init(sp[i].spritePosition,new TimeSpan(0,0,0,0,500));
                        explosions.Add(e);
                    }

                }
            }
            aliveCount = 0;
            // count alive ducks
            for (int i = 0; i < 100; i++)
            {
                if (sp[i].Alive)
                {
                    aliveCount++;
                }
            }

            oldMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);        // Draw the sprite.
            
            for (int i = 0; i < 100; i++)
            {
                sp[i].Draw(gameTime, spriteBatch);
            }

            foreach (Explosion e in explosions)
            {
                e.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.DrawString(font, "Ducks Left:" + aliveCount, new Vector2(0, 0), Color.Black);
            if(aliveCount>0)
                theTime=gameTime.TotalGameTime.TotalSeconds;
            String tm = String.Format("{0:0.00}", theTime);
            spriteBatch.DrawString(font, "Time " + tm, new Vector2(200, 0), Color.Black);

            //draw crosshairs
            Vector2 mousePos=new Vector2(Mouse.GetState().X,Mouse.GetState().Y);
            Vector2 mouseOrg=new Vector2(crossHairs.Width/2,crossHairs.Height/2);

            spriteBatch.Draw(crossHairs,mousePos,null, Color.White, 0, mouseOrg,1, SpriteEffects.None,0);

            spriteBatch.End();


            base.Draw(gameTime);
        }

        
         
    }
}
