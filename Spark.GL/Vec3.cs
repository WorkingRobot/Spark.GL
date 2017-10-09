namespace Spark.GL
{
    internal sealed class Vec3
    {
        public float X;
        public float Y;
        public float Z;

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
    }
}