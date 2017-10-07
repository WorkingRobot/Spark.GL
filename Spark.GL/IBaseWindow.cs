namespace Spark.GL
{
    internal interface IBaseWindow
    {
        void Start();
        void Exit();
        GameObject[] GetAllObjects();
        Camera GetCamera();
        void SetCamera();
        KeyboardInput GetKeyboardInput();
        MouseInput GetMouseInput();
    }
}
