using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour {

    Transform targetPos;
    Vector3 targetDir;
    float timer;

    public void SetTarget(Transform _targetPos) {
        targetPos = _targetPos;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0 || targetPos == null)
            Destroy(gameObject);
    }

    IEnumerator RotateToTheTarget() {
        while (enabled) {
            targetDir = GameManager.instance.player.transform.position - targetPos.position;
            targetDir.y = 0;
            float angleToRotateOverlay = Vector3.SignedAngle(targetDir, GameManager.instance.player.transform.forward, new Vector3 (0, 1, 0));
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angleToRotateOverlay));
            yield return null;
        }
    }

    public void Register(Transform target) {
        SetTarget(target);
        StartCoroutine(RotateToTheTarget());
        timer = 3;

    }

















    /*    private const float MaxTimer = 8.0f;
        private float timer = MaxTimer;

        private CanvasGroup canvasGroup = null;
        protected CanvasGroup CanvasGroup {
            get {
                if (canvasGroup == null) {
                    canvasGroup = GetComponent<CanvasGroup>();
                    if (canvasGroup == null) {
                        canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    }
                }
                return canvasGroup;
            }

        }

        private RectTransform rect = null;
        protected RectTransform Rect {
            get {
                if (rect == null) {
                    rect = GetComponent<RectTransform>();
                    if (rect == null) {
                        rect = gameObject.AddComponent<RectTransform>();
                    }
                }
                return rect;
            }

        }

        public Transform Target { get; protected set; } = null;
        private Transform player = null;

        private IEnumerator IE_Countdown = null;
        private Action unRegister = null;

        private Quaternion tRot = Quaternion.identity;
        private Vector3 tPos = Vector3.zero;

        public void Register(Transform target, Transform player, Action unRegister) {
            this.Target = target;
            this.player = player;
            this.unRegister = unRegister;

            StartCoroutine(RotateToTheTarget());
            StartTimer();

        }

        public void Restart() {
            timer = MaxTimer;
            StartTimer();
        }

        private void StartTimer() {
            if (IE_Countdown != null) { StopCoroutine(IE_Countdown); }
            IE_Countdown = Countdown();
            StartCoroutine(IE_Countdown);
        }

        IEnumerator RotateToTheTarget() {
            while (enabled) {

                if (Target) {
                    tPos = Target.position;
                    //tRot = Target.rotation;
                }
                Vector3 direction = player.position - tPos;

                tRot = Quaternion.LookRotation(direction);
                tRot.z = -tPos.y;
                tRot.x = 0;
                tRot.y = 0;

                Vector3 northDirection = new Vector3(0, 0, player.eulerAngles.y);
                Rect.localRotation = tRot * Quaternion.Euler(northDirection);

                yield return null;
            }


        }

        private IEnumerator Countdown() {
            while (CanvasGroup.alpha < 1.0f) {
                canvasGroup.alpha += 4 * Time.deltaTime;
                yield return null;
            }

            while (timer > 0) {
                timer--;
                yield return new WaitForSeconds(1);
            }

            while (canvasGroup.alpha > 0.0f) {
                CanvasGroup.alpha -= 2 * Time.deltaTime;
                yield return null;
            }
            unRegister();
            Destroy(gameObject);
        }*/
}
