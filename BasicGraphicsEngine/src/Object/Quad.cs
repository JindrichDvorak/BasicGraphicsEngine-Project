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

        private Quad(Vector3 position, float width, float height, Vector4 color, float rotationAngle)
            : base(GeometryType.QUAD, new Vector3[4], position, rotationAngle, color)
        { 
            _width = width;
            _height = height;

            UpdateBaseGeometry();
        }

        /// <summary>
        /// Konstruktor třídy <c>Quad</c>, který vytvoří obdélník pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Střed obdélníku: <c>Vector3</c>.</item>
        ///     <item>Šířka obdélníku: <c>float</c>.</item>
        ///     <item>Výška obdélníku: <c>float</c>.</item>
        ///     <item>Barva: <c>Vector4</c> (RGBA).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující střed obdélníku.</param>
        /// <param name="width">Racionální číslo určující šířku obdélníku.</param>
        /// <param name="height">Racionální číslo určující výšku obdélníku.</param>
        /// <param name="color">4D vektor určující barvu obdélníku ve formátu RGBA. Jednolivé složky nabývají racionálních 
        /// hodnot z intervalu (0; 1).</param>
        public Quad(Vector3 position, float width, float height, Vector4 color)
            : this(position, width, height, color, 0) { }

        /// <summary>
        /// Konstruktor třídy <c>Quad</c>, který vytvoří obdélník pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Střed obdélníku: <c>Vector3</c>.</item>
        ///     <item>Šířka obdélníku: <c>float</c>.</item>
        ///     <item>Výška obdélníku: <c>float</c>.</item>
        ///     <item>Barva: <c>System.Drawing.Color</c> (RGB).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující střed obdélníku.</param>
        /// <param name="width">Racionální číslo určující šířku obdélníku.</param>
        /// <param name="height">Racionální číslo určující výšku obdélníku.</param>
        /// <param name="color">Barva reprezentovaná objektem typu <c>System.Drawing.Color</c> určující barvu obdélníku ve formátu RGB.</param>
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
