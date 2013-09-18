using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SAT_Collision_Demo
{
    abstract class Shape{
        protected Vector2[] _points;
        protected Vector2 _position;
        protected Vector2 _velocity;
        protected float _ang_velocity;
        protected float _angle;
        protected Color _color;

        private static Texture2D _texture;

        public static Texture2D Texture
        {
            get { return Shape._texture; }
            set { Shape._texture = value; }
        }
        
      


        public Shape(Vector2 position,  float angularVelocity, Vector2 velocity) {
            _ang_velocity=angularVelocity;
            _position =position;
            _velocity = velocity;
        

        


        }

        public void Update(Rectangle screen){
            _angle+=_ang_velocity;
            //rotate the box
            Matrix rotation = Matrix.CreateRotationZ(_ang_velocity);
            for (int i = 0; i <_points.Length; i++)
            {
                _points[i] = Vector2.Transform(_points[i], rotation);
            }

            _position += _velocity;


            foreach (Vector2 p in _points)
            {
                Vector2 point =p+ _position;

                if (point.X > screen.Right && _velocity.X > 0)
                {
                    _velocity.X *= -1;

                }
                else if (point.X < screen.Left && _velocity.X < 0)
                {
                    _velocity.X *= -1;

                }
                else if (point.Y > screen.Bottom && _velocity.Y > 0)
                {
                    _velocity.Y *= -1;

                }
                else if (point.Y < screen.Top && _velocity.Y < 0)
                {
                    _velocity.Y *= -1;

                }
            }

            

        }

        public void Draw(SpriteBatch sb)
        {  
           
            int numpoints=_points.Length;
            
            for (int i = 0; i < numpoints; i++)
            {
            
               
                DrawLine(sb, _points[i]+_position,_points[(i+1)%numpoints]+_position);
            }
        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end )
        {
            Vector2 edge = end - start;
            float angle=(float)Math.Atan(edge.Y/edge.X);
                if (edge.X < 0)
                    angle = MathHelper.Pi + angle;

            sb.Draw(_texture,
	            new Rectangle((int)start.X,(int)start.Y, (int)edge.Length() , 1), 
	            null,
	            Color.Red,
	            angle,
	            new Vector2(0, 0),
	            SpriteEffects.None,
	            0);
        }

        abstract protected List<Vector2> GetAxes();

        public static bool CheckCollision(Shape shape1, Shape shape2)
        {// return true if collision has occured

            List<Vector2> shape1axes = shape1.GetAxes();
            List<Vector2> shape2axes = shape2.GetAxes();


            foreach (Vector2 axis in shape1axes)
            {
                if (objectsSperarated(axis, shape1,shape2))
                    return false;
            }

            foreach (Vector2 axis in shape2axes)
            {
                if (objectsSperarated(axis, shape2, shape1))
                    return false;
            }
           
            return true;
            
        }

        static bool objectsSperarated(Vector2 axis, Shape shape1, Shape shape2)
        {//return true if there is a gap between the objects when projected onto axis

            float min1 = float.MaxValue;
            float max1 = float.MinValue; 
            
            float min2 = float.MaxValue;
            float max2 = float.MinValue;

            foreach(Vector2 point in shape1._points)
            {// project this object on to axis
                float proj=Vector2.Dot(axis,shape1._position+point);
                if(proj<min1)min1=proj;
                if(proj>max1)max1=proj;
                
            }

            foreach (Vector2 point in shape2._points)
            {// project other object onto axis
                float proj=Vector2.Dot(axis,shape2._position+point);
                
                 if(proj>min1 && proj<max1)// point lies inside other object's projection
                     return false;


                 if (proj < min2) min2 = proj;
                 if (proj > max2) max2 = proj;
                
            }

            //check if projection of first object completely inside of projection of 2nd object
            if(min1>min2 && min1<max2)
                return false;
            return true;
        }




        internal static void bounce(Shape shape1, Shape shape2)
        {
            Vector2 displacement = shape1._position - shape2._position;
            Vector2 closingspeed = shape1._velocity - shape2._velocity;
            
            //check if they are already moving apart
            if(Vector2.Dot(displacement,closingspeed)>=0){
                return;// if moving apart, do nothing

            }
            displacement.Normalize();

            Vector2 bounce;
            
            //reverse the velocity vector in the direction of displacement
            bounce= Vector2.Dot(shape1._velocity, displacement) * displacement;

            shape1._velocity -= 2 * bounce;

            bounce = Vector2.Dot(shape2._velocity, displacement) * displacement;

            shape2._velocity -= 2 * bounce;


        }
    }
}
