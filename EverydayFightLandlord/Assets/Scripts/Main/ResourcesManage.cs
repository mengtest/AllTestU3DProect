using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Main
{
    /// <summary>
    /// 资源管理类
    /// </summary>
    [System.Serializable]
    public class ResourcesManage
    {
        public static Dictionary<string, UnityEngine.Object> dictionary = new Dictionary<string, UnityEngine.Object>();
        public static List<Sprite> headImage = new List<Sprite>();
        public static int headImageIndex = 0;

        /// <summary>加载全部图片资源进字典
        /// </summary>
        public static void LoadAll()
        {
            foreach (UnityEngine.Object item in Resources.LoadAll(""))
            {
                if (!dictionary.ContainsKey(item.name))
                {
                    dictionary.Add(item.name, item);
                }
            }
        }

        /// <summary>加载全部头像图片资源
        /// </summary>
        public static void LoadHeadImages()
        {
            foreach (Sprite head in Resources.LoadAll<Sprite>("head/"))
            {
                headImage.Add(head);
            }
        }

        /// <summary>从头像名字获取图片
        /// </summary>
        public static Sprite GetHeadImage(string _name)
        {
            for (int i = 0; i < headImage.Count; i++)
            {
                if (headImage[i].name == _name)
                {
                    return headImage[i];
                }
            }
            return null;
        }

        /// <summary>创建2D精灵
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static Sprite CreateSprite(string _str)
        {
            Texture2D img = dictionary[_str] as Texture2D;
            Sprite pic = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
            return pic;
        }
    }
}
