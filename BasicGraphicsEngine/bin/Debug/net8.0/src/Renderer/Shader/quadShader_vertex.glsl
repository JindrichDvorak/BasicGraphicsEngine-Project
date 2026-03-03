//Source Vertex
#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;

uniform mat4 viewMatrix;

out vec4 o_color;

void main()
{
    gl_Position = viewMatrix * vec4(position, 1.0);
    o_color = color;
}