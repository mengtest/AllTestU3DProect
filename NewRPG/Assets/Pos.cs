using UnityEngine;

public struct Pos {
	public int X { get; set; }
	public int Y { get; set; }
	public Pos (int x, int y) {
		X = x;
		Y = y;
	}
	public static implicit operator Vector3 (Pos payment) {
		return new Vector3 () {
			x = (float) payment.X,
				y = (float) payment.Y,
				z = 0,
		};
	}
	public static implicit operator Pos (Vector3 payment) {
		return new Pos () {
			X = (int) payment.x,
				Y = (int) payment.y,
		};
	}
	public static Pos operator + (Pos left, Pos right) {
		return new Pos () {
			X = left.X + right.X,
				Y = left.Y + right.Y,
		};
	}
}