using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SAT_Collision_Demo
{
    class Polygon:Shape
    {
        

        public Polygon(int radius, int sides, Vector2 pos, float angularVelocity, Vector2 velocity)
            : base(pos, angularVelocity, velocity)
        {


            _points = new Vector2[sides];
            for (int i = 0; i < sides; i++)
            {
                _points[i].X = (float)(radius * Math.Cos(i * MathHelper.TwoPi / sides));
                _points[i].Y = (float)(radius * Math.Sin(i * MathHelper.TwoPi / sides));
            }




        }




        protected override List<Vector2> GetAxes()
        {
            List<Vector2> axes = new List<Vector2>();

            Vector2 edge;

            for (int i = 0; i < (_points.Length-1); i++)
            {

                edge = _points[i + 1] - _points[i];
                axes.Add(new Vector2(edge.Y, -edge.X));
            }

            edge = _points[0] - _points[_points.Length-1];// get last edge
            axes.Add(new Vector2(edge.Y, -edge.X));

            return axes;

        }
    
    }
}
