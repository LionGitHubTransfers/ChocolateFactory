using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EmploySellerControl
{

    int bh;
    private string SELLEREMPLOYMONEY;
    private int m_SellerEmployMoney;
    public int SellerEmployMoney {
        get {
            return m_SellerEmployMoney;
        }
        set {
            int changeValue = value - m_SellerEmployMoney;
            m_SellerEmployMoney = value;
            LocalSave.SetInt(SELLEREMPLOYMONEY, value);
        }
    }

    private string SELLEREMPLOYLV;
    private int m_SellerEmployLv;

    public int SellerEmployLv {
        get {
            return m_SellerEmployLv;
        }
        set {
            if (m_SellerEmployLv == value) {
                return;
            }
            if (value == 1) {
                GetOneSeller();
            }
            else if (value > 1 && value <= LvMax) {
                counterUnLockTrigger.speed = 1f + value * 0.4f;
            }
            else if (value > LvMax) {
                return;
            }
            else {
                value = LvMax;
            }
            SellerEmployMoney = LvMoney[value - 1] * (bh + 1);
            int changeValue = value - m_SellerEmployLv;
            m_SellerEmployLv = value;
            LocalSave.SetInt(SELLEREMPLOYLV, value);
        }
    }

    int moneyMax;
    int[] LvMoney = new int[] { 1000, 1250, 1800,0};
    public int LvMax = 3;
    Transform Seller;
    SellerControl sellerControl;
    public CounterUnLockTrigger counterUnLockTrigger;
    //NavMeshAgent navMeshAgent;
    public EmploySellerControl(int _bh)
    {
        bh = _bh;
        SELLEREMPLOYMONEY = "SELLEREMPLOYMONEY" + bh.ToString();
        SELLEREMPLOYLV = "SELLEREMPLOYLV" + bh.ToString();
        m_SellerEmployLv = LocalSave.GetInt(SELLEREMPLOYLV, 0);
        m_SellerEmployMoney = LocalSave.GetInt(SELLEREMPLOYMONEY, LvMoney[m_SellerEmployLv] * (bh + 1));
        moneyMax = LvMoney[m_SellerEmployLv];
        if (m_SellerEmployLv > 0) {
            GetOneSeller();
        }
    }
    public bool IsMax() {
        if (SellerEmployLv >= LvMax) {
            return true;
        }
        else {
            return false;
        }
    }
    public void CameraMoveToOther() {
        if (Seller == null) {
            return;
        }
        CameraMgr.Instance.MoveToOther(sellerControl.gameObject.transform, 60f);
    }

    private void GetOneSeller() {
        Seller = ObjectPool.Instance.Get("Market", "seller", GardenMgr.Instance.GardenF).transform;
        if (bh == 1) {
            Seller.position = new Vector3(6.5f, 0f, -9.3f);
        }
        else {
            Seller.position = new Vector3(6.5f, 0f, -5.65f);
        }
        sellerControl = Seller.gameObject.GetComponent<SellerControl>();

    }

}
