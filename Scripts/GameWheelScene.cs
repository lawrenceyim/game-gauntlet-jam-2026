using System.Collections.Generic;
using Godot;

public partial class GameWheelScene : Node {
    [Export]
    private Texture2D _wheel1;

    [Export]
    private Texture2D _wheel2;

    [Export]
    private Texture2D _wheel3;

    [Export]
    private Texture2D _wheel4;

    public override void _Ready() {
        List<SpinningWheel.ColorDto> dtos = [
            new(25f, 0f, _wheel1, "Wheel 1"),
            new(25f, 25f, _wheel2, "Wheel 2"),
            new(25f, 50f, _wheel3, "Wheel 3"),
            new(25f, 75f, _wheel4, "Wheel 4")
        ];

        SpinningWheel spinningWheel = new();
        AddChild(spinningWheel);
        spinningWheel.Position = new Vector2(0, 0);

        spinningWheel.Init(dtos, new Vector2I(360, 360));
        spinningWheel.StartSpin();
    }
}