using System;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;

namespace Game {
    public class Player : MonoBehaviour {
        public XDocument xDocument;
        LineRenderer line;
        public GameObject obj;
        public Transform trans;
        public Canvas canvas = null;
        public Text lb_name = null;
        public Rigidbody _rigidbody;
        public GIPEndPoint curIp = null;
        public bool isLocalPlayer = false;
        public event Action StartCallBack = null;
        public Vector3 moveToPos = Vector3.zero;
        public float aimValue = 0.45f;
        public Vector3 curAimPos;
        public int JumpVal = 15;
        void FixedUpdate () {
            if (trans == null) {
                return;
            }
            if (isLocalPlayer) {
                if (_rigidbody.velocity != Vector3.zero) {
                    MessageManage.Self.Send_1_11 (
                        new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                            {
                                MessageData_1_11.OperType.move, new List<GVector3> () {
                                    trans.localPosition,
                                        trans.rotation.eulerAngles
                                }
                            }
                        });
                }
            } else {
                if (trans.localPosition != moveToPos) {
                    trans.localPosition = Vector3.Lerp (trans.localPosition, moveToPos, 1f);
                    if ((moveToPos - trans.localPosition).magnitude < 0.01f) {
                        trans.localPosition = moveToPos;
                    }
                }
            }
        }
        void Start () {
            Init (gameObject);
        }
        void Update () {
            if (trans == null) {
                return;
            }
            if (isLocalPlayer) {
                float x = 0;
                float y = 0;
                if (Input.GetKey (KeyCode.W)) {
                    y = 0.2f;
                }
                if (Input.GetKey (KeyCode.S)) {
                    y = -0.2f;
                }
                if (Input.GetKey (KeyCode.A)) {
                    x = -0.2f;
                }
                if (Input.GetKey (KeyCode.D)) {
                    x = 0.2f;
                }
                if (x != 0 || y != 0) {
                    Vector3 vet = new Vector3 (x, 0, y);
                    WantMove (vet);
                }

                if (Input.GetKeyDown (KeyCode.L)) {
                    LockPlayer ();
                }
                if (Input.GetKeyDown (KeyCode.K)) {
                    MessageManage.Self.Send_1_11 (
                        new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                            {
                                MessageData_1_11.OperType.jump, new List<GVector3> () {
                                    new GVector3 () {
                                        y = JumpVal
                                    }
                                }
                            }
                        }
                    );
                }
                if (Input.GetKeyDown (KeyCode.J)) {
                    MessageManage.Self.Send_1_11 (
                        new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                            {
                                MessageData_1_11.OperType.attack, new List<GVector3> ()
                            }
                        }
                    );
                }
                if (Input.GetKey (KeyCode.UpArrow)) {
                    MessageManage.Self.Send_1_11 (
                        new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                            {
                                MessageData_1_11.OperType.aim, new List<GVector3> () {
                                    new GVector3 (0, aimValue, 0),
                                        Vector3.down
                                }
                            }
                        }
                    );
                }
                if (Input.GetKey (KeyCode.DownArrow)) {
                    MessageManage.Self.Send_1_11 (
                        new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                            {
                                MessageData_1_11.OperType.aim, new List<GVector3> () {
                                    new GVector3 (0, aimValue, 0),
                                        Vector3.up
                                }
                            }
                        }
                    );
                }
            }
            SetNamePos ();
            curAimPos = Vector3.Lerp (trans.up.normalized * 2, trans.forward.normalized * 2, aimValue);
            DrawLine ();
        }

        public void SetAimValue (List<GVector3> _list) {
            Vector3 _aimValue = _list[0];
            Vector3 _dir = _list[1];
            aimValue = _aimValue.y;
            aimValue += _dir.y / 100;
            aimValue = Mathf.Clamp (aimValue, 0, 1);
        }
        void Init (GameObject _obj) {
            obj = _obj;
            trans = _obj.transform;
            canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
            lb_name = canvas.transform.Find ("lb_name").GetComponent<Text> ();
            lb_name = GameObject.Instantiate (lb_name.gameObject).GetComponent<Text> ();
            lb_name.gameObject.SetActive (true);
            lb_name.transform.SetParent (canvas.transform);
            line = obj.GetComponent<LineRenderer> ();
            _rigidbody = obj.GetComponent<Rigidbody> ();
            if (StartCallBack != null) {
                StartCallBack ();
            }
        }
        public void SetNamePos () {
            Vector3 tarpos = trans.position;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint (Camera.main, tarpos);
            lb_name.transform.position = pos;
        }
        public void WantMove (Vector3 _moveDir) {
            if (lockPlayer != null) {
                // Vector3 pos = trans.position - CameraController.Self.transform.position;
                Vector3 dirr = (lockPlayer.trans.position - trans.position).normalized;
                _moveDir = Quaternion.LookRotation (dirr, Vector3.up) * _moveDir;
            }
            Vector3 dir = _moveDir.normalized * 8f;
            Vector3 velocity = new Vector3 (dir.x, _rigidbody.velocity.y, dir.z);
            MessageManage.Self.Send_1_11 (
                new Dictionary<MessageData_1_11.OperType, List<GVector3>> () {
                    {
                        MessageData_1_11.OperType.wantMove, new List<GVector3> () {
                            _moveDir,
                            velocity,
                        }
                    }
                }
            );
        }
        public void Move (List<GVector3> _list) {
            Vector3 dir = _list[0];
            Vector3 velocity = _list[1];
            Quaternion quaDir = Quaternion.LookRotation (dir, Vector3.up); //返回看向这个目标的角度
            trans.rotation = Quaternion.Lerp (transform.rotation, quaDir, Time.fixedDeltaTime * 15);
            _rigidbody.velocity = velocity;
        }
        public void SetTrans (List<GVector3> _list) {
            Vector3 _pos = _list[0];
            Vector3 _rot = _list[1];
            moveToPos = _pos;
            trans.rotation = Quaternion.Slerp (trans.rotation, Quaternion.Euler (_rot), 1);
        }
        public void Jump (List<GVector3> _list) {
            Vector3 _jump = _list[0];
            _rigidbody.velocity = _rigidbody.velocity + _jump;
        }
        public void SetName (string _name) {
            lb_name.text = _name;
        }
        public void SetColor () {
            lb_name.color = Color.red;
        }
        public void Quit () {
            GameObject.Destroy (lb_name.gameObject);
            GameObject.Destroy (obj);
        }
        public void Attack () {
            Danmu _obj = GameObject.Instantiate<GameObject> (Resources.Load ("Danmu") as GameObject).GetComponent<Danmu> ();
            _obj.transform.localPosition = trans.localPosition + curAimPos;
            _obj.endPos = _obj.transform.localPosition + curAimPos * 5;
            _obj.p = this;
        }

        public Player lockPlayer = null;
        public void LockPlayer () {
            if (lockPlayer == null) {
                foreach (Player other in Main.Self.playerList.Values) {
                    if (other.curIp != curIp) {
                        float dis = Vector3.Distance (other.trans.position, trans.position);
                        if (dis < 10) {
                            lockPlayer = other;
                            CameraController.Self.SetLockPlayer (other);
                            return;
                        }
                    }
                }
            } else {
                lockPlayer = null;
                CameraController.Self.SetLockPlayer (null);
            }
        }
        //重力
        [Range (0.001f, 1)]
        public float gravity = 1f;
        //两点之间的距离
        [Range (0.2f, 1)]
        public float shootPower = 0.2f;
        public float offetPower = 0.2f;
        public bool isOpenDrawLine = true;
        //点集合
        List<Vector3> m_List = new List<Vector3> ();
        void DrawLine () {
            //Quaternion x Vector3计算
            //Vector3.forward旋转transform.rotation的位置，等同于transform.forward
            m_List.Add (transform.position);
            m_List.Add (transform.position + curAimPos);
            if (isOpenDrawLine) {
                Vector3 forward = curAimPos * shootPower;
                Vector3 lastPos = transform.position + curAimPos;
                Vector3 newPos = Vector3.zero;
                int idx = 0;
                while (lastPos.y > -0.5f && m_List.Count < 10000) {
                    idx++;
                    //Vector3.up表明地心引力往下
                    newPos = lastPos + forward + Vector3.up * idx * (-gravity * 0.03f);
                    m_List.Add (newPos);
                    lastPos = newPos;
                }
            }
            int iMax = m_List.Count;
            // line.SetVertexCount (iMax);
			line.positionCount = iMax;
            for (int i = 0; i < iMax; i++) {
                line.SetPosition (i, m_List[i]);
            }
            m_List.Clear ();
        }
    }
}