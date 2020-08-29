using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Cube : NetworkBehaviour {
	NetworkTransform networkTransform;
	void Start () {
		networkTransform = gameObject.GetComponent<NetworkTransform> ();
	}
	void Update () {
		//仅本地操作的玩家
		if (!isLocalPlayer) {
			return;
		}
		//移动
		var x = Input.GetAxis ("Horizontal") * 0.1f;
		var z = Input.GetAxis ("Vertical") * 0.1f;
		transform.Translate (x, 0, z);
	}
	void OnGUI () {

	}
}