using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.GL
{
    public class Mesh
    {
        public static Mesh Cube;
        public Vec3[] Vertices;
        public Vec3[] Triangles;
        public Vec3[] Colors;
        public Vec2[] Textures;

        static Mesh()
        {
            Vec3[] verts = new Vec3[]
            {
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, -1.0f, 1.0f),
                new Vec3(-1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, 1.0f, -1.0f),
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, 1.0f, -1.0f),
                new Vec3(1.0f, -1.0f, 1.0f),
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(1.0f, -1.0f, -1.0f),
                new Vec3(1.0f, 1.0f, -1.0f),
                new Vec3(1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, 1.0f, 1.0f),
                new Vec3(-1.0f, 1.0f, -1.0f),
                new Vec3(1.0f, -1.0f, 1.0f),
                new Vec3(-1.0f, -1.0f, 1.0f),
                new Vec3(-1.0f, -1.0f, -1.0f),
                new Vec3(-1.0f, 1.0f, 1.0f),
                new Vec3(-1.0f, -1.0f, 1.0f),
                new Vec3(1.0f, -1.0f, 1.0f),
                new Vec3(1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, -1.0f, -1.0f),
                new Vec3(1.0f, 1.0f, -1.0f),
                new Vec3(1.0f, -1.0f, -1.0f),
                new Vec3(1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, -1.0f, 1.0f),
                new Vec3(1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, 1.0f, -1.0f),
                new Vec3(-1.0f, 1.0f, -1.0f),
                new Vec3(1.0f, 1.0f, 1.0f),
                new Vec3(-1.0f, 1.0f, -1.0f),
                new Vec3(-1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, 1.0f, 1.0f),
                new Vec3(-1.0f, 1.0f, 1.0f),
                new Vec3(1.0f, -1.0f, 1.0f)
            };
            Vec3[] triangles = new Vec3[]
            {
                new Vec3(0,1,2),
                new Vec3(3,4,5),
                new Vec3(6,7,8),
                new Vec3(9,10,11),
                new Vec3(12,13,14),
                new Vec3(15,16,17),
                new Vec3(18,19,20),
                new Vec3(21,22,23),
                new Vec3(24,25,26),
                new Vec3(27,28,29),
                new Vec3(30,31,32),
                new Vec3(33,34,35)
            };
            Vec3[] colors = new Vec3[]
            {
                new Vec3(0.583f, 0.771f, 0.014f),
                new Vec3(0.609f, 0.115f, 0.436f),
                new Vec3(0.327f, 0.483f, 0.844f),
                new Vec3(0.822f, 0.569f, 0.201f),
                new Vec3(0.435f, 0.602f, 0.223f),
                new Vec3(0.310f, 0.747f, 0.185f),
                new Vec3(0.597f, 0.770f, 0.761f),
                new Vec3(0.559f, 0.436f, 0.730f),
                new Vec3(0.359f, 0.583f, 0.152f),
                new Vec3(0.483f, 0.596f, 0.789f),
                new Vec3(0.559f, 0.861f, 0.639f),
                new Vec3(0.195f, 0.548f, 0.859f),
                new Vec3(0.014f, 0.184f, 0.576f),
                new Vec3(0.771f, 0.328f, 0.970f),
                new Vec3(0.406f, 0.615f, 0.116f),
                new Vec3(0.676f, 0.977f, 0.133f),
                new Vec3(0.971f, 0.572f, 0.833f),
                new Vec3(0.140f, 0.616f, 0.489f),
                new Vec3(0.997f, 0.513f, 0.064f),
                new Vec3(0.945f, 0.719f, 0.592f),
                new Vec3(0.543f, 0.021f, 0.978f),
                new Vec3(0.279f, 0.317f, 0.505f),
                new Vec3(0.167f, 0.620f, 0.077f),
                new Vec3(0.347f, 0.857f, 0.137f),
                new Vec3(0.055f, 0.953f, 0.042f),
                new Vec3(0.714f, 0.505f, 0.345f),
                new Vec3(0.783f, 0.290f, 0.734f),
                new Vec3(0.722f, 0.645f, 0.174f),
                new Vec3(0.302f, 0.455f, 0.848f),
                new Vec3(0.225f, 0.587f, 0.040f),
                new Vec3(0.517f, 0.713f, 0.338f),
                new Vec3(0.053f, 0.959f, 0.120f),
                new Vec3(0.393f, 0.621f, 0.362f),
                new Vec3(0.673f, 0.211f, 0.457f),
                new Vec3(0.820f, 0.883f, 0.371f),
                new Vec3(0.982f, 0.099f, 0.879f)
            };
            Vec2[] textures = new Vec2[]
            {
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2(),
                new Vec2()
            };
            Cube = new Mesh(verts, triangles, colors, textures);
        }

        public Mesh(Vec3[] v, Vec3[] t, Vec3[] c, Vec2[] T)
        {
            Vertices = v;
            Triangles = t;
            Colors = c;
            Textures = T;
        }
        public Mesh(string filename, FileType type)
        {
            Mesh m = null;
            switch (type)
            {
                case FileType.OBJ: m = ObjReader.Read(File.ReadAllText(filename)); break;
                default: break;
            }
            Vertices = m.Vertices;
            Triangles = m.Triangles;
            Textures = m.Textures;
            Colors = m.Colors;
        }
        public Mesh(string filename) : this(filename, TypeFromFile(filename)) { }
        private static FileType TypeFromFile(string filename)
        {
            switch (filename.Split('.').Last().ToLower())
            {
                case "obj": return FileType.OBJ;
                default: return FileType.OBJ;
            }
        }
        public enum FileType {
            OBJ
        }
        

        internal int[] TrianglesToInds(int offset=0)
        {
            int[] ret = new int[Triangles.Length * 3];
            int i = 0;
            foreach(Vec3 t in Triangles)
            {
                ret[i] = (int)t.X+offset;
                ret[i+1] = (int)t.Y+offset;
                ret[i+2] = (int)t.Z+offset;
                i += 3;
            }
            return ret;
        }
    }
}
