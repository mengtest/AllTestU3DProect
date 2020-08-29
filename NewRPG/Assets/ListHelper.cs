using System.Collections.Generic;
using System.Linq;
//对 List<PoinT> 的一些扩展方法
public static class ListHelper {
	public static bool Exists (this List<Grid> points, Grid point) {
		foreach (Grid p in points)
			if ((p.data.X == point.data.X) && (p.data.Y == point.data.Y))
				return true;
		return false;
	}

	public static bool Exists (this List<Grid> points, int x, int y) {
		foreach (Grid p in points)
			if ((p.data.X == x) && (p.data.Y == y))
				return true;
		return false;
	}
}