namespace Spark.GL
{
    internal sealed class Vec2
    {
        public float X;
        public float Y;

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
    }
}