using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TankDemo2D_tranformations
{
    class Enemy:Tank
    {
        Random random;
        float target_rot;
       
        bool moving = true;
        //Texture2D tank, turret;
      
        public void Init(Random r, Rectangle bounds)
        {
            bounds.Inflate(-boundingRadius, -boundingRadius);

            position.X = (float)(bounds.Left + bounds.Width * r.NextDouble());
            position.Y = (float)(bounds.Top + bounds.Height * r.NextDouble());

            rotation = (float)(r.NextDouble() * MathHelper.TwoPi);

            random = r;
        }


        public  void Update(GameTime gameTime, Rectangle game_bounds, Vector3 drive_to)
        {
            
           /* 
            Vector2 tank_dir = new Vector2(1, 0);
            Matrix rot = Matrix.CreateRotationZ(rotation);
            tank_dir = Vector2.Transform(tank_dir, rot);


            game_bounds.Inflate(-boundingRadius, -boundingRadius);

            
           

            Vector2 oldtankpos = position;
            position.X = MathHelper.Clamp(position.X, game_bounds.Left, game_bounds.Right);
            position.Y = MathHelper.Clamp(position.Y, game_bounds.Top, game_bounds.Bottom);


            //bool touching_edge = !game_bounds.Contains((int)tank_pos.X, (int)tank_pos.Y);
            bool touching_edge = (!oldtankpos.Equals(position));

            //target_dir = 0;
            if (Vector2.Distance(position, drive_to) < tank_radius * 2)
            {
                moving = false;
            }
            if (Vector2.Distance(position, drive_to) < tank_radius * 4) //close to tank turn towards it
            {
                Vector2 displacement = Vector2.Subtract(drive_to, position);
                double angle = Math.Atan2(displacement.Y, displacement.X);
                target_rot = (float)angle;
                
            }
            else if (touching_edge)
                {

                    if (rotation < MathHelper.Pi)
                        target_rot += MathHelper.PiOver2;
                    else
                        target_rot -= MathHelper.PiOver2;
                    
                   

                    
                }
            else// do a random turn
            {
                moving = true;
                int do_turn = random.Next() % 1000;

                if (do_turn <= 5)
                {
                    target_rot = (float)(MathHelper.Pi * random.NextDouble())*MathHelper.TwoPi;
                    
                }
                
            }

            if (Math.Abs(target_rot - rotation) > 0.1) //turn_speed towards target rotation
            {
                int turn_dir=0;
                if (rotation < target_rot) 
                    turn_dir = +1;
                else
                     turn_dir = -1;
               

                float turn = MaxTurnSpeed * 4;
                rotation += turn_dir * turn;
                moving = false;

               
            }
            else{
                target_rot = rotation;
               
               
            }

           


            if(moving)
                    position = position + tank_dir * speed*0.9f;

            //tank_pos.X = MathHelper.Clamp(tank_pos.X, game_bounds.Left, game_bounds.Right);
            //tank_pos.Y = MathHelper.Clamp(tank_pos.Y, game_bounds.Top, game_bounds.Bottom);
           */
        }
    }
}
