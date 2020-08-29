using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {
	public GameObject bulletPrefab;
	Transform cube = null;
	void Update () {
		//仅本地操作的玩家
		if (!isLocalPlayer) {
			return;
		}
		//移动
		var x = Input.GetAxis ("Horizontal") * 0.1f;
		var z = Input.GetAxis ("Vertical") * 0.1f;
		transform.Translate (x, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			// 命令在客户端被执行，但是在服务器上调用
			CmdFire ();
		}
	}

	//仅本地对象被初始化时调用，对于客户端自己
	public override void OnStartLocalPlayer () {
		//将玩家自己的物体设置为红色
		cube = transform.Find ("Cube");
		cube.GetComponent<MeshRenderer> ().material.color = Color.red;
	}

	// 标识了[Command]的代码仅会运行在服务器上！
	[Command]
	void CmdFire () {
		// 为本地(服务器)创建子弹对象
		GameObject bullet = (GameObject) Instantiate (
			bulletPrefab,
			transform.position - transform.forward, //出现在物体前方
			Quaternion.identity);

		// 让子弹从玩家的正前方射出
		bullet.GetComponent<Rigidbody> ().velocity = -transform.forward * 4;

		// 在客户端生成子弹
		NetworkServer.Spawn (bullet);

		// 让子弹在2秒后自动消失
		//当子弹在服务器销毁的时候，客户端也会自动销毁
		Destroy (bullet, 2.0f);
	}

	//// 仅用作客户端甚至可以不需要具体实现函数
	//[Command]
	//void CmdFire()
}