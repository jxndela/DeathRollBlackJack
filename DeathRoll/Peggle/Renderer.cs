using Dalamud.Interface.Utility;
using DeathRoll.Windows;

namespace DeathRoll.Peggle;

public class Renderer
{
    private readonly Peggle Game;

    public Renderer(Peggle game)
    {
        Game = game;
    }

    public void Draw()
    {
        var drawlist = ImGui.GetWindowDrawList();
        var p = ImGui.GetCursorScreenPos();

        Game.ObjectHandler.Draw(drawlist, p);
    }

    private void DrawBackground(ImDrawListPtr drawlist, Vector2 p)
    {
        drawlist.AddRectFilled(p, new Vector2(p.X + Settings.Width, p.Y + Settings.Height), Helper.Background);
    }

    public void DrawGameOver()
    {
        var drawlist = ImGui.GetWindowDrawList();
        var p = ImGui.GetCursorScreenPos();

        var fullSize = new Vector2(p.X + Settings.Width, p.Y + Settings.Height);
        drawlist.AddRectFilled(p, fullSize, Helper.Background);
        drawlist.AddText(p, Helper.RaycastWhite, "You Lost !!!");
    }

    public void DrawMainMenu()
    {
        var drawlist = ImGui.GetWindowDrawList();
        var p = ImGui.GetCursorScreenPos();

        var fullSize = new Vector2(p.X + Settings.Width, p.Y + Settings.Height);
        drawlist.AddRectFilled(p, fullSize, Helper.Background);

        TitleRender();

        Game.Plugin.FontManager.SourceCode36.Push();
        ImGuiHelpers.ScaledDummy(20.0f);
        if (Helper.CenterButton("Play"))
        {
            Game.Play();
        }

        var textSize = ImGui.CalcTextSize("ESC - Shutdown Game");
        var heightSpacing = textSize.Y * 6.0f; // 5 line height
        var widthSpacing = textSize.X * 1.2f;
        var rightCorner = new Vector2(fullSize.X - widthSpacing, fullSize.Y - heightSpacing);
        drawlist.AddText(rightCorner, Helper.RaycastWhite, "Special Keys:");
        rightCorner.Y += textSize.Y;
        drawlist.AddText(rightCorner, Helper.RaycastWhite, "ALT - Show Mouse");
        rightCorner.Y += textSize.Y;
        drawlist.AddText(rightCorner, Helper.RaycastWhite, "ESC - Shutdown Game");

        Game.Plugin.FontManager.SourceCode36.Pop();
    }

    private void TitleRender()
    {
        ImGuiHelpers.ScaledDummy(50.0f);
        Helper.SetTextCenter("PEGGLE");
    }
}