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

        public  void Update(GameTime gameTime)
        {
            // Allows the game to exit
            position += velocity;

            position = Game1.WrapAround(position);
        }

        public void Draw(GameTime gt, SpriteBatch sb, bool red)
        {
            if(red)
                sb.Draw(texture, position,Color.Red);
            else
                sb.Draw(texture, position, Color.White);
          
        }

        internal void SetRandomPosition(Random r,Vector2 limits)
        {
           position=new Vector2(r.Next()%limits.X,r.Next()%limits.Y);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Y-=speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Y += speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X += speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X -= speed;

            position = Game1.WrapAround(position);
           
 
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

        static Vector2 BattleFieldSize;

        public static Vector2 WrapAround(Vector2 v)
        {
            if (v.X < 0)
                v.X = BattleFieldSize.X + v.X;
            if (v.X > BattleFieldSize.X)
                v.X -= BattleFieldSize.X;
            if (v.Y < 0)
                v.Y = BattleFieldSize.Y + v.Y;
            if (v.Y > BattleFieldSize.Y)
                v.Y -= BattleFieldSize.Y;

            return v;
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
            Sprite.Texture= Content.Load<Texture2D>("box");
            BattleFieldSize = new Vector2(terrain.Width, terrain.Height);
            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                Sprite s = new Sprite();
                s.SetRandomPosition(r,BattleFieldSize);
                s.SetRandomVelocity(r);
                sprites.Add(s);
            }

           
            camera.Position=Vector2.Zero;
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
                s.Update(gameTime);

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

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            

            spriteBatch.Draw(terrain, pos, 
                new Rectangle((int)(camera.Position.X-vpw/2), (int)(camera.Position.Y-vph/2),
                     graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();


            Vector3 cameraPos = new Vector3(-camera.Position.X, -camera.Position.Y, 0);
            Matrix cameraTrans = Matrix.CreateTranslation(cameraPos);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,cameraTrans);
            foreach(Sprite s in sprites)
                s.Draw(gameTime,spriteBatch, true);
            spriteBatch.End();
            
            
            bool adjust=false;
            if (camera.Position.X > BattleFieldSize.X - vpw)
            {

                Vector3 c = cameraPos;
                c.X = BattleFieldSize.X - camera.Position.X;
                cameraTrans = Matrix.CreateTranslation(c);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cameraTrans);
                foreach (Sprite s in sprites)
                {
                    if (s.Position.X < vpw)
                    {
                        s.Draw(gameTime, spriteBatch, false);
                    }
                }
                spriteBatch.End();
            }
            if (camera.Position.Y > BattleFieldSize.Y - vph)
            {
                Vector3 c = cameraPos;
                c.X = BattleFieldSize.Y - camera.Position.Y;
                cameraTrans = Matrix.CreateTranslation(c);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cameraTrans);
                foreach (Sprite s in sprites)
                {
                    if (s.Position.Y < vph)
                    {
                        s.Draw(gameTime, spriteBatch, false);
                    }
                }
                spriteBatch.End();
     
            }

            if (camera.Position.Y > BattleFieldSize.Y - vph && camera.Position.X > BattleFieldSize.X - vpw)
            {
                Vector3 c = cameraPos;
                c.X = BattleFieldSize.X - camera.Position.X;
                c.Y = BattleFieldSize.Y - camera.Position.Y;
                cameraTrans = Matrix.CreateTranslation(c);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cameraTrans);
                foreach (Sprite s in sprites)
                {
                    if (s.Position.X < vpw && s.Position.Y < vph)
                    {
                        s.Draw(gameTime, spriteBatch, false);
                    }
                }
                spriteBatch.End();

            }

            base.Draw(gameTime);
        }
    }
}
