using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Camera
    {
        Matrix proj;
        Game game;
        float aspectRatio;
        bool orthoView;

        public Camera(Game theGame)
        {
            game = theGame;
        }
        public Matrix Proj
        {
            get { return proj; }
            set { proj = value; }
        }
        Matrix view;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public void Init()
        {
            GraphicsDeviceManager gdm = (GraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));
            GraphicsDevice graphics = gdm.GraphicsDevice;
            aspectRatio=graphics.Viewport.AspectRatio;
            oldState = Keyboard.GetState();
            orthoView=true;
        }

        KeyboardState oldState;
        public void Update()
        {
            KeyboardState ks=Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Tab) && oldState.IsKeyUp(Keys.Tab))
            {
                orthoView = !orthoView;
            }
            if(orthoView){
                float width = 30000;
                Proj = Matrix.CreateOrthographic(width,width/aspectRatio,1,100000);
            
                View = Matrix.CreateLookAt(new Vector3(0, 10000, -7000), new Vector3(0,0,-7000), Vector3.Forward);
            }

            else{// perspective view
                Proj = Matrix.CreatePerspectiveFieldOfView(1, aspectRatio, 1, 100000);
            
                View = Matrix.CreateLookAt(new Vector3(0, 2500, 5000), Vector3.Zero+new Vector3(0,0,-5000), Vector3.Up);
            }

            oldState = ks;

        }
    }
}
