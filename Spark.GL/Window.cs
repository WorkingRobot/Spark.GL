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
        Camera camera;
        List<GameObject> gameObjects = new List<GameObject>();
        internal GameWindow gw;
        static internal Shader defaultShader;
        int ibo_elements;
        private bool updated;

        string Title;
        public int Width { private set; get; }
        public int Height { private set; get; }
        public Window(int width, int height, string title = "")
        {
            Width = width;
            Height = height;
            Title = title;
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

        public void Start()
        {
            gw = new GameWindow(Width, Height, GraphicsMode.Default, Title);
            GL4.ClearColor(Color4.CornflowerBlue);
            gw.UpdateFrame += Gw_UpdateFrame;
            gw.Load += Gw_Load;
            gw.RenderFrame += Gw_RenderFrame;
            foreach (GameObject go in gameObjects)
            {
                foreach(Component component in go.components)
                {
                    component.Awake();
                }
            }
            defaultShader = new Shader("C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/ColorShader/vs.glsl", "C:/Users/Aleks.Aleks-PC/Documents/Visual Studio 2017/Projects/Spark.GL/Spark.GL/Shaders/ColorShader/fs.glsl", true);
            gw.Run(60);
        }

        private void Gw_RenderFrame(object sender, FrameEventArgs e)
        {
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

            defaultShader.EnableVertexAttribArrays();

            int indiceat = 0;

            // Draw all our objects
            foreach (GameObject v in gameObjects)
            {
                MeshRenderer r = v.GetComponent<MeshRenderer>();
                if (r == null) continue;
                GL4.BindTexture(TextureTarget.Texture2D, v.material.textureID);
                GL4.UniformMatrix4(defaultShader.GetUniform("modelview"), false, ref r.ModelViewProjectionMatrix);

                if (defaultShader.GetAttribute("maintexture") != -1)
                {
                    GL4.Uniform1(defaultShader.GetAttribute("maintexture"), v.material.textureID);
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
            OpenTK.Input.Mouse.SetPosition(gw.Bounds.Left + gw.Bounds.Width / 2, gw.Bounds.Top + gw.Bounds.Height / 2);
            foreach (GameObject go in gameObjects)
            {
                foreach (Component component in go.components)
                {
                    component.Load();
                }
            }
        }

        private void Gw_UpdateFrame(object sender, FrameEventArgs e)
        {
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

            int vertcount = 0;
            foreach (GameObject go in gameObjects)
            {
                MeshRenderer renderer = go.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    verts.AddRange(Array.ConvertAll(renderer.mesh.Vertices, item => (Vector3)item));
                    inds.AddRange(renderer.mesh.TrianglesToInds(vertcount));
                    colors.AddRange(Array.ConvertAll(renderer.mesh.Colors, item => (Vector3)item));
                    texcoords.AddRange(Array.ConvertAll(renderer.mesh.Textures, item => (Vector2)item));
                    vertcount += renderer.mesh.Vertices.Length;
                }
            }
            Vector3[] vertdata = verts.ToArray();
            int[] inddata = inds.ToArray();
            Vector3[] coldata = colors.ToArray();
            Vector2[] texcoorddata = texcoords.ToArray();
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

            updated = true;
        }

        public GameObject GetObject(string name)
        {
            return gameObjects.Find(go => go.Name == name);
        }

        public void AddObject(GameObject obj)
        {
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

        int loadImage(string filename)
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
    }
}
