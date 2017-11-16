namespace Spark.GL
{
    public class Transform
    {
        public Vec3 Position;
        public Vec3 Rotation;
        public Vec3 Scale;
        
        public Transform(Vec3 pos, Vec3 rot, Vec3 scale)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
        public Transform()
        {
            Position = new Vec3();
            Rotation = new Vec3();
            Scale = new Vec3(1,1,1);
        }
    }
}