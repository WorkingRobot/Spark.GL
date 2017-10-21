using Spark.GL;
using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Window wind = new Window(600, 600, "Hey");
            Camera camera = new PerspectiveCamera(0.1f, 40, 1.3f);
            camera.Position += new Vec3(0, 0, -2);
            wind.SetCamera(camera);
            GameObject circle = new GameObject("circle");
            circle.transform.Position = new Vec3(0, 0, 1);
            var t = circle.AddComponent<CameraRotationThing>();
            t.w = wind;
            t.cam = camera;
            MeshFilter filter = circle.AddComponent<MeshFilter>();
            filter.mesh = new Mesh("cow.obj");
            MeshRenderer renderer = circle.AddComponent<MeshRenderer>();
            wind.AddObject(circle);
            wind.Start();
        }
    }

    public class CameraRotationThing : Component
    {
        public Window w;
        public Camera cam;
        public Vec2 lastmousepos;
        int t;
        public override void Awake()
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
            pos.Z += ki.KeyDown(Key.W) ? 0.02f : 0;
            pos.Z -= ki.KeyDown(Key.S) ? 0.02f : 0;
            pos.Y += ki.KeyDown(Key.Space) ? 0.02f : 0;
            pos.Y -= ki.KeyDown(Key.ShiftLeft) ? 0.02f : 0;
            pos.X += ki.KeyDown(Key.D) ? 0.02f : 0;
            pos.X -= ki.KeyDown(Key.A) ? 0.02f : 0;
            cam.Move(pos.X,pos.Y,pos.Z);
            var mi = w.GetMouseInput();
            Vec2 rot = new Vec2(mi.PosX, mi.PosY);
            cam.AddRotation((lastmousepos.X-rot.X) * 0.01f, (lastmousepos.Y - rot.Y) * 0.01f);
            Console.WriteLine(mi.PosX+", "+mi.PosY);
            mi.ResetMousePosition();
            //lastmousepos = rot;
            t++;
        }
    }
}
