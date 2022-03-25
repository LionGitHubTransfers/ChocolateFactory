using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EmployPorterControl {
    int bh;

    private string PORTEREMPLOYMONEY;
    private int m_PorterEmployMoney;

    public int PorterEmployMoney {
        get {
            return m_PorterEmployMoney;
        }
        set {
            int changeValue = value - m_PorterEmployMoney;
            m_PorterEmployMoney = value;
            LocalSave.SetInt(PORTEREMPLOYMONEY, value);
        }
    }

    private string PORTEREMPLOYLV;
    private int m_PorterEmployLv;

    public int PorterEmployLv {
        get {
            return m_PorterEmployLv;
        }
        set {
            if (value == 1) {
                GetOneSeller();
                navMeshAgent.speed = 3.5f;
                porterControl.maxNum = 1;
            }
            else if (value > 1 && value <= LvMax) {
                navMeshAgent.speed = 3f + value * 0.5f;
                porterControl.maxNum = value;
                porterControl.AddFxLv(0.7f);
            }
            else if (value > LvMax) {
                return;
            }
            else {
                value = LvMax;
            }
            PorterEmployMoney = LvMoney[value - 1] * (bh + 1);
            int changeValue = value - m_PorterEmployLv;
            m_PorterEmployLv = value;
            LocalSave.SetInt(PORTEREMPLOYLV, value);
        }
    }

    int moneyMax;
    int[] LvMoney = new int[] { 100, 300, 500, 750, 1000, 0};
    public int LvMax = 5;

    Transform Porter;
    PorterControl porterControl;
    NavMeshAgent navMeshAgent;

    public EmployPorterControl(int _bh) {
        bh = _bh;
        PORTEREMPLOYMONEY = "PORTEREMPLOYMONEY" + bh.ToString();
        PORTEREMPLOYLV = "PORTEREMPLOYLV" + bh.ToString();
        m_PorterEmployLv = LocalSave.GetInt(PORTEREMPLOYLV, 0);
        m_PorterEmployMoney = LocalSave.GetInt(PORTEREMPLOYMONEY, LvMoney[m_PorterEmployLv] * (bh + 1));
        moneyMax = LvMoney[m_PorterEmployLv];
        if (m_PorterEmployLv > 0) {
            GetOneSeller();
        }
    }
    public bool IsMax() {
        if (PorterEmployLv >= LvMax) {
            return true;
        }
        else {
            return false;
        }
    }
    public void CameraMoveToOther() {
        if (Porter == null) {
            return;
        }
        CameraMgr.Instance.MoveToOther(porterControl.gameObject.transform, 60f);
    }
    private void GetOneSeller() {
        Porter = ObjectPool.Instance.Get("Factory", "porter", GardenMgr.Instance.GardenF).transform;
        porterControl = Porter.gameObject.GetComponent<PorterControl>();
        porterControl.bh = bh;
        
        navMeshAgent = Porter.GetComponent<NavMeshAgent>();
        porterControl.maxNum = m_PorterEmployLv;
        navMeshAgent.speed = 3f + m_PorterEmployLv * 0.5f;
    }
}
