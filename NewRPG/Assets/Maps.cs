using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public struct MapStruct {
	public int Id { get; set; }
	public string Name { get; set; }
	public int Width { get; set; }
	public int Heigh { get; set; }
	public GridStruct[][] Data { get; set; }
}

public struct GridStruct {
	public int Type { get; set; }
}

public class Maps : MonoBehaviour {
	GridLayoutGroup layout;
	RectTransform rectTrans;
	static private Maps self;
	public static Maps Self {
		get {
			return self;
		}
	}

	public MapStruct mapData;
	public A_Star aStar;
	public Grid[][] gridObjs;
	public Player curPlayer;

	void Awake () {
		self = this;
	}

	public bool IsMove { get; set; }

	void Start () {
		IsMove = false;
		mapData = new MapStruct () {
			Width = 30,
				Heigh = 15,
		};
		layout = GetComponent<GridLayoutGroup> ();
		rectTrans = GetComponent<RectTransform> ();
		layout.constraintCount = mapData.Width;
		layout.cellSize = new Vector2 (rectTrans.rect.size.x / mapData.Width, rectTrans.rect.size.y / mapData.Heigh);

		StartCoroutine (InitMap ());

		Dispatcher.RegisterProtocalListener ("clickGrid", (o) => {
			Grid end = o[0] as Grid;
			curPlayer.curGrid.data.nextPoint = null;
			curPlayer.curGrid.data.lastPoint = null;
			end.data.nextPoint = null;
			end.data.lastPoint = null;

			A_Star a = new A_Star (gridObjs);
			a.FindPath (curPlayer.curGrid, end, false);
			curPlayer.curGrid = end;

			GridData cur = end.data;
			while (cur.lastPoint != null) {
				cur.lastPoint.nextPoint = cur;
				cur = cur.lastPoint;
			}

			while (cur != null) {
				cur.CurGrid.SetColor ();
				curPlayer.queue.Enqueue (cur);
				cur = cur.nextPoint;
			}
			IsMove = true;
		});
	}
	void Update () {

	}

	IEnumerator InitMap () {
		gridObjs = new Grid[mapData.Heigh][];
		for (int i = 0; i < mapData.Heigh; i++) {
			gridObjs[i] = new Grid[mapData.Width];
			for (int j = 0; j < mapData.Width; j++) {
				GameObject _obj = new GameObject (i.ToString ());
				_obj.transform.SetParent (transform);
				_obj.transform.localScale = Vector3.one;
				_obj.transform.localPosition = Vector2.zero;
				_obj.name = i + "_" + j;
				Grid grid = _obj.AddComponent<Grid> ();
				grid.data = new GridData (i, j);
				grid.data.CurGrid = grid;
				gridObjs[i][j] = grid;
			}
		}

		for (int i = 0; i < 100; i++) {
			int y = Random.Range (0, mapData.Width - 1);
			int x = Random.Range (0, mapData.Heigh - 1);
			gridObjs[x][y].data.gType = GridType.Wall;
		}

		yield return 1;

		aStar = new A_Star (gridObjs);
		curPlayer.curGrid = gridObjs[0][0];
		curPlayer.Refish ();
	}
}