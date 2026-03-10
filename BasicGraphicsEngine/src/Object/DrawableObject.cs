using System.Numerics;
using System.Runtime.CompilerServices;


namespace BasicGraphicsEngine
{
    internal enum GeometryType
    { 
        PARTICLE,
        TRIANGLE,
        QUAD,
        LINE,
        CIRCLE,
        CUBE,
        NONE
    }

    public abstract class DrawableObject
    {
        private GeometryType _geometryType;
        private Vector3[] _baseGeometry;
        protected Vector3 _position;
        protected float _rotationAngle;
        protected Vector4 _color;
        protected Vector4 _outlineColor = new Vector4(0, 0, 0, 1.0f);

        protected Vector3[] _vertices;

        internal DrawableObject(GeometryType geometryType, Vector3[] baseGeometry, Vector3 position, float rotationAngle, Vector4 color)
        {
            _geometryType = geometryType;
            _baseGeometry = baseGeometry;
            _position = position;
            _rotationAngle = rotationAngle;
            _color = color;

            _vertices = new Vector3[baseGeometry.Length];
        }

        internal static Vector3[] CreateDefaultGeometry(GeometryType geometryType)
        {
            switch (geometryType)
            {
                case GeometryType.TRIANGLE: return DefaultTriangleGeometry();
                case GeometryType.QUAD: return DefaultQuadGeometry();
                case GeometryType.CIRCLE: return DefaultCircleGeometry();
                case GeometryType.CUBE: return DefaultCubeGeometry();
            }

            return DefaultQuadGeometry();
        }

        private static Vector3[] DefaultTriangleGeometry()
        {
            Vector3[] vertices = new Vector3[3];

            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(120.0f));

            vertices[0] = new Vector3(0, 1, 0);
            vertices[1] = Vector3.Transform(vertices[0], rotateMatrix);
            vertices[2] = Vector3.Transform(vertices[1], rotateMatrix);

            return vertices;
        }

        private static Vector3[] DefaultQuadGeometry()
        {
            Vector3[] vertices = new Vector3[4];

            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(90.0f));

            vertices[0] = new Vector3(1, 1, 0);
            vertices[1] = Vector3.Transform(vertices[0], rotateMatrix);
            vertices[2] = Vector3.Transform(vertices[1], rotateMatrix);
            vertices[3] = Vector3.Transform(vertices[2], rotateMatrix);

            return vertices;
        }

        private static Vector3[] DefaultCircleGeometry()
        {
            return DefaultQuadGeometry();
        }

        private static Vector3[] DefaultCubeGeometry()
        {
            Vector3[] vertices = new Vector3[8];

            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(90.0f));

            Vector3 v1 = new Vector3(1, 1, 0);
            Vector3 v2 = Vector3.Transform(v1, rotateMatrix);
            Vector3 v3 = Vector3.Transform(v2, rotateMatrix);
            Vector3 v4 = Vector3.Transform(v3, rotateMatrix);

            Matrix4x4 translateMatrixZplus = Matrix4x4.CreateTranslation(new Vector3(0, 0, 1));
            Matrix4x4 translateMatrixZminus = Matrix4x4.CreateTranslation(new Vector3(0, 0, -1));

            vertices[0] = Vector3.Transform(v1, translateMatrixZplus);
            vertices[1] = Vector3.Transform(v2, translateMatrixZplus);
            vertices[2] = Vector3.Transform(v3, translateMatrixZplus);
            vertices[3] = Vector3.Transform(v4, translateMatrixZplus);

            vertices[4] = Vector3.Transform(v1, translateMatrixZminus);
            vertices[5] = Vector3.Transform(v2, translateMatrixZminus);
            vertices[6] = Vector3.Transform(v3, translateMatrixZminus);
            vertices[7] = Vector3.Transform(v4, translateMatrixZminus);

            return vertices;
        }

        // TODO: Expand to cover rotation around a custom axis.
        //           => 3D transformations are not fully supported.
        internal void UpdateVertices()
        {
            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(_rotationAngle));
            Matrix4x4 translateMatrix = Matrix4x4.CreateTranslation(_position);

            Matrix4x4 transformMatrix = rotateMatrix * translateMatrix;
            for (int i = 0; i < _baseGeometry.Length; i++)
            {
                _vertices[i] = Vector3.Transform(_baseGeometry[i], transformMatrix);
            }
        }

        public void SetPosition(Vector3 position)
        { 
            _position = position;
        }

        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        public void SetPosition(Vector2 position)
        {
            SetPosition(position.X, position.Y, _position.Z);
        }

        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        public void SetRotationAngle(float rotationAngle)
        { 
            _rotationAngle = rotationAngle;
        }

        public void SetColor(Vector4 color)
        { 
            _color = color;
        }

        public void SetColor(float r, float g, float b, float a)
        {
            SetColor(new Vector4(r, g, b, a));
        }

        public void SetColor(Vector3 color)
        {
            SetColor(color.X, color.Y, color.Z, 1.0f);
        }

        public void SetColor(float r, float g, float b)
        {
            SetColor(new Vector3(r, g, b));
        }

        public void SetColor(System.Drawing.Color color)
        {
            SetColor(new Vector4(color.R, color.G, color.B, color.A) / 255);
        }

        public void SetColorRGB(float r, float g, float b)
        {
            SetColor(r, g, b, _color[3]);
        }

        public void SetColorRGB(System.Drawing.Color color)
        {
            SetColorRGB(color.R, color.G, color.B);
        }

        // TODO: Check if the new geometry has the correct number of vertices.
        internal void SetBaseGeometry(Vector3[] baseGeometry)
        {
            _baseGeometry = baseGeometry;
        }

        internal void TransformBaseGeometry(Matrix4x4 transformMatrix)
        {
            for (int i = 0; i < _baseGeometry.Length; i++)
            {
                _baseGeometry[i] = Vector3.Transform(_baseGeometry[i], transformMatrix);
            }
        }

        // TODO: Expand to cover rotation around a custom axis.
        //           => 3D transformations are not fully supported.
        public void RotateBaseGeometry(float angle)
        {
            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(angle));

            TransformBaseGeometry(rotateMatrix);
        }

        public void ScaleBaseGeometry(Vector3 scaleVector)
        { 
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scaleVector);

            TransformBaseGeometry(scaleMatrix);
        }

        public void ScaleBaseGeometry(float scaleX, float scaleY, float scaleZ)
        {
            ScaleBaseGeometry(new Vector3(scaleX, scaleY, scaleZ));
        }

        public void ScaleBaseGeometry(Vector2 scaleVector)
        {
            ScaleBaseGeometry(scaleVector.X, scaleVector.Y, 1.0f);
        }

        public void ScaleBaseGeometry(float scaleX, float scaleY)
        {
            ScaleBaseGeometry(new Vector2(scaleX, scaleY));
        }

        public void ScaleBaseGeometry(float scale)
        {
            ScaleBaseGeometry(scale, scale, scale);
        }

        // TODO: SetGeometryType(GeometryType geometryType);

        public Vector3 GetPosition()
        {
            return _position;
        }

        public Vector2 GetPosition2D()
        {
            return new Vector2(_position.X, _position.Y);
        }

        public float GetRotationAngle()
        {
            return _rotationAngle;
        }

        public Vector4 GetColor()
        {
            return _color;
        }

        internal Vector4 GetOutlineColorInternal()
        {
            return _outlineColor;
        }

        internal GeometryType GetGeometryType()
        {
            return _geometryType;
        }

        abstract internal float[] CreateVertexData();
    }
}