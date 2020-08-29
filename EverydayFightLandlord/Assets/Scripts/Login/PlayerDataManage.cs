using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login {
	[System.Serializable]
	public class PlayerDataManage : Singleton<PlayerDataManage> {
		public List<PlayerData> list = new List<PlayerData> ();
		public PlayerData curPlayer;

		public static string _xmlPath;

		/// <summary>
		/// 第一次创建文本
		/// </summary>
		public void FristCreateDocument () {
			//新建xml实例
			XmlDocument xmlDoc = new XmlDocument ();

			//创建根节点，最上层节点
			XmlElement root = xmlDoc.CreateElement ("root");
			xmlDoc.AppendChild (root);

			//二级节点
			XmlElement player = xmlDoc.CreateElement ("player");
			root.AppendChild (player);

			xmlDoc.Save (_xmlPath);
		}

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="_username"></param>
		/// <param name="_password"></param>
		public void CreateUser (string _username, string _password) {
			XmlDocument doc = new XmlDocument ();
			doc.Load (_xmlPath);
			XmlNode nodeList = doc.SelectSingleNode ("root/player");
			//创建用户节点
			XmlElement el = doc.CreateElement ("p");
			el.SetAttribute ("id", list.Count + "");
			el.SetAttribute ("name", "");
			el.SetAttribute ("username", _username);
			el.SetAttribute ("password", _password);
			el.SetAttribute ("score", "0");
			el.SetAttribute ("image", "");
			nodeList.AppendChild (el);
			//保存文件
			doc.Save (_xmlPath);
		}

		/// <summary>
		/// 更新当前角色到Xml文档
		/// </summary>
		public void UpdateCurPlayerInfoToXml () {
			XmlDocument doc = new XmlDocument ();
			doc.Load (_xmlPath);
			XmlNodeList list = doc.SelectSingleNode ("root/player").ChildNodes;

			foreach (XmlElement element in list) {
				if (element.GetAttribute ("id") == curPlayer.id) {
					element.SetAttribute ("name", curPlayer.name);
					element.SetAttribute ("password", curPlayer.password);
					element.SetAttribute ("score", curPlayer.score);
					element.SetAttribute ("image", curPlayer.imageRef);
				}
			}
			doc.Save (_xmlPath);
		}

		/// <summary>
		/// 更新角色信息到Xml文档
		/// </summary>
		/// <param name="_data"></param>
		public void UpdatePlayerInfoToXml (PlayerData _data) {
			XmlDocument doc = new XmlDocument ();
			doc.Load (_xmlPath);
			XmlNodeList list = doc.SelectSingleNode ("root/player").ChildNodes;

			foreach (XmlElement element in list) {
				if (element.GetAttribute ("id") == _data.id) {
					element.SetAttribute ("name", _data.name);
					element.SetAttribute ("username", _data.username);
					element.SetAttribute ("password", _data.password);
					element.SetAttribute ("score", _data.score);
					element.SetAttribute ("image", _data.imageRef);
				}
			}
			doc.Save (_xmlPath);
		}
	}
}