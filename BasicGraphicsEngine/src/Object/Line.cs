using System.Numerics;


namespace BasicGraphicsEngine
{
    public class Line : DrawableObject
    {
        private Vector3 _endPoint;
        private float _thickness;

        internal static int VertexCount = 4;
        internal static int VertexIndexStride = 7;
        internal static int InstanceIndexStride = VertexCount * VertexIndexStride;

        public Line(Vector3 position, Vector3 endPoint, float thickness, Vector4 color, float rotationAngle)
            : base(GeometryType.LINE, new Vector3[4], position, rotationAngle, color)
        {
            _endPoint = endPoint;
            _thickness = thickness;

            UpdateBaseGeometry();
        }

        public Line(Vector3 position, Vector3 endPoint, float thickness, Vector4 color)
            : this(position, endPoint, thickness, color, 0) { }

        public Line(Vector3 position, Vector3 endPoint, float thickness, System.Drawing.Color color)
            : this(position, endPoint, thickness, new Vector4(1, 1, 1, 1), 0)
        {
            SetColor(color);
        }

        private void UpdateBaseGeometry()
        {
            float len = _endPoint.Length();
            Vector3 xDir = new Vector3(1, 0, 0);
            float deviation = (float)Math.Acos(Vector3.Dot(_endPoint, xDir) / len);

            Vector3 thicVec = new Vector3(0, _thickness, _endPoint.Z);
            Vector3 rotLineVec = new Vector3(len, 0, _endPoint.Z);
            SetBaseGeometry([
                thicVec,
                -thicVec,
                rotLineVec - thicVec,
                rotLineVec + thicVec
            ]);

            Vector3 crossProd = Vector3.Cross(xDir, _endPoint);
            int sign = 1;
            if (crossProd.Z < 0)
            { 
                sign = -1;
            }
            RotateBaseGeometry(sign * float.RadiansToDegrees(deviation));
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

        new public void SetRotationAngle(float rotationAngle)
        { 
            base.SetRotationAngle(rotationAngle);

            
            float len = _endPoint.Length();
            _endPoint.X = len * (float)Math.Cos(float.DegreesToRadians(rotationAngle));
            _endPoint.Y = len * (float)Math.Sin(float.DegreesToRadians(rotationAngle));
        }

        public void SetEndPoint(Vector3 endPoint)
        { 
            _endPoint = endPoint;

            UpdateBaseGeometry();
        }

        public void SetEndPoint(float x, float y, float z)
        { 
            SetEndPoint(new Vector3(x, y, z));
        }

        public void SetEndPoint(Vector2 endPoint)
        {
            SetEndPoint(endPoint.X, endPoint.Y, 0.0f);
        }

        public void SetEndPoint(float x, float y)
        {
            SetEndPoint(new Vector2(x, y));
        }

        public void SetThickness(float thickness)
        { 
            _thickness = thickness;

            UpdateBaseGeometry();
        }

        public Vector3 GetEndPoint()
        { 
            return _position + _endPoint;
        }

        public float GetThickness()
        { 
            return _thickness;
        }
    }
}
