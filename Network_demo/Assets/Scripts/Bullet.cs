using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//打中玩家后销毁子弹
	//当服务器上的子弹被销毁了
	//因为它是由网络生产管理器产生的对象，所以在其他客户端上这个子弹也会被销毁。
	void OnCollisionEnter (Collision collision) {
		var hit = collision.gameObject;
		var hitCombat = hit.GetComponent<Combat> ();
		if (hitCombat != null) {
			//打中敌人减血
			hitCombat.TakeDamage (10);
			//销毁子弹
			Destroy (gameObject);
		}
	}
}