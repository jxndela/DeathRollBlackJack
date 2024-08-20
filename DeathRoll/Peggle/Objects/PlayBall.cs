using DeathRoll.Windows;

namespace DeathRoll.Peggle.Objects;

public class PlayBall : BaseObject
{
    public bool Fired;

    private float FireAngle;
    private Vector2 FirePosition;

    public static PlayBall CreateBall() => new(new Vector2(Settings.HalfWidth, 0), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero));

    public PlayBall(Vector2 position, Vector2 velocity, Size size) : base(position, velocity, size, Helper.PlayerYellow)
    {

    }

    public void Fire()
    {
        Fired = true;
        Position = FirePosition;

        var xOff = Settings.MuzzleVelocity * MathF.Cos(FireAngle);
        var yOff = Settings.MuzzleVelocity * MathF.Sin(FireAngle);
        Velocity = new Vector2(MathF.Abs(xOff) < 0.0001 ? 0 : xOff, MathF.Abs(yOff) < 0.0001 ? 0 : yOff);
    }

    public void Update()
    {
        if (!Fired)
            return;

        Velocity.Y += Peggle.DeltaTimeMil * Settings.Gravity;
        Position += Velocity * Peggle.DeltaTimeMil;

        CollisionCheck();
    }

    public void Draw(ImDrawListPtr drawlist, Vector2 screenPos)
    {
        if (!Fired)
        {
            var cursor = ImGui.GetIO().MousePos - screenPos;

            var x = cursor.X - Position.X;
            var y = cursor.Y - Position.Y;
            var angle = MathF.Atan2(y, x);

            var pos = Position + new Vector2(
                MathF.Cos(angle) * Settings.OffsetFromTop,
                MathF.Max(MathF.Sin(angle) * Settings.OffsetFromTop, 0)
            );

            FireAngle = angle;
            FirePosition = pos;

            drawlist.AddCircleFilled(pos + screenPos, Size.Radius, Color);
            return;
        }

        var top = new Vector2(Position.X - Size.Radius, Position.Y - Size.Radius);
        drawlist.AddCircleFilled(screenPos+top, Size.Radius, Helper.PlayerYellow);
    }
}