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

        internal Camera(ProjectionType projectionType, uint sceneHeight, float sceneDepth, uint viewportWidth, uint viewportHeight, Vector3 position)
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

            if (forwardKeyPressed)
            {
                _position += _up * _movementSpeed * dt;
                _changedTransform = true;
            }
            if (backwardKeyPressed)
            {
                _position -= _up * _movementSpeed * dt;
                _changedTransform = true;
            }
            if (rightKeyPressed)
            {
                _position += _right * _movementSpeed * dt;
                _changedTransform = true;
            }
            if (leftKeyPressed)
            {
                _position -= _right * _movementSpeed * dt;
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

            Vector2 mouseDiff = (mousePosition - (Vector2)_lastMousePosition) * _mouseSensitivity;
            _lastMousePosition = mousePosition;

            _position -= new Vector3(mouseDiff.X, -mouseDiff.Y, 0.0f);
            UpdateVectors();
            _changedTransform = true;
        }

        internal void ResetLastMousePosition(MouseButton button)
        {
            if (button == MouseButton.Right) _lastMousePosition = null;
        }

        internal void Zoom(ScrollWheel scrollWheel)
        {
            _zoom -= scrollWheel.Y * _scrollSensitivity;
            if (_zoom < 0.005) _zoom = 0.005f;
            else if (_zoom > 5) _zoom = 5.0f;

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

        public void SetPosition(Vector3 position)
        { 
            _position = position;

            _changedTransform = true;
        }

        public void SetPosition(float x, float y, float z)
        { 
            SetPosition(new Vector3(x, y, z));
        }

        public void SetPosition(Vector2 position)
        {
            SetPosition(position.X, position.Y, 0.0f);
        }

        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        public void SetRotationAngle(float rotationAngle)
        {
            _rotationAngle = rotationAngle;

            _changedTransform = true;
        }

        public void SetZoom(float zoom)
        {
            _zoom = zoom;

            _changedProjection = true;
        }

        public void SetSceneHeight(uint sceneHeight)
        {
            _sceneHeight = sceneHeight;

            _changedProjection = true;
        }

        public void SetSceneDepth(float sceneDepth)
        { 
            _sceneDepth = sceneDepth;

            _changedProjection = true;
        }

        public Vector3 GetPosition()
        { 
            return _position;
        }

        public float GetRotationAngle()
        {
            return _rotationAngle;
        }

        public float GetZoom()
        {
            return _zoom;
        }

        public uint GetSceneHeight()
        {
            return _sceneHeight;
        }

        public float GetSceneDepth()
        {
            return _sceneDepth;
        }
    }
}
