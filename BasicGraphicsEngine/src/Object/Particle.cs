using System.Numerics;

namespace BasicGraphicsEngine
{
    public class Particle : DrawableObject
    {
        private float _size;

        internal static int VertexCount = 1;
        internal static int VertexIndexStride = 8;
        internal static int InstanceIndexStride = VertexCount * VertexIndexStride;

        /// <summary>
        /// Konstruktor třídy <c>Particle</c>, který vytvoří bod (ve tvaru čtverce) pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Poloha bodu: <c>Vector3</c>.</item>
        ///     <item>Velikost bodu: <c>float</c>.</item>
        ///     <item>Barva: <c>Vector4</c> (RGBA).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující polohu bodu.</param>
        /// <param name="size">Racionální číslo určující velikost bodu.</param>
        /// <param name="color">4D vektor určující barvu bodu ve formátu RGBA. Jednolivé složky nabývají racionálních 
        /// hodnot z intervalu (0; 1).</param>
        public Particle(Vector3 position, float size, Vector4 color)
            : base(GeometryType.PARTICLE, [
                new Vector3(0, 0, 0)
            ], position, 0, color)
        {
            _size = size;
        }

        /// <summary>
        /// Konstruktor třídy <c>Particle</c>, který vytvoří bod (ve tvaru čtverce) pomocí následujících parametrů (v uvedeném pořadí):
        /// <list type="bullet">
        ///     <item>Poloha bodu: <c>Vector3</c>.</item>
        ///     <item>Velikost bodu: <c>float</c>.</item>
        ///     <item>Barva: <c>System.Drawing.Color</c> (RGB).</item>
        /// </list>
        /// </summary>
        /// <param name="position">3D vektor určující polohu bodu.</param>
        /// <param name="size">Racionální číslo určující velikost bodu.</param>
        /// <param name="color">Barva reprezentovaná objektem typu <c>System.Drawing.Color</c> určující barvu bodu ve formátu RGB.</param>
        public Particle(Vector3 position, float size, System.Drawing.Color color)
            : this(position, size, new Vector4(1, 1, 1, 1))
        {
            SetColor(color);
        }

        internal override float[] CreateVertexData()
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

                // Size:
                vertexData[j + 3] = _size;

                // Color:
                vertexData[j + 4] = _color[0];
                vertexData[j + 5] = _color[1];
                vertexData[j + 6] = _color[2];
                vertexData[j + 7] = _color[3];

                j += VertexIndexStride;
            }

            return vertexData;
        }
    }
}
