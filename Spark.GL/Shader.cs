﻿using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;
using System.Linq;
using GL4 = OpenTK.Graphics.OpenGL4.GL;

namespace Spark.GL
{
    public class Shader
    {

        string cachedVS;
        string cachedFS;

        public int ProgramID = -1;
        public int VShaderID = -1;
        public int FShaderID = -1;
        public int AttributeCount = 0;
        public int UniformCount = 0;

        public Dictionary<string, AttributeInfo> Attributes = new Dictionary<string, AttributeInfo>();
        public Dictionary<string, UniformInfo> Uniforms = new Dictionary<string, UniformInfo>();
        public Dictionary<string, uint> Buffers = new Dictionary<string, uint>();

        public Shader()
        {
            ProgramID = GL4.CreateProgram();
        }

        private void loadShader(string code, ShaderType type, out int address)
        {
            address = GL4.CreateShader(type);
            GL4.ShaderSource(address, code);
            GL4.CompileShader(address);
            GL4.AttachShader(ProgramID, address);
            Console.WriteLine(GL4.GetShaderInfoLog(address));
        }

        public void LoadShaderFromString(string code, ShaderType type)
        {
            if (type == ShaderType.VertexShader)
            {
                loadShader(code, type, out VShaderID);
            }
            else if (type == ShaderType.FragmentShader)
            {
                loadShader(code, type, out FShaderID);
            }
        }

        private void CacheFile(string filename, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                if (type == ShaderType.VertexShader)
                {
                    cachedVS = sr.ReadToEnd();
                }
                else if (type == ShaderType.FragmentShader)
                {
                    cachedFS = sr.ReadToEnd();
                }
            }
        }

        public void LoadShaderFromFile(string filename, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                if (type == ShaderType.VertexShader)
                {
                    String s = sr.ReadToEnd();
                    loadShader(s, type, out VShaderID);
                }
                else if (type == ShaderType.FragmentShader)
                {
                    loadShader(sr.ReadToEnd(), type, out FShaderID);
                }
            }
        }

        public void Link()
        {
            Console.WriteLine("linking...");
            GL4.LinkProgram(ProgramID);
            Console.WriteLine("'linked'");
            Console.WriteLine(GL4.GetProgramInfoLog(ProgramID));
            Console.WriteLine("proginfo log ^^");
            GL4.GetProgram(ProgramID, GetProgramParameterName.ActiveAttributes, out AttributeCount);
            GL4.GetProgram(ProgramID, GetProgramParameterName.ActiveUniforms, out UniformCount);
            Console.WriteLine("get programs");
            for (int i = 0; i < AttributeCount; i++)
            {
                AttributeInfo info = new AttributeInfo();
                int length = 0;

                StringBuilder name = new StringBuilder();

                GL4.GetActiveAttrib(ProgramID, i, 256, out length, out info.size, out info.type, name);

                info.name = name.ToString();
                info.address = GL4.GetAttribLocation(ProgramID, info.name);
                Attributes.Add(name.ToString(), info);
            }
            Console.WriteLine("attribs ^ :)");

            for (int i = 0; i < UniformCount; i++)
            {
                Console.WriteLine("start " + i);
                UniformInfo info = new UniformInfo();
                Console.WriteLine("uniform maked");
                int length = 0;

                StringBuilder name = new StringBuilder();
                //Console.WriteLine("string maked");
                
                GL4.GetActiveUniform(ProgramID, i, 256, out length, out info.size, out info.type, name);
                Console.WriteLine("gl4 uniform");

                info.name = name.ToString();
                Uniforms.Add(name.ToString(), info);
                info.address = GL4.GetUniformLocation(ProgramID, info.name);
                Console.WriteLine("do stuff "+info.name);
            }
            Console.WriteLine("uniforms ^^");
        }

        public void GenBuffers()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                uint buffer = 0;
                GL4.GenBuffers(1, out buffer);

                Buffers.Add(Attributes.Values.ElementAt(i).name, buffer);
            }

            for (int i = 0; i < Uniforms.Count; i++)
            {
                uint buffer = 0;
                GL4.GenBuffers(1, out buffer);

                Buffers.Add(Uniforms.Values.ElementAt(i).name, buffer);
            }
        }

        public void EnableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL4.EnableVertexAttribArray(Attributes.Values.ElementAt(i).address);
            }
        }

        public void DisableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL4.DisableVertexAttribArray(Attributes.Values.ElementAt(i).address);
            }
        }

        public int GetAttribute(string name)
        {
            if (Attributes.ContainsKey(name))
            {
                return Attributes[name].address;
            }
            else
            {
                return -1;
            }
        }

        public int GetUniform(string name)
        {
            if (Uniforms.ContainsKey(name))
            {
                return Uniforms[name].address;
            }
            else
            {
                return -1;
            }
        }

        public uint GetBuffer(string name)
        {
            if (Buffers.ContainsKey(name))
            {
                return Buffers[name];
            }
            else
            {
                return 0;
            }
        }



        public Shader(string vshader, string fshader)
        {
            CacheFile(vshader, ShaderType.VertexShader);
            CacheFile(fshader, ShaderType.FragmentShader);
        }

        public void GLLoad()
        {
            Console.WriteLine("creating \"program\"");
            ProgramID = GL4.CreateProgram();
            Console.WriteLine("Loading shaders from file");
            LoadShaderFromString(cachedVS, ShaderType.VertexShader);
            LoadShaderFromString(cachedFS, ShaderType.FragmentShader);
            Console.WriteLine("linking");
            Link();
            Console.WriteLine("genbuffers");
            GenBuffers();
            Console.WriteLine("shader finish.");
        }

        public class UniformInfo
        {
            public String name = "";
            public int address = -1;
            public int size = 0;
            public ActiveUniformType type;
        }

        public class AttributeInfo
        {
            public String name = "";
            public int address = -1;
            public int size = 0;
            public ActiveAttribType type;
        }
    }
}