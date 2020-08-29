using System;
using System.Net;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class Player2 : MonoBehaviour {
        public Vector3 jiaodu;
        public float lidu;
        LineRenderer line;
        Transform l0;
        Transform l1;
        public void Start () {
            l0 = transform.Find ("ll/0");
            l1 = transform.Find ("ll/1");
        }
        Vector3 curRot = Vector3.zero;
        Vector3 rot = Vector3.zero;
        int isLeft = 0;
        public float maxSpeed;
        public float curSpeed;
        public Vector3 endPos;
        public Vector3 endRot;

        public void Move (Vector3 _dic) {
            curSpeed = Mathf.Lerp (curSpeed, maxSpeed, 0.01f);
            curSpeed = Mathf.Clamp (curSpeed, 0, maxSpeed);
            endPos = transform.position + _dic * 0.1f * curSpeed;
        }

        public void LerpMove () {
            if (transform.position != endPos) {
                transform.position = Vector3.Lerp (transform.position, endPos, 0.7f * curSpeed);
                if ((endPos - transform.position).magnitude < 0.01f) {
                    transform.position = endPos;
                }
            }
        }
        public void LerpRot () {
            if (isLeft != 0) {
                float angle = Vector3.Angle (transform.forward, l0.forward);
                Vector3 t = transform.localRotation.eulerAngles;
                if (isLeft == 1) {
                    transform.localRotation = Quaternion.Euler (t + new Vector3 (0, -angle * curSpeed * 0.05f, 0));
                } else if (isLeft == 2) {
                    transform.localRotation = Quaternion.Euler (t + new Vector3 (0, angle * curSpeed * 0.05f, 0));
                }
            }
        }
        void FixedUpdate () {

        }

        void Update () {
            if (Input.GetKey (KeyCode.W)) {
                Move (l0.forward);
                LerpRot ();
            } else if (Input.GetKey (KeyCode.S)) {
                Move (-l0.forward);
                LerpRot ();
            } else {
                curSpeed = Mathf.Lerp (curSpeed, 0, 0.01f);
                Move (Vector3.zero);
            }
            LerpMove ();
            if (Input.GetKey (KeyCode.A)) {
                if (isLeft != 1) {
                    curSpeed -= 0.5f;
                }
                isLeft = 1;
                curRot = l0.localRotation.eulerAngles;
                if (curRot.x > 315 || curRot.x <= 45) {
                    curRot += Vector3.left;
                }
                l0.localRotation = Quaternion.Euler (curRot);
                l1.localRotation = Quaternion.Euler (curRot);
            } else if (Input.GetKey (KeyCode.D)) {
                if (isLeft != 2) {
                    curSpeed -= 0.5f;
                }
                isLeft = 2;
                curRot = l0.localRotation.eulerAngles;
                if (curRot.x > 315 || curRot.x <= 45) {
                    curRot += Vector3.right;
                }
                l0.localRotation = Quaternion.Euler (curRot);
                l1.localRotation = Quaternion.Euler (curRot);
            } else {
                isLeft = 0;
                if (l0.localRotation.eulerAngles != Vector3.zero) {
                    l0.localRotation = Quaternion.Lerp (l0.localRotation, Quaternion.Euler (Vector3.zero), 0.1f);
                }
                if (l1.localRotation.eulerAngles != Vector3.zero) {
                    l1.localRotation = Quaternion.Lerp (l0.localRotation, Quaternion.Euler (Vector3.zero), 0.1f);
                }
            }
        }
    }
}