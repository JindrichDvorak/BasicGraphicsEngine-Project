//Source Fragment
#version 330 core

in float o_size;
in vec4 o_color;

out vec4 color;

void main()
{
    color = o_color;
}