using DeathRoll.Peggle.Objects;

namespace DeathRoll.Peggle;

public class ObjectHandler
{
    private PlayBall Ball = PlayBall.CreateBall();
    private readonly List<Peg> Targets = [
        new Peg(new Vector2(300,300), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(315,315), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(330,330), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(345,345), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(360,360), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(375,375), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(390,390), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(405,405), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(420,420), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(600,500), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(200,400), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Blue),
        new Peg(new Vector2(220,420), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Orange),
        new Peg(new Vector2(320,350), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Green),
        new Peg(new Vector2(370,410), Vector2.Zero, new Size(true, 8.0f, Vector2.Zero), PegType.Purple),
    ];

    public void Update()
    {
        Ball.Update();
        foreach (var target in Targets)
            target.Update(Ball);
    }

    public void Draw(ImDrawListPtr drawlist, Vector2 screenPos)
    {
        Ball.Draw(drawlist, screenPos);
        foreach (var target in Targets)
            target.Draw(drawlist, screenPos);
    }

    public void HandleMouseClick()
    {
        if (!Ball.Alive)
            Ball = PlayBall.CreateBall();
        else if (!Ball.Fired)
            Ball.Fire();
    }
}