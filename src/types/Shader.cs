using System;
using System.IO;
using System.Numerics;

using Silk.NET.OpenGL;

namespace Project;

public class Shader
{
    private GL opengl = Program.opengl;
    private uint handle;

    public Shader(string vertPath, string fragPath)
    {
        handle = CompileProgram(vertPath, fragPath);
    }

    private uint CompileProgram(string vertPath, string fragPath)
    {
        string vertCode = File.ReadAllText(vertPath);
        string fragCode = File.ReadAllText(fragPath);

        uint vertex = CompileShader(GLEnum.VertexShader, vertCode);
        uint fragment = CompileShader(GLEnum.FragmentShader, fragCode);
        
        uint program = opengl.CreateProgram();
        opengl.AttachShader(program, vertex);
        opengl.AttachShader(program, fragment);
        opengl.LinkProgram(program);
        
        opengl.GetProgram(program, GLEnum.LinkStatus, out int status);
        if (status == 0) throw new Exception(opengl.GetProgramInfoLog(program));

        opengl.DeleteShader(vertex);
        opengl.DeleteShader(fragment);

        return program;
    }

    private uint CompileShader(GLEnum type, string source)
    {
        uint shader = opengl.CreateShader(type);
        opengl.ShaderSource(shader, source);
        opengl.CompileShader(shader);
        
        opengl.GetShader(shader, GLEnum.CompileStatus, out int status);
        if (status == 0) throw new Exception(opengl.GetShaderInfoLog(shader));

        return shader;
    }

    public void Use()
    {
        opengl.UseProgram(handle);
    }

    public void SetBool(string name, bool value)
    {
        opengl.Uniform1(opengl.GetUniformLocation(handle, name), value ? 1 : 0);
    }

    public void SetFloat(string name, float value)
    {
        opengl.Uniform1(opengl.GetUniformLocation(handle, name), value);
    }

    public void SetVector3(string name, Vector3 value)
    {
        opengl.Uniform3(opengl.GetUniformLocation(handle, name), value.X, value.Y, value.Z);
    }

    public void SetVector4(string name, Vector4 value)
    {
        opengl.Uniform4(opengl.GetUniformLocation(handle, name), value.X, value.Y, value.Z, value.W);
    }

    public unsafe void SetMatrix4(string name, Matrix4x4 value)
    {
        opengl.UniformMatrix4(opengl.GetUniformLocation(handle, name), 1, false, (float*)&value);
    }

    public unsafe void SetTexture(string name, uint texture, int unit)
    {
        opengl.ActiveTexture(TextureUnit.Texture0 + unit);
        opengl.BindTexture(TextureTarget.Texture2D, texture);
        opengl.Uniform1(opengl.GetUniformLocation(handle, name), unit);
    }
}