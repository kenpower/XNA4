using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SAT_Collision_Demo
{
    class Triangle : Shape
    {



        public Triangle(int height, Vector2 pos, float angularVelocity, Vector2 velocity)
            : base(pos, angularVelocity, velocity)
        {

            _ang_velocity = angularVelocity;


            _velocity = velocity;


            float halfh = height / 2;
            float w = (float)(height / Math.Tan(MathHelper.Pi / 3));
            _position = pos;



            _points = new Vector2[3];

            _points[0] = new Vector2(0, halfh); //top
            _points[1] = new Vector2(-w, -halfh);//left
            _points[2] = new Vector2(+w, -halfh);//right




        }




        protected override List<Vector2> GetAxes()
        {
            List<Vector2> axes = new List<Vector2>();

            Vector2 edge, axis;

            //for a traingle we  need three axes
            edge = _points[1] - _points[0];
            axes.Add(new Vector2(edge.Y, -edge.X));
            

            edge = _points[2] - _points[1];
            axes.Add(new Vector2(edge.Y, -edge.X));
            

            edge = _points[0] - _points[2];
            axes.Add(new Vector2(edge.Y, -edge.X));
            

            return axes;

        }
    }
       
}
