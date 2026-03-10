//Source Fragment
#version 330 core

in vec3 o_localPosition;
in vec3 o_center;
in float o_radius;
in float o_outlineThickness;
in vec4 o_color;
in vec4 o_outlineColor;

out vec4 color;

float[2] circle(vec3 localCoord, vec3 center, float radius, float outlineThickness)
{
    vec3 circVec = localCoord - center;
    float dist = length(circVec);
    float d = 0.001 * radius;
    float innerRadius = radius - outlineThickness;
    float[2] result;
    if(dist < innerRadius)
    {
        result[0] = 0;
        result[1] = smoothstep(innerRadius + d, innerRadius - d, dist);
    }
    else
    {
        result[0] = 1;
        result[1] = smoothstep(radius + d, radius - d, dist) - smoothstep(innerRadius + d, innerRadius - d, dist);
    }
    return result;
}

void main()
{
    float[] circle = circle(o_localPosition, o_center, o_radius, o_outlineThickness);

    if(circle[1] == 0) discard;

    vec4 baseColor;
    if(circle[0] == 1)
    {
        baseColor = o_outlineColor;
    }
    else
    {        
        baseColor = o_color;
    }

    color = baseColor * circle[1];
}