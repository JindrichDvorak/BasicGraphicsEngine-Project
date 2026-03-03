using Silk.NET.OpenGL;


namespace BasicGraphicsEngine
{
    internal abstract class DynamicBuffer
    {
        private GL _gl;
        protected uint _maxInstances;
        private uint _maxVertices;
        private uint _maxIndices;

        private uint _VAO = 0;
        private uint _VBO = 0;
        private uint _IBO = 0;

        private BufferLayout _bufferLayout;

        public unsafe DynamicBuffer(GL gl)
        {
            _gl = gl;
            _VAO = _gl.GenVertexArray();
            _VBO = _gl.GenBuffer();
            _IBO = _gl.GenBuffer();
        }

        private unsafe void SetupVertexAttributes()
        {
            BufferElement[] elements = _bufferLayout.GetBufferElements();
            for (uint i = 0; i < elements.Length; i++)
            {
                BufferElement element = elements[i];

                _gl.EnableVertexAttribArray(i);
                _gl.VertexAttribPointer(
                    i,
                    element.Count,
                    element.OpenGLType,
                    element.IsNormalized,
                    _bufferLayout.GetStride(),
                    (void*)element.Offset
                );
            }
        }

        internal unsafe void CreateBuffer(uint maxVertices, uint maxIndices, BufferLayout layout, uint[] indices)
        {
            _maxVertices = maxVertices;
            _maxIndices = maxIndices;
            _bufferLayout = layout;

            _gl.BindVertexArray(_VAO);

            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _VBO);
            uint vertexStride = _bufferLayout.GetStride();
            _gl.BufferData(BufferTargetARB.ArrayBuffer, _maxVertices * vertexStride, null,
                BufferUsageARB.DynamicDraw);

            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _IBO);
            _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, _maxIndices * sizeof(uint), indices,
                BufferUsageARB.StaticDraw);

            SetupVertexAttributes();
        }

        public unsafe void SetVertexData(float[] vertexData)
        {
            _gl.BindVertexArray(_VAO);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _VBO);
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _IBO);

            _gl.BufferSubData<float>(BufferTargetARB.ArrayBuffer, 0, (nuint)vertexData.Length * sizeof(float), vertexData);
        }

        public uint GetMaxInstances()
        {
            return _maxInstances;
        }

        public uint GetMaxVertices()
        {
            return _maxVertices;
        }

        public uint GetMaxIndices()
        {
            return _maxIndices;
        }
    }
}
