using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LitJson;
using UnityEngine;

namespace Game {
	public class Test : MonoBehaviour {
		public List<MessageData<BaseMessageData>> p = new List<MessageData<BaseMessageData>> ();

		void Start () {
			// MessageData_1_1 temp = new MessageData_1_1 ();
			// temp.dealFlg = 1;
			// temp._ip = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 8888);
			// temp.players = new List<MessageData_1_1.PlayerInfo> () {
			// 	new MessageData_1_1.PlayerInfo () { pos = new Vector3 (0, 0, 0), rot = new Vector3 (1, 2, 3) },
			// 		new MessageData_1_1.PlayerInfo () { pos = new Vector3 (0, 0, 0), rot = new Vector3 (1, 2, 3) },
			// 		new MessageData_1_1.PlayerInfo () { pos = new Vector3 (0, 0, 0), rot = new Vector3 (1, 2, 3) },
			// 		new MessageData_1_1.PlayerInfo () { pos = new Vector3 (0, 0, 0), rot = new Vector3 (1, 2, 3) }
			// };
			// p.Add (new MessageData<BaseMessageData> () { body = temp });
			// byte[] b = ProtoBufTools.Serialize (p);

			// List<MessageData<BaseMessageData>> a = ProtoBufTools.DeSerialize<List<MessageData<BaseMessageData>>> (b);
			// Debug.Log (JsonMapper.ToJson (a));
			// Debug.Log (a.body._ip.ToString ());
			// StartCoroutine (DownloadCoroutine ());
			// StartCoroutine (DownloadCoroutine2 ());
		}
		Dictionary<string, AssetBundle> temp = new Dictionary<string, AssetBundle> ();
		IEnumerator DownloadCoroutine () {
			string url = "assetbundles_win/shared_res/";
			string[] files = new string[] {
				"soldier_relation",
				"effect_relation",
				"Particles_Alpha_Blended",
				"shared_atlas",
				"building_2_1_atlas",
			};
			foreach (string item in files) {
				AssetBundle ab1 = AssetBundle.LoadFromMemory (File.ReadAllBytes (Application.dataPath + "/qmresources/" + url + item));
				temp.Add (item, ab1);
				yield return 1;
			}

			Object[] _obj = temp["building_2_1_atlas"].LoadAllAssets ();

			Transform trans = transform.Find ("test");
			SkinnedMeshRenderer skin = trans.GetComponent<SkinnedMeshRenderer> ();
			skin.materials[0] = _obj[1] as Material;
			skin.materials[0].mainTexture = _obj[0] as Texture2D;
			string url2 = "assetbundles_win/mainscenebuilding/";
			string[] files2 = new string[] {
				"B_Drunkery",
				"B_Notice",
				"B_WorldBoss",
				"B_shangdian",
				"GroundDown2",
			};

			foreach (string item in files2) {
				AssetBundle ab1 = AssetBundle.LoadFromMemory (File.ReadAllBytes (Application.dataPath + "/qmresources/" + url2 + item));
				Object obj2 = ab1.LoadAsset (Application.dataPath + "/qmresources/" + url2 + "MonoBehaviour " + item + "_smdata");

				Object[] _obj2 = ab1.LoadAllAssets ();

				Debug.Log (item + " Start");
				Debug.Log (_obj2.Length);
				foreach (var item2 in _obj2) {
					Debug.Log (item2.name);
				}

				// Debug.Log (obj2.name);
				GameObject obj = (GameObject) GameObject.Instantiate (ab1.LoadAsset (item, typeof (GameObject)));
				Renderer[] meshSkinRenderer = obj.GetComponentsInChildren<Renderer> ();
				for (int i = 0; i < meshSkinRenderer.Length; i++) {
					// Debug.Log(meshSkinRenderer[i].material.shader.name);
					meshSkinRenderer[i].material.shader = Shader.Find ("Mobile/Particles/Alpha Blended");
					meshSkinRenderer[i].material.mainTexture = _obj[0] as Texture2D;
				}
				temp.Add (item, ab1);
				yield return 1;
			}
		}
		public enum RESOURCE_TYPE : int {
			UNKNOWN,
			SOLDIER,
			EFFECT,
			AVATAR_ATLAS,
			BIG_LEVEL_ICON, // 大关卡页面的图标
			SCENE_MAP, // 场景地图
			LUA_FILE,
			SOLDIER_ICON,
			MAIN_SCENE_OBJ,
			AUDIOCLIP,
			SKILL_ICON,
			PROPS_ICON,
			EQUIP_ICON,
			GUILD_FLAG_ICON,

			TEXTURE_ATLAS, //smoothMoves动画的TextureAtlas
			SHARED_ATLAS, //SmoothMoves动画间共享的TextureAtlas，目前仅有三个shadow
			SHARED_SHADER, //SmoothMoves动画间共享的shader文件，目前仅有一个
			RES_RELATION, //SmoothMoves动画的TextureAtlas引用关系
			SM_DATA, //SMAnimationData
			XML_RESOURCE, //关卡配置
			CONFIG, //配置bin
			TEXTURE_ICON, //其它的图片
		}

		// Update is called once per frame
		void Update () {

		}
	}
}