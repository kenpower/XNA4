using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SAT_Collision_Demo
{
    class Box: Shape
    {

        int width, height;


        public Box(int w, int h, Vector2 pos, float angularVelocity,Vector2 velocity):base(pos, angularVelocity,velocity){
            
            _ang_velocity=angularVelocity;
            width = w;
            height = h;
           
            _velocity = velocity;
        
            float halfw=width/2;
            float halfh=height/2;
            _position=pos;
            //texture_centre = new Vector2(10, 10);


            _points = new Vector2[4];

            _points[0] = new Vector2(-halfw, halfh);
            _points[1] = new Vector2(halfw, halfh);
            _points[2] = new Vector2(halfw, -halfh);
            _points[3] = new Vector2(-halfw, -halfh);
        


        }

        //public void Update(){
        //    _angle+=_ang_velocity;
        //    //rotate the box
        //    Matrix rotation = Matrix.CreateRotationZ(_ang_velocity);
        //    for (int i = 0; i <_points.Length; i++)
        //    {
        //        _points[i] = Vector2.Transform(_points[i], rotation);
        //    }

        //    _position += _velocity;
        //}

        //public void Draw(SpriteBatch sb)
        //{  
        //    Vector2 p=_position;
        //    sb.Draw(_texture, new Rectangle((int)p.X, (int)p.Y, width, height), null, Color.White, _angle, texture_centre, SpriteEffects.None, 0);

        //}


        protected override List<Vector2> GetAxes() { 
            List<Vector2> axes=new List<Vector2>();

            Vector2 edge,axis;
            
            //for a box we only need two axes
            edge= _points[1] - _points[0];
            axis = new Vector2(edge.Y, -edge.X);
            axes.Add(axis);
            
            edge = _points[2] - _points[1];
            axis = new Vector2(edge.Y, -edge.X);
            axes.Add(axis);
            
            return axes;

        }
        //public bool CheckCollision(Box b) {// return true if collision has occured

        //    Vector2 edge; 
        //    Vector2 axis;

        //    edge= _points[1] - _points[0];
        //    axis = new Vector2(edge.Y, -edge.X);

        //    if(testForOverlapOnAxis(axis, b)==false)
        //        return false;

        //    edge = _points[2] - _points[1];
        //    axis = new Vector2(edge.Y, -edge.X);

        //    if (testForOverlapOnAxis(axis, b) == false)
        //        return false;

        //    edge = b._points[1] - b._points[0];
        //    axis = new Vector2(edge.Y, -edge.X);

        //    if (testForOverlapOnAxis(axis, b) == false)
        //        return false;

        //    edge = b._points[2] - b._points[1];
        //    axis = new Vector2(edge.Y, -edge.X);

        //    if (testForOverlapOnAxis(axis, b) == false)
        //        return false;

        //    return true;
            
        //}

        //bool testForOverlapOnAxis(Vector2 axis, Box b) {

        //    float min=float.MaxValue;
        //    float max=float.MinValue;

        //    for (int i = 0; i < _points.Length; i++)
        //    {// project this object
        //        float proj=Vector2.Dot(axis,_position+_points[i]);
        //        if(proj<min)min=proj;
        //        if(proj>max)max=proj;
                
        //    }

        //    for (int i = 0; i < b._points.Length; i++)
        //    {// project other object
        //        float proj=Vector2.Dot(axis,b._position+b._points[i]);
                
        //         if(proj>min && proj<max)// point lies inside this object's projection
        //             return true;

                
        //    }
        //    return false;
        //}



    }
}
