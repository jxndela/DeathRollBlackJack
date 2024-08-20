using DeathRoll.Windows;

namespace DeathRoll.Peggle.Objects;

public enum PegType
{
    Blue,
    Orange,
    Green,
    Purple,
}

public static class PegColorExtension
{
    public static uint ToColor(this PegType type)
    {
        return type switch
        {
            PegType.Blue => Helper.PegBlue,
            PegType.Orange => Helper.PegOrange,
            PegType.Green => Helper.PegGreen,
            PegType.Purple => Helper.PegPurple,

            _ => Helper.NumberRed
        };
    }
}

public class Peg : BaseObject
{
    public PegType Type;

    public Peg(Vector2 position, Vector2 velocity, Size size, PegType type) : base(position, velocity, size)
    {
        Type = type;
        Color = type.ToColor();
    }

    public void Update(PlayBall ball)
    {
        if (CollisionCheck(ball))
            Alive = false;
    }

    public void Draw(ImDrawListPtr drawlist, Vector2 screenPos)
    {
        var top = new Vector2(Position.X - Size.Radius, Position.Y - Size.Radius);
        drawlist.AddCircleFilled(screenPos+top, Size.Radius, Color);
    }

    private bool CollisionCheck(PlayBall ball)
    {
        var r = ball.Size.Radius + Size.Radius;
        var dx = ball.Position.X - Position.X;
        var dy = ball.Position.Y - Position.Y;
        if (dx * dx + dy * dy > r * r)
            return false;

        var uv = Vector2.Normalize(ball.Position - Position);

        ball.Position = Position + uv * (Size.Radius + ball.Size.Radius);
        ball.Velocity = new Vector2(
            (ball.Velocity.X - 2 * uv.X * Vector2.Dot(ball.Velocity, uv)) * 0.9f,
            (ball.Velocity.Y - 2 * uv.Y * Vector2.Dot(ball.Velocity, uv)) * 0.9f);

        return true;
    }
}