using Silk.NET.OpenGL;

namespace BasicGraphicsEngine
{
    internal class ParticleBuffer : DynamicBuffer
    {
        public ParticleBuffer(GL gl, uint maxInstances) : base(gl)
        {
            _maxInstances = maxInstances;

            BufferLayout layout = new BufferLayout(new BufferElement[]{
                new BufferElement("position", BufferDataType.VEC_3, false),
                new BufferElement("size", BufferDataType.FLOAT, false),
                new BufferElement("color", BufferDataType.VEC_4, false)
            });

            uint[] indices = new uint[maxInstances];
            for (uint i = 0; i < maxInstances; i++)
            {
                indices[i] = i;
            }

            CreateBuffer(maxInstances, maxInstances, layout, indices);
        }
    }
}
