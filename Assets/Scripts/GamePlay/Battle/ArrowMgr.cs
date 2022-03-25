using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMgr : Singleton<ArrowMgr>
{
    Transform ArrowF;
    Transform Arrow;
    public List<Transform> ArrowSum = new List<Transform>();
    public int numnow;


    private string  ENDNOW= "ENDNOW";
	private bool m_endnow;
    public bool endnow {
        get {
            return m_endnow;
        }
        set {
            m_endnow = value;
            LocalSave.SetBool(ENDNOW, value);
        }
    }


    public void Init() {
        m_endnow = LocalSave.GetBool(ENDNOW, false);
        ArrowF = new GameObject("ArrowF").transform;
        InitArrow();
    }

    void InitArrow() {
        Arrow = ObjectPool.Instance.Get("MainRoad", "Arrow", ArrowF).transform;
        foreach (Transform ss in Arrow) {
            ArrowSum.Add(ss);
        }
        if (endnow) {
            UnActive();
        }
    }
    int num = 0;

    public void ArrowBh() {
        
        if (num >= 1 || (GardenMgr.Instance.GardenLv > 0 && GardenMgr.Instance.FirstGetCocoa())) {
            if (num > 1) {
            }
            else {
                num = 1;
            }
            if (num >= 2 || (FactoryMgr.Instance.FactoryLv > -1 && FactoryMgr.Instance.FirstGetCocoa())) {
                if (num > 2) {
                }
                else {
                    num = 2;
                }
                if (num >= 3 || (FactoryMgr.Instance.FactoryLv > 0 && PlayerMgr.Instance.playerControl.playerCondition == PlayerCondition.CarryChocolate)) {
                    if (num > 3) {
                    }
                    else {
                        num = 3;
                    }
                    if(num >= 4 || (MarketMgr.Instance.chocolateNum[0] > 0 && MarketMgr.Instance.FirstDisplayNum())) {
                        if (num > 4) {
                        }
                        else {
                            num = 4;
                        }
                        if (num >= 5 || (MarketMgr.Instance.counterLv > 0 && MarketMgr.Instance.counterControl?.counterOnes[0]?.counterUnLockTrigger?.onceAddMoney == true) ){
                            num = 5;
                        }
                    }

                }
            }
        }
        numnow = num;
        if (num >= ArrowSum.Count) {
            UnActive();
            return;
        }
        for (int i = 0; i < ArrowSum.Count; i++) {
            if (i == num) {
                ArrowSum[i].gameObject.SetActive(true);
            }
            else {
                ArrowSum[i].gameObject.SetActive(false);
            }
        }
    }
    public bool IsEnd() {
        if (endnow) {
            return true;
        }
        if (numnow >= ArrowSum.Count) {
            return true;
        }
        else {
            return false;
        }
    }

    private void UnActive() {
        foreach (Transform ss in ArrowSum) {
            ss.gameObject.SetActive(false);
        }
    }
}
