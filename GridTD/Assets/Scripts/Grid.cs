using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts;

public class Grid : MonoBehaviour,IPointerClickHandler,IDragHandler,IDropHandler,IPointerEnterHandler,IPointerExitHandler{


    GameController gameController;

    public GridType type; 
	// Use this for initialization
	void Start () 
    {
        type = GridType.NullGrid;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        image = GetComponent<Image>();
	}
    Image image;
	// Update is called once per frame
	void Update () 
    {
        switch (type)
        {
            //case GridType.NullGrid:
            //    image.color = Color.white;
            //    break;
            case GridType.Road:
                image.color = Color.red;
                break;
            case GridType.Barrier:
                image.color = Color.black;
                break;
            case GridType.Turret:
                image.color = Color.blue;
                break;
        }
	}
    public void Prin()
    {
        print("www");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameController.turretInfoPanel.activeSelf||gameController.gridsLeftMenus.activeSelf||gameController.gridsRightMenus.activeSelf)
        {
            gameController.gridsLeftMenus.SetActive(false);
            gameController.gridsRightMenus.SetActive(false);
            gameController.turretInfoPanel.SetActive(false);
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (type == GridType.NullGrid)
            {
                gameController.gridsLeftMenus.SetActive(!gameController.gridsLeftMenus.activeSelf);
                gameController.gridsLeftMenus.transform.position = Input.mousePosition;
            }
            else if (type == GridType.Turret)
            {
                RectTransform rectTrans = gameController.turretInfoPanel.transform as RectTransform;
                if ((Camera.main.pixelWidth - Input.mousePosition.x) < rectTrans.rect.width)
                {
                    rectTrans.pivot = new Vector2(1, 1);
                    rectTrans.localPosition = Vector3.zero;
                }
                else
                {
                    rectTrans.pivot = new Vector2(0, 1);
                    rectTrans.localPosition = Vector3.zero;
                }
                print(Camera.main.pixelHeight - Input.mousePosition.y);
                if ((Camera.main.pixelHeight - Input.mousePosition.y) > rectTrans.rect.height)
                {
                    rectTrans.pivot = new Vector2(0, 0);
                    rectTrans.localPosition = Vector3.zero;
                }
                else
                {       
                    rectTrans.pivot = new Vector2(0, 1);
                    rectTrans.localPosition = Vector3.zero;
                }

                gameController.turretInfoPanel.SetActive(!gameController.turretInfoPanel.activeSelf);
                gameController.turretInfoPanel.transform.position = Input.mousePosition;
                gameController.turretInfoPanel.GetComponent<TurretInfoPanelController>().clickGO = transform.GetChild(0).gameObject;
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (type == GridType.NullGrid)
            {
                gameController.gridsRightMenus.SetActive(!gameController.gridsRightMenus.activeSelf);
                gameController.gridsRightMenus.transform.position = Input.mousePosition;
            }
        }

        GameObject.Find("GridsMenus").GetComponent<GridsMenusController>().go = gameObject;
    }

    int i = 0;
    public void OnDrag(PointerEventData eventData)
    {
        print(i++);
    }

    public void OnDrop(PointerEventData eventData)
    {
        print(6666);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Color co = GetComponent<Image>().color;
        co.a = 255;
        GetComponent<Image>().color = co;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Color co = GetComponent<Image>().color;
        co.a = 0;
        GetComponent<Image>().color = co;
    }
}
