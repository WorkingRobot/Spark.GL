using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spark.GL
{
    class ObjReader
    {
        static Regex Rv = new Regex(@"v ([-.\d]+) ([-.\d]+) ([-.\d]+)");
        static Regex Rf = new Regex(@"f (?:(\d+) ?){3}");
        public static Mesh Read(string input)
        {
            List<Vec3> verts = new List<Vec3>();
            List<Vec3> trigs = new List<Vec3>();
            List<Vec3> colors = new List<Vec3>();
            List<Vec2> texs = new List<Vec2>();
            string[] lines = input.Split('\n');
            foreach(string line in lines)
            {
                Match m = Rv.Match(line);
                if (m.Success)
                {
                    Vec3 v = new Vec3();
                    bool success = float.TryParse(m.Groups[0].Value, out v.X);
                    success &= float.TryParse(m.Groups[1].Value, out v.Y);
                    success &= float.TryParse(m.Groups[2].Value, out v.Z);
                    if (success) verts.Add(v);
                    else Console.WriteLine("Error parsing vertex: " + line);
                }
                m = Rf.Match(line);
                if (m.Success)
                {
                    Vec3 v = new Vec3();
                    bool success = float.TryParse(m.Groups[0].Value, out v.X);
                    success &= float.TryParse(m.Groups[1].Value, out v.Y);
                    success &= float.TryParse(m.Groups[2].Value, out v.Z);
                    if (success) {
                        trigs.Add(v);
                        colors.Add(new Vec3(Math.Sin(v.Z), Math.Sin(v.Z), Math.Sin(v.Z)));
                        texs.Add(new Vec2(Math.Sin(v.Z), Math.Sin(v.Z)));
                    }
                    else Console.WriteLine("Error parsing vertex: " + line);
                }
            }
            return new Mesh(verts.ToArray(), trigs.ToArray(), colors.ToArray(), texs.ToArray());
        }
    }
}
