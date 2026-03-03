using Silk.NET.OpenGL;


namespace BasicGraphicsEngine
{
    internal enum BufferDataType
    { 
        BOOL,
        INT,
        FLOAT,
        VEC_2,
        VEC_3,
        VEC_4,
        MAT_4
    }

    internal struct BufferElement(string name, BufferDataType type, bool isNormalized)
    {
        public string Name = name;
        public BufferDataType Type = type;
        public bool IsNormalized = isNormalized;
        public VertexAttribPointerType OpenGLType = ConvertType(type);
        public uint Size = GetTypeSize(type);
        public int Count = GetTypeCount(type);
        public uint Offset = 0;

        private static VertexAttribPointerType ConvertType(BufferDataType type)
        {
            switch (type)
            {
                case BufferDataType.BOOL:
                    return VertexAttribPointerType.Int;
                case BufferDataType.INT:
                    return VertexAttribPointerType.Int;
                case BufferDataType.FLOAT:
                    return VertexAttribPointerType.Float;
                case BufferDataType.VEC_2:
                    return VertexAttribPointerType.Float;
                case BufferDataType.VEC_3:
                    return VertexAttribPointerType.Float;
                case BufferDataType.VEC_4:
                    return VertexAttribPointerType.Float;
                case BufferDataType.MAT_4:
                    return VertexAttribPointerType.Float;
                default:
                    break;
            }

            return VertexAttribPointerType.Float;
        }

        private static uint GetTypeSize(BufferDataType type)
        {
            switch (type)
            {
                case BufferDataType.BOOL:
                    return 4;
                case BufferDataType.INT:
                    return 4;
                case BufferDataType.FLOAT:
                    return 4;
                case BufferDataType.VEC_2:
                    return 8;
                case BufferDataType.VEC_3:
                    return 12;
                case BufferDataType.VEC_4:
                    return 16;
                case BufferDataType.MAT_4:
                    return 64;
                default: 
                    break;
            }

            return 0;
        }

        private static int GetTypeCount(BufferDataType type)
        {
            switch (type)
            {
                case BufferDataType.BOOL:
                    return 1;
                case BufferDataType.INT:
                    return 1;
                case BufferDataType.FLOAT:
                    return 1;
                case BufferDataType.VEC_2:
                    return 2;
                case BufferDataType.VEC_3:
                    return 3;
                case BufferDataType.VEC_4:
                    return 4;
                case BufferDataType.MAT_4:
                    return 16;
                default:
                    break;
            }

            return 0;
        }


    }

    internal class BufferLayout
    {
        private BufferElement[] _bufferElements;
        private uint _stride;

        public unsafe BufferLayout(BufferElement[] bufferElements)
        {
            _bufferElements = bufferElements;

            uint offset = 0;
            for(uint i = 0; i < _bufferElements.Length; i++)
            {
                _bufferElements[i].Offset = offset;
                offset += _bufferElements[i].Size;
            }
            _stride = offset;
        }

        public BufferElement[] GetBufferElements()
        { 
            return _bufferElements;
        }

        public uint GetStride()
        { 
            return _stride;
        }
    }
}
