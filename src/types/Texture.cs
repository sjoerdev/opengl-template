using System.Numerics;
using Silk.NET.OpenGL;

namespace Project;

public unsafe class Texture
{
    private GL opengl => Program.opengl;
    public uint handle;

    public Vector2 resolution;

    private InternalFormat internalFormat;
    private PixelFormat pixelFormat;
    private PixelType pixelType;

    public Texture(Vector2 resolution, InternalFormat internalFormat, PixelFormat pixelFormat, PixelType pixelType)
    {
        this.resolution = resolution;
        this.internalFormat = internalFormat;
        this.pixelFormat = pixelFormat;
        this.pixelType = pixelType;
        handle = opengl.GenTexture();
        SetSize(resolution);
        SetParameters(TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.ClampToBorder);
    }

    public void Bind() => opengl.BindTexture(TextureTarget.Texture2D, handle);

    public void Unbind() => opengl.BindTexture(TextureTarget.Texture2D, 0);

    public void SetPixels(byte[] data)
    {
        fixed (void* ptr = data)
        {
            opengl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (uint)resolution.X, (uint)resolution.Y, 0, pixelFormat, pixelType, ptr);
        }
    }

    public void SetSize(Vector2 resolution)
    {
        this.resolution = resolution;
        Bind();
        opengl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (uint)resolution.X, (uint)resolution.Y, 0, pixelFormat, pixelType, null);
        Unbind();
    }

    public void SetParameters(TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapMode)
    {
        Bind();
        opengl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        opengl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
        opengl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
        opengl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        Unbind();
    }
}