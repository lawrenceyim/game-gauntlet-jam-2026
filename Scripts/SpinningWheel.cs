using System;
using Godot;
using System.Collections.Generic;

public partial class SpinningWheel : Node2D {
	private readonly List<TextureProgressBar> _bars = [];

	private Node2D _wheelParent;
	private Vector2I _wheelSize = new(300, 300);
	private RandomNumberGenerator _rng = new();
	private bool _isSpinning = false;
	private double _durationInSeconds = 4;
	private double _secondsElapsed;
	private float _targetRotation;
	private List<ColorDto> _colorDtos; // Offset must be in ascending order
	private int _numberOfSpins = 5;

	public override void _Ready() {
		_rng.Randomize();
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
			float percentage = 100 - ((finalAngleDeg) % 360) / 3.6f;
			GD.Print($"Final angle: {finalAngleDeg}° | As percentage: {percentage:F2}% Color: {_GetColor(percentage, _colorDtos)}");

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

	public void Init(List<ColorDto> colorDtos) {
		colorDtos.Sort((a, b) => a.Offset.CompareTo(b.Offset));
		_InitWheel(colorDtos);
	}


	// Return true if spin started, false if already spinning
	public bool StartSpin() {
		if (_isSpinning) {
			return false;
		}

		_targetRotation = 360 * _numberOfSpins + _rng.RandfRange(0f, 360f);
		_secondsElapsed = 0;
		_isSpinning = true;
		return true;
	}

	private void _InitWheel(List<ColorDto> wheelSizes) {
		_colorDtos = wheelSizes;
		_wheelParent = new Sprite2D();
		AddChild(_wheelParent);

		foreach (ColorDto wheelSize in wheelSizes) {
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

	private ColorDto _GetColor(float percentage, List<ColorDto> colors) {
		for (int i = 1; i < _colorDtos.Count; i++) {
			if (colors[i].Offset > percentage) {
				return colors[i - 1];
			}
		}

		return colors[^1];
	}

	public record ColorDto(float Percentage, float Offset, Texture2D Texture, string Description);
}
