using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenMgr : Singleton<GardenMgr> {

    public Transform GardenF;
    Transform Garden;
    List<Transform> cocoaBase = new List<Transform>();
    List<cocoaControl> cocoaControls = new List<cocoaControl>();

    Vector3 cocoaStartPos = new Vector3(3.75f, 0f, 6.25f);
    float cocoax = 7.5f;
    float cocoaz = 6.25f;

    private const string GARDENLV = "GARDENLV";
    private int m_GardenLv;
    
    public int GardenLv {
        get {
            return m_GardenLv;
        }
        set {
            int changeValue = value - m_GardenLv;
            
            m_GardenLv = value;
            LocalSave.SetInt(GARDENLV, value);
            Send.SendMsg(SendType.GardenLvChange, changeValue, m_GardenLv);
            if (value == 1) {
                FactoryMgr.Instance.SetView();
            }
        }
    }
    int GardenLvMax = 6;

    private const string LOCKMONEY = "LOCKMONEY";
    private int m_LockMoney;

    public int LockMoney {
        get {
            return m_LockMoney;
        }
        set {
            int changeValue = value - m_LockMoney;
            m_LockMoney = value;
            LocalSave.SetInt(LOCKMONEY, value);
        }
    }

    List<Transform> EmploySum = new List<Transform>();
    private const string EMPLOYOBJ = "EMPLOYOBJ";
    private int m_EmployObj;

    public int EmployObj {
        get {
            return m_EmployObj;
        }
        set {
            int changeValue = value - m_EmployObj;
            m_EmployObj = value;
            LocalSave.SetInt(EMPLOYOBJ, value);
        }
    }


    public void Init() {
        if (GardenF == null) {
            GardenF = new GameObject("GardenF").transform;
        }
        InitGarden();
        m_GardenLv = LocalSave.GetInt(GARDENLV, 0);
        m_LockMoney = LocalSave.GetInt(LOCKMONEY, 5);
        m_EmployObj = LocalSave.GetInt(EMPLOYOBJ, 0);
        InitCocoa();
        InitEmployObj();
        InitMsg();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }
    private void InitGarden() {
        Garden = ObjectPool.Instance.Get("MainRoad", "garden", GardenF).transform;
    }
    public bool FirstGetCocoa() {
        if (cocoaControls[0]?.nowBoxNumTrue > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    private void AddcocoaBase(bool active, int money) {
        Transform Objectgo = ObjectPool.Instance.Get("garden", "cocoaBase", Garden).transform;
        Objectgo.localPosition = culCocoaPos(cocoaBase.Count);
        cocoaBase.Add(Objectgo);
        cocoaControl _cocoaControl = Objectgo.gameObject.GetComponent<cocoaControl>();
        _cocoaControl.OnStart(active, money, cocoaControls.Count);
        cocoaControls.Add(_cocoaControl);

    }
    private Vector3 culCocoaPos(int bh) {
        Vector3 pos;
        float x, y, z;
        int h, l;
        h = bh / 2;
        l = bh % 2;
        x = cocoaStartPos.x - l * cocoax;
        y = 0f;
        z = cocoaStartPos.z - h * cocoaz;
        pos = new Vector3(x, y, z);
        return pos;
    }

    private void InitCocoa() {
        for (int i = 0; i < m_GardenLv; i++) {
            AddcocoaBase(true, 0);
        }
        if (m_GardenLv == GardenLvMax) {
            return;
        }
        AddcocoaBase(false, m_LockMoney);
    }

    public void AddCocoa() {
        
        if (m_GardenLv >= GardenLvMax - 1) {
            GardenLv++;
            return;
        }
        GardenLv++;
        int nowmoney = CulCocoaUnlockMoney();
        LockMoney = nowmoney;
        AddcocoaBase(false, nowmoney);
        if (GardenLv < 5) {
            AddEmployObj();
            EmployObj++;
        }
    }

    private int CulCocoaUnlockMoney() {
        return m_GardenLv * 50;
    }

    private void InitEmployObj() {
        for (int i = 0; i < m_EmployObj; i++) {
            AddEmployObj();
        }
    }

    private void AddEmployObj() {
        BattleWindow.Instance.LockFarmerNum++;
        //BattleWindow.Instance.EmployerTipActive();
        BattleWindow.Instance.ReSetEmployUI();
        //Transform ObjectGo = ObjectPool.Instance.Get("garden", "employSign_farmer", Garden).transform;
        //ObjectGo.localPosition = culEmployPos(EmploySum.Count);
        //EmployFarmerControl employControl = ObjectGo.GetComponent<EmployFarmerControl>();
        //employControl.OnceStart(EmploySum.Count);
        //EmploySum.Add(ObjectGo);
    }
    public int TipGet() {
        int num;
        if (m_GardenLv < 1) {
            num = 1;
        }
        else {
            num = 4 - (m_GardenLv - 1) % 4;
        }
       
        return num;
    }

    private Vector3 culEmployPos(int bh) {
        return new Vector3(5f, 0.01f, 5f - bh * 5f);
    }

    public Transform GetMoreCocoasTree(int bh) {
        Transform nowcocoatree = cocoaControls[bh - 1].transform;
        if (cocoaControls[bh - 1].isUnlock && !cocoaControls[bh - 1].isFull) {
            return nowcocoatree;
        }
        else {
            return null;
        }
        //List<cocoaControl> _cocoaControls = new List<cocoaControl>();
        //foreach (cocoaControl ss in cocoaControls) {
        //    if (ss.isUnlock && !ss.isFull) {
        //        _cocoaControls.Add(ss);
        //    }
        //}
        //if (_cocoaControls.Count == 0) {
        //    return null;
        //}

        //int RandBh = Random.Range(0, _cocoaControls.Count);
        //return _cocoaControls[RandBh]?.gameObject.transform;
    }

    public Transform GetMoreCocoasBox(int bh) {
        Transform nowcocoatreebox = cocoaControls[bh - 1].transform;
        if (cocoaControls[bh - 1].isFull) {
            return cocoaControls[bh - 1].Box.transform;
        }
        else {
            return null;
        }
        //List<cocoaControl> _cocoaControls = new List<cocoaControl>();
        //foreach (cocoaControl ss in cocoaControls) {
        //    if (ss.isFull) {
        //        _cocoaControls.Add(ss);
        //    }
        //}
        //if (_cocoaControls.Count == 0) {
        //    return null;   
        //}
        //int RandBh = Random.Range(0, _cocoaControls.Count);

        //return _cocoaControls[RandBh]?.Box.transform;
    }

    //public Transform GetMoreCocoasHasBox() {
    //    List<cocoaControl> _cocoaControls = new List<cocoaControl>();
    //    foreach (cocoaControl ss in cocoaControls) {
    //        if (!ss.HasBox) {
    //            _cocoaControls.Add(ss);
    //        }
    //    }
    //    if (_cocoaControls.Count == 0) {
    //        return null;
    //    }
    //    int RandBh = Random.Range(0, _cocoaControls.Count);
    //    return _cocoaControls[RandBh]?.Box.transform;
    //}

}
