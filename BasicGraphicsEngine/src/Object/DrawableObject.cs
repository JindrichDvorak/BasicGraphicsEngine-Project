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

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje nastavit polohu objektu pomocí vektoru.
        /// </summary>
        /// <param name="position">3D vektor určující novou polohu objektu.</param>
        public void SetPosition(Vector3 position)
        { 
            _position = position;
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje nastavit polohu objektu zadáním jednotlivých souřadnic.
        /// </summary>
        /// <param name="x">Racionální číslo určující x-ovou souřadnici nové pozice objektu.</param>
        /// <param name="y">Racionální číslo určující y-ovou souřadnici nové pozice objektu.</param>
        /// <param name="z">Racionální číslo určující z-ovou souřadnici nové pozice objektu.</param>
        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu objektu při zachování jeho původní výšky 
        /// (z-ová souřadnice pozice) pomocí vektoru.
        /// </summary>
        /// <param name="position">2D vektor určující x-ovou a y-ovou souřadnici nové pozice objektu.</param>
        public void SetPosition(Vector2 position)
        {
            SetPosition(position.X, position.Y, _position.Z);
        }

        /// <summary>
        /// Metoda <c>SetPosition()</c> umožňuje programaticky nastavit polohu objektu při zachování jeho původní výšky 
        /// (z-ová souřadnice pozice) zadáním x-ové a y-ové souřadnice.
        /// </summary>
        /// <param name="x">Racionální číslo určující x-ovou souřadnici nové pozice objektu.</param>
        /// <param name="y">Racionální číslo určující y-ovou souřadnici nové pozice objektu.</param>
        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// Metoda <c>SetRotationAngle()</c> otočí objekt kolem osy z o úhel (v radiánech), který určuje parametr <c>rotationAngle</c>.
        /// </summary>
        /// <param name="rotationAngle">Racionální číslo reprezentující velikost úhlu v radiánech.</param>
        public void SetRotationAngle(float rotationAngle)
        { 
            _rotationAngle = rotationAngle;
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu ve formátu RGBA určenou 4D vektorem.
        /// </summary>
        /// <param name="color">4D vektor reprezentující barvu ve formátu RGBA. Jednolivé složky nabývají racionálních hodnot z intervalu (0; 1).</param>
        public void SetColor(Vector4 color)
        { 
            _color = color;
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu ve formátu RGBA určenou pomocí jednotlivých složek.
        /// </summary>
        /// <param name="r">Racionální číslo z intervalu (0; 1) reprezentující červenou složku výsledné barvy.</param>
        /// <param name="g">Racionální číslo z intervalu (0; 1) reprezentující zelenou složku výsledné barvy.</param>
        /// <param name="b">Racionální číslo z intervalu (0; 1) reprezentující modrou složku výsledné barvy.</param>
        /// <param name="a">Racionální číslo z intervalu (0; 1) reprezentující alfa složku výsledné barvy.</param>
        public void SetColor(float r, float g, float b, float a)
        {
            SetColor(new Vector4(r, g, b, a));
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu ve formátu RGB určenou 3D vektorem.
        /// </summary>
        /// <param name="color">3D vektor reprezentující barvu ve formátu RGB. Jednolivé složky nabývají racionálních hodnot z intervalu (0; 1).</param>
        public void SetColor(Vector3 color)
        {
            SetColor(color.X, color.Y, color.Z, 1.0f);
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu ve formátu RGB určenou pomocí jednotlivých složek.
        /// </summary>
        /// <param name="r">Racionální číslo z intervalu (0; 1) reprezentující červenou složku výsledné barvy.</param>
        /// <param name="g">Racionální číslo z intervalu (0; 1) reprezentující zelenou složku výsledné barvy.</param>
        /// <param name="b">Racionální číslo z intervalu (0; 1) reprezentující modrou složku výsledné barvy.</param>
        public void SetColor(float r, float g, float b)
        {
            SetColor(new Vector3(r, g, b));
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu typu <c>System.Drawing.Color</c>.
        /// </summary>
        /// <param name="color">Barva reprezentovaná objektem typu <c>System.Drawing.Color</c>.</param>
        public void SetColor(System.Drawing.Color color)
        {
            SetColor(new Vector4(color.R, color.G, color.B, color.A) / 255);
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu ve formátu RGB určenou pomocí jednotlivých složek 
        /// při zachování alfa složky předchozí barvy.
        /// </summary>
        /// <param name="r">Racionální číslo z intervalu (0; 1) reprezentující červenou složku výsledné barvy.</param>
        /// <param name="g">Racionální číslo z intervalu (0; 1) reprezentující zelenou složku výsledné barvy.</param>
        /// <param name="b">Racionální číslo z intervalu (0; 1) reprezentující modrou složku výsledné barvy.</param>
        public void SetColorRGB(float r, float g, float b)
        {
            SetColor(r, g, b, _color[3]);
        }

        /// <summary>
        /// Metoda <c>SetColor()</c> umožňuje změnit barvu objektu na zadanou barvu typu <c>System.Drawing.Color</c> 
        /// při zachování alfa složky předchozí barvy.
        /// </summary>
        /// <param name="color">Barva reprezentovaná objektem typu <c>System.Drawing.Color</c>.</param>
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
        internal void RotateBaseGeometry(float angle)
        {
            Matrix4x4 rotateMatrix = Matrix4x4.CreateRotationZ(float.DegreesToRadians(angle));

            TransformBaseGeometry(rotateMatrix);
        }

        internal void ScaleBaseGeometry(Vector3 scaleVector)
        { 
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scaleVector);

            TransformBaseGeometry(scaleMatrix);
        }

        internal void ScaleBaseGeometry(float scaleX, float scaleY, float scaleZ)
        {
            ScaleBaseGeometry(new Vector3(scaleX, scaleY, scaleZ));
        }

        internal void ScaleBaseGeometry(Vector2 scaleVector)
        {
            ScaleBaseGeometry(scaleVector.X, scaleVector.Y, 1.0f);
        }

        internal void ScaleBaseGeometry(float scaleX, float scaleY)
        {
            ScaleBaseGeometry(new Vector2(scaleX, scaleY));
        }

        internal void ScaleBaseGeometry(float scale)
        {
            ScaleBaseGeometry(scale, scale, scale);
        }

        // TODO: SetGeometryType(GeometryType geometryType);

        /// <summary>
        /// Funkce <c>GetPosition()</c> vrací okamžitou polohu objektu jako 3D vektor.
        /// </summary>
        /// <returns>Pozici objektu jako <c>Vector3</c>.</returns>
        public Vector3 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Funkce <c>GetPosition2D()</c> vrací okamžitou polohu objektu jako 2D vektor v rovině xy.
        /// </summary>
        /// <returns>Pozici objektu jako <c>Vector2</c>, který obsahuje x-ovou a y-ovou souřadnici skutečné polohy kamery (<c>Vector3</c>).</returns>
        public Vector2 GetPosition2D()
        {
            return new Vector2(_position.X, _position.Y);
        }

        /// <summary>
        /// Funkce <c>GetRotationAngle()</c> vrací aktuální úhel natočení objektu kolem osy z jako reálné číslo v radiánech.
        /// </summary>
        /// <returns>Úhel (v radiánech) natočení objektu kolem osy z jako <c>float</c>.</returns>
        public float GetRotationAngle()
        {
            return _rotationAngle;
        }

        /// <summary>
        /// Funkce <c>GetColor()</c> vrací barvu objektu ve formátu RGBA jako 4D vektor.
        /// </summary>
        /// <returns>Barvu ve formátu RGBA jako <c>Vector4</c>.</returns>
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