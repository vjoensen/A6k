﻿#version 460 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 TexCoord;

uniform mat4 transform;
uniform vec4 spriteInfo;

void main()
{
    gl_Position = transform * vec4(aPos, 1.0f);
    TexCoord = vec2( aTexCoord.x*spriteInfo.z +spriteInfo.x ,aTexCoord.y*spriteInfo.w +spriteInfo.y);
}