using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TankDemo2D_tranformations
{
    class Turret:Sprite
    {
        public Tank tank;

       

        public new void Update(GameTime gameTime, Rectangle game_bounds)
        {
            scale = 0.5f;

            KeyboardState kbs = Keyboard.GetState();

            if (kbs.IsKeyDown(Keys.X))
            {
               rotation += MaxTurnSpeed;
            }

            if (kbs.IsKeyDown(Keys.Z))
            {
                rotation -= MaxTurnSpeed;
            }

            position = tank.position;

            base.Update(gameTime, game_bounds);
        }
    }
}
