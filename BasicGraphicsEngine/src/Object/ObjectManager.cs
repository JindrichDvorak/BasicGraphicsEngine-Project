using System.Numerics;


namespace BasicGraphicsEngine
{
    internal struct VertexData
    {
        public float[] ParticleVertexData;
        public float[] QuadVertexData;
        public float[] LineVertexData;
        public float[] CircleVertexData;
    }

    internal class ObjectManager
    {
        private int _maxParticles;
        private List<Particle> _particles = [];

        private int _maxQuads;
        private List<Quad> _quads = [];

        private int _maxLines;
        private List<Line> _lines = [];

        private int _maxCircles;
        private List<Circle> _circles = [];

        private VertexData _vertexData;

        public ObjectManager(int maxParticles, int maxQuads, int maxLines, int maxCircles)
        {
            _maxParticles = maxParticles;
            _maxQuads = maxQuads;
            _maxLines = maxLines;
            _maxCircles = maxCircles;
        }

        public void AddObject(DrawableObject obj)
        {
            switch (obj.GetGeometryType())
            {
                case GeometryType.PARTICLE:
                    if (_particles.Count < _maxParticles)
                    {
                        _particles.Add((Particle)obj);
                        _particles.Sort((a, b) => Comparer<float>.Default.Compare(b.GetPosition()[2], a.GetPosition()[2]));
                    } 
                    break;
                case GeometryType.QUAD:
                    if (_quads.Count < _maxQuads)
                    {
                        _quads.Add((Quad)obj);
                        _particles.Sort((a, b) => Comparer<float>.Default.Compare(b.GetPosition()[2], a.GetPosition()[2]));
                    } 
                    break;
                case GeometryType.LINE:
                    if (_lines.Count < _maxLines)
                    {
                        _lines.Add((Line)obj);
                        _particles.Sort((a, b) => Comparer<float>.Default.Compare(b.GetPosition()[2], a.GetPosition()[2]));
                    } 
                    break;
                case GeometryType.CIRCLE:
                    if (_circles.Count < _maxCircles)
                    {
                        _circles.Add((Circle)obj);
                        _particles.Sort((a, b) => Comparer<float>.Default.Compare(b.GetPosition()[2], a.GetPosition()[2]));
                    }
                    break;
            }
        }

        public void RemoveObject(DrawableObject obj)
        {
            switch (obj.GetGeometryType())
            {
                case GeometryType.PARTICLE: _particles.Remove((Particle)obj); break;
                case GeometryType.QUAD: _quads.Remove((Quad)obj); break;
                case GeometryType.LINE: _lines.Remove((Line)obj); break;
                case GeometryType.CIRCLE: _circles.Remove((Circle)obj); break;
            }
        }

        private void UpdateParticleData()
        {
            int numOfInstances = _particles.Count;
            int vertexIndexStride = 1 * 8;
            _vertexData.ParticleVertexData = new float[numOfInstances * vertexIndexStride];

            int j = 0;
            for (int i = 0; i < numOfInstances; i++)
            {
                _particles[i].UpdateVertices();
                float[] instanceData = _particles[i].CreateVertexData();

                Array.Copy(instanceData, 0, _vertexData.ParticleVertexData, j, vertexIndexStride);

                j += vertexIndexStride;
            }
        }

        private void UpdateQuadData()
        {
            int numOfInstances = _quads.Count;
            int vertexIndexStride = 4 * 7;
            _vertexData.QuadVertexData = new float[numOfInstances * vertexIndexStride];

            int j = 0;
            for (int i = 0; i < numOfInstances; i++)
            {
                _quads[i].UpdateVertices();
                float[] instanceData = _quads[i].CreateVertexData();

                Array.Copy(instanceData, 0, _vertexData.QuadVertexData, j, vertexIndexStride);

                j += vertexIndexStride;
            }
        }

        private void UpdateLineData()
        {
            int numOfInstances = _lines.Count;
            int vertexIndexStride = 4 * 7;
            _vertexData.LineVertexData = new float[numOfInstances * vertexIndexStride];

            int j = 0;
            for (int i = 0; i < numOfInstances; i++)
            {
                _lines[i].UpdateVertices();
                float[] instanceData = _lines[i].CreateVertexData();

                Array.Copy(instanceData, 0, _vertexData.LineVertexData, j, vertexIndexStride);

                j += vertexIndexStride;
            }
        }

        private void UpdateCircleData()
        {
            int numOfInstances = _circles.Count;
            int vertexIndexStride = 4 * 16;
            _vertexData.CircleVertexData = new float[numOfInstances * vertexIndexStride];

            int j = 0;
            for (int i = 0; i < numOfInstances; i++)
            {
                _circles[i].UpdateVertices();
                float[] instanceData = _circles[i].CreateVertexData();

                Array.Copy(instanceData, 0, _vertexData.CircleVertexData, j, vertexIndexStride);

                j += vertexIndexStride;
            }
        }

        public void UpdateVertexData()
        {
            UpdateParticleData();
            UpdateQuadData();
            UpdateLineData();
            UpdateCircleData();
        }

        public VertexData GetVertexData()
        { 
            return _vertexData;
        }
    }
}
