using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.Input;
using System.Numerics;


namespace BasicGraphicsEngine
{
    public struct Settings()
    {
        public int MaxParticles = 1000;
        public int MaxQuads = 100;
        public int MaxLines = 100;
        public int MaxCircles = 100;

        public Vector3 CameraPosition = new Vector3(0, 0, 10);
        public uint CameraSceneHeight = 25;
        public float CameraSceneDepth = 10.0f;
        public float CameraMovementSpeed = 5.0f;
        public float CameraMouseSensitivity = 0.1f;

        public System.Drawing.Color BackgroundColor = System.Drawing.Color.CornflowerBlue;
    }

    public abstract class Application
    {
        private IWindow _window;
        private IInputContext _input;
        private Renderer _renderer;
        private ObjectManager _objectManager;

        private uint _viewportWidth;
        private uint _viewportHeight;

        // Settings:
        public Settings Settings = new Settings();

        // User input:
        public UserInput UserInput;

        public Application(string title, uint viewportWidth, uint viewportHeight, bool enableVSync, int samples) 
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;

            WindowOptions options = WindowOptions.Default;
            options.Title = title;
            options.Size = new Vector2D<int>((int)_viewportWidth, (int)_viewportHeight);
            options.VSync = enableVSync;
            options.Samples = samples;

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Resize += OnResize;
        }

        public Application(string title, uint viewportWidth, uint viewportHeight)
            : this(title, viewportWidth, viewportHeight, true, 4) { }

        public void StartApplication()
        {
            _window.Run();
        }

        private void OnLoad()
        {
            _input = _window.CreateInput();
            UserInput = new UserInput(_input);

            _renderer = new Renderer(_window, _viewportWidth, _viewportHeight, 
                (uint)Settings.MaxParticles, 
                (uint)Settings.MaxQuads,
                (uint)Settings.MaxLines,
                (uint)Settings.MaxCircles,
                Settings.CameraSceneHeight,
                Settings.CameraSceneDepth,
                Settings.CameraPosition,
                Settings.BackgroundColor
            );
            _renderer.OnWindowResize(_viewportWidth, _viewportHeight);

            _objectManager = new ObjectManager(Settings.MaxParticles, Settings.MaxQuads, Settings.MaxLines, Settings.MaxCircles);

            SetupInput();

            Setup();
        }

        private void OnUpdate(double dt)
        {
            Loop((float)dt);

            _objectManager.UpdateVertexData();
        }

        private void OnRender(double dt)
        {
            _renderer.ClearWindow();
            _renderer.Render((float)dt, _input, _objectManager.GetVertexDataOpaque(), _objectManager.GetTransparentBatches());
        }

        private void OnResize(Vector2D<int> size)
        {
            _renderer.OnWindowResize((uint)size.X, (uint)size.Y);
        }

        private void SetupInput()
        {
            for (int i = 0; i < _input.Keyboards.Count; i++)
            {
                _input.Keyboards[i].KeyDown += OnKeyDown;
                _input.Keyboards[i].KeyUp += OnKeyUp;
            }
            for (int i = 0; i < _input.Mice.Count; i++)
            {
                _input.Mice[i].MouseMove += OnMouseMove;
                _input.Mice[i].Scroll += OnScroll;
                _input.Mice[i].MouseDown += OnMouseDown;
                _input.Mice[i].MouseUp += OnMouseUp;
            }
        }

        private void OnKeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            if (key == Key.Escape) _window.Close();

            _renderer.OnKeyDown(key);

            KeyDownEvent(UserInput.SilkKeyToUserKey(key));
        }

        private void OnKeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
            KeyUpEvent(UserInput.SilkKeyToUserKey(key));
        }

        private void OnMouseMove(IMouse mouse, Vector2 position)
        {
            _renderer.OnMouseMove(mouse, position);

            MouseMoveEvent(position);
        }

        private void OnScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            _renderer.OnMouseScroll(scrollWheel);

            MouseScrollEvent(scrollWheel.X, scrollWheel.Y);
        }

        private void OnMouseDown(IMouse mouse, MouseButton button)
        {
            MouseButtonDownEvent(UserInput.SilkMouseButtonToUserMouseButton(button));
        }

        private void OnMouseUp(IMouse mouse, MouseButton button)
        {
            _renderer.OnMouseUp(button);

            MouseButtonUpEvent(UserInput.SilkMouseButtonToUserMouseButton(button));
        }

        abstract public void Setup();

        abstract public void Loop(float dt);

        virtual public void KeyDownEvent(UserKeyboardKey key) { }

        virtual public void KeyUpEvent(UserKeyboardKey key) { }

        virtual public void MouseButtonDownEvent(UserMouseButton button) { }

        virtual public void MouseButtonUpEvent(UserMouseButton button) { }

        virtual public void MouseMoveEvent(Vector2 position) { }

        virtual public void MouseScrollEvent(float mouseWheelX, float mouseWheelY) { }

        public void AddObject(DrawableObject obj)
        {
            _objectManager.AddObject(obj);
        }

        public void RemoveObject(DrawableObject obj)
        {
            _objectManager.RemoveObject(obj);
        }

        public void SetBackgroundColor(System.Drawing.Color color)
        { 
            Settings.BackgroundColor = color;

            _renderer.SetClearColor(color);
        }

        public Camera GetCamera()
        {
            return _renderer.GetCamera();
        }
    }
}
