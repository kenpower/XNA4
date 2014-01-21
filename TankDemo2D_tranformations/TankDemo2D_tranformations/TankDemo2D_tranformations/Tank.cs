using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TankDemo2D_tranformations
{
    class Tank: Sprite
    {
        public Turret turret;

        

        public Tank()
        {
            scale = 0.5f;
            MaxSpeed = 3;
        }

        public float bulletRotation
        {
            get { return rotation+turret.rotation; }
            
        }


        public new void Update(GameTime gameTime, Rectangle game_bounds)
        {
            KeyboardState kbs = Keyboard.GetState();

            Vector3 dir = Direction;
            velocity = Vector3.Zero;

            if (kbs.IsKeyDown(Keys.Up))
            {
                velocity += dir * MaxSpeed;
            }

            if (kbs.IsKeyDown(Keys.Down))
            {
                velocity += dir * MaxSpeed*-1;
            }

            if (kbs.IsKeyDown(Keys.Left))
            {
                rotation -= MaxTurnSpeed;
            }

            if (kbs.IsKeyDown(Keys.Right))
            {
                rotation += MaxTurnSpeed;
            }


           

            base.Update(gameTime,  game_bounds);

           

        }

        
    }
}
