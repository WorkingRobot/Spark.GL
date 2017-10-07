namespace Spark.GL
{
    public interface IComponent
    {
        void Awake();
        void Load();
        void Update();
        void Render();
        void Exit();
        void OnEnable();
        void OnDisable();
    }
}