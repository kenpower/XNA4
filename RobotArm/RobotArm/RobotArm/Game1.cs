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

namespace RobotArm
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D body, upperArm, lowerArm, hand;
        Vector3 bodyPos;
        Vector3 bodyJointPos, upperarmJointPos, lowerArmJointPos;
        float upperArmAngle, lowerArmAngle, handAngle = -MathHelper.PiOver2;
        Matrix upperArmOrigin, lowerArmOrigin, handOrigin;
        Matrix camera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 600;

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

            body=Content.Load<Texture2D>("body");
            upperArm=Content.Load<Texture2D>("lowerarm");
            lowerArm=Content.Load<Texture2D>("upperarm");
            hand=Content.Load<Texture2D>("hand");


            bodyJointPos=new Vector3(452,209,0);

            // see the bodyx,lowerarmx, upperarmx sprites to see what the values below represent
            upperArmOrigin = Matrix.CreateTranslation(-27,-403,-0);// upper arm rotates around this point, without this the upper arm rotates around top-left of sprite
            upperarmJointPos = new Vector3(376, 87, 0); //this is the position within the sprite that the lower arm is connected to


            lowerArmOrigin = Matrix.CreateTranslation(-33, -88, 0);// lower arm rotates around this point
            lowerArmJointPos = new Vector3(27, 428, 0);

            handOrigin = Matrix.CreateTranslation(-167, -71, 0);// hand  rotates around this point

            bodyPos = new Vector3(100, 200,0);

            camera = Matrix.CreateScale(0.5f); //zoom out
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
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here


            float angleInc=0.03f;
            float maxSpeed = 10;

            if (kb.IsKeyDown(Keys.Left))
            {
                bodyPos.X+=maxSpeed; 
            }
            if (kb.IsKeyDown(Keys.Right))
            {
                bodyPos.X -= maxSpeed; 
            }

            if (kb.IsKeyDown(Keys.A))
            {
                handAngle += angleInc;
            }
            if (kb.IsKeyDown(Keys.S))
            {
                handAngle -= angleInc;
            }

            if (kb.IsKeyDown(Keys.Q))
            {
                upperArmAngle += angleInc;
            }
            if (kb.IsKeyDown(Keys.W))
            {
                upperArmAngle -= angleInc;
            }

            if (kb.IsKeyDown(Keys.Z))
            {
                lowerArmAngle += angleInc;
            }
            if (kb.IsKeyDown(Keys.X))
            {
                lowerArmAngle -= angleInc;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

           
            Matrix bodyTransform = Matrix.CreateTranslation(bodyPos)  * camera;
            Matrix upperArmTransform = upperArmOrigin * Matrix.CreateRotationZ(upperArmAngle) * Matrix.CreateTranslation(bodyJointPos) * bodyTransform;
            Matrix lowerArmTransform = lowerArmOrigin * Matrix.CreateRotationZ(lowerArmAngle) * Matrix.CreateTranslation(upperarmJointPos) * upperArmTransform;
            Matrix handTransform = handOrigin * Matrix.CreateRotationZ(handAngle) * Matrix.CreateTranslation(lowerArmJointPos) * lowerArmTransform;

      
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, bodyTransform);
            spriteBatch.Draw(body, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, upperArmTransform);
            spriteBatch.Draw(upperArm, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, lowerArmTransform);
            spriteBatch.Draw(lowerArm, Vector2.Zero,  Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, handTransform);
            spriteBatch.Draw(hand, Vector2.Zero,  Color.White);
            spriteBatch.End();

  





            base.Draw(gameTime);
        }
    }
}
