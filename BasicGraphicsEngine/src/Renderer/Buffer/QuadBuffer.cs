using Silk.NET.OpenGL;


namespace BasicGraphicsEngine
{
    internal class QuadBuffer : DynamicBuffer
    {
        public QuadBuffer(GL gl, uint maxInstances) : base(gl)
        { 
            _maxInstances = maxInstances;

            BufferLayout layout = new BufferLayout(new BufferElement[]{
                new BufferElement("position", BufferDataType.VEC_3, false),
                new BufferElement("color", BufferDataType.VEC_4, false)
            });

            uint[] indices = new uint[maxInstances * 6];
            uint j = 0;
            for (uint i = 0; i < maxInstances * 6; i += 6)
            {
                indices[i + 0] = j + 0;
                indices[i + 1] = j + 1;
                indices[i + 2] = j + 2;
                indices[i + 3] = j + 2;
                indices[i + 4] = j + 3;
                indices[i + 5] = j + 0;

                j += 4;
            }

            CreateBuffer(maxInstances * 4, maxInstances * 6, layout, indices);
        }
    }
}
