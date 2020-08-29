using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestImage : MonoBehaviour {
	LineRenderer line;
	//重力
	[Range (0.001f, 1)]
	public float gravity = 0.13f;
	//两点之间的距离
	[Range (0.2f, 1)]
	public float power = 0.2f;
	//点集合
	List<Vector3> m_List = new List<Vector3> ();
	private void Start () {
		line = GetComponent<LineRenderer> ();
	}
	void OnDrawGizmos () {
		//Quaternion x Vector3计算
		//Vector3.forward旋转transform.rotation的位置，等同于transform.forward
		Vector3 forward = transform.rotation * Vector3.forward * power;
		Vector3 newPos = transform.position;
		Vector3 lastPos = newPos;
		m_List.Add (newPos);
		int i = 0, iMax = 0;
		float dis = 0;
		while (lastPos.y > -0.5f && m_List.Count < 10000) {
			i++;
			//Vector3.up表明地心引力往下
			newPos = lastPos + forward + Vector3.up * i * (-gravity * 0.03f);
			dis += Vector3.Distance (lastPos, newPos);
			m_List.Add (newPos);
			lastPos = newPos;
		}
		iMax = m_List.Count;
		Gizmos.color = Color.red;
		line.SetVertexCount (iMax);
		for (i = 0; i < iMax; i++) {
			line.SetPosition (i, m_List[i]);
		}
		m_List.Clear ();
	}
}