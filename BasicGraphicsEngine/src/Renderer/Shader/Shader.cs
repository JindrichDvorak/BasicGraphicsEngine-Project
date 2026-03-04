using Silk.NET.OpenGL;
using System.Numerics;


namespace BasicGraphicsEngine
{
    internal class Shader
    {
        private uint _id;
        private Dictionary<string, int> _uniformLocations;

        private GL _gl;

        private string _shaderName;

        public Shader(GL gl, string shaderName, string VertexSourcePath, string FragmentSourcePath)
        {
            _gl = gl;
            _uniformLocations = new Dictionary<string, int>();
            _shaderName = shaderName;

            string vertexSource = ParseSource(VertexSourcePath);
            uint vertexShader = CompileShader(ShaderSourceType.VERTEX, vertexSource);

            string fragmentSource = ParseSource(FragmentSourcePath);
            uint fragmentShader = CompileShader(ShaderSourceType.FRAGMENT, fragmentSource);

            CreateShaderProgram(vertexShader, fragmentShader);
        }

        public void UseShader()
        {
            _gl.UseProgram(_id);
        }

        private enum ShaderSourceType
        { 
            VERTEX,
            FRAGMENT,
            NONE
        };

        private string GetShaderTypeString(ShaderSourceType type)
        {
            switch (type)
            {
                case ShaderSourceType.VERTEX: return "Vertex";
                case ShaderSourceType.FRAGMENT: return "Fragment";
            }

            return "None";
        }

        private string ParseSource(string sourcePath)
        {
            ShaderSourceType type = ShaderSourceType.NONE;
            string sourceString = "";

            StreamReader sr = new StreamReader(sourcePath);
            string? line = sr.ReadLine();
            while (line != null)
            {
                if (line.Contains("//Source"))
                {
                    if (line.Contains("Vertex"))
                    {
                        type = ShaderSourceType.VERTEX;
                    }
                    else if (line.Contains("Fragment"))
                    {
                        type = ShaderSourceType.FRAGMENT;
                    }
                }
                else
                {
                    if (type == ShaderSourceType.NONE)
                    {
                        Console.WriteLine("Wrong shader source");
                        return sourceString;
                    }
                    if (sourceString == "")
                    {
                        sourceString = line;
                    }
                    else
                    {
                        sourceString += "\n" + line;
                    }
                }
                line = sr.ReadLine();
            }
            return sourceString;
        }

        private uint CompileShader(ShaderSourceType shaderType, string source)
        {
            uint shaderId = 0;
            switch (shaderType)
            {
                case ShaderSourceType.VERTEX:
                    shaderId = _gl.CreateShader(ShaderType.VertexShader);
                    break;
                case ShaderSourceType.FRAGMENT:
                    shaderId = _gl.CreateShader(ShaderType.FragmentShader);
                    break;
            }
            
            _gl.ShaderSource(shaderId, source);
            _gl.CompileShader(shaderId);

            _gl.GetShader(shaderId, ShaderParameterName.CompileStatus, out int vStatus);
            if (vStatus != (int)GLEnum.True) 
                throw new Exception($"{_shaderName}: {GetShaderTypeString(shaderType)} shader failed to compile: " + _gl.GetShaderInfoLog(shaderId));

            return shaderId;
        }

        private void CreateShaderProgram(uint vertexShader, uint fragmentShader)
        {
            _id = _gl.CreateProgram();
            _gl.AttachShader(_id, vertexShader);
            _gl.AttachShader(_id, fragmentShader);
            _gl.LinkProgram(_id);

            _gl.GetProgram(_id, ProgramPropertyARB.LinkStatus, out int lStatus);
            if (lStatus != (int)GLEnum.True) throw new Exception($"{_shaderName}: Shader failed to link: " + _gl.GetProgramInfoLog(_id));

            _gl.DetachShader(_id, vertexShader);
            _gl.DetachShader(_id, fragmentShader);
            _gl.DeleteShader(vertexShader);
            _gl.DeleteShader(fragmentShader);
        }

        private int GetUniformLocation(string name)
        {
            int location;
            if (_uniformLocations.TryGetValue(name, out location))
            {
                return location;
            }
            else
            {
                location = _gl.GetUniformLocation(_id, name);
                _uniformLocations[name] = location;
                return location;
            }
        }

        public void SetBool(string name, bool value)
        {
            _gl.Uniform1(GetUniformLocation(name), (uint)(value ? 1 : 0));
        }

        public void SetInt(string name, int value)
        {
            _gl.Uniform1(GetUniformLocation(name), value);
        }

        public void SetFloat(string name, float value)
        {
            _gl.Uniform1(GetUniformLocation(name), value);
        }

        public void SetVec2(string name, Vector2 value)
        {
            _gl.Uniform2(GetUniformLocation(name), value);
        }

        public void SetVec2(string name, float x, float y)
        {
            _gl.Uniform2(GetUniformLocation(name), x, y);
        }

        public void SetVec3(string name, Vector3 value)
        {
            _gl.Uniform3(GetUniformLocation(name), value);
        }

        public void SetVec3(string name, float x, float y, float z)
        {
            _gl.Uniform3(GetUniformLocation(name), x, y, z);
        }

        public void SetVec4(string name, Vector4 value)
        {
            _gl.Uniform4(GetUniformLocation(name), value);
        }

        public void SetVec4(string name, float x, float y, float z, float w)
        {
            _gl.Uniform4(GetUniformLocation(name), x, y, z, w);
        }

        public unsafe void SetMat4(string name, Matrix4x4 mat4)
        { 
            _gl.UniformMatrix4(GetUniformLocation(name), 1, false, (float*)&mat4);
        }
    }
}
