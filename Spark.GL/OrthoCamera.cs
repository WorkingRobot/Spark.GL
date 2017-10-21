using OpenTK;

namespace Spark.GL
{
    public class OrthoCamera : Camera
    {
		public float width, height;
        public OrthoCamera(float clipNear, float clipFar, float width, float height)
        {
            this.clipNear = clipNear;
            this.clipFar = clipFar;
            this.width = width;
			this.height = height;
        }
        public override Matrix4 CreateView(int width, int height)
        {
            return Matrix4.CreateOrthographic(this.width, this.height, clipNear, clipFar);
        }
    }
}