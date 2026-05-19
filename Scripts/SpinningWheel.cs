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

	public override void _Ready() {
		_InitWheel([
			new WheelSizeDto(18.42f, 0f, _blackWheel),
			new WheelSizeDto(27.15f, 18.42f, _blueWheel),
			new WheelSizeDto(11.73f, 45.57f, _greenWheel),
			new WheelSizeDto(29.88f, 57.30f, _redWheel),
			new WheelSizeDto(12.82f, 87.18f, _yellowWheel)
		]);
	}

	public override void _Process(double delta) {
		foreach (TextureProgressBar bar in _bars) {
			bar.Rotation += (float)delta * 5;
		}
	}

	private void _InitWheel(List<WheelSizeDto> wheelSizes) {
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

	private record WheelSizeDto(float Percentage, float Offset, Texture2D Texture);
}
