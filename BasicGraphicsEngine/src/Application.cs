using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.Input;
using System.Numerics;


namespace BasicGraphicsEngine
{
    
    public struct Settings()
    {
        /// <summary>
        /// Maximální počet objektů třídy <c>Particle</c>, které je možné vykreslit.
        /// </summary>
        public int MaxParticles = 1000;
        /// <summary>
        /// Maximální počet objektů třídy <c>Quad</c>, které je možné vykreslit.
        /// </summary>
        public int MaxQuads = 100;
        /// <summary>
        /// Maximální počet objektů třídy <c>Line</c>, které je možné vykreslit.
        /// </summary>
        public int MaxLines = 100;
        /// <summary>
        /// Maximální počet objektů třídy <c>Circle</c>, které je možné vykreslit.
        /// </summary>
        public int MaxCircles = 100;

        /// <summary>
        /// Počáteční poloha kamery při spuštění programu.
        /// </summary>
        public Vector3 CameraPosition = new Vector3(0, 0, 10);
        /// <summary>
        /// Výška zorného pole kamery při základním přiblížení (<c>zoom = 1</c>).
        /// </summary>
        public uint CameraSceneHeight = 25;
        /// <summary>
        /// Hloubka zorného pole kamery.
        /// </summary>
        public float CameraSceneDepth = 10.0f;
        /// <summary>
        /// Rychlost posuvného pohybu kamery při použití "šipek" na klávesnici.
        /// </summary>
        public float CameraMovementSpeed = 5.0f;
        /// <summary>
        /// Rychlost posuvného pohybu kamery při použití myši (dragování při stisknutém pravém tlačítku).
        /// </summary>
        public float CameraMouseSensitivity = 0.1f;

        /// <summary>
        /// Barva pozadí (clear color).
        /// </summary>
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
        /// <summary>
        /// Objekt obsahující základní nastavení programu. Tato nastavení je možné změnit přepsáním 
        /// konkrétních proměnných tohoto objektu v těle konstruktoru třídy <c>BGEapp()</c>.
        /// </summary>
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

        /// <summary>
        /// Metoda <c>Setup()</c> umožňuje definovat příkazy, které se provedou před vykreslením prvního snímku.
        /// </summary>
        abstract public void Setup();

        /// <summary>
        /// Metoda <c>Loop()</c> umožňuje definovat příkazy, které se provádí před vykreslením každého snímku. Proměnná <c>dt</c> 
        /// odpovídá času (v ms), jak dlouho trvalo vykreslení předchozího snímku.
        /// <list type="bullet">
        ///     <item>Konkrétně se tato metoda volá ještě před tím, než se interně přepisují pozice objektů.</item>
        ///     <item>Pokud chcete kameru ovládat programaticky, tak její referenci je možné získat příkazem: <code>GetCamera()</code></item>
        /// </list>
        /// </summary>
        /// <param name="dt">Racionální číslo reprezentující čas (v ms), po který se vykresloval předchozí snímek.</param>
        abstract public void Loop(float dt);

        /// <summary>
        /// Metoda <c>KeyDownEvent()</c> umožňuje detekovat stisknutí konkrétní klávesy a následně definovat příkazy, které se po 
        /// této detekci mají provést. Stisknutá klávesa, která tuto metodu vyvolala je uložena v parametru <c>key</c>.
        /// <list type="bullet">
        ///     <item>Klávesy, které je možné detekovat definuje enumerace <c>UserKeyboardKey</c>.</item>
        ///     <item>Pro příkazy, které se mají provést právě po detekování dané stisknuté klávesy platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro klávesu "Space": <code>if (key == UserKeyboardKey.Space) { příkaz }</code></item>
        ///         <item>Po detekci se daný příkaz provede pouze jednou.</item>
        ///     </list>
        ///     <item>Pro příkazy, které se mají provádět, když je daná klávesa stále stisknutá platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro klávesu "A": <code>if (UserInput.IsKeyPressed(UserKeyboardKey.A)) { příkaz }</code></item>
        ///         <item>Daný příkaz se opakuje každý snímek, dokud je daná klávesa stisknutá (ale neblokuje vykreslování).</item>
        ///         <item>Tuto detekci je možné provádět i mimo <c>KeyDownEvent()</c>, ale zde to "dává smysl".</item>
        ///     </list>
        /// </list>
        /// </summary>
        /// <param name="key">Stisknutá klávesa reprezentovaná enumerací <c>UserKeyboardKey</c>.</param>
        virtual public void KeyDownEvent(UserKeyboardKey key) { }

        /// <summary>
        /// Metoda <c>KeyUpEvent()</c> umožňuje detekovat uvolnění konkrétní klávesy a následně definovat příkazy, které se po 
        /// této detekci mají provést. Uvolněná klávesa, která tuto metodu vyvolala je uložena v parametru <c>key</c>.
        /// <list type="bullet">
        ///     <item>Klávesy, které je možné detekovat definuje enumerace <c>UserKeyboardKey</c>.</item>
        ///     <item>Pro příkazy, které se mají provést právě po detekování dané uvoněné klávesy platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro klávesu "Space": <code>if (key == UserKeyboardKey.Space) { příkaz }</code></item>
        ///         <item>Po detekci se daný příkaz provede pouze jednou.</item>
        ///     </list>
        /// </list>
        /// </summary>
        /// <param name="key">Uvolněná klávesa reprezentovaná enumerací <c>UserKeyboardKey</c>.</param>
        virtual public void KeyUpEvent(UserKeyboardKey key) { }

