using OpenTK;
using System;

namespace Spark.GL
{
    public class MeshRenderer : Component
    {
        MeshFilter f;
        internal Mesh mesh { get { return f.mesh; } }
        internal Matrix4 ModelMatrix;
        internal Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        internal Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;
        public override void Awake()
        {
            f = gameObject.GetComponent<MeshFilter>();
            if (f == null)
            {
                throw new MissingMemberException(gameObject.Name+" requires a MeshFilter object to render");
            }
        }

        public void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(gameObject.transform.Scale) * Matrix4.CreateRotationX(gameObject.transform.Rotation.X) * Matrix4.CreateRotationY(gameObject.transform.Rotation.Y) * Matrix4.CreateRotationZ(gameObject.transform.Rotation.Z) * Matrix4.CreateTranslation(gameObject.transform.Position);
        }
    }
}
