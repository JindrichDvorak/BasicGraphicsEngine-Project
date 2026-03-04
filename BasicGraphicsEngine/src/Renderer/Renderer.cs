using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;


namespace BasicGraphicsEngine
{
    internal class Renderer
    {
        private GL _gl;
        private Camera _camera;

        // Points:
        private Shader _particleShader;
        private ParticleBuffer _particleBuffer;

        // Quads:
        private Shader _quadShader;
        private QuadBuffer _quadBuffer;

        // Lines (same geometry as quads):
        private Shader _lineShader;
        private QuadBuffer _lineBuffer;

        // Circles:
        private Shader _circleShader;
        private CircleBuffer _circleBuffer;

        private Color _clearColor = Color.CornflowerBlue;

        public Renderer(IWindow window, uint viewportWidth, uint viewportHeight, uint maxParticles, uint maxQuads, uint maxLines, uint maxCircles, 
            uint cameraSceneHeight, float cameraSceneDepth, Vector3 cameraPosition, Color backgroundColor)
        {
            _gl = window.CreateOpenGL();
            _gl.Enable(EnableCap.DepthTest);
            _gl.Enable(GLEnum.ProgramPointSize);
            _gl.Enable(GLEnum.Multisample);
            _gl.Enable(EnableCap.Blend);
            _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            _gl.Viewport(0, 0, viewportWidth, viewportHeight);
            _camera = new Camera(ProjectionType.ORTHOGRAPHIC, cameraSceneHeight, cameraSceneDepth, viewportWidth, viewportHeight, cameraPosition);

            _particleShader = new Shader(_gl,
                "Particle",
                "src/Renderer/Shader/particleShader_vertex.glsl",
                "src/Renderer/Shader/particleShader_fragment.glsl"
            );
            _particleBuffer = new ParticleBuffer(_gl, maxParticles);

            _quadShader = new Shader(_gl,
                "Quad",
                "src/Renderer/Shader/quadShader_vertex.glsl",
                "src/Renderer/Shader/quadShader_fragment.glsl"
            );
            _quadBuffer = new QuadBuffer(_gl, maxQuads);

            _lineShader = _quadShader;
            _lineBuffer = new QuadBuffer(_gl, maxLines);

            _circleShader = new Shader(_gl,
                "Circle",
                "src/Renderer/Shader/circleShader_vertex.glsl",
                "src/Renderer/Shader/circleShader_fragment.glsl"
            );
            _circleBuffer = new CircleBuffer(_gl, maxCircles);

            SetClearColor(backgroundColor);
        }

        public void Render(float dt, IInputContext input, VertexData vertexData)
        {
            _camera.Update(input, dt);

            if (vertexData.ParticleVertexData != null) RenderParticles(vertexData.ParticleVertexData);
            if (vertexData.QuadVertexData != null) RenderQuads(vertexData.QuadVertexData);
            if (vertexData.LineVertexData != null) RenderLines(vertexData.LineVertexData);
            if (vertexData.CircleVertexData != null) RenderCircles(vertexData.CircleVertexData);
        }

        private unsafe void RenderParticles(float[] vertices)
        {
            _particleShader.UseShader();
            _particleShader.SetMat4("viewMatrix", _camera.GetViewMatrix());

            _particleBuffer.SetVertexData(vertices);
            _gl.DrawElements(PrimitiveType.Points, _particleBuffer.GetMaxIndices(), DrawElementsType.UnsignedInt, (void*)0);
        }

        private unsafe void RenderQuads(float[] vertices)
        {
            _quadShader.UseShader();
            _quadShader.SetMat4("viewMatrix", _camera.GetViewMatrix());

            _quadBuffer.SetVertexData(vertices);
            _gl.DrawElements(PrimitiveType.Triangles, _quadBuffer.GetMaxIndices(), DrawElementsType.UnsignedInt, (void*)0);
        }

        private unsafe void RenderLines(float[] vertices)
        {
            _lineShader.UseShader();
            _lineShader.SetMat4("viewMatrix", _camera.GetViewMatrix());

            _lineBuffer.SetVertexData(vertices);
            _gl.DrawElements(PrimitiveType.Triangles, _lineBuffer.GetMaxIndices(), DrawElementsType.UnsignedInt, (void*)0);
        }

        private unsafe void RenderCircles(float[] vertices)
        {
            _circleShader.UseShader();
            _circleShader.SetMat4("viewMatrix", _camera.GetViewMatrix());

            _circleBuffer.SetVertexData(vertices);
            _gl.DrawElements(PrimitiveType.Triangles, _quadBuffer.GetMaxIndices(), DrawElementsType.UnsignedInt, (void*)0);
        }

        public void SetClearColor(Color color) 
        {
            _clearColor = color;
            _gl.ClearColor(color);
        }

        public void ClearWindow()
        {
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public void OnWindowResize(uint viewportWidth, uint viewportHeight)
        {
            _gl.Viewport(0, 0, viewportWidth, viewportHeight);
            _camera.SetViewportSize(viewportWidth, viewportHeight);
        }

        public void OnKeyDown(Key key) { }

        public void OnMouseMove(IMouse mouse, Vector2 mousePosition)
        {
            if(mouse.IsButtonPressed(MouseButton.Right)) _camera.Pan(mousePosition);
        }

        public void OnMouseScroll(ScrollWheel scrollWheel)
        {
            _camera.Zoom(scrollWheel);
        }

        public void OnMouseUp(MouseButton button)
        {
            _camera.ResetLastMousePosition(button);
        }
    }
}
