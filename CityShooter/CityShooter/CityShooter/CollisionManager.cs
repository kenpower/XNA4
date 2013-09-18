using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CityShooter
{

    public class CollisionManager
    {
        public CollisionManager()
	    {
	    }

        public static void DetectCameraBuildingCollisions(Camera camera,Building building){
            if (building.BoundingBox.Max.Y < camera.BoundingSphere.Center.Y)
                return;
            if(camera.BoundingSphere.Intersects(building.BoundingBox)){
                float leftSep = building.BoundingBox.Min.X - (camera.BoundingSphere.Center.X + camera.BoundingSphere.Radius) ;
                float rightSep =building.BoundingBox.Max.X -(camera.BoundingSphere.Center.X - camera.BoundingSphere.Radius);
                float topSep =  building.BoundingBox.Max.Z - (camera.BoundingSphere.Center.Z - camera.BoundingSphere.Radius);
                float botSep =  building.BoundingBox.Min.Z -(camera.BoundingSphere.Center.Z + camera.BoundingSphere.Radius);

                float moveXaxis=rightSep;
                if(Math.Abs(leftSep)<Math.Abs(rightSep))
                    moveXaxis = leftSep;
                float moveZaxis=topSep;
                if(Math.Abs(botSep)<Math.Abs(topSep))
                    moveZaxis = botSep;

                if (Math.Abs(moveXaxis) < Math.Abs(moveZaxis))
                {
                    camera.ShiftAlongX(moveXaxis);
                }
                else
                {
                    camera.ShiftAlongZ(moveZaxis);
                }
                
            }
        }
    }
}