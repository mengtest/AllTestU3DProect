using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	//血条样式
	GUIStyle healthStyle;
	GUIStyle backStyle;
	Combat combat;

	void Awake () {
		combat = GetComponent<Combat> ();
	}

	void OnGUI () {
		InitStyles ();

		// 绘制一个血条

		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);

		// 绘制血条背景
		GUI.color = Color.grey;
		GUI.backgroundColor = Color.grey;
		GUI.Box (new Rect (pos.x - 26, Screen.height - pos.y + 20, Combat.maxHealth / 2, 7), ".", backStyle);

		// 绘制当前血量
		GUI.color = Color.green;
		GUI.backgroundColor = Color.green;
		GUI.Box (new Rect (pos.x - 25, Screen.height - pos.y + 21, combat.health / 2, 5), ".", healthStyle);
	}

	void InitStyles () {
		if (healthStyle == null) {
			healthStyle = new GUIStyle (GUI.skin.box);
			healthStyle.normal.background = MakeTex (2, 2, new Color (0f, 1f, 0f, 1.0f));
		}

		if (backStyle == null) {
			backStyle = new GUIStyle (GUI.skin.box);
			backStyle.normal.background = MakeTex (2, 2, new Color (0f, 0f, 0f, 1.0f));
		}
	}

	Texture2D MakeTex (int width, int height, Color col) {
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D (width, height);
		result.SetPixels (pix);
		result.Apply ();
		return result;
	}
}