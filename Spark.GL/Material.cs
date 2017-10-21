using System.Collections.Generic;

namespace Spark.GL
{
    public class Material
    {
        public Shader shader;
        public int textureID = 0;
        public SortedDictionary<string, ShaderInput> shaderInputs = new SortedDictionary<string, ShaderInput>();
        public Material()
        {
            shader = Window.defaultShader;
        }
    }

    //to be added
    public class ShaderInput
    {
        object obj;
        ShaderInputType type;
        public ShaderInput(Vec2 obj){
            this.obj = obj;
            type = ShaderInputType.VEC2;
        }
        public ShaderInput(Vec3 obj)
        {
            this.obj = obj;
            type = ShaderInputType.VEC3;
        }
        public ShaderInput(Vec4 obj)
        {
            this.obj = obj;
            type = ShaderInputType.VEC4;
        }
        public ShaderInput(int obj)
        {
            this.obj = obj;
            type = ShaderInputType.INT;
        }
        public ShaderInput(float obj)
        {
            this.obj = obj;
            type = ShaderInputType.FLOAT;
        }
        public ShaderInput(double obj)
        {
            this.obj = obj;
            type = ShaderInputType.DOUBLE;
        }
        private enum ShaderInputType { VEC2, VEC3, VEC4, INT, FLOAT, DOUBLE }
    }
}