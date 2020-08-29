using System;
using UnityEngine;

namespace Game {
	public class CollisionEvent : MonoBehaviour {
		public event Action<Collision> OnCollisionEnterEvent;
		public event Action<Collision> OnCollisionExitEvent;
		public event Action<Collision> OnCollisionStayEvent;
		public void SetPos (Vector3 _pos) {
			transform.position = _pos;
		}
		private void OnCollisionEnter (Collision other) {
			if (OnCollisionEnterEvent != null) {
				OnCollisionEnterEvent (other);
			}
		}
		private void OnCollisionExit (Collision other) {
			if (OnCollisionExitEvent != null) {
				OnCollisionExitEvent (other);
			}
		}

		private void OnCollisionStay (Collision other) {
			if (OnCollisionStayEvent != null) {
				OnCollisionStayEvent (other);
			}
		}
	}
}