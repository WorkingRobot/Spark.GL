using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.GL
{
    internal class Mesh
    {
        public Vec3[] Vertices;
        public Vec3[] Triangles;
        public Vec3[] Colors;
        public Vec2[] Textures;
        public Mesh(Vec3[] v, Vec3[] t, Vec3[] c, Vec2[] T)
        {
            Vertices = v;
            Triangles = t;
            Colors = c;
            Textures = T;
        }
        public Mesh(string filename, FileType type)
        {
            switch (type)
            {
                case FileType.OBJ: ObjReader.Read(File.ReadAllText(filename)); break;
                default: break;
            }
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
        internal enum FileType {
            OBJ
        }
        
    }
}
