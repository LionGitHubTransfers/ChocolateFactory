using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateControl : MonoBehaviour
{
    List<Transform> condition = new List<Transform>();

    public int ShuntNum;

    public int nowbh = 0;
    SplineFollower splineFollower;
    Transform EndTrans;

    public void OnStart(Transform _EndTrans) {
        EndTrans = _EndTrans;
        splineFollower = EndTrans.gameObject.GetComponent<SplineFollower>();
        condition.Clear();
        foreach (Transform ss in transform) {
            condition.Add(ss);
            ss.gameObject.SetActive(false);
        }
        condition[0].gameObject.SetActive(true);
        ChangeCondition(0);
    }

    public void ChangeSpline(SplineComputer splineComputer) {
        splineFollower.spline = splineComputer;
        splineFollower.SetDistance(0f);
    }

    public void ChangeSpeed(float speed) {
        splineFollower.followSpeed = speed;
    }

    public void ChangeCondition(int bh) {
        if (nowbh == bh) {
            return;
        }
        foreach (Transform ss in condition) {
            ss.gameObject.SetActive(false);
        }
        condition[bh].gameObject.SetActive(true);
        nowbh = bh;
    }

    public void ChangRotate(bool isdefault) {
        if (isdefault == true) {
            transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        }
        else {
            transform.localEulerAngles = Vector3.zero;
        }
    }

    private void Update() {
        transform.position = EndTrans.position;
    }

    public void RecObj() {
        splineFollower.spline = ConveyorMgr.Instance.SplineSum[0];
        splineFollower.SetDistance(0f);
        ChangRotate(false);
        ObjectPool.Instance.Recycle(gameObject);
        splineFollower = null;
        ObjectPool.Instance.Recycle(EndTrans.gameObject);
    }
}
