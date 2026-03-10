using System.Numerics;


namespace BasicGraphicsEngine
{
    public class Circle : DrawableObject
    {
        private float _radius;
        private float _outlineThickness;

        internal static int VertexCount = 4;
        internal static int VertexIndexStride = 16;
        internal static int InstanceIndexStride = VertexCount * VertexIndexStride;

        private Circle(Vector3 position, float radius, Vector4 color, float outlineThickness, Vector4 outlineColor, float rotationAngle)
            : base(GeometryType.CIRCLE, new Vector3[4], position, rotationAngle, color)
        {
            _radius = radius;
            _outlineThickness = outlineThickness;
            _outlineColor = outlineColor;

            UpdateBaseGeometry();
        }

        public Circle(Vector3 position, float radius, Vector4 color, float outlineThickness, Vector4 outlineColor)
            : this(position, radius, color, outlineThickness, outlineColor, 0) { }

        public Circle(Vector3 position, float radius, System.Drawing.Color color, float outlineThickness, Vector4 outlineColor)
            : this(position, radius, new Vector4(1, 1, 1, 1), outlineThickness, outlineColor, 0) 
        {
            SetColor(color);
        }

        public Circle(Vector3 position, float radius, Vector4 color, float outlineThickness, System.Drawing.Color outlineColor)
            : this(position, radius, color, outlineThickness, new Vector4(1, 1, 1, 1), 0) 
        {
            SetOutlineColor(outlineColor);
        }

        public Circle(Vector3 position, float radius, System.Drawing.Color color, float outlineThickness, System.Drawing.Color outlineColor)
            : this(position, radius, new Vector4(1, 1, 1, 1), outlineThickness, new Vector4(1, 1, 1, 1), 0)
        {
            SetColor(color);
            SetOutlineColor(outlineColor);
        }

        public Circle(Vector3 position, float radius, Vector4 color)
            : this(position, radius, color, 0, new Vector4(0, 0, 0, 0), 0) { }

        public Circle(Vector3 position, float radius, System.Drawing.Color color)
            : this(position, radius, new Vector4(1, 1, 1, 1)) 
        {
            SetColor(color);
        }

        private void UpdateBaseGeometry()
        {
            float d = _radius * (float)Math.Sqrt(2);

            SetBaseGeometry([
                new Vector3(1, 1, 0) * d,
                new Vector3(-1, 1, 0) * d,
                new Vector3(-1, -1, 0) * d,
                new Vector3(1, -1, 0) * d
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

                // Center:
                vertexData[j + 3] = _position.X;
                vertexData[j + 4] = _position.Y;
                vertexData[j + 5] = _position.Z;

                // Radius:
                vertexData[j + 6] = _radius;

                // Outline thickness:
                vertexData[j + 7] = _outlineThickness;

                // Color:
                vertexData[j + 8] = _color[0];
                vertexData[j + 9] = _color[1];
                vertexData[j + 10] = _color[2];
                vertexData[j + 11] = _color[3];

                // Outline color:
                vertexData[j + 12] = _outlineColor[0];
                vertexData[j + 13] = _outlineColor[1];
                vertexData[j + 14] = _outlineColor[2];
                vertexData[j + 15] = _outlineColor[3];

                j += VertexIndexStride;
            }

            return vertexData;
        }

        public void SetRadius(float radius)
        { 
            _radius = radius;

            UpdateBaseGeometry();
        }

        public void SetOutlineThicness(float outlineThickness)
        {
            _outlineThickness = outlineThickness;
        }

        public void SetOutlineColor(Vector4 outlineColor)
        { 
            _outlineColor = outlineColor;
        }

        public void SetOutlineColor(float r, float g, float b, float a)
        {
            SetOutlineColor(new Vector4(r, g, b, a));
        }

        public void SetOutlineColor(Vector3 outlineColor)
        {
            SetOutlineColor(outlineColor.X, outlineColor.Y, outlineColor.Z, 1.0f);
        }

        public void SetOutlineColor(float r, float g, float b)
        {
            SetOutlineColor(new Vector3(r, g, b));
        }

        public void SetOutlineColor(System.Drawing.Color color)
        {
            SetOutlineColor(new Vector4(color.R, color.G, color.B, color.A) / 255);
        }

        public float GetRadius()
        {
            return _radius;
        }

        public float GetOutlineThickness()
        {
            return _outlineThickness;
        }

        public Vector4 GetOutlineColor()
        {
            return _outlineColor;
        }
    }
}
