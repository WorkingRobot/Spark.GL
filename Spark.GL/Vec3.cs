namespace Spark.GL
{
    internal sealed class Vec3
    {
        public double X;
        public double Y;
        public double Z;

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
            X = x;
            Y = y;
            Z = z;
        }
    }
}