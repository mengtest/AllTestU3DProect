using System.Collections.Generic;
using UnityEngine;

public class EllipseTest : MonoBehaviour {  
    public GameObject cubeModel;  
    public float r = 3;  
    public float R = 5;  
    private float angle = 0;  
    private int cubeCount = 14;
    private Vector3 center = Vector3.zero;  
    // Use this for initialization  
    List<Transform> list = new List<Transform>();
    void Start () {  
        center = cubeModel.transform.position;  
        for (int i=0; i < cubeCount; i++) {  
            GameObject cube = (GameObject)GameObject.Instantiate (cubeModel);  
            list.Add(cube.transform);
        }  
    }  

    void Update()
    {
        for (int i=0; i<list.Count; i++) {  
            Transform cube = list[i];
            float hudu = (angle/180)*Mathf.PI;  
            float xx = center.x + R*Mathf.Cos(hudu);  
            float yy = center.y + r*Mathf.Sin(hudu);  
            cube.position = new Vector3(xx,yy,0);  
            angle += 30;  
        }  
    }
}