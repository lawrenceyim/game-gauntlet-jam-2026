using System;
using Godot;
using System.Collections.Generic;

public partial class SpinningWheel : Node2D {
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

    private Node2D _wheelParent;

    private Vector2I _wheelSize = new(300, 300);

    private List<TextureProgressBar> _bars = [];
    private RandomNumberGenerator _rng = new();
    private bool _isSpinning = false;
    private double _durationInSeconds = 4;
    private double _secondsElapsed;
    private float _targetRotation;
    private List<WheelSizeDto> _wheelSizes;

    public override void _Ready() {
        _InitWheel([
            new WheelSizeDto(18.42f, 0f, _blackWheel, "Black"),
            new WheelSizeDto(27.15f, 18.42f, _blueWheel, "Blue"),
            new WheelSizeDto(11.73f, 45.57f, _greenWheel, "Green"),
            new WheelSizeDto(29.88f, 57.30f, _redWheel, "Red"),
            new WheelSizeDto(12.82f, 87.18f, _yellowWheel, "Yellow")
        ]);
        _rng.Randomize();
        _StartSpin();
    }

    public override void _Process(double delta) {
        if (!_isSpinning) {
            return;
        }

        _secondsElapsed += delta;

        if (_secondsElapsed >= _durationInSeconds) {
            float snapRotation = Mathf.DegToRad(_targetRotation % 360);
            foreach (TextureProgressBar bar in _bars) {
                bar.Rotation = snapRotation;
            }

            float finalAngleDeg = Mathf.RadToDeg(_bars[0].Rotation) % 360;
            GD.Print($"Final angle: {finalAngleDeg}° | As percentage: {((finalAngleDeg) % 360) / 3.6f:F1}%");

            _isSpinning = false;
            return;
        }

        double progress = _secondsElapsed / _durationInSeconds;
        double eased = 1 - Math.Pow(1 - progress, 3); // ease-out cubic
        float currentAngle = (float)(_targetRotation * eased);
        foreach (TextureProgressBar bar in _bars) {
            bar.Rotation = Mathf.DegToRad(currentAngle);
        }
    }

    private void _InitWheel(List<WheelSizeDto> wheelSizes) {
        _wheelSizes = wheelSizes;
        _wheelParent = new Sprite2D();
        AddChild(_wheelParent);

        foreach (WheelSizeDto wheelSize in wheelSizes) {
            TextureProgressBar bar = new TextureProgressBar();
            _wheelParent.AddChild(bar);

            bar.TextureProgress = wheelSize.Texture;
            bar.FillMode = (int)TextureProgressBar.FillModeEnum.Clockwise;
            bar.SetRadialInitialAngle(wheelSize.Offset * 3.6f);
            bar.SetFillDegrees(360f);
            bar.Value = wheelSize.Percentage + 1;
            bar.PivotOffset = _wheelSize / 2;

            _bars.Add(bar);
        }
    }

    private void _StartSpin() {
        if (_isSpinning) {
            return;
        }

        int spins = 5;
        _targetRotation = 360 * spins + _rng.RandfRange(0f, 360f);
        _secondsElapsed = 0;
        _isSpinning = true;
    }

    private record WheelSizeDto(float Percentage, float Offset, Texture2D Texture, string Description);
}