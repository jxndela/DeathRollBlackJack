using DeathRoll.Windows;

namespace DeathRoll.Data;

public class UnoPlayer
{
    public string Name;
    public List<UnoCard> Cards = [];

    public bool HasUno;
    public readonly bool IsHost;

    public UnoPlayer(string name, bool isHost = false)
    {
        Name = name;
        IsHost = isHost;
    }
}

public class UnoCard(UnoCards type, UnoColors color)
{
    public UnoCards Type = type;
    public UnoColors Color = color;

    public UnoColors? SelectedColor;

    public string[] ShowCard()
    {
        var space = Color != UnoColors.Colorless ? " " : "";
        var value = Parse();

        return
        [
            Cards.BlankCard,
            string.Format(Cards.SuitCard, value)
        ];
    }

    public string ShowCardSimple()
    {
        var value = Parse();
        return $"{value}";
    }

    private string Parse()
    {
        return Type switch
        {
            UnoCards.Reverse => "R",
            UnoCards.Skip => "S",
            UnoCards.DrawTwo => "+2",
            UnoCards.DrawFour => "+4",
            UnoCards.Wild => "W",
            _ => ((int) Type).ToString()
        };
    }
}

public enum UnoColors
{
    Colorless,
    Red,
    Yellow,
    Blue,
    Green
}

public enum UnoCards
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Reverse = 10,
    Skip = 11,
    DrawTwo = 12,
    DrawFour = 13,
    Wild = 14,
}

public class Uno
{
    private readonly Plugin Plugin;

    public const uint DeckSize = 109;
    public const uint StartHand = 7;

    public List<UnoPlayer> Players = [];
    public int CurrentPlayerIndex;

    public UnoCard? CurrentTop;
    public Queue<UnoCard> CardDeck = new();

    public bool GameStarted;

    public Uno(Plugin plugin)
    {
        Plugin = plugin;
    }

    public void FillDeckAndShuffle()
    {
        var deck = new List<UnoCard>();
        foreach (var color in Enum.GetValues(typeof(UnoColors)).Cast<UnoColors>())
            foreach (var card in color.ToCards())
                deck.AddRange(Enumerable.Range(0, card.NumberOfCards()).Select(_ => new UnoCard(card, color)));

        deck.Shuffle();
        CardDeck = new Queue<UnoCard>(deck);
    }

    public void DealStartingHand()
    {
        foreach (var player in Players)
            for (var i = 0; i < StartHand; i++)
                player.Cards.Add(CardDeck.Dequeue());
    }

    public bool IsAllowed(UnoCard card)
    {
        if (card.Color == UnoColors.Colorless)
            return true;

        if (CurrentTop == null)
            return true;

        if (CurrentTop.Type is UnoCards.Wild or UnoCards.DrawFour)
            return CurrentTop.SelectedColor == card.Color;

        return CurrentTop.Color == card.Color;
    }

    public bool PlayCard(UnoCard card)
    {
        if (!IsAllowed(card))
            return false;

        if (CurrentTop != null)
            CardDeck.Enqueue(CurrentTop);

        var player = Players[CurrentPlayerIndex];
        player.Cards.Remove(card);
        CurrentTop = card;

        if (player.Cards.Count == 1 && !player.HasUno)
            player.Cards.Add(Draw());

        return true;
    }

    public void CallUno()
    {
        Players[CurrentPlayerIndex].HasUno = true;
    }

    public UnoCard Draw()
    {
        return CardDeck.Dequeue();
    }
}

public static class UnoExtensions
{
    public static uint ToImGuiColor(this UnoColors color)
    {
        return color switch
        {
            UnoColors.Red => Helper.Vec4ToUintColor(Helper.Red),
            UnoColors.Green => Helper.Vec4ToUintColor(Helper.Green),
            UnoColors.Blue => Helper.Vec4ToUintColor(Helper.SoftBlue),
            UnoColors.Yellow => Helper.Vec4ToUintColor(Helper.Yellow),
            UnoColors.Colorless => Helper.Vec4ToUintColor(Helper.LighterGrey)
        };
    }

    public static UnoCards[] ToCards(this UnoColors color)
    {
        return color switch
        {
            UnoColors.Colorless => [UnoCards.Reverse, UnoCards.Skip, UnoCards.DrawTwo, UnoCards.DrawFour, UnoCards.Wild],
            _ => [UnoCards.Zero, UnoCards.One, UnoCards.Two, UnoCards.Three, UnoCards.Four, UnoCards.Five, UnoCards.Six, UnoCards.Seven, UnoCards.Eight, UnoCards.Nine]
        };
    }

    public static int NumberOfCards(this UnoCards cards)
    {
        return cards switch
        {
            UnoCards.Zero => 1,
            UnoCards.One => 2,
            UnoCards.Two => 2,
            UnoCards.Three => 2,
            UnoCards.Four => 2,
            UnoCards.Five => 2,
            UnoCards.Six => 2,
            UnoCards.Seven => 2,
            UnoCards.Eight => 2,
            UnoCards.Nine => 2,
            UnoCards.Reverse => 2,
            UnoCards.Skip => 2,
            UnoCards.DrawTwo => 2,
            UnoCards.DrawFour => 4,
            UnoCards.Wild => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(cards), cards, null)
        };
    }

    // From: https://stackoverflow.com/a/1262619
    private static readonly Random Rng = new();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}