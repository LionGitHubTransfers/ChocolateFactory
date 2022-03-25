using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGoodsRoot : MonoBehaviour
{
    int bh;
    DisplayGoodsControl displayGoodsControl;
    GoodsRootCondition goodsRootCondition = GoodsRootCondition.None;
    List<Transform> chocolateSum = new List<Transform>();
    public int nowNum = 0;
    public int nowTureNum = 0;
    List<Transform> CustomerSum = new List<Transform>();
    List<Transform> porterSum = new List<Transform>();
    private void Start() {
        displayGoodsControl = transform.parent.gameObject.GetComponent<DisplayGoodsControl>();
        bh = displayGoodsControl.bh;
        foreach (Transform ss in transform) {
            chocolateSum.Add(ss);
            ss.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 3) {
            if (other.gameObject.tag == "Player") {
                InvokeRepeating("DisplayChocolate", 0.25f, 0.25f);
            }
            else if (other.gameObject.tag == "Customer") {
                CustomerSum.Add(other.gameObject.transform);
                InvokeRepeating("DisplayChocolateCustomer", 0.25f, 0.25f);
            }

        }
        else if (other.gameObject.layer == 7) {
            if (other.gameObject.tag == "porter") {
                porterSum.Add(other.gameObject.transform);
                if (porterSum.Count == 1) {
                    InvokeRepeating("DisplayChocolatePorter", 0.25f, 0.25f);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3 ) {
            if (other.gameObject.tag == "Player") {
                CancelInvoke("DisplayChocolate");
            }
            else if (other.gameObject.tag == "Customer") {
                CustomerSum.Remove(other.gameObject.transform);
                if (CustomerSum.Count <= 0) {
                    CancelInvoke("DisplayChocolateCustomer");
                }
            }
        }
        else if (other.gameObject.layer == 7) {
            if (other.gameObject.tag == "porter") {
                porterSum.Remove(other.gameObject.transform);
                if (porterSum.Count == 0) {
                    CancelInvoke("DisplayChocolatePorter");
                }
            }
        }
    }
    public bool JudgeMax() {
        if (nowNum < chocolateSum.Count) {
            return false;
        }
        else {
            return true;
        }
    }
    private void DisplayChocolate() {
        if (goodsRootCondition == GoodsRootCondition.Full) {
            return;
        }
        PlayerControl playerControl = PlayerMgr.Instance.Player.gameObject.GetComponent<PlayerControl>();
        if (playerControl.nowChocolateNum > 0) {
            int nowbh = bh * 2 + 1;
            if (playerControl.GetEndChocolatebh2D(nowbh)) {
                GetChocolateView(playerControl.ChocolateEnds[playerControl.nowChocolateNum - 1].position, bh * 2);
                playerControl.RemoveChocolate(nowbh);
            }
        }
    }

    private void DisplayChocolatePorter() {
        if (goodsRootCondition == GoodsRootCondition.Full) {
            return;
        }
        foreach (Transform ss in porterSum) {
            PorterControl porterControl = ss.gameObject.GetComponent<PorterControl>();
            if (porterControl.porterCondition != PorterCondition.Display) {
                return;
            }
            if (porterControl.nowNum > 0) {
                if (bh == porterControl.GetEndChocolatebh2D()) {
                    GetChocolateView(porterControl.ChocolateSum[porterControl.nowNum - 1].position, bh * 2);
                    porterControl.RemoveChocolate();
                }
            }
            if (goodsRootCondition == GoodsRootCondition.Full) {
                return;
            }
        }
    }

    private void DisplayChocolateCustomer() {
        if (nowNum == 0) {
            return;
        }
        foreach (Transform ss in CustomerSum) {

            CustomerControl customerControl = ss.gameObject.GetComponent<CustomerControl>();
            if (customerControl.chocolateBh == bh) {
                if (customerControl.customerFace == CustomerFace.Sad) {
                    return;
                }
                if (!customerControl.IsChocolateMax()) {
                    customerControl.GetChocolate(chocolateSum[nowNum - 1]);
                    chocolateSum[nowNum - 1].gameObject.SetActive(false);
                    nowNum--;
                    nowTureNum--;
                    goodsRootCondition = GoodsRootCondition.None;
                }
            }
            if (nowNum == 0) {
                return;
            }
        }
    }



    private void GetChocolateView(Vector3 startPos, int bh) {
        Transform ObjectGo = ObjectPool.Instance.Get("Factory", "ChocolateEnd").transform;
        ObjectGo.position = startPos;
        ChocolateEndControl chocolateEndControl = ObjectGo.gameObject.GetComponent<ChocolateEndControl>();
        chocolateEndControl.OnStart(bh + 1);
        Vector3 StartPos = startPos;
        Vector3 EndPos = chocolateSum[nowTureNum].position;
        Tween tween = DOTween.To(setter: value => {
            ObjectGo.position = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f);
        ObjectGo.DORotate(chocolateSum[nowTureNum].rotation.eulerAngles, 0.4f);
        nowTureNum++;
        if (nowTureNum >= chocolateSum.Count) {
            goodsRootCondition = GoodsRootCondition.Full;
        }
        tween.OnComplete(() => {
            ObjectPool.Instance.Recycle(ObjectGo.gameObject);
            chocolateSum[nowNum].gameObject.SetActive(true);
            nowNum++;
        });
        
    }
}

public enum GoodsRootCondition {
    None,
    Full,
}
