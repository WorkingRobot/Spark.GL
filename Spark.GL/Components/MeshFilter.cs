namespace Spark.GL
{
    public class MeshFilter : Component
    {
        public Material material { get; set; }
        public Mesh mesh { get; set; }

        public MeshFilter()
        {
            material = new Material();
        }
    }
}
