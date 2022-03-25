using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketMgr : Singleton<MarketMgr> {

    Transform MarketF;
    Transform Market;
    Transform CheckOut;
    public MarketControl marketControl;
    public CounterControl counterControl;
    private const string CHOCOLATENUM = "CHOCOLATENUM";
    private int[] m_chocolateNum = new int[5];
    public int[] chocolateNum {
        get {
            return m_chocolateNum;
        }
        set {
            for (int i = 0; i < 5; i++) {

                switch (i) {
                    case 0:
                        if (value[i] > 6) {
                            return;
                        }
                        break;
                    case 1:
                        if (value[i] > 4) {
                            return;
                        }
                        break;
                    case 2:
                        if (value[i] > 3) {
                            return;
                        }
                        break;
                    case 3:
                        if (value[i] > 2) {
                            return;
                        }
                        break;
                    case 4:
                        if (value[i] > 1) {
                            return;
                        }
                        break;
                }
                if (m_chocolateNum[i] != value[i]) {
                    if(i == 0 && value[i] == 0) {
                        ChangeChocolateMoney(i, 0);
                        LocalSave.SetInt(CHOCOLATENUM + (i + 1).ToString(), value[i]);
                        m_chocolateNum = value;
                        counterLv++;
                        ReSetCounterView();
                        return;
                    }
                    else if (value[i] == 1) {
                        if (i + 1 < 5) {

                            ChangeChocolateMoney(i, 30 + value[i] * 50 * (i + 1));
                            value[i + 1] = 0;
                            ChangeChocolateMoney(i + 1, 500 * (i + 1));
                            LocalSave.SetInt(CHOCOLATENUM + (i + 1).ToString(), value[i]);
                            LocalSave.SetInt(CHOCOLATENUM + (i + 2).ToString(), value[i + 1]);
                            m_chocolateNum = value;
                            return;
                        }
                    }
                    ChangeChocolateMoney(i, 30 + value[i] * 50 * (i + 1));
                    LocalSave.SetInt(CHOCOLATENUM + (i + 1).ToString(), value[i]);
                }
            }
            ReSetCounterView();
            m_chocolateNum = value;
        }
    }

    public void ChangeChocolateNum(int bh, int value) {
        int[] nowValue = new int[5];
        for (int i = 0; i < 5; i++) {
            if (i == bh) {
                nowValue[i] = value;
            }
            else {
                nowValue[i] = chocolateNum[i];
            }
        }
        chocolateNum = nowValue;
        marketControl.SetView();
    }

    private const string CHOCOLATEMONEY = "CHOCOLATEMONEY";
    private int[] m_chocolateMoney = new int[5];
    public int[] chocolateMoney {
        get {
            return m_chocolateMoney;
        }
        set {
            for (int i = 0; i < 5; i++) {
                if (m_chocolateMoney[i] != value[i]) {

                    LocalSave.SetInt(CHOCOLATEMONEY + (i + 1).ToString(), value[i]);
                }
            }
            m_chocolateMoney = value;
        }
    }
    public void ChangeChocolateMoney(int bh, int value) {
        int[] nowValue = new int[5];
        for (int i = 0; i < 5; i++) {
            if (i == bh) {
                nowValue[i] = value;
            }
            else {
                nowValue[i] = chocolateMoney[i];
            }
        }
        chocolateMoney = nowValue;
    }

    private const string COUNTERLV = "COUNTERLV";
    private int m_counterLv;
    public int counterLv {
        get {
            return m_counterLv;
        }
        set {
            if (value > 2) {
                return;
            }
            if (m_counterLv != value) {
                if (m_counterLv < 0) {
                    counterMoney = 0;
                }
                else {
                    counterMoney = 1000 * value;
                    BattleWindow.Instance.LockSellerNum++;
                    //BattleWindow.Instance.EmployerTipActive();
                    BattleWindow.Instance.ReSetEmployUI();
                }
             
            }
            m_counterLv = value;
            LocalSave.SetInt(COUNTERLV, value);
            //if (m_counterLv == 1) {
            //    //ChangeChocolateNum(0, chocolateNum[0] + 1);
            //    //marketControl.SetView();
            //    MarketMgr.Instance.counterLv++;
            //    MarketMgr.Instance.ReSetCounterView();
            //}
        }
    }
    public void ReSetCounterView() {
        counterControl.SetView();
    }

    private const string COUNTERMONEY = "COUNTERMONEY";
    private int m_counterMoney;
    public int counterMoney {
        get {
            return m_counterMoney;
        }
        set {
            m_counterMoney = value;
            LocalSave.SetInt(COUNTERMONEY, value);
        }
    }
    public List<DisplayGoodsControl> displayGoodsControls = new List<DisplayGoodsControl>();
    List<ShelfIni> nowShelfs = new List<ShelfIni>();
    public class ShelfIni {
        public Transform nowShelf;
        public List<DisplayGoodsRoot> ShelfSum = new List<DisplayGoodsRoot>();
        public List<Transform> CustomerPos = new List<Transform>();
        public List<DisplayGoodsControl> displayGoodsControls = new List<DisplayGoodsControl>();
        public ShelfIni(Transform _tran) {
            nowShelf = _tran;
        }
        public void AddRoot(DisplayGoodsRoot _root, Transform cusPos, DisplayGoodsControl displayGoodsControl) {
            ShelfSum.Add(_root);
            CustomerPos.Add(cusPos);
            displayGoodsControls.Add(displayGoodsControl);
        }
    }
    public void Init() {
        if (MarketF == null) {
            MarketF = new GameObject("MarketF").transform;
        }
        InitMarket();
        for (int i = 0; i < 5; i++) {
            m_chocolateNum[i] = LocalSave.GetInt(CHOCOLATENUM + (i + 1).ToString(), -1);
            m_chocolateMoney[i] = LocalSave.GetInt(CHOCOLATEMONEY + (i + 1).ToString(), 0);
            nowShelfs.Add(new ShelfIni(Market.gameObject.GetChildControl<Transform>((i + 1).ToString())));
            foreach (Transform ss in nowShelfs[i].nowShelf) {
                DisplayGoodsRoot displayGoodsRoot = ss.gameObject.GetChildControl<DisplayGoodsRoot>("chocolateRoot");
                Transform tran = ss.gameObject.GetChildControl<Transform>("CustomerPos");
                DisplayGoodsControl displayGoodsControl = ss.gameObject.GetComponent<DisplayGoodsControl>();
                nowShelfs[i].AddRoot(displayGoodsRoot, tran, displayGoodsControl);
            }
        }
        m_counterLv = LocalSave.GetInt(COUNTERLV, -1);
        m_counterMoney = LocalSave.GetInt(COUNTERMONEY, 0);
        
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }

    private void InitMarket() {
        Market = ObjectPool.Instance.Get("Market", "commodity", MarketF).transform;
        marketControl = Market.gameObject.GetComponent<MarketControl>();
        CheckOut = ObjectPool.Instance.Get("Market", "checkOut", MarketF).transform;
        counterControl = CheckOut.gameObject.GetComponent<CounterControl>();
        foreach (Transform ss in Market) {
            foreach (Transform sss in ss) {
                displayGoodsControls.Add(sss.gameObject.GetComponent<DisplayGoodsControl>());
            }
        }
    }
    public bool FirstDisplayNum() {
        if (displayGoodsControls[0]?.displayGoodsRoot?.nowNum > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    public int ReturnDisplayChocolateNum() {
        int num = 0;
        foreach (DisplayGoodsControl ss in displayGoodsControls) {
            if (ss.displayGoodsCondition == DisplayGoodsCondition.UnLock) {
                num++;
            }
        }
        return num;
    }

    public Transform GetChocolateBh2GoodsPos(int bh) {
        List<Transform> CustomerPos = new List<Transform>();
        int i = 0;
        foreach (DisplayGoodsRoot ss in nowShelfs[bh - 1].ShelfSum) {
            if (ss.nowNum > 0) {
                CustomerPos.Add(nowShelfs[bh - 1].CustomerPos[i]);
            }
            i++;
        }
        if (CustomerPos.Count <= 0) {
            return null;
        }
        else {
            int nowNum = Random.Range(0, CustomerPos.Count);
            return CustomerPos[nowNum];
        }
    }

    public Transform GetChocolateBh2GoodsPosNull(int bh) {
        List<Transform> CustomerPos = new List<Transform>();
        int i = 0;
        foreach (DisplayGoodsControl ss in nowShelfs[bh - 1].displayGoodsControls) {
            if (ss.displayGoodsCondition == DisplayGoodsCondition.UnLock) {
                CustomerPos.Add(nowShelfs[bh - 1].CustomerPos[i]);
            }
            i++;
        }
        if (CustomerPos.Count <= 0) {
            return null;
        }
        int nowNum = Random.Range(0, CustomerPos.Count);
        return CustomerPos[nowNum];
    }

    public Transform GetDisplayPos(int bh) {
        List<Transform> CustomerPos = new List<Transform>();
        int i = 0;
        foreach (DisplayGoodsRoot ss in nowShelfs[bh - 1].ShelfSum) {
            if (!ss.JudgeMax()) {
                CustomerPos.Add(nowShelfs[bh - 1].CustomerPos[i]);
            }
            i++;
        }
        if (CustomerPos.Count <= 0) {
            return null;
        }
        else {
            int nowNum = Random.Range(0, CustomerPos.Count);
            return CustomerPos[nowNum];
        }
    }
    public Transform GetDisplayZero(int bh) {
        for (int i = bh - 1; i > 0; i--) {
            if (chocolateNum[i] == -1) {

            }
            else {
                return nowShelfs[i].displayGoodsControls[0].transform;
            }
        }
        return nowShelfs[0].displayGoodsControls[0].transform;
    }

    public int[] DisplayChocolateNeed() {
        const int chocolateType = 5;
        int[] startBh = { 1, 2, 3, 4, 5 };
        int[] startNum = new int[chocolateType] { 0, 0, 0, 0, 0 };
        int MaxNum = 1000;
        for (int i = 0; i < chocolateType; i++) {
            Transform nowShelf = Market.gameObject.GetChildControl<Transform>((i + 1).ToString());
            if (chocolateNum[i] == 0) {
                startNum[i] = MaxNum;
            }
            else {
                foreach (Transform ss in nowShelf) {
                    DisplayGoodsControl displayGoodsControl = ss.gameObject.GetComponent<DisplayGoodsControl>();
                    if (displayGoodsControl.displayGoodsCondition != DisplayGoodsCondition.UnLock) {
                    }
                    else {
                        DisplayGoodsRoot displayGoodsRoot = ss.gameObject.GetChildControl<DisplayGoodsRoot>("chocolateRoot");
                        startNum[i] += displayGoodsRoot.nowTureNum;
                    }
                }
            }
            
        }
        for (int i = 0; i < chocolateType - 1; i++) {
            for (int j = 0; j < chocolateType - 1 - i; j++) {
                if (startNum[j] > startNum[j + 1]) {
                    int temp = startNum[j];
                    startNum[j] = startNum[j + 1];
                    startNum[j + 1] = temp;
                    temp = startBh[j];
                    startBh[j] = startBh[j + 1];
                    startBh[j + 1] = temp;
                }
            }
        }
        if (startNum[0] == MaxNum) {
            return null;
        }

        return startBh;
    }

    public int DisplayChocolateNeedNum() {
        const int chocolateType = 5;
        int[] startBh = { 1, 2, 3, 4, 5 };
        int[] startNum = new int[chocolateType] { 0, 0, 0, 0, 0 };
        int MaxNum = 1000;
        for (int i = 0; i < chocolateType; i++) {
            Transform nowShelf = Market.gameObject.GetChildControl<Transform>((i + 1).ToString());
            if (chocolateNum[i] <= 0) {
                startNum[i] = MaxNum;
            }
            else {
                foreach (Transform ss in nowShelf) {
                    DisplayGoodsControl displayGoodsControl = ss.gameObject.GetComponent<DisplayGoodsControl>();
                    if (displayGoodsControl.displayGoodsCondition != DisplayGoodsCondition.UnLock) {
                    }
                    else {
                        DisplayGoodsRoot displayGoodsRoot = ss.gameObject.GetChildControl<DisplayGoodsRoot>("chocolateRoot");
                        startNum[i] += displayGoodsRoot.nowTureNum;
                    }
                }
            }
        }
        //Debug.LogError("start--" + startBh[0] + " " + startBh[1] + " " + startBh[2] + " " + startBh[3] + " " + startBh[4] + " " + " _Num  " + startNum[0] + " " + startNum[1] + " " + startNum[2] + " " + startNum[3] + " " + startNum[4] + " ");
        for (int i = 0; i < chocolateType - 1; i++) {
            for (int j = 0; j < chocolateType - 1 - i; j++) {
                if (startNum[j] > startNum[j + 1]) {
                    int temp = startNum[j];
                    startNum[j] = startNum[j + 1];
                    startNum[j + 1] = temp;
                    temp = startBh[j];
                    startBh[j] = startBh[j + 1];
                    startBh[j + 1] = temp;
                }
            }
        }
        //Debug.LogError("end--" + startBh[0] + " " + startBh[1] + " " + startBh[2] + " " + startBh[3] + " " + startBh[4] + " " + " _Num  " + startNum[0] + " " + startNum[1] + " " + startNum[2] + " " + startNum[3] + " " + startNum[4] + " ");
        if (startNum[0] == MaxNum) {
            return -1;
        }
        if (startNum[chocolateType - 1] == 0) {
            return 1;
        }
        for (int i = chocolateType - 1; i > 0; i--) {
            if (startNum[i] != MaxNum) {
                return startBh[i];
            }
        }
        return -1;
       
    }

    public Transform GetChechOutPos(CustomerControl customerControl) {
        if (m_counterLv <= 0) {
            return null;
        }
        return counterControl.GetCustomerPos(customerControl);
    }

    
}
