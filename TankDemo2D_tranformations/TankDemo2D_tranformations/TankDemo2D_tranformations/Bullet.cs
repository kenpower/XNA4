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
    class Bullet:Sprite
    {
       

        public void Update(GameTime gameTime, Rectangle game_bounds)
        {


            velocity = Direction * MaxSpeed;

            base.Update(gameTime, game_bounds);
            
            World = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation + MathHelper.PiOver2) * Matrix.CreateTranslation(position.X, position.Y, 0);

        }

        public void Draw(Matrix cameraMatrix)
        {
            
            base.Draw(cameraMatrix);

          

        }
    }
}
