using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace RedStream
{
    public class RsPhysics
    {
        private int fel;
        public struct TracePair
        {
            public RsGameObject obj;
            public float distance;
        };
        public struct Cone
        {
            public Vector3 Pos, Axis;
            public float Angle;
        }
        public static bool Collides(RsGameObject obj_1, RsGameObject obj_2)
        {
            bool bCollided;
            obj_1.BoundingSphereOnlyForAsenicsUse.Intersects(ref obj_2.BoundingSphereOnlyForAsenicsUse, out bCollided);
            return bCollided;
        }
        public static Vector3 GetCollisionPoint(RsGameObject obj_1, RsGameObject obj_2)
        {
            BoundingSphere s1 = obj_1.BoundingSphereOnlyForAsenicsUse, s2 = obj_2.BoundingSphereOnlyForAsenicsUse;
            Vector3 diff = s2.Center - s1.Center;
            Vector3 dir = diff;
            dir.Normalize();
            float length=diff.Length();
            if(length!=0.0f)
                diff*=(s1.Radius+s2.Radius-length)/length;
            length = diff.Length();
            dir *= s1.Radius - length / 2;
            return dir + s1.Center;
        }
        public static void CollideObjects(float time)
        {
            IEnumerable<RsGameObject> gameObjects =
                from obj in RedStream.Game.Components
                where obj is RsGameObject
                select (RsGameObject)obj;
            RsGameObject[] objects=gameObjects.ToArray<RsGameObject>();
            
            for (int i = 0; i < objects.Length; i++)
            {
                RsGameObject obj1 = objects[i];
                for (int j = i + 1; j < objects.Length; j++)
                {
                    RsGameObject obj2 = objects[j];
                    if (Collides(obj1, obj2))
                    {
                        obj1.ReactToCollision(obj2);
                        obj2.ReactToCollision(obj1);
                    }
                }
            }
            foreach (RsGameObject obj in gameObjects)
                obj.Advance(time);

        }
        public static float? RayTrace(RsGameObject obj, Ray ray)
        {
            return ray.Intersects(obj.BoundingSphereOnlyForAsenicsUse);
        }
        /*public static TracePair[] TraceAll(Ray ray)
        {
            int nElements = 0, nMaxElements=5;;
            TracePair[] pairs = new TracePair[nMaxElements];
            IEnumerable<RsGameObject> objects =
                from obj in RedStream.Game.Components
                where obj is RsGameObject
                select (RsGameObject)obj;
            //get all objects that intersect
            foreach (RsGameObject obj in objects)
            {
                float? dist = RayTrace(obj, ray);
                if (dist != null)
                {
                    if (nElements >= nMaxElements)
                        Array.Resize<TracePair>(ref pairs, nMaxElements += 5);
                    pairs[nElements].distance = (float)dist;
                    pairs[nElements].obj = obj;
                    nElements++;
                }
            }
            //trim the array
            Array.Resize<TracePair>(ref pairs, nElements);
            //sort them from closest to farthest
            for(int i=0;i<pairs.Length;i++)

        }*/
        public static TracePair TraceClosest(Ray ray)
        {
            IEnumerable<RsGameObject> objects =
                from obj in RedStream.Game.Components
                where obj is RsGameObject
                select (RsGameObject)obj;
            TracePair pair=new TracePair();
            pair.distance=RedStream.Scene.Camera.FarViewPlane;
            pair.obj = null;
            foreach (RsGameObject obj in objects)
            {
                float? dist = RayTrace(obj, ray);
                if (dist != null && (float)dist < pair.distance)
                {
                    pair.distance = (float)dist;
                    pair.obj = obj;
                }
            }
            return pair;
        }
        //cone sphere collision
        public static bool Collides(BoundingSphere s, Cone c)
        {
            float angle = MathHelper.ToRadians(c.Angle);
            float sin = (float)Math.Sin(angle), cos = (float)Math.Cos(angle);
            Vector3 U = c.Pos - (s.Radius / sin) * c.Axis;
            Vector3 D = s.Center - U;
            if (Vector3.Dot(c.Axis, D) >= D.Length() * cos)
            {
                D = s.Center - c.Pos;
                if (-Vector3.Dot(c.Axis, D) >= D.Length() * sin)
                    return D.Length() <= s.Radius;
                else return true;
            }
            else return false;
        }
    }
}
