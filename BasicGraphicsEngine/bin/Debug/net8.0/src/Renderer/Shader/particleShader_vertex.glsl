//Source Vertex
#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in float size;
layout (location = 2) in vec4 color;

uniform mat4 viewMatrix;

out float o_size;
out vec4 o_color;

void main()
{
    gl_PointSize = size;

    gl_Position = viewMatrix * vec4(position, 1.0);
    o_size = size;
    o_color = color;
}