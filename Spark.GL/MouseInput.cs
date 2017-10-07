using OpenTK.Input;

namespace Spark.GL
{
    internal struct MouseInput
    {
        private MouseState state;
        public IBaseWindow window;
        public MouseInput(IBaseWindow window) {
            state = Mouse.GetState();
            this.window = window;
        }
        public bool ButtonDown(MouseButton button)
        {
            return state.IsButtonDown((OpenTK.Input.MouseButton)button);
        }
        public bool ButtonUp(MouseButton button)
        {
            return state.IsButtonUp((OpenTK.Input.MouseButton)button);
        }
        public int PosX { get { return state.X; } }
        public int PosY { get { return state.Y; } }
        public float ScrollSpeed { get { return state.WheelPrecise; } }
    }
    internal enum MouseButton
    {
        Left = 0,
        Right = 2,
        Middle = 1
    }
}