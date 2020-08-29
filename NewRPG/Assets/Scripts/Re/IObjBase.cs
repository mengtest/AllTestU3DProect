using UnityEngine;

public interface IObjBase {
    GameObject obj { get; set; }
    Transform trans { get; set; }
    void Show ();
    void Hide ();
}