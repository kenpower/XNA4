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

namespace TwoWayScrollingDemo
{
    public class Collider
    {


        public static Rectangle KeepInGameWorld(Rectangle rect, Rectangle gameWord)
        {
           if(gameWord.Contains(rect)){
               return rect;
           }
           Point location=rect.Location;

           if(rect.Left<gameWord.Left){
               location.X=gameWord.Left;
           }
           if(rect.Bottom>gameWord.Bottom){
               location.Y = gameWord.Bottom - rect.Height;
           }
           if(rect.Right>gameWord.Right){
               location.X=gameWord.Right-rect.Width;
           }
            if(rect.Top<gameWord.Top){
               location.Y=gameWord.Top;
           }
           rect.Location=location;
           return rect;
        }
    }

    public class Sprite
    {
        static Texture2D texture;

        public static Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        Vector2 position;
        Vector2 velocity;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public  void Update(GameTime gameTime,Rectangle world)
        {
            // Allows the game to exit

            if (position.X < world.Left || position.X > world.Right)
            {
                velocity.X = -velocity.X;
            }

            if (position.Y< world.Top || position.Y > world.Bottom)
            {
                velocity.Y = -velocity.Y;
            }

            position += velocity;

           
        }

        public void Draw(GameTime gt, SpriteBatch sb, bool red)
        {
            if(red)
                sb.Draw(texture, position,Color.Red);
            else
                sb.Draw(texture, position, Color.White);
          
        }

        internal void SetRandomPosition(Random r,Rectangle limits)
        {
           position=new Vector2(r.Next()%limits.Width,r.Next()%limits.Height);
        }

        internal void SetRandomVelocity(Random r)
        {
             int maxv=10;
             velocity = new Vector2(r.Next() % maxv - maxv / 2, r.Next() % maxv - maxv / 2);
        }
    }

    public class Camera
    {
        Vector2 position;
        Rectangle window;

        public Rectangle Window
        {
            get { return window; }
            set { window = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
      

        float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public  void Update(GameTime gameTime)
        {


            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Y -= speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Y += speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X += speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X -= speed;

            //move the window
            window.Location = new Point((int)(position.X-window.Width/2),(int)(position.Y-window.Height/2));
           
            //Rectangle rect = new Rectangle((int)(position.X - window.Width / 2),(int)( position.Y - window.Height / 2), window.Width / 2, window.Height);
            window = Collider.KeepInGameWorld(window, Game1.BattleFieldRect);
           
            position=Game1.PointToVector(window.Center);

           
 
        }


  
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D terrain;
        Texture2D radar;
        Texture2D radar_mask;
        Texture2D clouds;

        Vector2 cloudPos = Vector2.Zero;
        Vector2 cloudVel = new Vector2(1,1);


        Vector2 pos=Vector2.Zero;
        Vector2 scroll=Vector2.Zero;
        Camera camera=new Camera();
        List<Sprite> sprites=new List<Sprite>();

        bool stop = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        static Rectangle battleFieldRect;

        public static Rectangle BattleFieldRect
        {
            get { return Game1.battleFieldRect; }
            set { Game1.battleFieldRect = value; }
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            terrain = Content.Load<Texture2D>("terrain");
            radar = Content.Load<Texture2D>("radar screen");
            radar_mask = Content.Load<Texture2D>("radar screen mask"); 
            clouds = Content.Load<Texture2D>("clouds4");
            Sprite.Texture= Content.Load<Texture2D>("box");
            BattleFieldRect = new Rectangle(0,0,terrain.Width, terrain.Height);
            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                Sprite s = new Sprite();
                s.SetRandomPosition(r,BattleFieldRect);
                s.SetRandomVelocity(r);
                sprites.Add(s);
            }

           
            camera.Position=Vector2.Zero;
            camera.Window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            camera.Speed=10;


        
            // TODO: use this.Content to load your game content here
        }

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                stop = true;
            }
            else 
                stop = false;
               

            camera.Update(gameTime);
            foreach(Sprite s in sprites)
                s.Update(gameTime, Game1.BattleFieldRect);

            cloudPos += cloudVel;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Use this one instead!
            if (stop)
            {
                stop = false;
            }
            int vpw = graphics.GraphicsDevice.Viewport.Width;
            int vph = graphics.GraphicsDevice.Viewport.Height;

            Rectangle rect = new Rectangle(0, 0, vpw, vph);

            spriteBatch.Begin();
            
            Rectangle terrainRect=new Rectangle(
                    (int)(camera.Position.X-vpw/2), 
                    (int)(camera.Position.Y-vph/2),
                    vpw, 
                    vph);
            spriteBatch.Draw(terrain, Vector2.Zero, 

                terrainRect, Color.White);
            spriteBatch.End();


            Vector3 cameraPos = new Vector3(-camera.Position.X, -camera.Position.Y, 0);
            Matrix cameraTrans = Matrix.CreateTranslation(cameraPos);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,cameraTrans);
            foreach(Sprite s in sprites)
                s.Draw(gameTime,spriteBatch, true);
            spriteBatch.End();


           


        


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, null, null);


            spriteBatch.Draw(clouds,
                new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                    graphics.GraphicsDevice.Viewport.Height),
                 new Rectangle((int)cloudPos.X, (int)cloudPos.Y, 300,300),
                Color.White);
            spriteBatch.Draw(clouds,
                new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                    graphics.GraphicsDevice.Viewport.Height),
                    new Rectangle((int)cloudPos.X+100, (int)cloudPos.Y+200, 300, 300),
                    Color.White);

            spriteBatch.End();


            Matrix radarTrans = Matrix.CreateScale(0.04f);
            Matrix radarScreenTrans = Matrix.CreateScale(0.3f);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, radarScreenTrans);
            spriteBatch.Draw(radar, Vector2.Zero, Color.White);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, radarTrans);
            foreach (Sprite s in sprites)
                s.Draw(gameTime, spriteBatch, true);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, radarScreenTrans);
            spriteBatch.Draw(radar_mask, Vector2.Zero, Color.White);
            spriteBatch.End();

            
            

           

            base.Draw(gameTime);
        }

        internal static Vector2 PointToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
