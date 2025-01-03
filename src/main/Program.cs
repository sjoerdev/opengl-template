using System.Numerics;
using System.Drawing;

using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Input;

namespace Project;

static unsafe class Program
{
    public static GL opengl;
    public static IWindow window;
    public static IInputContext input;
    public static ImGuiController igcontroller;

    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new(1280, 720);
        options.Title = "Program";
        window = Window.Create(options);
        window.Load += Load;
        window.Render += (delta) => Render((float)delta);
        window.Resize += (res) => Resize((Vector2)res);
        window.Run();
        window.Dispose();
    }

    static void Load()
    {
        opengl = GL.GetApi(window);
        input = window.CreateInput();
        igcontroller = new ImGuiController(opengl, window, input);
        opengl.Enable(EnableCap.DepthTest);
        opengl.ClearColor(Color.Black);
    }

    static void Render(float delta)
    {
        opengl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        igcontroller.Render(delta);
    }

    static void Resize(Vector2 res)
    {
        opengl.Viewport(new Size((int)res.X, (int)res.Y));
    }
}