using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RedStream
{
    static class RsUtil
    {
        static Random Random;

        static RsUtil()
        {
            Random = new Random();
        }
        public static Vector3 GetRandomVector(float min, float max)
        {
            double angle1 = Random.NextDouble() * Math.PI * 2, angle2 = Random.NextDouble() * Math.PI * 2;
            Vector3 v=new Vector3();
            v.X = GetRandomFloat(min, max) * (float)Math.Cos(angle1);
            v.Y = GetRandomFloat(min, max) * (float)Math.Sin(angle1);
            v.Z = GetRandomFloat(min, max) * (float)Math.Cos(angle2);
            return v;
        }
        public static Vector3 GetRandomVector(Vector3 min, Vector3 max)
        {
            Vector3 v = new Vector3();
            v.X = GetRandomFloat(min.X, max.X);
            v.Y = GetRandomFloat(min.Y, max.Y);
            v.Z = GetRandomFloat(min.Z, max.Z);
            return v;
        }
        public static float GetRandomFloat(float min, float max)
        {
            float d = max - min;
            return min + d * (float)Random.NextDouble();
        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public static int Mod(int n, int mod)
        {
            if (mod == 1)
                return 0;
            if (n >= 0)
                return n % mod;
            else
            {
                n = -n;
                n %= mod;
                return mod - n;
            }
        }
    }
}
