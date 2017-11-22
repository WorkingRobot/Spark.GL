using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.GL
{
    public class FaceVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoord;

        public FaceVertex(Vector3 pos, Vector3 norm, Vector2 texcoord)
        {
            Position = pos;
            Normal = norm;
            TextureCoord = texcoord;
        }
    }
    public class Mesh
    {
        public static Mesh Cube;
        /*private Vec3[] _verts;
        public Vec3[] Verticies {
            set
            {
                _verts = value;
                Center = CalculateCenter();
                Size = CalculateSize();
            }
            get
            {
                return _verts;
            }
        }
        public Vec3[] Triangles;
        public Vec3[] Colors;
        public Vec2[] Textures {
            get {
                List<Vector2> normals = new List<Vector2>();
                foreach (var face in faces)
                {
                    normals.Add(face.Item1.TextureCoord);
                    normals.Add(face.Item2.TextureCoord);
                    normals.Add(face.Item3.TextureCoord);
                }
                return Array.ConvertAll(normals.ToArray(), item => new Vec2(item));
            }
        }
        private Vector3[] _normals;
        public Vec3[] Normals {
            get
            {
                if (_normals.Length > 0) return Array.ConvertAll(_normals.ToArray(), item => new Vec3(item));
                List<Vector3> normals = new List<Vector3>();
                foreach (var face in faces)
                {
                    normals.Add(face.Item1.Normal);
                    normals.Add(face.Item2.Normal);
                    normals.Add(face.Item3.Normal);
                }
                return Array.ConvertAll(normals.ToArray(), item => new Vec3(item));
            }
            set
            {
                _normals = Array.ConvertAll(value, item => (Vector3)item);
            }
        }*/
        public Vec3[] Verticies
        {
            get
            {
                List<Vec3> v = new List<Vec3>();
                foreach (Tuple<FaceVertex, FaceVertex, FaceVertex> vert in faces)
                {
                    v.Add(new Vec3(vert.Item1.Position));
                    v.Add(new Vec3(vert.Item2.Position));
                    v.Add(new Vec3(vert.Item3.Position));
                }
                return v.ToArray();
            }
            set
            {
                for (int i = 0; i < faces.Count; ++i)
                {
                    FaceVertex a = faces[i].Item1;
                    FaceVertex b = faces[i].Item2;
                    FaceVertex c = faces[i].Item3;
                    a.Position = value[i * 3];
                    b.Position = value[i * 3 + 1];
                    c.Position = value[i * 3 + 2];
                    faces[i] = new Tuple<FaceVertex, FaceVertex, FaceVertex>(a, b, c);
                }
            }
        }
        public Vec2[] Textures
        {
            get
            {
                List<Vec2> v = new List<Vec2>();
                foreach (Tuple<FaceVertex, FaceVertex, FaceVertex> vert in faces)
                {
                    v.Add(new Vec2(vert.Item1.TextureCoord));
                    v.Add(new Vec2(vert.Item2.TextureCoord));
                    v.Add(new Vec2(vert.Item3.TextureCoord));
                }
                return v.ToArray();
            }
            set
            {
                for (int i = 0; i < faces.Count; ++i)
                {
                    FaceVertex a = faces[i].Item1;
                    FaceVertex b = faces[i].Item2;
                    FaceVertex c = faces[i].Item3;
                    a.TextureCoord = value[i * 3];
                    b.TextureCoord = value[i * 3 + 1];
                    c.TextureCoord = value[i * 3 + 2];
                    faces[i] = new Tuple<FaceVertex, FaceVertex, FaceVertex>(a, b, c);
                }
            }
        }
        public Vec3[] Normals
        {
            get
            {
                List<Vec3> v = new List<Vec3>();
                foreach (Tuple<FaceVertex, FaceVertex, FaceVertex> vert in faces)
                {
                    v.Add(new Vec3(vert.Item1.Normal));
                    v.Add(new Vec3(vert.Item2.Normal));
                    v.Add(new Vec3(vert.Item3.Normal));
                }
                return v.ToArray();
            }
            set
            {
                for (int i = 0; i < faces.Count; ++i)
                {
                    FaceVertex a = faces[i].Item1;
                    FaceVertex b = faces[i].Item2;
                    FaceVertex c = faces[i].Item3;
                    a.Normal = value[i * 3];
                    b.Normal = value[i * 3 + 1];
                    c.Normal = value[i * 3 + 2];
                    faces[i] = new Tuple<FaceVertex, FaceVertex, FaceVertex>(a, b, c);
                }
            }
        }
        public Vec3[] Triangles
        {
            get
            {
                Vec3[] list = new List<Vec3>(faces.Count);
                int i = 0;
                foreach (Tuple<FaceVertex, FaceVertex, FaceVertex> tri in faces)
                {
                    list[i/3] = new Vec3(i, i + 1, i + 2));
                    i += 3;
                }
                return list;
            }
        }

        public List<Tuple<FaceVertex, FaceVertex, FaceVertex>> faces = new List<Tuple<FaceVertex, FaceVertex, FaceVertex>>();

        //public Material material;
        private Vec3 _center;

        public Vec3 Center {
            get {
                if (_center == null)
                {
                    _center = CalculateCenter();
                }
                return _center;
            }
        }

        private Vec3 _size;
        public Vec3 Size {
            get
            {
                if (_size == null)
                {
                    _size = CalculateSize();
                }
                return _size;
            }
        }


        private Vec3 CalculateCenter()
        {
            float x, y, z;
            x = y = z = 0;
            foreach (Vec3 v in Verticies)
            {
                x += v.X;
                y += v.Y;
                z += v.Z;
            }
            return new Vec3(x / Verticies.Length, y / Verticies.Length, z / Verticies.Length);
        }

        private Vec3 CalculateSize()
        {
            // Capital are the max, lowercase are min
            float X, Y, Z, x, y, z;
            X = Y = Z = x = y = z = 0;
            foreach (Vec3 v in Verticies)
            {
                if (v.X > X)
                {
                    X = v.X;
                }
                else if (v.X < x)
                {
                    x = v.X;
                }
                if (v.Y > Y)
                {
                    Y = v.Y;
                }
                else if (v.Y < y)
                {
                    y = v.Y;
                }
                if (v.Z > Z)
                {
                    Z = v.Z;
                }
                else if (v.Z < z)
                {
                    z = v.Z;
                }
            }
            return new Vec3(X - x, Y - y, Z - z)/new Vec3(2,2,2);
        }

        static Mesh()
        {
            Vec3[] verts = new Vec3[]
            {
                //left
                new Vec3(-0.5f, -0.5f,  -0.5f),
                new Vec3(0.5f, 0.5f,  -0.5f),
                new Vec3(0.5f, -0.5f,  -0.5f),
                new Vec3(-0.5f, 0.5f,  -0.5f),

                //back
                new Vec3(0.5f, -0.5f,  -0.5f),
                new Vec3(0.5f, 0.5f,  -0.5f),
                new Vec3(0.5f, 0.5f,  0.5f),
                new Vec3(0.5f, -0.5f,  0.5f),

                //right
                new Vec3(-0.5f, -0.5f,  0.5f),
                new Vec3(0.5f, -0.5f,  0.5f),
                new Vec3(0.5f, 0.5f,  0.5f),
                new Vec3(-0.5f, 0.5f,  0.5f),

                //top
                new Vec3(0.5f, 0.5f,  -0.5f),
                new Vec3(-0.5f, 0.5f,  -0.5f),
                new Vec3(0.5f, 0.5f,  0.5f),
                new Vec3(-0.5f, 0.5f,  0.5f),

                //front
                new Vec3(-0.5f, -0.5f,  -0.5f),
                new Vec3(-0.5f, 0.5f,  0.5f),
                new Vec3(-0.5f, 0.5f,  -0.5f),
                new Vec3(-0.5f, -0.5f,  0.5f),

                //bottom
                new Vec3(-0.5f, -0.5f,  -0.5f),
                new Vec3(0.5f, -0.5f,  -0.5f),
                new Vec3(0.5f, -0.5f,  0.5f),
                new Vec3(-0.5f, -0.5f,  0.5f)
            };
            Vec3[] triangles = new Vec3[]
            {
                //left
                new Vec3(0,1,2),new Vec3(0,3,1),

                //back
                new Vec3(4,5,6),new Vec3(4,6,7),

                //right
                new Vec3(8,9,10),new Vec3(8,10,11),

                //top
                new Vec3(13,14,12),new Vec3(13,15,14),

                //front
                new Vec3(16,17,18),new Vec3(16,19,17),

                //bottom 
                new Vec3(20,21,22),new Vec3(20,22,23)
            };
            Vec3[] colors = new Vec3[]
            {
                //left
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),

                //back
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),

                //right
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),

                //top
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),

                //front
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),

                //bottom
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0),
                new Vec3(0, 0,  0)
            };
            Vec2[] textures = new Vec2[]
            {
                // left
                new Vec2(0.0f, 0.0f),
                new Vec2(-1.0f, 1.0f),
                new Vec2(-1.0f, 0.0f),
                new Vec2(0.0f, 1.0f),
 
                // back
                new Vec2(0.0f, 0.0f),
                new Vec2(0.0f, 1.0f),
                new Vec2(-1.0f, 1.0f),
                new Vec2(-1.0f, 0.0f),
 
                // right
                new Vec2(-1.0f, 0.0f),
                new Vec2(0.0f, 0.0f),
                new Vec2(0.0f, 1.0f),
                new Vec2(-1.0f, 1.0f),
 
                // top
                new Vec2(0.0f, 0.0f),
                new Vec2(0.0f, 1.0f),
                new Vec2(-1.0f, 0.0f),
                new Vec2(-1.0f, 1.0f),
 
                // front
                new Vec2(0.0f, 0.0f),
                new Vec2(1.0f, 1.0f),
                new Vec2(0.0f, 1.0f),
                new Vec2(1.0f, 0.0f),
 
                // bottom
                new Vec2(0.0f, 0.0f),
                new Vec2(0.0f, 1.0f),
                new Vec2(-1.0f, 1.0f),
                new Vec2(-1.0f, 0.0f)
            };
            Cube = new Mesh(verts, triangles, colors, textures);
        }

        public Mesh() { }

        public Mesh(string filename, FileType type)
        {
            Mesh m = null;
            switch (type)
            {
                case FileType.OBJ: m = ObjReader.Read(File.ReadAllText(filename)); break;
                default: break;
            }
            faces = m.faces;
        }
        public Mesh(IEnumerable<Vec3> verts, IEnumerable<Vec3> tris, IEnumerable<Vec3> colors, IEnumerable<Vec2> tex)
        {
            List<Tuple<FaceVertex, FaceVertex, FaceVertex>> _faces = new List<Tuple<FaceVertex, FaceVertex, FaceVertex>>();
            foreach (Vec3 triangle in tris)
            {
                _faces.Add(new Tuple<FaceVertex,FaceVertex,FaceVertex>(
                    new FaceVertex(verts.ElementAt((int)triangle.X), new Vec3(), tex.ElementAt((int)triangle.X)),
                    new FaceVertex(verts.ElementAt((int)triangle.Y), new Vec3(), tex.ElementAt((int)triangle.Y)),
                    new FaceVertex(verts.ElementAt((int)triangle.Z), new Vec3(), tex.ElementAt((int)triangle.Z))
                    ));
            }
            faces = _faces;
            if (Window.window.shaders.FirstOrDefault(x => x.Value == Window.defaultShader).Key.Contains("lit")) //im too lazy to make multiple ifs. who do you think i am
            {
                CalculateNormals();
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
        public enum FileType {
            OBJ
        }

        public void CalculateNormals()
        {
            Vector3[] normals = new Vector3[Verticies.Length];
            Vector3[] verts = Array.ConvertAll(Verticies, item => (Vector3)item);
            int[] inds = TrianglesToInds();

            // Compute normals for each face
            for (int i = 0; i < inds.Length; i += 3)
            {
                Vector3 v1 = verts[inds[i]];
                Vector3 v2 = verts[inds[i + 1]];
                Vector3 v3 = verts[inds[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < Normals.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            Normals = Array.ConvertAll(normals, item => new Vec3(item));
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
