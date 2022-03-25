using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayControl : MonoBehaviour
{
    Transform production;
    List<Transform> productionSum = new List<Transform>();
    Text numtext;
    public int nowNum = 0;
    int maxNum = 20;
    PlayerControl playerControl;
    List<PorterControl> porterControls = new List<PorterControl>();
    int nowbh;
    bool tipOnce = false;
    private void Start() {
        nowbh = int.Parse(transform.parent.parent.parent.gameObject.name);
        production = gameObject.GetChildControl<Transform>("production");
        foreach (Transform ss in production) {
            productionSum.Add(ss);
            ss.gameObject.SetActive(false);
        }
        numtext = gameObject.GetChildControl<Text>("sign/num");
        ReSetUI();
    }

    private void ReSetUI() {
        numtext.text = nowNum.ToString();
    }

    public void OnTriggerEnterSign(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            tipOnce = true;
            playerControl = other.gameObject.GetComponent<PlayerControl>();
            InvokeRepeating("MoveChocolate2Player", 0.25f, 0.25f);
        }
        else if (other.gameObject.layer == 7 && other.gameObject.tag == "porter") {
           
            PorterControl porterControl = other.gameObject.GetComponent<PorterControl>();
            if (nowbh == porterControl.bh) {
                porterControls.Add(porterControl);
                if (porterControls.Count == 1) {
                    InvokeRepeating("MoveChocolate2Porter", 0.25f, 0.25f);
                }
            }
        }
    }
    public void OnTriggerExitSign(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            tipOnce = false;
            CancelInvoke("MoveChocolate2Player");
        }
        else if (other.gameObject.layer == 7 && other.gameObject.tag == "porter") {
            PorterControl porterControl = other.gameObject.GetComponent<PorterControl>();
            porterControls.Remove(porterControl);
            if (porterControls.Count == 0) {
                CancelInvoke("MoveChocolate2Porter");
            }
        }
    }
    public void OnTriggerEnterProduction(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            ChocolateControl chocolateControl = other.gameObject.GetComponent<ChocolateControl>();
            int bh = chocolateControl.nowbh;
            AddChorolate(other.gameObject.transform.position, bh);
            chocolateControl.RecObj();
        }
    }

    private bool JudgeMax() {
        if (nowNum >= maxNum) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool JudgeEmpty() {
        if (nowNum > 0) {
            return false;
        }
        else {
            return true;
        }
    }
    private void AddChorolate(Vector3 StartPos, int bh) {
        if (JudgeMax()) {

        }
        else {
            Transform ObjectGo = ObjectPool.Instance.Get("Factory", "ChocolateEnd").transform;
            ChocolateEndControl chocolateEndControl = ObjectGo.gameObject.GetComponent<ChocolateEndControl>();
            chocolateEndControl.OnStart(bh);
            Vector3 EndPos = productionSum[nowNum].position;
            Tween tween = DOTween.To(setter: value => {
                ObjectGo.position = EMath.Parabola(StartPos, EndPos, 1f, value);
            }, startValue: 0, endValue: 1, duration: 0.5f);
            tween.OnComplete(() => {
                ObjectPool.Instance.Recycle(ObjectGo.gameObject);
                productionSum[nowNum].gameObject.SetActive(true);
                nowNum++;
                ReSetUI();
            });
            
        }
    }

    private void MoveChocolate2Player() {
        if (playerControl.playerCondition == PlayerCondition.CarryCocoa) {
            return;
        }
        if (playerControl.SearchChocolateMax()) {
            return;
        }
        if (nowbh != 1 && MarketMgr.Instance.chocolateNum[nowbh - 1] <= 0) {
            if (tipOnce) {
                CameraMgr.Instance.MoveToOther(MarketMgr.Instance.GetDisplayZero(nowbh), 1f);

                BattleWindow.Instance.TipAc(1);
                tipOnce = false;
            }
            
            return;
        }
        else {
            int nowChocolateBh = DecreaseChorolate(playerControl.ChocolateEnds[playerControl.nowChocolateNum]);
            if (nowChocolateBh < 0) {
                return;
            }
            playerControl.IsCarryChocolate(nowChocolateBh);
        }
    }

    private void MoveChocolate2Porter() {
        foreach (PorterControl ss in porterControls) {
            if (ss.porterCarryCondition != PorterCarryCondition.full) {
                int nowChocolateBh = DecreaseChorolate(ss.ChocolateSum[ss.nowNum]);
                if (nowChocolateBh < 0) {
                    return;
                }
                ss.IsCarryChocolate(nowChocolateBh);

            }
        }
    }

    private int DecreaseChorolate(Transform trans) {
        if (nowNum - 1 < 0) {
            return -1;
        }
        Transform ObjectGo = ObjectPool.Instance.Get("Factory", "ChocolateEnd", trans.parent).transform;
        ObjectGo.transform.position = productionSum[nowNum - 1].position;
        ObjectGo.transform.rotation = productionSum[nowNum - 1].rotation;
        ChocolateDisplayControl chocolateDisplayControl = productionSum[nowNum - 1].gameObject.GetComponent<ChocolateDisplayControl>();
        int endBh = chocolateDisplayControl.nowbh;
        ChocolateEndControl chocolateEndControl = ObjectGo.gameObject.GetComponent<ChocolateEndControl>();
        chocolateEndControl.OnStart(endBh);
        productionSum[nowNum - 1].gameObject.SetActive(false);
        nowNum--;
        ReSetUI();
        Vector3 StartPos = ObjectGo.localPosition;
        Vector3 EndPos = trans.localPosition;
        Tween tween = DOTween.To(setter: value => {
            ObjectGo.localPosition = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f);
        tween.OnComplete(() => {
            ObjectPool.Instance.Recycle(ObjectGo.gameObject);
            trans.gameObject.SetActive(true);
        });
        ObjectGo.DORotate(trans.localEulerAngles, 0.4f);
        return endBh;
    }
}
