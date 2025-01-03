using System;
using System.Numerics;
using System.Drawing;

using Silk.NET.OpenGL;

namespace Project;

public unsafe class Framebuffer
{
    private GL opengl => Program.opengl;

    public uint handle;

    public Vector2 resolution;
    public Texture colorTexture;
    public Texture depthTexture;

    public Framebuffer(Vector2 resolution)
    {
        this.resolution = resolution;

        // create framebuffer
        handle = opengl.GenFramebuffer();

        // create textures
        colorTexture = new Texture(resolution, InternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
        depthTexture = new Texture(resolution, InternalFormat.DepthComponent, PixelFormat.DepthComponent, PixelType.Float);

        // attach textures
        Bind();
        opengl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTexture.handle, 0);
        opengl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTexture.handle, 0);
        Unbind();

        // check errors
        var status = opengl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != GLEnum.FramebufferComplete) throw new Exception("Framebuffer is not complete!");
    }

    public void Bind()
    {
        opengl.BindFramebuffer(GLEnum.Framebuffer, handle);
    }

    public void Unbind()
    {
        opengl.BindFramebuffer(GLEnum.Framebuffer, 0);
    }

    public void Clear(Color color)
    {
        Bind();
        opengl.ClearColor(color);
        opengl.Clear((uint)(GLEnum.ColorBufferBit | GLEnum.DepthBufferBit));
        Unbind();
    }

    public void Resize(Vector2 resolution)
    {
        this.resolution = resolution;
        Bind();
        colorTexture.SetSize(resolution);
        depthTexture.SetSize(resolution);
        Unbind();
    }
}