using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

//敌人生成器
//Server Only 不会在客户端执行生效
public class EnemySpawner : NetworkBehaviour {

	public GameObject enemyPrefab;
	public int numEnemies;

	// 服务器启动生效时
	public override void OnStartServer () {
		for (int i = 0; i < numEnemies; i++) {
			//随机位置
			var pos = new Vector3 (
				Random.Range (-8.0f, 8.0f),
				0.2f,
				Random.Range (-8.0f, 8.0f)
			);
			var rotation = Quaternion.Euler (Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
			var enemy = (GameObject) Instantiate (enemyPrefab, pos, rotation);
			//在客户端创建敌人
			NetworkServer.Spawn (enemy);
		}
	}
}