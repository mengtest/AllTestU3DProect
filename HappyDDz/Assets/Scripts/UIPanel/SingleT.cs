using System;
public class SingleT<T> where T : class, new () {
    private static T _self;
    public static T Self {
        get {
            if (_self == null) {
                _self = new T ();
            }
            return _self;
        }
    }
}