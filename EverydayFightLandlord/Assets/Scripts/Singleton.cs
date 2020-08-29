using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    //泛型单例
    public class Singleton<T> where T : Singleton<T>,new ()
    {
        private static T _instance = null;
        public static T GetInstance()
        {
            if (_instance==null)
            {
                _instance = new T();
				_instance.Init();
            }
            return _instance;
        }
		public virtual void Init(){

		}
    }
}
