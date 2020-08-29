using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.Login
{
    [System.Serializable]
    public class PlayerData
    {
        public string id;
        public string name;
        public string username;
        public string password;
        public string score;
        public string imageRef;

        public PlayerData(XmlElement emlent)
        {
            id = emlent.GetAttribute("id");
            name = emlent.GetAttribute("name");
            username = emlent.GetAttribute("username");
            password = emlent.GetAttribute("password");
            score = emlent.GetAttribute("score");
            imageRef = emlent.GetAttribute("image");
        }

        public PlayerData(PlayerData data)
        {
            id = data.id;
            name = data.name;
            username = data.username;
            password = data.password;
            score = data.score;
            imageRef = data.imageRef;
        }

        public PlayerData()
        {
			imageRef = "1";
			score = "0";
			name = "66";
        }
    }
}