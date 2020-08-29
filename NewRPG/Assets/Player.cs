using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour {
	public Grid curGrid;
	public Queue queue = new Queue ();
	public Vector3 Position {
		get {
			return transform.position;
		}
		set {
			transform.position = value;
		}
	}

	public void Refish () {
		Position = curGrid.transform.position;
	}

	void Start () {
		UIClickHelper.AddClick (gameObject, () => {
			Debug.Log ("is player");
		});
		StartCoroutine (Move ());
	}
	Tweener tw;

	IEnumerator Move () {
		while (true) {
			if (queue.Count > 0) {
				if (tw != null && tw.IsPlaying ()) {
					tw.Kill ();
					tw = null;
				}
				GridData _grid = queue.Dequeue () as GridData;
				Grid grid = _grid.CurGrid;
				grid.RefishColor ();
				tw = transform.DOMove (new Vector3 (grid.transform.position.x, grid.transform.position.y, Position.z), 0.2f);
				tw.Play ();
				yield return new WaitForSeconds (0.2f);
			}
			yield return 1;
		}
	}

	void Update () { }
}