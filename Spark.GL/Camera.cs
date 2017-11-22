using OpenTK;
using System;

namespace Spark.GL
{
    public class Camera
    {
        public Vec3 Position = new Vec3();
        //ROTATION IS IN RADS (360 = 2PI)
        public Vec3 Rotation = new Vec3();
        public float clipNear;
        public float clipFar;

        internal Camera() { }

        public virtual Matrix4 CreateView(int width, int height)
        {
            throw new NotSupportedException("This is a base class for both types of cameras. Choose either the perspective or orthographic camera.");
        }

        internal Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin(Rotation.X) * Math.Cos(Rotation.Y));
            lookat.Y = (float)Math.Sin(Rotation.Y);
            lookat.Z = (float)(Math.Cos(Rotation.X) * Math.Cos(Rotation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }
        public void Move(float x, float z, float y)
        {
            Vector3 offset = new Vector3();
            Vector3 forward = new Vector3((float)Math.Sin(Rotation.X), 0, (float)Math.Cos(Rotation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();

            Position += new Vec3(offset);
        }
        public void AddRotation(float x, float y)
        {
            Rotation.X = (Rotation.X + x) % ((float)Math.PI * 2.0f);
            Rotation.Y = Math.Max(Math.Min(Rotation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }
    }
}