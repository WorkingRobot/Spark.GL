using OpenTK;

namespace Spark.GL
{
    class Light
    {
        public Light(Vec3 position, Vec3 color, float diffuseintensity = 1.0f, float ambientintensity = 1.0f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;

            Type = LightType.Point;
            Direction = new Vec3(0, 0, 1);
            ConeAngle = 15.0f;
        }

        public Vec3 Position
        {
            get { return new Vec3(position); }
            set { position = value; }
        }
        public Vec3 Color
        {
            get { return new Vec3(color); }
            set { color = value; }
        }
        public float DiffuseIntensity;
        public float AmbientIntensity;

        public LightType Type;
        public Vec3 Direction
        {
            get { return new Vec3(direction); }
            set { direction = value; }
        }
        public float ConeAngle;

        internal Vector3 position;
        internal Vector3 color;
        internal Vector3 direction;
    }

    enum LightType { Point, Spot, Directional }
}
