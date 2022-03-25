using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EmployFarmerControl
{

    int bh;
    private string FARMEREMPLOYMONEY;
    private int m_FarmerEmployMoney;
    public int FarmerEmployMoney {
        get {
            return m_FarmerEmployMoney;
        }
        set {
            int changeValue = value - m_FarmerEmployMoney;
            m_FarmerEmployMoney = value;
            LocalSave.SetInt(FARMEREMPLOYMONEY, value);
        }
    }
    private string FARMEREMPLOYLV;
    private int m_FarmerEmployLv;
    public int FarmerEmployLv {
        get {
            return m_FarmerEmployLv;
        }
        set {
            if (value == 1) {
                GetOneFarmer();
                navMeshAgent.speed = 3.5f;
            }
            else if (value > 1 && value <= LvMax) {
                navMeshAgent.speed = 3f + value * 0.5f;
                farmerControl.attrackControl_Farmer.OnceAttrackTimes = m_FarmerEmployLv;
                farmerControl.AddFxLv(0.7f);
            }
            else if (value > LvMax) {
                return;
            }
            else {
                value = LvMax;
            }
            FarmerEmployMoney = LvMoney[value - 1] * (bh + 1);
            int changeValue = value - m_FarmerEmployLv;
            m_FarmerEmployLv = value;
            LocalSave.SetInt(FARMEREMPLOYLV, value);
        }
    }
    int moneyMax;
    int[] LvMoney = new int[] { 100, 250, 350, 500, 700, 800, 0};
    public int LvMax = 6;

    Transform Farmer;
    FarmerControl farmerControl;
    NavMeshAgent navMeshAgent;
    public EmployFarmerControl(int _bh) {
        bh = _bh;
        FARMEREMPLOYMONEY = "FARMEREMPLOYMONEY" + bh.ToString();
        FARMEREMPLOYLV = "FARMEREMPLOYLV" + bh.ToString();
        m_FarmerEmployLv = LocalSave.GetInt(FARMEREMPLOYLV, 0);
        m_FarmerEmployMoney = LocalSave.GetInt(FARMEREMPLOYMONEY, LvMoney[m_FarmerEmployLv] * (bh + 1));
        moneyMax = LvMoney[FarmerEmployLv];
        if (m_FarmerEmployLv > 0) {
            GetOneFarmer();
        }
    }
    public bool IsMax() {
        if (FarmerEmployLv >= LvMax) {
            return true;
        }
        else {
            return false;
        }
    }
    public void CameraMoveToOther() {
        if (Farmer == null) {
            return;
        }
        CameraMgr.Instance.MoveToOther(farmerControl.gameObject.transform, 60f);
    }
    private void GetOneFarmer() {
        Farmer = ObjectPool.Instance.Get("garden", "farmer", GardenMgr.Instance.GardenF).transform;
        Farmer.position = new Vector3(-18f, 0f, -20f);
        farmerControl = Farmer.gameObject.GetComponent<FarmerControl>();
        farmerControl.OnStart(bh);
        navMeshAgent = Farmer.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 3f + m_FarmerEmployLv * 0.5f;
        farmerControl.attrackControl_Farmer.OnceAttrackTimes = m_FarmerEmployLv;
    }

}
