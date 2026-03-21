using System.Collections.Specialized;
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

    internal struct RenderBatch(GeometryType type, float[] vertexData)
    {
        public GeometryType Type = type;
        public float[] VertexData = vertexData;
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

        // Rendering data:
        private VertexData _vertexDataOpaque;
        private List<RenderBatch> _transparentBatches = [];

        private SortedDictionary<float, DrawableObject> _opaqueObjects = new SortedDictionary<float, DrawableObject>(
            //Comparer<float>.Create((a, b) => b.CompareTo(a))
        );
        private SortedDictionary<float, DrawableObject> _transparentObjects = new SortedDictionary<float, DrawableObject>(
            Comparer<float>.Create((a, b) => b.CompareTo(a))
        );

        private float _zAddDiff = 0.001f;

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
                    } 
                    break;
                case GeometryType.QUAD:
                    if (_quads.Count < _maxQuads)
                    {
                        _quads.Add((Quad)obj);
                    } 
                    break;
                case GeometryType.LINE:
                    if (_lines.Count < _maxLines)
                    {
                        _lines.Add((Line)obj);
                    } 
                    break;
                case GeometryType.CIRCLE:
                    if (_circles.Count < _maxCircles)
                    {
                        _circles.Add((Circle)obj);
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

        private void SortAndUpdateObjects<T>(List<T> objects) where T : DrawableObject
        {
            _opaqueObjects.Clear();

            for (int i = 0; i < objects.Count; i++)
            {
                DrawableObject obj = objects[i];
                obj.UpdateVertices();

                float objZ = -obj.GetPosition()[2];
                bool addSuccess = false;
                if (obj.GetColor()[3] < 1.0f || obj.GetOutlineColorInternal()[3] < 1.0f)
                {
                    while (!addSuccess)
                    {
                        addSuccess = _transparentObjects.TryAdd(objZ, obj);
                        objZ += _zAddDiff;
                    }
                }
                else
                {
                    while (!addSuccess)
                    {
                        addSuccess = _opaqueObjects.TryAdd(objZ, obj);
                        objZ += _zAddDiff;
                    }
                }
            }
        }

        private void UpdateParticleData()
        {
            SortAndUpdateObjects(_particles);

            _vertexDataOpaque.ParticleVertexData = new float[_opaqueObjects.Count * Particle.InstanceIndexStride];
            int j = 0;
            foreach (var keyValue in _opaqueObjects)
            {
                float[] instanceData = keyValue.Value.CreateVertexData();

                Array.Copy(instanceData, 0, _vertexDataOpaque.ParticleVertexData, j, Particle.InstanceIndexStride);

                j += Particle.InstanceIndexStride;
            }
        }

        private void UpdateQuadData()
        {
            SortAndUpdateObjects(_quads);

            _vertexDataOpaque.QuadVertexData = new float[_opaqueObjects.Count * Quad.InstanceIndexStride];
            int j = 0;
            foreach (var keyValue in _opaqueObjects)
            {
                float[] instanceData = keyValue.Value.CreateVertexData();

                Array.Copy(instanceData, 0, _vertexDataOpaque.QuadVertexData, j, Quad.InstanceIndexStride);

                j += Quad.InstanceIndexStride;
            }
        }

        private void UpdateLineData()
        {
            SortAndUpdateObjects(_lines);

            _vertexDataOpaque.LineVertexData = new float[_opaqueObjects.Count * Line.InstanceIndexStride];
            int j = 0;
            foreach (var keyValue in _opaqueObjects)
            {
                float[] instanceData = keyValue.Value.CreateVertexData();

                Array.Copy(instanceData, 0, _vertexDataOpaque.LineVertexData, j, Line.InstanceIndexStride);

                j += Line.InstanceIndexStride;
            }
        }

        private void UpdateCircleData()
        {
            SortAndUpdateObjects(_circles);

            _vertexDataOpaque.CircleVertexData = new float[_opaqueObjects.Count * Circle.InstanceIndexStride];
            int j = 0;
            foreach (var keyValue in _opaqueObjects)
            {
                float[] instanceData = keyValue.Value.CreateVertexData();

                Array.Copy(instanceData, 0, _vertexDataOpaque.CircleVertexData, j, Circle.InstanceIndexStride);

                j += Circle.InstanceIndexStride;
            }
        }

        private RenderBatch CreateRenderBatch(GeometryType type, List<DrawableObject> objects)
        {
            int instanceIndexStride = 1;
            switch (type)
            {
                case GeometryType.PARTICLE: instanceIndexStride = Particle.InstanceIndexStride; break;
                case GeometryType.QUAD: instanceIndexStride = Quad.InstanceIndexStride; break;
                case GeometryType.LINE: instanceIndexStride = Line.InstanceIndexStride; break;
                case GeometryType.CIRCLE: instanceIndexStride = Circle.InstanceIndexStride; break;
            }

            float[] vertexData = new float[objects.Count * instanceIndexStride];
            int j = 0;
            foreach (DrawableObject obj in objects)
            {
                float[] instanceData = obj.CreateVertexData();

                Array.Copy(instanceData, 0, vertexData, j, instanceIndexStride);

                j += instanceIndexStride;
            }

            return new RenderBatch(type, vertexData);
        }

        private void CreateTransparentBatches()
        {
            _transparentBatches.Clear();

            GeometryType lastGeometryType = GeometryType.NONE;
            List<DrawableObject> objects = [];
            int i = 1;
            foreach (var keyValue in _transparentObjects)
            { 
                DrawableObject obj = keyValue.Value;
                GeometryType type = obj.GetGeometryType();

                if(i == 1) objects.Add(obj);
                else 
                {
                    if (lastGeometryType == type)
                    {
                        objects.Add(obj);
                    }
                    else
                    {
                        _transparentBatches.Add(CreateRenderBatch(lastGeometryType, objects));

                        objects.Clear();
                        objects.Add(obj);
                    }
                }

                lastGeometryType = type;
                i++;
            }

            _transparentBatches.Add(CreateRenderBatch(lastGeometryType, objects));
        }

        public void UpdateVertexData()
        {
            _transparentObjects.Clear();

            UpdateParticleData();
            UpdateQuadData();
            UpdateLineData();
            UpdateCircleData();

            CreateTransparentBatches();
        }

        public VertexData GetVertexDataOpaque()
        { 
            return _vertexDataOpaque;
        }

        public List<RenderBatch> GetTransparentBatches()
        {
            return _transparentBatches;
        }
    }
}
