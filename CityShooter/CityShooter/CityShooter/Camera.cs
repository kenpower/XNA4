using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CityShooter
{
    public class Camera
    {
        Vector3 pos;
        Vector3 dir;
        Vector3 up;

        float fieldOfView;
        float aspectRatio;
        float near;
        float far;

        Matrix view;


        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        Matrix projection;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        float speed = 0.050f;
        float angVel = 0.0025f;

        BoundingSphere boundingSphere;

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
            set { boundingSphere = value; }
        }

        public void Init(Vector3 p, Vector3 lookat, Vector3 u,float FOV,float ar,float n, float f)
        {
            pos = p;
            dir = (lookat - p);
            dir.Normalize();
            up = u;

            fieldOfView = FOV;
            aspectRatio = ar;
            near = n;
            far = f;
            UpdateView();
            UpdateProj();

            boundingSphere = new BoundingSphere();
            boundingSphere.Radius = 5;

        }

        void UpdateView()
        {
            view= Matrix.CreateLookAt(pos, pos + dir, up);

            boundingSphere.Center = pos;
        }
        void UpdateProj()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio,near, far);

        }


        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.RightShift) || kb.IsKeyDown(Keys.LeftShift))
            {

                Vector3 right = Vector3.Cross(dir, up);
                right *= speed * gameTime.ElapsedGameTime.Milliseconds;

                Vector3 Up = Vector3.Up;
                Up *= speed * gameTime.ElapsedGameTime.Milliseconds;

                if (kb.IsKeyDown(Keys.Left))
                {// pan
                    pos -= right;
                }
                if (kb.IsKeyDown(Keys.Right))
                {// pan
                    pos += right;
                }
                
                if (kb.IsKeyDown(Keys.Up))
                {// pan
                    pos += up;
                }
                if (kb.IsKeyDown(Keys.Down))
                {// pan
                    pos -= Up;
                }
            }
            else
            {//no shift
                float angle = angVel * gameTime.ElapsedGameTime.Milliseconds;
                if (kb.IsKeyDown(Keys.Left))
                {// yaw
                    Matrix rot = Matrix.CreateFromAxisAngle(up, angle);
                    dir = Vector3.Transform(dir, rot);
                }
                if (kb.IsKeyDown(Keys.Right))
                {// yaw
                    Matrix rot = Matrix.CreateFromAxisAngle(up, -angle);
                    dir = Vector3.Transform(dir, rot);

                }
            }

            if (kb.IsKeyDown(Keys.Up))
            {// forward
                pos += dir * (speed * gameTime.ElapsedGameTime.Milliseconds);
            }
            if (kb.IsKeyDown(Keys.Down))
            {// Back
                pos -= dir * (speed * gameTime.ElapsedGameTime.Milliseconds);
            }

            UpdateView();
            UpdateProj();
        }





        internal void ShiftAlongX(float move)
        {
            pos.X += move;
            UpdateView();
            UpdateProj();
        }

        internal void ShiftAlongZ(float move)
        {
            pos.Z += move;
            UpdateView();
            UpdateProj();
        }

        public Vector3 Position { get { return pos; } set { pos=value;} }

        public Vector3 Direction { get { return dir; } set { dir = value; } }

        public Vector3 Up { get { return up; } set { up = value; } }
    }
}
