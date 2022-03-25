using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : Singleton<CameraMgr> {
    Transform CameraF;
    Transform Camera;
    Vector3 Dir;
    bool isOther = false;
    Transform nowOther;
    float smoothing = 5f;
    Vector3 targetCamPos;
    float nowTime = 0f;
    float timeLong = 0f;
    public void Init() {
        if (CameraF == null) {
            CameraF = new GameObject("CameraF").transform;
        }
        InitCamera();
        InitMsg();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }
    public void OnLateUpdate() {
        if (isOther) {
            //Camera.position = nowOther.position - Dir;
            targetCamPos = nowOther.position - Dir / 2f;
            if (nowTime < timeLong) {
                nowTime += Time.deltaTime;
            }
            else {
                isOther = false;
                nowTime = 0f;
            }
        }
        else {
            //Camera.position = PlayerMgr.Instance.Player.transform.position - Dir;
            targetCamPos = PlayerMgr.Instance.Player.transform.position - Dir;
        }

        Camera.position = Vector3.Lerp(Camera.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void MoveToOther(Transform ObjectGo, float _timeLong) {
        nowOther = ObjectGo;
        timeLong = _timeLong;
        isOther = true;
    }

    public void MoveToPlayer() {
        isOther = false;
        nowTime = 0f;
    }

    private void InitCamera() {
        Camera = ObjectPool.Instance.Get("Player", "camPos", CameraF).transform;
        Camera.position = new Vector3(0f, 11f, -30f);
        Dir = PlayerMgr.Instance.Player.position - Camera.position;
    }
}
