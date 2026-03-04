//Source Vertex
#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 center;
layout (location = 2) in float radius;
layout (location = 3) in float outlineThickness;
layout (location = 4) in vec4 color;
layout (location = 5) in vec4 outlineColor;

uniform mat4 viewMatrix;

out vec3 o_localPosition;
out vec3 o_center;
out float o_radius;
out float o_outlineThickness;
out vec4 o_color;
out vec4 o_outlineColor;

void main()
{
    gl_Position = viewMatrix * vec4(position, 1.0);
    o_localPosition = position;
    o_center = center;
    o_radius = radius;
    o_outlineThickness = outlineThickness;
    o_color = color;
    o_outlineColor = outlineColor;
}