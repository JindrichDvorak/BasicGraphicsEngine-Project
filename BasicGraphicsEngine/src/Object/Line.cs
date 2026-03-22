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

        private Line(Vector3 position, Vector3 endPoint, float thickness, Vector4 color, float rotationAngle)
            : base(GeometryType.LINE, new Vector3[4], position, rotationAngle, color)
        {
            _endPoint = endPoint;
            _thickness = thickness;

            UpdateBaseGeometry();
        }

        /// <summary>
        /// Konstruktor třídy <c>Line</c>, který vytvoří orientovanou úsečku pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Počáteční bod úsečky: <c>Vector3</c>.</item>
        ///     <item>Koncový bod úsečky: <c>Vector3</c>.</item>
        ///     <item>Tloušťka úsečky: <c>float</c>.</item>
        ///     <item>Barva: <c>Vector4</c> (RGBA).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující počáteční bod úsečky.</param>
        /// <param name="endPoint">3D vektor určující koncový bod úsečky.</param>
        /// <param name="thickness">Racionální číslo určující tloušťku úsečky.</param>
        /// <param name="color">4D vektor určující barvu úsečky ve formátu RGBA. Jednolivé složky nabývají racionálních 
        /// hodnot z intervalu (0; 1).</param>
        public Line(Vector3 position, Vector3 endPoint, float thickness, Vector4 color)
            : this(position, endPoint, thickness, color, 0) { }

        /// <summary>
        /// Konstruktor třídy <c>Line</c>, který vytvoří orientovanou úsečku pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Počáteční bod úsečky: <c>Vector3</c>.</item>
        ///     <item>Koncový bod úsečky: <c>Vector3</c>.</item>
        ///     <item>Tloušťka úsečky: <c>float</c>.</item>
        ///     <item>Barva: <c>System.Drawing.Color</c> (RGB).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující počáteční bod úsečky.</param>
        /// <param name="endPoint">3D vektor určující koncový bod úsečky.</param>
        /// <param name="thickness">Racionální číslo určující tloušťku úsečky.</param>
        /// <param name="color">Barva reprezentovaná objektem typu <c>System.Drawing.Color</c> určující barvu úsečky ve formátu RGB.</param>
        public Line(Vector3 position, Vector3 endPoint, float thickness, System.Drawing.Color color)
            : this(position, endPoint, thickness, new Vector4(1, 1, 1, 1), 0)
        {
            SetColor(color);
        }

        private void UpdateBaseGeometry()
        {
            Vector3 dir = _endPoint - _position;
            float len = dir.Length();
            Vector3 xDir = new Vector3(1, 0, 0);
            float deviation = (float)Math.Acos(Vector3.Dot(dir, xDir) / len);

            Vector3 startVertex = new Vector3(0, _thickness, 0);
            Vector3 endVertex = new Vector3(len, 0, dir.Z);
            SetBaseGeometry([
                startVertex,
                -startVertex,
                endVertex - startVertex,
                endVertex + startVertex
            ]);

            Vector3 crossProd = Vector3.Cross(xDir, dir);
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
