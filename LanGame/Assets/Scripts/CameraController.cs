using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game {
	public enum CameraMode {
		/// <summary>
		/// 俯视
		/// </summary>
		Normal,
		/// <summary>
		/// 第三视角透视
		/// </summary>
		Perspective,
		/// <summary>
		/// 锁定视角
		/// </summary>
		LockCamara,
	}
	public class CameraController : MonoBehaviour {
		private static CameraController _self = null;
		public static CameraController Self {
			get {
				return _self;
			}
		}

		public List<GVector3[]> cameraModeList = new List<GVector3[]> {
			new GVector3[] {
				new Vector3 (0, 8f, -6f),
					new Vector3 (54, 0, 0),
			},
			new GVector3[] {
				new Vector3 (0, 2.2f, -4f), //位置
					new Vector3 (20, 0, 0), //旋转角度
			},
		};
		public CameraMode cameraMode = CameraMode.Normal;
		void Start () {
			_self = this;
		}
		void Update () {
			if (Main.Self.curPlayer != null && lockPlayer == null) {
				GVector3[] info;
				float lerpTime = 0.05f;
				switch (cameraMode) {
					case CameraMode.Normal:
						info = cameraModeList[0];
						transform.position = Vector3.Lerp (transform.position, Main.Self.curPlayer.trans.position + info[0], lerpTime);
						transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (info[1]), lerpTime);
						break;
					case CameraMode.Perspective:
						info = cameraModeList[1];
						lerpTime = 0.1f;
						transform.position = Vector3.Lerp (transform.position, Main.Self.curPlayer.trans.position + info[0], lerpTime);
						transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (info[1]), lerpTime);
						break;
					case CameraMode.LockCamara:
						if (lockPlayer == null) {
							cameraMode = CameraMode.Normal;
							break;
						}
						Transform one = Main.Self.curPlayer.trans;
						Transform two = lockPlayer.trans;
						Vector3 dic = (two.position - one.position).normalized;
						transform.position = Vector3.Lerp (transform.position, one.position - dic * 3 + Vector3.up, 1);
						transform.rotation = Quaternion.LookRotation (dic, Vector3.up);
						return;
					default:
						break;
				}
			}
		}
		Quaternion lockRot = Quaternion.identity;
		Player lockPlayer = null;
		public void SetLockPlayer (Player player) {
			if (lockPlayer == null && player != null) {
				lockPlayer = player;
			} else {
				lockPlayer = null;
				lockRot = Quaternion.identity;
				return;
			}
		}
	}
}