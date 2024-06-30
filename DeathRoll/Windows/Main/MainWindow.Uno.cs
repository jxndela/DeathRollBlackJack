using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using DeathRoll.Data;

namespace DeathRoll.Windows.Main;

public partial class MainWindow
{
    private void UnoMode()
    {
        return;

        if (!Plugin.Uno.GameStarted)
        {
            Plugin.Uno.GameStarted = true;

            Plugin.Uno.Players.Add(new UnoPlayer("Infi", true));
            Plugin.Uno.Players.Add(new UnoPlayer("Grey Parser"));
            Plugin.Uno.Players.Add(new UnoPlayer("RPer"));

            Plugin.Uno.FillDeckAndShuffle();
            Plugin.Uno.DealStartingHand();
        }

        if (Plugin.Uno.Players.Count < 2)
            return;

        var firstPos = ImGui.GetCursorPos();
        var totalSpace = ImGui.GetContentRegionAvail();
        var space = totalSpace.X / (Plugin.Uno.Players.Count - 1) / 2;
        foreach (var (player, idx) in Plugin.Uno.Players.Where(p => !p.IsHost).Select((val, i) => (val, i)))
        {
            ImGui.SetCursorPosY(firstPos.Y);

            ImGui.SetCursorPosX(space);
            ImGui.TextUnformatted(player.Name);
            ImGui.SetCursorPosX(space);
            ImGui.TextUnformatted($"Cards: {player.Cards.Count}");
            space += totalSpace.X / (Plugin.Uno.Players.Count - 1);
        }

        ImGui.NewLine();

        var middlePosition = totalSpace / 2;
        ImGui.SetCursorPos(new Vector2(middlePosition.X - 70, middlePosition.Y - 150));
        var middleCard = new UnoCard(UnoCards.Six, UnoColors.Red);
        using (ImRaii.PushColor(ImGuiCol.Text, middleCard.Color.ToImGuiColor()))
        {
            GameCardRender(middleCard);
        }

        ImGui.NewLine();
        var newlinePos = ImGui.GetCursorPos();
        ImGui.SetCursorPos(new Vector2(newlinePos.X, totalSpace.Y - 200));
        var host = Plugin.Uno.Players.Find(p => p.IsHost);

        var currentX = newlinePos.X;
        var orgCursor = ImGui.GetCursorPos();
        ImGui.Text($"{host!.Name}");
        foreach (var card in host.Cards)
        {
            ImGui.SetCursorPos(new Vector2(currentX, orgCursor.Y));
            using (ImRaii.PushColor(ImGuiCol.Text, card.Color.ToImGuiColor()))
            {
                GameCardRender(card);
            }
            currentX += 110 * ImGuiHelpers.GlobalScale;
        }
    }

    private void GameCardRender(UnoCard card)
    {
        var s = card.ShowCard();
        Plugin.FontManager.Jetbrains22.Push();
        ImGui.Text(s[0]);
        Plugin.FontManager.Jetbrains22.Pop();

        ImGui.SameLine();

        var p = ImGui.GetCursorPos();
        ImGui.SetCursorPos(new Vector2(p.X - 70 * ImGuiHelpers.GlobalScale, p.Y + 100 * ImGuiHelpers.GlobalScale));
        Plugin.FontManager.SourceCode36.Push();
        ImGui.Text(s[1]);
        Plugin.FontManager.SourceCode36.Pop();
    }
}