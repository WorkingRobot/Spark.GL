using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GL4 = OpenTK.Graphics.OpenGL4.GL;

namespace Spark.GL
{
    public class Window : IBaseWindow
    {
        internal static Window window;
        Camera camera;
        List<GameObject> gameObjects = new List<GameObject>();
        List<Light> lights = new List<Light>();
        const int MAX_LIGHTS = 5;
        Matrix4 view = Matrix4.Identity;
        public Dictionary<string, int> textures = new Dictionary<string, int>();
        public Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
        public Dictionary<String, Material> materials = new Dictionary<string, Material>();
        internal GameWindow gw;
        static internal Shader defaultShader;
        int ibo_elements;
        private bool woken = false;
        private bool updated;

        string Title;
        public int Width { private set; get; }
        public int Height { private set; get; }
        public Window(int width, int height, string title = "")
        {
            window = this;
            Width = width;
            Height = height;
            Title = title;
            Light sunLight = new Light(new Vec3(-5,-3,0), new Vec3(0.7f, 0.7f, 0.7f));
            sunLight.Type = LightType.Directional;
            sunLight.Direction = new Vec3(sunLight.position.Normalized());
            lights.Add(sunLight);
            shaders.Add("color", new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/ColorShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/ColorShader/fs.glsl"));
            shaders.Add("tex", new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/TexShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/TexShader/fs.glsl"));
            shaders.Add("lit", new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/LitShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/LitShader/fs.glsl"));
            shaders.Add("multilit", new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/MultiLitShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/MultiLitShader/fs.glsl"));
            shaders.Add("advlit", new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/AdvLitShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/AdvLitShader/fs.glsl"));
        }
        public void Exit()
        {
            gw.Exit();
        }

        public GameObject[] GetAllObjects()
        {
            return gameObjects.ToArray();
        }

        public Camera GetCamera()
        {
            return camera;
        }

        public KeyboardInput GetKeyboardInput()
        {
            return new KeyboardInput(this);
        }

        public MouseInput GetMouseInput()
        {
            return new MouseInput(this);
        }

        public void SetCamera(Camera cam)
        {
            camera = cam;
        }

        public void Wake()
        {
            gw = new GameWindow(Width, Height, GraphicsMode.Default, Title);
            GL4.ClearColor(Color4.CornflowerBlue);
            
            gw.Load += Gw_Load;
        }

        public void Run()
        {
            foreach (GameObject go in gameObjects)
            {
                foreach (Component component in go.components)
                {
                    component.Load();
                }
            }
            gw.Run(30, 30);
        }

        private void Gw_RenderFrame(object sender, FrameEventArgs e)
        {
            if (!woken) return;
            foreach (GameObject go in gameObjects)
            {
                foreach (Component component in go.components)
                {
                    component.Render();
                }
            }

            if (!updated) return;
            GL4.Viewport(0, 0, Width, Height);
            GL4.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL4.Enable(EnableCap.DepthTest);

            GL4.UseProgram(defaultShader.ProgramID);
            defaultShader.EnableVertexAttribArrays();

            int indiceat = 0;

            // Draw all our objects
            foreach (GameObject v in gameObjects)
            {
                MeshRenderer r = v.GetComponent<MeshRenderer>();
                if (r == null) continue;
                GL4.BindTexture(TextureTarget.Texture2D, r.f.material.TextureID);
                GL4.UniformMatrix4(defaultShader.GetUniform("modelview"), false, ref r.ModelViewProjectionMatrix);
                if (defaultShader.GetAttribute("maintexture") != -1)
                {
                    GL4.Uniform1(defaultShader.GetAttribute("maintexture"), r.f.material.TextureID);
                }

                if (defaultShader.GetUniform("view") != -1)
                {
                    GL4.UniformMatrix4(defaultShader.GetUniform("view"), false, ref view);
                }

                if (defaultShader.GetUniform("model") != -1)
                {
                    GL4.UniformMatrix4(defaultShader.GetUniform("model"), false, ref r.ModelMatrix);
                }

                if (defaultShader.GetUniform("material_ambient") != -1)
                {
                    GL4.Uniform3(defaultShader.GetUniform("material_ambient"), ref r.f.material.AmbientColor);
                }

                if (defaultShader.GetUniform("material_diffuse") != -1)
                {
                    GL4.Uniform3(defaultShader.GetUniform("material_diffuse"), ref r.f.material.DiffuseColor);
                }

                if (defaultShader.GetUniform("material_specular") != -1)
                {
                    GL4.Uniform3(defaultShader.GetUniform("material_specular"), ref r.f.material.SpecularColor);
                }

                if (defaultShader.GetUniform("material_specExponent") != -1)
                {
                    GL4.Uniform1(defaultShader.GetUniform("material_specExponent"), r.f.material.SpecularExponent);
                }

                if (defaultShader.GetUniform("light_position") != -1)
                {
                    GL4.Uniform3(defaultShader.GetUniform("light_position"), ref lights[0].position);
                }

                if (defaultShader.GetUniform("light_color") != -1)
                {
                    GL4.Uniform3(defaultShader.GetUniform("light_color"), ref lights[0].color);
                }

                if (defaultShader.GetUniform("light_diffuseIntensity") != -1)
                {
                    GL4.Uniform1(defaultShader.GetUniform("light_diffuseIntensity"), lights[0].DiffuseIntensity);
                }

                if (defaultShader.GetUniform("light_ambientIntensity") != -1)
                {
                    GL4.Uniform1(defaultShader.GetUniform("light_ambientIntensity"), lights[0].AmbientIntensity);
                }


                for (int i = 0; i < Math.Min(lights.Count, MAX_LIGHTS); i++)
                {
                    if (defaultShader.GetUniform("lights[" + i + "].position") != -1)
                    {
                        GL4.Uniform3(defaultShader.GetUniform("lights[" + i + "].position"), ref lights[i].position);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].color") != -1)
                    {
                        GL4.Uniform3(defaultShader.GetUniform("lights[" + i + "].color"), ref lights[i].color);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                    {
                        GL4.Uniform1(defaultShader.GetUniform("lights[" + i + "].diffuseIntensity"), lights[i].DiffuseIntensity);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].ambientIntensity") != -1)
                    {
                        GL4.Uniform1(defaultShader.GetUniform("lights[" + i + "].ambientIntensity"), lights[i].AmbientIntensity);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].direction") != -1)
                    {
                        GL4.Uniform3(defaultShader.GetUniform("lights[" + i + "].direction"), ref lights[i].direction);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].type") != -1)
                    {
                        GL4.Uniform1(defaultShader.GetUniform("lights[" + i + "].type"), (int)lights[i].Type);
                    }

                    if (defaultShader.GetUniform("lights[" + i + "].coneAngle") != -1)
                    {
                        GL4.Uniform1(defaultShader.GetUniform("lights[" + i + "].coneAngle"), lights[i].ConeAngle);
                    }
                }

                GL4.DrawElements(BeginMode.Triangles, r.mesh.Triangles.Length*3, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += r.mesh.Triangles.Length * 3;
            }

            defaultShader.DisableVertexAttribArrays();

            GL4.Flush();
            gw.SwapBuffers();
            updated = false;
        }

        private void Gw_Load(object sender, EventArgs e)
        {
            GL4.GenBuffers(1, out ibo_elements);
            shaders["color"].GLLoad();
            shaders["tex"].GLLoad();
            shaders["lit"].GLLoad();
            shaders["multilit"].GLLoad();
            shaders["advlit"].GLLoad();
            defaultShader = shaders["advlit"];
            woken = true;
            OpenTK.Input.Mouse.SetPosition(gw.Bounds.Left + gw.Bounds.Width / 2, gw.Bounds.Top + gw.Bounds.Height / 2);
            gw.RenderFrame += Gw_RenderFrame;
            gw.UpdateFrame += Gw_UpdateFrame;
        }

        private void Gw_UpdateFrame(object sender, FrameEventArgs e)
        {
            if (!woken) return;
            foreach (GameObject go in gameObjects)
            {
                foreach (Component component in go.components)
                {
                    component.Update();
                }
            }

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            int vertcount = 0;
            foreach (GameObject go in gameObjects)
            {
                MeshRenderer renderer = go.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    verts.AddRange(Array.ConvertAll(renderer.mesh.Verticies, item => (Vector3)item));
                    inds.AddRange(renderer.mesh.TrianglesToInds(vertcount));
                    //colors.AddRange(Array.ConvertAll(renderer.mesh.Colors, item => (Vector3)item));
                    texcoords.AddRange(Array.ConvertAll(renderer.mesh.Textures, item => (Vector2)item));
                    normals.AddRange(Array.ConvertAll(renderer.mesh.Normals, item => (Vector3)item));
                    vertcount += renderer.mesh.Verticies.Length;
                }
            }
            Vector3[] vertdata = verts.ToArray();
            int[] inddata = inds.ToArray();
            Vector3[] coldata = colors.ToArray();
            Vector2[] texcoorddata = texcoords.ToArray();
            Vector3[] normdata = normals.ToArray();
            GL4.BindBuffer(BufferTarget.ArrayBuffer, defaultShader.GetBuffer("vPosition"));

            GL4.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL4.VertexAttribPointer(defaultShader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (defaultShader.GetAttribute("vColor") != -1)
            {
                GL4.BindBuffer(BufferTarget.ArrayBuffer, defaultShader.GetBuffer("vColor"));
                GL4.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL4.VertexAttribPointer(defaultShader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Buffer texture coordinates if shader supports it
            if (defaultShader.GetAttribute("texcoord") != -1)
            {
                GL4.BindBuffer(BufferTarget.ArrayBuffer, defaultShader.GetBuffer("texcoord"));
                GL4.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL4.VertexAttribPointer(defaultShader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (defaultShader.GetAttribute("vNormal") != -1)
            {
                GL4.BindBuffer(BufferTarget.ArrayBuffer, defaultShader.GetBuffer("vNormal"));
                GL4.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                GL4.VertexAttribPointer(defaultShader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Update model view matrices
            foreach (GameObject v in gameObjects)
            {
                MeshRenderer r = v.GetComponent<MeshRenderer>();
                if (r == null) continue;
                r.CalculateModelMatrix();
                r.ViewProjectionMatrix = camera.GetViewMatrix() * camera.CreateView(Width, Height);
                r.ModelViewProjectionMatrix = r.ModelMatrix * r.ViewProjectionMatrix;
            }

            GL4.UseProgram(defaultShader.ProgramID);

            GL4.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL4.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL4.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(inddata.Length * sizeof(int)), inddata, BufferUsageHint.StaticDraw);
            view = camera.GetViewMatrix();

            updated = true; 
        }

        public GameObject GetObject(string name)
        {
            return gameObjects.Find(go => go.Name == name);
        }

        public void AddObject(GameObject obj)
        {
            obj.window = this;
            gameObjects.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
            gameObjects.Remove(obj);
        }


        //for later

        int loadImage(Bitmap image)
        {
            int texID = GL4.GenTexture();

            GL4.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL4.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL4.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        public int loadImage(string filename)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return loadImage(file);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }

        public void loadMaterials(String filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!materials.ContainsKey(mat.Key))
                {
                    materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in materials.Values)
            {
                if (File.Exists(mat.AmbientMap) && !textures.ContainsKey(mat.AmbientMap))
                {
                    textures.Add(mat.AmbientMap, loadImage(mat.AmbientMap));
                }

                if (File.Exists(mat.DiffuseMap) && !textures.ContainsKey(mat.DiffuseMap))
                {
                    textures.Add(mat.DiffuseMap, loadImage(mat.DiffuseMap));
                }

                if (File.Exists(mat.SpecularMap) && !textures.ContainsKey(mat.SpecularMap))
                {
                    textures.Add(mat.SpecularMap, loadImage(mat.SpecularMap));
                }

                if (File.Exists(mat.NormalMap) && !textures.ContainsKey(mat.NormalMap))
                {
                    textures.Add(mat.NormalMap, loadImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !textures.ContainsKey(mat.OpacityMap))
                {
                    textures.Add(mat.OpacityMap, loadImage(mat.OpacityMap));
                }
            }
        }

    }
}
