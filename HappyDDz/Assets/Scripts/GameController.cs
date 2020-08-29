using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingleT<GameController> {
	GameState gameState = GameState.None;

	public void Init () {
		DDZMain.Self.updateEvent += Update;
	}
	Coroutine coroutine = null;
	public void StartGame () {
		if (gameState != GameState.GameStart) {
			// gameState = GameState.GameStart;
			if (coroutine != null) {
				DDZMain.Self.StopCoroutine (coroutine);
			}
			// GameInit ();
			coroutine = DDZMain.Self.StartCoroutine (UIUserPanel.Self.DealPoker (RandomPokerData ()));
		}
	}

	public void Update () {

	}

	public void GameInit () {
		foreach (UIUser user in UIUserPanel.Self.userList) {
			user.handsPoker.Init ();
		}
	}

	private int[] RandomPokerData () {
		int[] result = new int[54];
		bool[] ifPoker = new bool[54];
		int randomIndex = Random.Range (0, ifPoker.Length);
		int isOkIndex = 0;

		while (isOkIndex < ifPoker.Length) {
			if (ifPoker[randomIndex]) {
				randomIndex = Random.Range (0, ifPoker.Length);
			} else {
				ifPoker[randomIndex] = true;
				result[isOkIndex++] = randomIndex;
			}
		}
		return result;
	}
}