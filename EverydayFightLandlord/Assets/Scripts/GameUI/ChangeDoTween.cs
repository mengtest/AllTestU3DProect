using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeDoTween : MonoBehaviour,IPointerClickHandler{

	// Use this for initialization
	void Start ()
    {
        transform.localPosition = Vector3.zero;
        if (GetComponent<Image>().enabled)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.25f);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(Vector3.zero, 0.25f);
        Destroy(gameObject, 0.25f);
    }
}
