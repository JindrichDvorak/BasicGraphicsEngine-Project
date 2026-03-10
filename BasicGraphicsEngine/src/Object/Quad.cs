using System.Numerics;


namespace BasicGraphicsEngine
{
    public class Quad : DrawableObject
    {
        private float _width;
        private float _height;

        internal static int VertexCount = 4;
        internal static int VertexIndexStride = 7;
        internal static int InstanceIndexStride = VertexCount * VertexIndexStride;

        public Quad(Vector3 position, float width, float height, Vector4 color, float rotationAngle)
            : base(GeometryType.QUAD, new Vector3[4], position, rotationAngle, color)
        { 
            _width = width;
            _height = height;

            UpdateBaseGeometry();
        }

        public Quad(Vector3 position, float width, float height, Vector4 color)
            : this(position, width, height, color, 0) { }

        public Quad(Vector3 position, float width, float height, System.Drawing.Color color)
            : this(position, width, height, new Vector4(1, 1, 1, 1), 0)
        {
            SetColor(color);
        }

        private void UpdateBaseGeometry()
        {
            SetBaseGeometry([
                new Vector3(_width, _height, 0) / 2,
                new Vector3(-_width, _height, 0) / 2,
                new Vector3(-_width, -_height, 0) / 2,
                new Vector3(_width, -_height, 0) / 2
            ]);
        }

        override internal float[] CreateVertexData()
        {
            UpdateVertices();

            float[] vertexData = new float[InstanceIndexStride];
            int j = 0;
            for (int i = 0; i < _vertices.Length; i++)
            {
                // Position:
                vertexData[j + 0] = _vertices[i].X;
                vertexData[j + 1] = _vertices[i].Y;
                vertexData[j + 2] = _vertices[i].Z;

                // Color:
                vertexData[j + 3] = _color[0];
                vertexData[j + 4] = _color[1];
                vertexData[j + 5] = _color[2];
                vertexData[j + 6] = _color[3];

                j += VertexIndexStride;
            }

            return vertexData;
        }

        public void SetWidth(float width)
        { 
            _width = width;

            UpdateBaseGeometry();
        }

        public void SetHeight(float height)
        { 
            _height = height;

            UpdateBaseGeometry();
        }

        public float GetWidth() 
        { 
            return _width; 
        }

        public float GetHeight()
        {
            return _height;
        }
    }
}
