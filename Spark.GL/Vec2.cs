using OpenTK;

namespace Spark.GL
{
    public class Vec2
    {
        public float X;
        public float Y;

        internal Vector2 vector2 { get { return new Vector2(X, Y); } }

        public Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vec2(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }
        public Vec2()
        {
            X = Y = 0;
        }
        public static implicit operator Vector2(Vec2 d) => d.vector2;
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
    }
}