using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLog : MonoBehaviour {
	[Header ("是否开启屏幕日志")]
#if UNITY_EDITOR
	public bool isShowScreenLog = false;
#else
	bool isShowScreenLog = false;
#endif
	[Header ("Log最大行数")]
	int maxCount = 100;
	[Header ("每个日志的最大字数")]
	[SerializeField]
	int perMaxNum = 500;

	private static List<object> mLines = new List<object> ();
	Texture2D tex, tex2;
	GUIStyle view = new GUIStyle ();
	GUIStyle black = new GUIStyle ();
	GUIStyle red = new GUIStyle ();
	GUIStyle yellow = new GUIStyle ();
	GUIStyle to = new GUIStyle ();
	GUIStyle ver = new GUIStyle ();
	bool isHandle = false;

	void Awake () {

	}

	void Start () {
		tex = new Texture2D (1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel (0, 0, new Color (1, 1, 1, 0.8f));
		tex.Apply ();
		tex2 = new Texture2D (1, 1, TextureFormat.RGBA32, false);
		tex2.SetPixel (0, 0, new Color (0, 0, 0, 0.5f));
		tex2.Apply ();
		view.normal.background = tex;
		to.normal.background = tex2;
		to.normal.textColor = Color.white;
		to.alignment = TextAnchor.MiddleCenter;

		black.normal.textColor = Color.black;
		red.normal.textColor = Color.red;
		yellow.normal.textColor = Color.blue;
		ver.normal.textColor = Color.black;
		ver.normal.background = tex;
		ver.alignment = TextAnchor.MiddleCenter;

		Application.logMessageReceived += HandleLog;
		isHandle = true;
	}

	private void HandleLog (string logString, string stackTrace, LogType type) {
		Debug.Log (logString);
		if (type == LogType.Error || type == LogType.Exception) {
			Log (logString, type);
			Log (stackTrace, type);
		} else {
			Log (logString, type);
		}
	}

	public void Log (string logString, LogType type) {
		if (Application.isPlaying) {
			if (mLines.Count >= maxCount) {
				mLines.RemoveAt (0);
			}
			string str = logString;
			if (logString.Length > perMaxNum)
				str = logString.Substring (0, perMaxNum);
			mLines.Add (new object[] { str, type });
		}
	}

	public Vector2 scrollPosition;
	void OnGUI () {
		BeginUIResizing ();
		if (isShowScreenLog) {
			scrollPosition = GUILayout.BeginScrollView (scrollPosition, view, GUILayout.Width (Screen.width * (NativeResolution.y / Screen.height)), GUILayout.Height (NativeResolution.y));
			for (int i = mLines.Count - 1; i >= 0; i--) {
				object[] objs = (object[]) mLines[i];
				string str = (string) objs[0];
				LogType t = (LogType) objs[1];
				if (t == LogType.Error || t == LogType.Exception)
					GUILayout.Label (str, red);
				else if (t == LogType.Warning)
					GUILayout.Label (str, yellow);
				else
					GUILayout.Label (str, black);
			}
			GUILayout.EndScrollView ();
		}

		isShowScreenLog = GUI.Toggle (new Rect (Screen.width * (NativeResolution.y / Screen.height) * 0.5f - 35, 5, 70, 30), isShowScreenLog, "显示日志", to);

		if (GUI.Button (new Rect (Screen.width * (NativeResolution.y / Screen.height) * 0.6f - 35, 5, 70, 30), "清空日志", to)) {
			mLines.Clear ();
		}

		EndUIResizing ();
	}

	public static Vector2 NativeResolution = new Vector2 (640, 480);
	private static float fx = -1.0f;
	private static float fy = -1.0f;
	private static Vector3 _offset = Vector3.zero;
	Matrix4x4 m;
	Vector3 scale;

	static List<Matrix4x4> stack = new List<Matrix4x4> ();
	public void BeginUIResizing () {
		Vector2 nativeSize = NativeResolution;
		stack.Add (GUI.matrix);
		m = new Matrix4x4 ();
		var w = (float) Screen.width;
		var h = (float) Screen.height;
		var aspect = w / h;
		var offset = Vector3.zero;
		fx = (Screen.width / nativeSize.x);
		fy = (Screen.height / nativeSize.y);
		scale = new Vector3 (fy, fy, fy);
		m.SetTRS (offset, Quaternion.identity, scale);
		GUI.matrix *= m;
	}

	public void EndUIResizing () {
		GUI.matrix = stack[stack.Count - 1];
		stack.RemoveAt (stack.Count - 1);
	}
}