        /// <summary>
        /// Metoda <c>MouseButtonDownEvent()</c> umožňuje detekovat stisknutí konkrétního tlačíka myši a následně definovat 
        /// příkazy, které se po této detekci mají provést. Stisknuté tlačítko, které tuto metodu vyvolalo je uloženo v parametru <c>button</c>.
        /// <list type="bullet">
        ///     <item>Tlačítka myši, která je možné detekovat definuje enumerace <c>UserMouseButton</c>.</item>
        ///     <item>Pro příkazy, které se mají provést právě po detekování daného stisknutého tlačítka platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro levé tlačítko: <code>if (button == UserMouseButton.Left) { příkaz }</code></item>
        ///         <item>Po detekci se daný příkaz provede pouze jednou.</item>
        ///     </list>
        ///     <item>Pro příkazy, které se mají provádět, když je dané tlačítko stále stisknuté platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro levé tlačítko: <code>if (UserInput.IsMouseButtonPressed(UserMouseButton.Left)) { příkaz }</code></item>
        ///         <item>Daný příkaz se opakuje každý snímek, dokud je dané tlačítko stisknuté (ale neblokuje vykreslování).</item>
        ///         <item>Tuto detekci je možné provádět i mimo <c>MouseButtonDownEvent()</c>, ale zde to "dává smysl".</item>
        ///     </list>
        /// </list>
        /// </summary>
        /// <param name="button">Stisknuté tlačítko myši reprezentované enumerací <c>UserMouseButton</c>.</param>
        virtual public void MouseButtonDownEvent(UserMouseButton button) { }

        /// <summary>
        /// Metoda <c>MouseButtonUpEvent()</c> umožňuje detekovat uvolnění konkrétního tlačíka myši a následně definovat 
        /// příkazy, které se po této detekci mají provést. Uvolněné tlačítko, které tuto metodu vyvolalo je uloženo v parametru <c>button</c>.
        /// <list type="bullet">
        ///     <item>Tlačítka myši, která je možné detekovat definuje enumerace <c>UserMouseButton</c>.</item>
        ///     <item>Pro příkazy, které se mají provést právě po detekování daného uvoněného tlačítka platí:</item>
        ///     <list type="bullet">
        ///         <item>Například pro levé tlačítko: <code>if (button == UserMouseButton.Left) { příkaz }</code></item>
        ///         <item>Po detekci se daný příkaz provede pouze jednou.</item>
        ///     </list>
        /// </list>
        /// </summary>
        /// <param name="button">Uvolněné tlačítko myši reprezentované enumerací <c>UserMouseButton</c>.</param>
        virtual public void MouseButtonUpEvent(UserMouseButton button) { }

        /// <summary>
        /// Metoda <c>MouseMoveEvent()</c> umožňuje definovat příkazy, které se mají provést, když 
        /// myš změní svou polohu, která je uložena v proměnné <c>position</c> typu <c>Vector2</c>.
        /// </summary>
        /// <param name="position">2D vektor obsahující okamžitou polohu myši.</param>
        virtual public void MouseMoveEvent(Vector2 position) { }

        /// <summary>
        /// Metoda <c>MouseScrollEvent()</c> umožňuje definovat příkazy, které se mají provést, když se změní natočení 
        /// kolečka myši, které je popsáno pomocí dvou parametrů <c>mouseWheelX</c> a <c>mouseWheelY</c>.
        /// </summary>
        /// <param name="mouseWheelX">Racionální číslo reprezentující horizontální natočení kolečka myši -- prakticky 
        /// není potřeba používat.</param>
        /// <param name="mouseWheelY">Racionální číslo reprezentující vertikální natočení kolečka myši.</param>
        virtual public void MouseScrollEvent(float mouseWheelX, float mouseWheelY) { }

        /// <summary>
        /// Metoda <c>AddObject()</c> přidá zadaný objekt do seznamu vykreslovaných objektů => objekt bude bude ve všech 
        /// dalších snímcích vykreslován.
        /// </summary>
        /// <param name="obj">Reference na objekt třídy, která dědí buď přímo, nebo nepřímo z třídy <c>DrawableObject</c>.</param>
        public void AddObject(DrawableObject obj)
        {
            _objectManager.AddObject(obj);
        }

        /// <summary>
        /// Metoda <c>RemoveObject()</c> odstraní zadaný objekt ze seznamu vykreslovaných objektů => objekt již nebude v dalších 
        /// snímcích vykreslován.
        /// </summary>
        /// <param name="obj">Reference na objekt třídy, která dědí buď přímo, nebo nepřímo z třídy <c>DrawableObject</c>.</param>
        public void RemoveObject(DrawableObject obj)
        {
            _objectManager.RemoveObject(obj);
        }

        /// <summary>
        /// Metoda <c>SetBackgroundColor()</c> umožňuje změnit barvu pozadí (clear color).
        /// </summary>
        /// <param name="color">Nová barva pozadí typu <c>System.Drawing.Color</c>.</param>
        public void SetBackgroundColor(System.Drawing.Color color)
        { 
            Settings.BackgroundColor = color;

            _renderer.SetClearColor(color);
        }

        /// <summary>
        /// Funkce <c>GetCamera()</c> vrací referenci na kameru, která zobrazuje scénu.
        /// </summary>
        /// <returns>Referenci na objekt typu <c>Camera</c>, který zobrazuje scénu.</returns>
        public Camera GetCamera()
        {
            return _renderer.GetCamera();
        }
    }
}
