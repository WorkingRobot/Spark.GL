using System;
using OpenTK;

namespace Spark.GL
{
    public class Vec3
    {
        public float X;
        public float Y;
        public float Z;

        internal Vector3 vector3 { get { return new Vector3(X, Y, Z); } }

        public Vec3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }
        public Vec3()
        {
            X = Y = Z = 0;
        }
        internal Vec3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static implicit operator Vector3(Vec3 d) => d.vector3;
        public static implicit operator string(Vec3 d) => String.Format("{0}, {1}, {2}",d.X,d.Y,d.Z);
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
}