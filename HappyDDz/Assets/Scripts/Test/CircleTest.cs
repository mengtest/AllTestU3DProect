using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleTest : MonoBehaviour {

	public float radius = 10;	//圆半径：距离中心点多远
	public float angle = 190;			//角度
	public int segments = 15;	//等分
	public Vector3 centerCircle =new Vector3(0,0,0);	//中心点
	public GameObject _go;
	List<Transform> list = new List<Transform>();

	void Start () {
		Vector3[] vertices = new Vector3[segments + 1];  
		vertices[0] = centerCircle;  
		float deltaAngle = Mathf.Deg2Rad * angle / segments;
		float currentAngle = 0;  
		for (int i = 1; i < vertices.Length; i++)  
		{  
			list.Add(Instantiate(_go).transform);
			float cosA = Mathf.Cos(currentAngle);  
			float sinA = Mathf.Sin(currentAngle);  
			vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);  
			currentAngle += deltaAngle;  
			list[list.Count - 1].position = vertices[i];
		}  
	}
	
	void Update () 
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].LookAt(_go.transform);
		}
	}
}
