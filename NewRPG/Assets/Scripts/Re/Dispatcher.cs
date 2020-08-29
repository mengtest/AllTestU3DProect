using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public static class Dispatcher
{
	public delegate void Handler(object[] _obj);
    private static Dictionary<string, Handler> handlers = new Dictionary<string, Handler>();
    public static void DummyHandler(object data)
    {
    }
	
    public static void RegisterProtocalListener(string type, Handler handler)
	{
		if (handlers.ContainsKey(type) == false)
        {
            handlers.Add(type, new Handler(DummyHandler));
		}
        handlers[type] += handler;
	}
	
    public static void CancelProtocalListener(string type, Handler handler)
	{
		if (handlers.ContainsKey(type))
		{
			handlers[type] -= handler;
		}
	}
	
	public static void SendProtocalEvent(string type)
	{
		if (handlers.ContainsKey(type))
		{
			handlers[type](null);
		}
	}
	
	public static void SendProtocalEvent(string type,params object[] data)
	{
		if (handlers.ContainsKey(type))
		{
			handlers[type](data);
		}
	}
}
