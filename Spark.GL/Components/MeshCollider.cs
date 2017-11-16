using System;
using System.Diagnostics;

namespace Spark.GL
{
    public class MeshCollider : Component
    {
        MeshFilter f;
        internal Mesh mesh { get { return f.mesh; } }

        public override void Load()
        {
            f = gameObject.GetComponent<MeshFilter>();
            if (f == null)
            {
                throw new MissingMemberException(gameObject.Name + " requires a MeshFilter object to use");
            }
        }

        public bool IsCollision(GameObject qo)
        {
            Mesh q = qo.GetComponent<MeshFilter>().mesh;
            Vec3 pPos = (mesh.Center * gameObject.transform.Scale) + gameObject.transform.Position;
            Vec3 qPos = (q.Center * qo.transform.Scale) + qo.transform.Position;
            Vec3 pSiz = mesh.Size * gameObject.transform.Scale;
            Vec3 qSiz = q.Size * qo.transform.Scale;
            if (Math.Abs(pPos.X - qPos.X) < pSiz.X + qSiz.X)
            {
                if (Math.Abs(pPos.Y - qPos.Y) < pSiz.Y + qSiz.Y)
                {
                    if (Math.Abs(pPos.Z - qPos.Z) < pSiz.Z + qSiz.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Update() {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null) return;
            rb.RBUpdate();
            foreach (GameObject o in window.GetAllObjects()){
                if (o == gameObject) continue;
                MeshCollider mc = o.GetComponent<MeshCollider>();
                if (mc == null) continue;
                if (mc.IsCollision(gameObject))
                {
                    Console.WriteLine("collision");
                    rb.velocity.Y = (gameObject.transform.Position.Y+mesh.Size.Y)-(o.transform.Position.Y+mc.mesh.Size.Y);
                }
            }
            rb.RBUpdate2();
        }
    }
}
