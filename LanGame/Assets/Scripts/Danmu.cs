using System;
using System.Collections;
using UnityEngine;

namespace Game {
	public class Danmu : MonoBehaviour {
		public Player p;
		public Rigidbody _rigidbody;
		public Vector3 endPos;
		public Vector3 v;
		void Start () {
			Vector3 dir = (endPos - transform.localPosition).normalized + p.curAimPos;
			dic = dir * 0.1f * p.shootPower * 5; //偏移5点力度
			_rigidbody = GetComponent<Rigidbody> ();
		}

		public Vector3 dic;
		int time = 0;
		float jumpTime = 0;
		void FixedUpdate () {
			time++;
			if (time > 1000) {
				Destroy (gameObject);
				return;
			}
			transform.localPosition += dic;
			dic = Vector3.Lerp (dic, Vector3.zero, jumpTime);
		}
		private void OnCollisionEnter (Collision other) {
			if (other.gameObject.tag == "Player") {
				Destroy (gameObject);
			}
			jumpTime += 0.005f;
		}
	}
}