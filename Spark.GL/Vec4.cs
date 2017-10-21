using OpenTK;

namespace Spark.GL
{
    public class Vec4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        internal Vector4 vector4 { get { return new Vector4(X, Y, Z, W); } }

        public Vec4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vec4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vec4(double x, double y, double z, double w)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
            W = (float)w;
        }
        public Vec4()
        {
            X = Y = Z = W = 0;
        }
        public static implicit operator Vector4(Vec4 d) => d.vector4;
        public static Vec4 operator +(Vec4 a, Vec4 b) => new Vec4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Vec4 operator -(Vec4 a, Vec4 b) => new Vec4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Vec4 operator *(Vec4 a, Vec4 b) => new Vec4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        public static Vec4 operator /(Vec4 a, Vec4 b) => new Vec4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    }
}