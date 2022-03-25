using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainersControl : MonoBehaviour
{
    public int bh;
    Text numtxt;
    int nowNum = 0;
    bool shunt = false;

    float nowTime = 0f;
    float maxTime = 3f;
    float speedTime = 1f;
    float chocolateSpeed = 1f;
    float chocolateSpeednow = 0f;
    MoldControl moldControl;
    void Start() {
        bh = int.Parse(transform.parent.parent.parent.gameObject.name);
        numtxt = gameObject.GetChildControl<Text>("Canvas/Text");
        moldControl = transform.parent.parent.gameObject.GetChildControl<MoldControl>("moldingMachine/Mold");
        ReSetUI();
    }
    private void ReSetUI() {
        numtxt.text = nowNum.ToString();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            ChocolateControl chocolateControl = other.gameObject.GetComponent<ChocolateControl>();
            int ShuntNum = chocolateControl.ShuntNum;
            if (ShuntNum == bh) {
                shunt = true;
            }
            else {
                shunt = false;
            }
            if (FactoryMgr.Instance.FactoryLv > bh) {
                if (shunt) {
                    chocolateControl.RecObj();
                    nowNum++;
                    ReSetUI();
                }
                else {
                    if (bh == 5) {
                        chocolateControl.ChangRotate(false);
                        chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh]);
                        shunt = !shunt;
                    }
                    else {
                        chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh + 1]);
                        shunt = !shunt;
                    }
                }
            }
            else if (FactoryMgr.Instance.FactoryLv > bh - 1) {
                chocolateControl.RecObj();
                nowNum++;
                ReSetUI();
            }
            else {
                chocolateControl.RecObj();
            }
        }
    }
    private void Update() {
        chocolateSpeednow = chocolateSpeed + moldControl.GetSpeed();
        if (nowNum <= 0) {
            return;
        }
        if (nowTime < maxTime) {
            nowTime += chocolateSpeednow * Time.deltaTime;
        }
        else {
            AddOneChocolate();
            nowTime = 0f;
        }
    }
    private void AddOneChocolate() {
        Transform ObjectGo = ObjectPool.Instance.Get("Factory", "FollowObj").transform;
        ObjectGo.position = transform.position;
        SplineFollower splineFollower = ObjectGo.gameObject.GetComponent<SplineFollower>();
        splineFollower.spline = ConveyorMgr.Instance.SplineSum[2 * bh];
        splineFollower.SetDistance(0f);
        splineFollower.followSpeed = chocolateSpeednow;
        Transform ObjectChocolate = ObjectPool.Instance.Get("Factory", "Chocolate").transform;
        ObjectChocolate.position = transform.position + new Vector3(-1f, 0f, 0f);
        ChocolateControl chocolateControl = ObjectChocolate.gameObject.GetComponent<ChocolateControl>();
        chocolateControl.OnStart(ObjectGo);
        chocolateControl.ChangeCondition(1);
        nowNum--;
        ReSetUI();
    }
}
