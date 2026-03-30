using Silk.NET.Input;
using System.Numerics;


namespace BasicGraphicsEngine
{
    internal enum ProjectionType
    {
        ORTHOGRAPHIC,
        PERSPECTIVE
    }

    public class Camera
    {
        // Camera parameters:
        ProjectionType _projectionType;
        private Vector3 _position;
        private float _rotationAngle = 0;
        private Vector3 _front;
        private Vector3 _up;
        private Vector3 _right;
        private Vector3 _worldUp = new Vector3(0, 1, 0);
        private float _yaw = -90.0f;
        private float _pitch = 0.0f;
        private float _zoom = 1f;
        private float _minZoom = 0.005f;
        private float _maxZoom = 5f;
        private uint _sceneHeight;
        private float _sceneDepth;
        private uint _viewportWidth;
        private uint _viewportHeight;
        private float _aspectRatio;
        private float _movementSpeed = 5f;
        private float _mouseSensitivity = 0.1f;
        private float _scrollSensitivity = 0.1f;

        // Matrices:
        private Matrix4x4 _projectionMatrix;
        private Matrix4x4 _transformMatrix;
        private Matrix4x4 _viewMatrix;

        // Events:
        private bool _changedProjection = true;
        private bool _changedTransform = true;

        private Vector2? _lastMousePosition;

        internal Camera(ProjectionType projectionType, uint sceneHeight, float sceneDepth, uint viewportWidth, uint viewportHeight, Vector3 position, 
            float movementSpeed, float mouseSensitivity)
        {
            _projectionType = projectionType;
            _position = position;
            _sceneHeight = sceneHeight;
            _sceneDepth = sceneDepth;
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            _aspectRatio = (float)viewportWidth / viewportHeight;

            UpdateVectors();
            _changedProjection = true;
            _changedTransform = true;
            UpdateViewMatrix();
        }

        private void UpdateVectors()
        {
            float yawRad = float.DegreesToRadians(_yaw);
            float pitchRad = float.DegreesToRadians(_pitch);

            _front.X = (float)(Math.Cos(yawRad) * Math.Cos(pitchRad));
            _front.Y = (float)Math.Sin(yawRad);
            _front.Z = (float)(Math.Sin(yawRad) * Math.Cos(pitchRad));
            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, _worldUp));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        private void UpdateProjectionMatrix()
        {
            switch (_projectionType)
            {
                case ProjectionType.ORTHOGRAPHIC:
                    _projectionMatrix = Matrix4x4.CreateOrthographic(_sceneHeight * _aspectRatio * _zoom, _sceneHeight * _zoom, 0.1f, _sceneDepth);
                    break;
            }
        }

        private void UpdateTransformMatrix()
        {
            //_transformMatrix = Matrix4x4.CreateLookAt(_position, new Vector3(0, 0 ,0), _up);
            Matrix4x4 translateMatrixPlus = Matrix4x4.CreateTranslation(_position.X, _position.Y, 0.0f);
            Matrix4x4 translateMatrixMinus = Matrix4x4.CreateTranslation(-_position.X, -_position.Y, 0.0f);
            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(_rotationAngle));

