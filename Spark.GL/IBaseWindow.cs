namespace Spark.GL
{
    public interface IBaseWindow
    {
        void Wake();
        void Run();
        void Exit();
        GameObject[] GetAllObjects();
        GameObject GetObject(string name);
        void AddObject(GameObject obj);
        void RemoveObject(GameObject obj);
        Camera GetCamera();
        void SetCamera(Camera camera);
        KeyboardInput GetKeyboardInput();
        MouseInput GetMouseInput();
    }
}
