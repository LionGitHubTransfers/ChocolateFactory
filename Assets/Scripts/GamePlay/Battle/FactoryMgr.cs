using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryMgr : Singleton<FactoryMgr> {
    Transform FactoryF;
    Transform Factory;
    Factory0Control factory0Control;

    private string FACTORYLOCK = "FACTORYLOCK";
    private bool m_FactoryLock;

    public bool FactoryLock {
        get {
            return m_FactoryLock;
        }
        set {
            m_FactoryLock = value;
            LocalSave.SetBool(FACTORYLOCK, m_FactoryLock);
        }
    }
    private string FACTORYLOCKMONEY = "FACTORYLOCKMONEY";
    private int m_FactoryLockMoney;

    public int FactoryLockMoney {
        get {
            return m_FactoryLockMoney;
        }
        set {
            m_FactoryLockMoney = value;
            LocalSave.SetInt(FACTORYLOCKMONEY, m_FactoryLockMoney);
        }
    }

    private string FACTORYLV = "FACTORYLV";
    private int m_FactoryLv;

    public int FactoryLv {
        get {
            return m_FactoryLv;
        }
        set {
            if (m_FactoryLv != value) {
                FactoryLockMoney = CulUnlockMoney(value);
            }
            m_FactoryLv = value;
            if (m_FactoryLv > -1) {
                SetView();
            }
            LocalSave.SetInt(FACTORYLV, m_FactoryLv);
            Send.SendMsg(SendType.FactoryLvChange);
            if (value == 1) {
                MarketMgr.Instance.ChangeChocolateNum(0, MarketMgr.Instance.chocolateNum[0] + 1);
                MarketMgr.Instance.marketControl.SetView();
                //MarketMgr.Instance.counterLv++;
                //MarketMgr.Instance.ReSetCounterView();
            }
        }
    }
    Transform normal;
    List<Transform> FactorySum = new List<Transform>();
    public List<FactoryControl> FactoryControls = new List<FactoryControl>();

    public void Init() {
        if (FactoryF == null) {
            FactoryF = new GameObject("FactoryF").transform;
        }
        m_FactoryLock = LocalSave.GetBool(FACTORYLOCK, false);
        m_FactoryLockMoney = LocalSave.GetInt(FACTORYLOCKMONEY, 0);
        m_FactoryLv = LocalSave.GetInt(FACTORYLV, -1);
        InitFactory();
        InitMsg();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }
    public bool FirstGetCocoa() {
        if (factory0Control?.cocoaNum > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    public bool FirstGetChocolate() {
        if (FactoryControls[0]?.displayControl?.nowNum > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    public void SetView() {
        if (GardenMgr.Instance.GardenLv > 0) {
            Factory.gameObject.SetActive(true);
            for (int i = 0; i < m_FactoryLv + 1; i++) {
                if (i >= 5) {
                    continue;
                }
                FactorySum[i].gameObject.SetActive(true);
            }
            for (int i = m_FactoryLv + 1; i < 5; i++) {
                FactorySum[i].gameObject.SetActive(false);
            }
        }
        else {
            Factory.gameObject.SetActive(false);
        }
       
    }
    private int CulUnlockMoney(int value) {

        //if (m_FactoryLv == -1) {
        //    return 0;
        //}
        //else {
            return (value + 1) * 500;
        //}
        
    }

    private void InitFactory() {
        Factory = ObjectPool.Instance.Get("MainRoad", "factory", FactoryF).transform;
        normal = Factory.gameObject.GetChildControl<Transform>("flowLine/normal");
        foreach (Transform ss in normal) {
            FactorySum.Add(ss);
            FactoryControls.Add(ss.gameObject.GetComponent<FactoryControl>());
            ss.gameObject.SetActive(false);
        }
        factory0Control = Factory.gameObject.GetChildControl<Factory0Control>("flowLine/0");
        factory0Control.OnStart();
        SetView();
    }

    public Transform GetChococlatePos(int bh) {
        FactoryControl factoryControl = FactorySum[bh - 1].gameObject.GetComponent<FactoryControl>();
        Transform oncePos = factoryControl.DisplaySign;

        return oncePos;
    }
}
