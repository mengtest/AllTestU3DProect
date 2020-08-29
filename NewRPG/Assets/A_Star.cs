using System;
using System.Collections.Generic;
using System.Linq;

public class A_Star {
	public const int OBLIQUE = 14;
	public const int STEP = 10;
	public Grid[][] mazeArr { get; private set; }
	List<Grid> closeList;
	List<Grid> waitList;
	public A_Star (Grid[][] maze) {
		mazeArr = maze;
		waitList = new List<Grid> ();
		closeList = new List<Grid> ();
	}
	public void FindPath (Grid start, Grid end, bool IsIgnoreCorner) {
		waitList.Add (start);
		while (waitList.Count != 0) {
			waitList = waitList.OrderBy (p => p.data.F).ToList (); //找出F值最小的点
			var startPoint = waitList[0];
			waitList.RemoveAt (0);
			closeList.Add (startPoint); //已经找过了
			var roundPoints = FindRoundPoints (startPoint, IsIgnoreCorner); //找出它相邻的点
			foreach (Grid roundPoint in roundPoints) {
				if (waitList.Exists (roundPoint)) {
					ComparePath (startPoint, roundPoint); //计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
				} else {
					AddNewPath (startPoint, end, roundPoint); //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
				}
			}
			if (waitList.Contains (end))
				return;
		}
		return;
	}
	private void ComparePath (Grid startPoint, Grid roundPoint) {
		var G = CalcG (startPoint, roundPoint);
		if (G < roundPoint.data.G) {
			roundPoint.data.lastPoint = startPoint.data;
			roundPoint.data.G = G;
			roundPoint.data.CalcF ();
		}
	}
	private void AddNewPath (Grid start, Grid end, Grid roundPoint) {
		roundPoint.data.lastPoint = start.data;
		roundPoint.data.G = CalcG (start, roundPoint);
		roundPoint.data.H = CalcH (end, roundPoint);
		roundPoint.data.CalcF ();
		waitList.Add (roundPoint);
	}
	private int CalcG (Grid start, Grid point) {
		int G = (Math.Abs (point.data.X - start.data.X) + Math.Abs (point.data.Y - start.data.Y)) == 2 ? OBLIQUE : STEP;
		int parentG = point.data.lastPoint != null ? point.data.lastPoint.G : 0;
		return G + parentG;
	}

	private int CalcH (Grid end, Grid point) {
		int step = Math.Abs (point.data.X - end.data.X) + Math.Abs (point.data.Y - end.data.Y);
		return step * STEP;
	}

	//获取某个点周围可以到达的点
	public List<Grid> FindRoundPoints (Grid centerPoint, bool IsIgnoreCorner) {
		var surroundPoints = new List<Grid> (9);

		//for (int x = centerPoint.data.X - 1; x <= centerPoint.data.X + 1; x++)
		//	for (int y = centerPoint.data.Y - 1; y <= centerPoint.data.Y + 1; y++) {
		//		if (CanReach (centerPoint, x, y, IsIgnoreCorner)) {
		//            surroundPoints.Add(mazeArr[x][y]);
		//            //surroundPoints.Add(x, y); 
		//        }
		//}

		if (CanReach (centerPoint, centerPoint.data.X - 1, centerPoint.data.Y, IsIgnoreCorner))
			surroundPoints.Add (mazeArr[centerPoint.data.X - 1][centerPoint.data.Y]);
		if (CanReach (centerPoint, centerPoint.data.X + 1, centerPoint.data.Y, IsIgnoreCorner))
			surroundPoints.Add (mazeArr[centerPoint.data.X + 1][centerPoint.data.Y]);
		if (CanReach (centerPoint, centerPoint.data.X, centerPoint.data.Y - 1, IsIgnoreCorner))
			surroundPoints.Add (mazeArr[centerPoint.data.X][centerPoint.data.Y - 1]);
		if (CanReach (centerPoint, centerPoint.data.X, centerPoint.data.Y + 1, IsIgnoreCorner))
			surroundPoints.Add (mazeArr[centerPoint.data.X][centerPoint.data.Y + 1]);
		return surroundPoints;
	}

	//在二维数组对应的位置不为障碍物
	private bool CanReach (int x, int y) {
		if (x >= mazeArr.Length || x < 0) {
			return false;
		}
		if (y >= mazeArr[x].Length || y < 0) {
			return false;
		}
		return mazeArr[x][y].data.gType != GridType.Wall;
	}

	public bool CanReach (Grid start, int x, int y, bool IsIgnoreCorner) {
		if (!CanReach (x, y) || closeList.Exists (x, y))
			return false;
		else {
			if (Math.Abs (x - start.data.X) + Math.Abs (y - start.data.Y) == 1)
				return true;
			//如果是斜方向移动, 判断是否 "拌脚"
			else {
				if (CanReach (Math.Abs (x - 1), y) && CanReach (x, Math.Abs (y - 1)))
					return true;
				else
					return IsIgnoreCorner;
			}
		}
	}
}