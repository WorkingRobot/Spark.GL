using OpenTK;

namespace Spark.GL
{
    public class PerspectiveCamera : Camera
    {
        public float FOV;
        public PerspectiveCamera(float clipNear, float clipFar, float FOV)
        {
            this.clipNear = clipNear;
            this.clipFar = clipFar;
            this.FOV = FOV;
        }
        public override Matrix4 CreateView(int width, int height)
        {
            return Matrix4.CreatePerspectiveFieldOfView(FOV, (float)width / height, clipNear, clipFar);
        }
    }
}