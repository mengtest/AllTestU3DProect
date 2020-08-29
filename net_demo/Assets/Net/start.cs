using UnityEngine;
using System.Collections;
using Game;

public class start : MonoBehaviour {

	// Use this for initialization
	void Start () {
	Client.Start();
	}

	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Callback sent to all game objects before the application is quit.
	/// </summary>
	void OnApplicationQuit()
	{
        Client.Close();
	}
}
