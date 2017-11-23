using OpenTK.Input;

namespace Spark.GL
{
    public struct MouseInput
    {
        private MouseState state;
        private MouseState cursor;
        public Window window;
        public MouseInput(Window window) {
            state = Mouse.GetState();
            cursor = Mouse.GetCursorState();
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
        public int PosX { get { return cursor.X; } }
        public int PosY { get { return cursor.Y; } }
        public float ScrollSpeed { get { return state.WheelPrecise; } }

        public void ResetMousePosition()
        {
            Mouse.SetPosition(window.gw.Bounds.Left + window.gw.Bounds.Width / 2, window.gw.Bounds.Top + window.gw.Bounds.Height / 2);
        }

        public Vec2 Center { get { return new Vec2(window.gw.Bounds.Left + window.gw.Bounds.Width / 2, window.gw.Bounds.Top + window.gw.Bounds.Height / 2); } }
    }
    public enum MouseButton
    {
        Left = 0,
        Right = 2,
        Middle = 1
    }
}