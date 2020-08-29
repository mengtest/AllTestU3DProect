using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

//战斗
public class Combat : NetworkBehaviour {

	//血量
	public const int maxHealth = 100;
	public bool destroyOnDeath;

	//状态同步
	[SyncVar]
	public int health = maxHealth;

	//战斗减血
	public void TakeDamage (int amount) {
		//仅服务器处理
		if (!isServer) {
			return;
		}
		health -= amount;
		if (health <= 0) {
			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				//服务器同步状态恢复至满血
				health = maxHealth;
				//服务器通知玩家复活了
				// 在服务器上执行，在客户端上被调用
				RpcRespawn ();
			}
		}
	}

	//复活玩家
	[ClientRpc]
	void RpcRespawn () {
		//由客户端移动对象到起点位置
		if (isLocalPlayer) {
			//移动回零点原点位置
			transform.position = Vector3.zero;
		}
	}
}