using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Spark.GL
{
    class ObjReader
    {
        public static Mesh Read(string input)
        {
            List<Vec3> verts = new List<Vec3>();
            List<Vec3> trigs = new List<Vec3>();
            List<Vec3> colors = new List<Vec3>();
            List<Vec2> texs = new List<Vec2>();
            string[] lines = input.Split('\n');
            foreach(string line in lines)
            {
                //Console.WriteLine(line.StartsWith("v "));
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vec3 vec = new Vec3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success &= float.TryParse(vertparts[1], out vec.Y);
                        success &= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vec3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vec2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }
                    //Console.WriteLine(vec.X);
                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vec3 face = new Vec3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success &= int.TryParse(faceparts[1], out i2);
                        success &= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Vec3(i1 - 1, i2 - 1, i3 - 1);
                            trigs.Add(face);
                        }
                    }
                }
            }
            return new Mesh(verts.ToArray(), trigs.ToArray(), colors.ToArray(), texs.ToArray());
        }
    }
}
