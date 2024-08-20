namespace DeathRoll.Peggle;

public static class Settings
{
    public const int Width = 800;
    public const int Height = 700;
    public const int HalfWidth = Width / 2;
    public const int HalfHeight = Height / 2;

    public const float OffsetFromTop = 80;

    public const float Gravity = 650;
    public const float MuzzleVelocity = 750;

    public const float MouseSensitivity = 0.0003f;
    public const int MouseMaxRel = 40;
    public const int MouseBorderLeft = 100;
    public const int MouseBorderRight = Width - MouseBorderLeft;

    public const double DegToRad = Math.PI / 180.0;

    public const string CreditsTextTemplate =
        """
        Peggle1
        """;
}