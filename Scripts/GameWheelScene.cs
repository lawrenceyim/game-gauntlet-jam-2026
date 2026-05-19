using System.Collections.Generic;
using Godot;

public partial class GameWheelScene : Node {
    [Export]
    private Texture2D _blackWheel;

    [Export]
    private Texture2D _blueWheel;

    [Export]
    private Texture2D _greenWheel;

    [Export]
    private Texture2D _redWheel;

    [Export]
    private Texture2D _yellowWheel;

    public override void _Ready() {
        List<SpinningWheel.ColorDto> dtos = [
            new SpinningWheel.ColorDto(
                18.42f, 0f, _blackWheel, "Black"),
            new SpinningWheel.ColorDto(27.15f, 18.42f, _blueWheel, "Blue"),
            new SpinningWheel.ColorDto(11.73f, 45.57f, _greenWheel, "Green"),
            new SpinningWheel.ColorDto(29.88f, 57.30f, _redWheel, "Red"),
            new SpinningWheel.ColorDto(12.82f, 87.18f, _yellowWheel, "Yellow")
        ];

        SpinningWheel spinningWheel = new SpinningWheel();
        AddChild(spinningWheel);
        spinningWheel.Init(dtos);
        spinningWheel.StartSpin();
    }
}