            _transformMatrix = translateMatrixMinus * translateMatrixMinus * rotateMatrix * translateMatrixPlus;
        }

        private void UpdateViewMatrix()
        {
            if (_changedProjection) UpdateProjectionMatrix();
            if (_changedTransform) UpdateTransformMatrix();
            if (_changedProjection || _changedTransform)
            {
                _viewMatrix = _transformMatrix * _projectionMatrix;
                _changedProjection = false;
                _changedTransform = false;
            }
        }

        internal void Update(IInputContext input, float dt)
        {
            bool forwardKeyPressed = false;
            bool backwardKeyPressed = false;
            bool rightKeyPressed = false;
            bool leftKeyPressed = false;
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                if (input.Keyboards[i].IsKeyPressed(Key.Up)) forwardKeyPressed = true;
                if (input.Keyboards[i].IsKeyPressed(Key.Down)) backwardKeyPressed = true;
                if (input.Keyboards[i].IsKeyPressed(Key.Right)) rightKeyPressed = true;
                if (input.Keyboards[i].IsKeyPressed(Key.Left)) leftKeyPressed = true;
            }

            float movementDiff = _movementSpeed * _zoom * dt;

            if (forwardKeyPressed)
            {
                _position += _up * movementDiff;
                _changedTransform = true;
            }
            if (backwardKeyPressed)
            {
                _position -= _up * movementDiff;
                _changedTransform = true;
            }
            if (rightKeyPressed)
            {
                _position += _right * movementDiff;
                _changedTransform = true;
            }
            if (leftKeyPressed)
            {
                _position -= _right * movementDiff;
                _changedTransform = true;
            }

            UpdateViewMatrix();
        }

        internal void Pan(Vector2 mousePosition)
        {
            /*
            if(_lastMousePosition == null) _lastMousePosition = mousePosition;

            Vector2 mouseDiff = (mousePosition - (Vector2)_lastMousePosition) * _rotationSensitivity;
            _lastMousePosition = mousePosition;

            _yaw -= mouseDiff.X;
            _pitch -= mouseDiff.Y;

            if (_pitch > 89.0f) _pitch = 89.0f;
            else if (_pitch < -89.0f) _pitch = -89.0f;

            UpdateVectors();
            _changedTransform = true;
            */

            if (_lastMousePosition == null) _lastMousePosition = mousePosition;

            Vector2 mouseDiff = (mousePosition - (Vector2)_lastMousePosition) * _mouseSensitivity * _zoom;
            _lastMousePosition = mousePosition;

            _position -= new Vector3(mouseDiff.X, -mouseDiff.Y, 0.0f);
            UpdateVectors();
            _changedTransform = true;
        }

        internal void ResetLastMousePosition(MouseButton button)
        {
            if (button == MouseButton.Right) _lastMousePosition = null;
        }

        private void ClampZoom()
        {
            if (_zoom < _minZoom) _zoom = _minZoom;
            else if (_zoom > _maxZoom) _zoom = _maxZoom;
        }

        private void ClampZoom(float zoom)
        {
            _zoom = zoom;
            ClampZoom();
        }

        internal void Zoom(ScrollWheel scrollWheel)
        {
            _zoom -= scrollWheel.Y * _scrollSensitivity;
            ClampZoom();

            _changedProjection = true;
        }

        internal Matrix4x4 GetViewMatrix()
        {
            return _viewMatrix;
        }

        internal Matrix4x4 GetTransfromMatrix()
        {
            return _transformMatrix;
        }

        internal Matrix4x4 GetProjectionMatrix()
        {
            return _projectionMatrix;
        }

        internal float GetAspectRatio()
        {
            return _aspectRatio;
        }

        internal void SetViewportSize(uint viewportWidth, uint viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;

            _aspectRatio = (float)_viewportWidth / _viewportHeight;

            _changedProjection = true;
        }

        internal void SetProjectionType(ProjectionType projectionType)
        {
            _projectionType = projectionType;

            _changedProjection = true;
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu kamery pomocí vektoru.
        /// </summary>
        /// <param name="position">3D vektor určující novou pozici kamery.</param>
        public void SetPosition(Vector3 position)
        {
            _position = position;

            _changedTransform = true;
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu kamery zadáním jednotlivých souřadnic.
        /// </summary>
        /// <param name="x">Racionální číslo určující x-ovou souřadnici nové pozice kamery.</param>
        /// <param name="y">Racionální číslo určující y-ovou souřadnici nové pozice kamery.</param>
        /// <param name="z">Racionální číslo určující z-ovou souřadnici nové pozice kamery.</param>
        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu kamery při zachování její původní výšky 
        /// (z-ová souřadnice pozice) pomocí vektoru.
        /// </summary>
        /// <param name="position">2D vektor určující x-ovou a y-ovou souřadnici nové pozice kamery.</param>
        public void SetPosition(Vector2 position)
        {
            SetPosition(position.X, position.Y, _position.Z);
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu kamery při zachování její původní výšky 
        /// (z-ová souřadnice pozice) pomocí jednotlivých souřadnic.
        /// </summary>
        /// <param name="x">Racionální číslo určující x-ovou souřadnici nové pozice kamery.</param>
        /// <param name="y">Racionální číslo určující y-ovou souřadnici nové pozice kamery.</param>
        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// Metoda <c>SetRotationAngle()</c> otočí kameru kolem osy z o úhel (v radiánech), který určuje parametr <c>rotationAngle</c>.
        /// </summary>
        /// <param name="rotationAngle">Racionální číslo reprezentující velikost úhlu v radiánech.</param>
        public void SetRotationAngle(float rotationAngle)
        {
            _rotationAngle = rotationAngle;

            _changedTransform = true;
        }

        /// <summary>
        /// Metoda <c>SetZoom()</c> umožňuje programaticky regulovat přiblížení zorného pole kamery pomocí parametru <c>zoom</c>.
        /// </summary>
        /// <param name="zoom">Racionální číslo z intervalu (0.005; 5). S rostoucí hodnotou zoom se kamera "oddaluje" => 
        /// zvětšuje se její zorné pole.</param>
        public void SetZoom(float zoom)
        {
            ClampZoom(zoom);

            _changedProjection = true;
        }

        /// <summary>
        /// Metoda <c>SetSceneHeight()</c> umožňuje nastavení výšky zorného pole kamery při základním přiblížení (<c>zoom = 1</c>) 
        /// pomocí parametru <c>sceneHeight</c>. Šířka zorného pole se podle zadané hodnoty výšky automaticky nastaví tak, aby 
        /// zachovala poměr stran okna aplikace.
        /// </summary>
        /// <param name="sceneHeight">Přirozené číslo reprezentující základní výšku zorného pole kamery.</param>
        public void SetSceneHeight(uint sceneHeight)
        {
            _sceneHeight = sceneHeight;

            _changedProjection = true;
        }

        /// <summary>
        /// Metoda <c>SetSceneDepth()</c> umožňuje nastavení hloubky zorného pole kamery pomocí parametru <c>sceneDepth</c>. 
        /// Hloubka zorného pole určuje, jak daleko se nachází ještě viditelná rovina od kamery. Všechny další roviny, které jsou 
        /// od kamery vzdálenější se již nezobrazí.
        /// </summary>
        /// <param name="sceneDepth">Racionální číslo určující hloubku zorného pole kamery.</param>
        public void SetSceneDepth(float sceneDepth)
        {
            _sceneDepth = sceneDepth;

            _changedProjection = true;
        }

        /// <summary>
        /// Metoda <c>SetMovementSpeed()</c> umožňuje nastavit rychlost posouvání kamery pomocí šipek na klávesnici na hodnotu 
        /// parametru <c>movementSpeed</c>.
        /// </summary>
        /// <param name="movementSpeed">Racionální číslo určující rychlost posuvného pohybu kamery pomocí šipek na klávesnici.</param>
        public void SetMovementSpeed(float movementSpeed)
        { 
            _movementSpeed = movementSpeed;
        }

        /// <summary>
        /// Metoda <c>SetMouseSensitivity()</c> umožňuje nastavit rychlost posouvání kamery dragováním myši (při držení pravého tlačítka) na 
        /// hodnotu parametru <c>mouseSensitivity</c>.
        /// </summary>
        /// <param name="mouseSensitivity">Racionální číslo určující rychlost posouvání kamery dragováním myši.</param>
        public void SetMouseSensitivity(float mouseSensitivity)
        { 
            _mouseSensitivity = mouseSensitivity;
        }

        /// <summary>
        /// Funkce <c>GetPosition()</c> vrací okamžitou polohu kamery jako 3D vektor.
        /// </summary>
        /// <returns>Pozici kamery jako <c>Vector3</c>.</returns>
        public Vector3 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Funkce <c>GetPosition2D()</c> vrací okamžitou polohu kamery jako 2D vektor v rovině xy.
        /// </summary>
        /// <returns>Pozici kamery jako <c>Vector2</c>, který obsahuje x-ovou a y-ovou souřadnici skutečné polohy kamery (<c>Vector3</c>).</returns>
        public Vector2 GetPosition2D()
        {
            return new Vector2(_position.X, _position.Y);
        }

        /// <summary>
        /// Funkce <c>GetRotationAngle()</c> vrací aktuální úhel natočení kamery kolem osy z jako reálné číslo v radiánech.
        /// </summary>
        /// <returns>Úhel (v radiánech) natočení kamery kolem osy z jako <c>float</c>.</returns>
        public float GetRotationAngle()
        {
            return _rotationAngle;
        }

        /// <summary>
        /// Funkce <c>GetZoom()</c> vrací aktuální hodnotu přiblížení zorného pole kamery jako reálné číslo.
        /// </summary>
        /// <returns>Racionální číslo určující přiblížení kamery jako <c>float</c>.</returns>
        public float GetZoom()
        {
            return _zoom;
        }

        /// <summary>
        /// Funkce <c>GetSceneHeight()</c> vrací aktuální hodnotu výšky zorného pole kamery jako přirozené číslo.
        /// </summary>
        /// <returns>Přirozené číslo reprezentující výšku zorného pole kamery jako <c>uint</c>.</returns>
        public uint GetSceneHeight()
        {
            return _sceneHeight;
        }

        /// <summary>
        /// Funkce <c>GetSceneDepth()</c> vrací aktuální hodnotu hloubky zorného pole kamery jako reálné číslo.
        /// </summary>
        /// <returns>Racionální číslo reprezentující hloubku zorného pole kamery jako <c>float</c>.</returns>
        public float GetSceneDepth()
        {
            return _sceneDepth;
        }
    }
}
