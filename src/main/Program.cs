using System.Numerics;
using System.Drawing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Hexa.NET.ImGui;

namespace Project;

public static class Program
{
    public static GL opengl;
    public static IWindow window;
    public static IInputContext input;
    public static ImGuiController igcontroller;

    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.Title = "Window";
        window = Window.Create(options);
        window.Load += StartWindow;
        window.Update += UpdateWindow;
        window.Render += RenderWindow;
        window.FramebufferResize += ResizeWindow;
        window.Run();
        window.Dispose();
    }

    static void StartWindow()
    {
        opengl = GL.GetApi(window);
        input = window.CreateInput();
        igcontroller = new ImGuiController(opengl, window, input);
    }

    static void UpdateWindow(double deltaTime)
    {
        igcontroller.Update((float)deltaTime);
    }

    static void RenderWindow(double deltaTime)
    {
        opengl.Enable(EnableCap.DepthTest);
        opengl.ClearColor(Color.Black);
        opengl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        ImGui.ShowDemoWindow();

        igcontroller.Render();
    }

    static void ResizeWindow(Vector2D<int> size)
    {
        opengl.Viewport(size);
    }
}