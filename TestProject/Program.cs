using Spark.GL;
using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Window wind = new Window(600, 600, "Hey");
            Console.WriteLine("waking");
            wind.Wake();
            Console.WriteLine("getting materials");
            wind.loadMaterials("opentk.mtl");
            Camera camera = new PerspectiveCamera(0.1f, 1000, 1.3f);
            camera.Position += new Vec3(0, 0, 0);
            wind.SetCamera(camera);
            GameObject circle = new GameObject("floor");
            circle.transform.Position = new Vec3(0, 0, 0);
            var t = circle.AddComponent<CameraRotationThing>();
            t.w = wind;
            t.cam = camera;
            circle.AddComponent<MeshCollider>();
            MeshFilter filter = circle.AddComponent<MeshFilter>();
            filter.mesh = Mesh.Cube;
            wind.materials["opentk2"].TextureID = 2;
            filter.material = wind.materials["opentk1"];
            circle.transform.Scale = new Vec3(20f, 1f, 20f);
            MeshRenderer renderer = circle.AddComponent<MeshRenderer>();
            circle = new GameObject("banana");
            circle.transform.Position = new Vec3(0, 20, 0);
            //circle.AddComponent<Rigidbody>();
            circle.AddComponent<MeshCollider>();
            filter = circle.AddComponent<MeshFilter>();
            filter.mesh = Mesh.Cube;
            wind.materials["opentk1"].TextureID = 2;
            filter.material = wind.materials["opentk1"];
            circle.transform.Scale = new Vec3(1, 1, 1);
            renderer = circle.AddComponent<MeshRenderer>();
            Console.WriteLine("runnng");
            wind.Run();
        }
    }

    public class CameraRotationThing : Component
    {
        public Window w;
        public Camera cam;
        public Vec2 lastmousepos;
        int t;
        const float speed = 0.01f;
        const float rotspeed = 0.005f;
        public override void Load()
        {
            var a = w.GetMouseInput();
            a.ResetMousePosition();
            a = w.GetMouseInput();
            lastmousepos = new Vec2(a.PosX, a.PosY);
        }
        public override void Update()
        {
            var ki = w.GetKeyboardInput();
            Vec3 pos = new Vec3();
            pos.Z += ki.KeyDown(Key.W) ? speed : 0;
            pos.Z -= ki.KeyDown(Key.S) ? speed : 0;
            pos.Y += ki.KeyDown(Key.Space) ? speed : 0;
            pos.Y -= ki.KeyDown(Key.ShiftLeft) ? speed : 0;
            pos.X += ki.KeyDown(Key.D) ? speed : 0;
            pos.X -= ki.KeyDown(Key.A) ? speed : 0;
            cam.Move(pos.X,pos.Y,pos.Z);
            var mi = w.GetMouseInput();
            Vec2 rot = new Vec2(mi.PosX, mi.PosY);
            cam.AddRotation((lastmousepos.X-rot.X) * rotspeed, (lastmousepos.Y - rot.Y) * rotspeed);
            mi.ResetMousePosition();
            //lastmousepos = rot;
            t++;
        }
    }
}
