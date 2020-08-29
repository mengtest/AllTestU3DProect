using System;

[Serializable]
public class GridData {
	public GridData lastPoint { get; set; }

	public GridData nextPoint { get; set; }

	public GridType gType { get; set; }

	public Grid CurGrid { get; set; }

	public int F { get; set; } //F=G+H
	public int G { get; set; }

	public int H { get; set; }

	public int X { get; set; }

	public int Y { get; set; }

	public GridData (int x, int y) {
		this.X = x;
		this.Y = y;
	}
	public void CalcF () {
		this.F = this.G + this.H;
	}
}

public enum GridType {
	Road,
	Wall,
}