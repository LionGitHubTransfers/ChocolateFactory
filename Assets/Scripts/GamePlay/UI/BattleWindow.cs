using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {
    
    EventTrigger imgCtrl;

    Transform MouseControl;
    Transform In;
    Transform Out;
    Transform SourceRoot;
    List<Transform> SourceObj = new List<Transform>();
    List<Text> SourceTxt = new List<Text>();
    List<int> SourceNum = new List<int>();
    Button WorkerBtn;
    Transform WorkerStatus;
    Button WorkerStatusClose;
    List<Button> btnPage = new List<Button>();
    List<Transform> UnSelect = new List<Transform>();
    List<Transform> Select = new List<Transform>();
    List<Transform> ListPage = new List<Transform>();


    public int LockFarmerNum = 0;

    List<Transform> por_farmer = new List<Transform>();
    List<Button> por_farmer_Lock = new List<Button>();
    List<Transform> por_farmer_select = new List<Transform>();
    List<Transform> por_farmer_select2 = new List<Transform>();
    List<Button> por_farmer_employ = new List<Button>();
    List<Text> por_farmer_employ_num = new List<Text>();
    List<Button> por_farmer_uplv = new List<Button>();
    List<Text> por_farmer_uplv_num = new List<Text>();
    List<Text> por_farmer_uplv_lv = new List<Text>();
    List<Transform> por_farmer_Max = new List<Transform>();
    List<Text> por_farmer_efficiency = new List<Text>();
    List<EmployFarmerControl> employFarmerControls = new List<EmployFarmerControl>();
    private string LOCKPORTERNUM = "LOCKPORTERNUM";
    private int m_LockPorterNum;
    public int LockPorterNum {
        get {
            return m_LockPorterNum;
        }
        set {
            m_LockPorterNum = value;
            LocalSave.SetInt(LOCKPORTERNUM, value);
        }
    }
    List<Transform> por_worker = new List<Transform>();
    List<Button> por_worker_Lock = new List<Button>();
    List<Transform> por_worker_select = new List<Transform>();
    List<Transform> por_worker_select2 = new List<Transform>();
    List<Button> por_worker_employ = new List<Button>();
    List<Text> por_worker_employ_num = new List<Text>();
    List<Button> por_worker_uplv = new List<Button>();
    List<Text> por_worker_uplv_num = new List<Text>();
    List<Text> por_worker_uplv_lv = new List<Text>();
    List<Transform> por_worker_Max = new List<Transform>();
    List<Text> por_worker_efficiency = new List<Text>();
    List<EmployPorterControl> employPorterControls = new List<EmployPorterControl>();
    private string LOCKSELLERNUM = "LOCKSELLERNUM";
    private int m_LockSellerNum;
    public int LockSellerNum {
        get {
            return m_LockSellerNum;
        }
        set {
            m_LockSellerNum = value;
            LocalSave.SetInt(LOCKSELLERNUM, value);
        }
    }
    List<Transform> por_seller = new List<Transform>();
    List<Button> por_seller_Lock = new List<Button>();
    List<Transform> por_seller_select = new List<Transform>();
    List<Transform> por_seller_select2 = new List<Transform>();
    List<Button> por_seller_employ = new List<Button>();
    List<Text> por_seller_employ_num = new List<Text>();
    List<Button> por_seller_uplv = new List<Button>();
    List<Text> por_seller_uplv_num = new List<Text>();
    List<Text> por_seller_uplv_lv = new List<Text>();
    List<Transform> por_seller_Max = new List<Transform>();
    List<Text> por_seller_efficiency = new List<Text>();
    public List<EmploySellerControl> employSellerControls = new List<EmploySellerControl>();
    protected override void InitCtrl() {
        m_LockPorterNum = LocalSave.GetInt(LOCKPORTERNUM, 0);
        m_LockSellerNum = LocalSave.GetInt(LOCKSELLERNUM, 0);
        imgCtrl = gameObject.GetChildControl<EventTrigger>("imgCtrl");
        MouseControl = gameObject.GetChildControl<Transform>("MouseControl");
        MouseControl.gameObject.SetActive(false);
        In = MouseControl.GetChild(0);
        Out = In.GetChild(0);
        PlayerMgr.Instance.MouseControl = MouseControl;
        PlayerMgr.Instance.In = In.gameObject.GetComponent<RectTransform>();
        PlayerMgr.Instance.Out = Out.gameObject.GetComponent<RectTransform>();
        SourceRoot = gameObject.GetChildControl<Transform>("SourceRoot");
        foreach (Transform ss in SourceRoot) {
            SourceObj.Add(ss);
            SourceTxt.Add(ss.gameObject.GetChildControl<Text>("num"));
            SourceNum.Add(0);
        }
        SourceNum[0] = CurrencyMgr.Instance.Gold;
        WorkerBtn = gameObject.GetChildControl<Button>("WorkerBtn");
        WorkerStatus = gameObject.GetChildControl<Transform>("employUI");
        WorkerStatusClose = gameObject.GetChildControl<Button>("employUI/close");
        //EmployerTip = gameObject.GetChildControl<Transform>("WorkerBtn/handAnim");
        //EmployerTip.gameObject.SetActive(false);
        foreach (Transform ss in WorkerStatus) {
            if (ss.gameObject.name.Contains("page")) {
                btnPage.Add(ss.gameObject.GetComponent<Button>());
                UnSelect.Add(ss.GetChild(0));
                Select.Add(ss.GetChild(1));
            }
            else if (ss.gameObject.name.Contains("List")) {
                ListPage.Add(ss);
                if (ListPage.Count == 1) {
                    int i = 1;
                    foreach (Transform sss in ss) {
                        por_farmer.Add(sss);
                        por_farmer_Lock.Add(sss.gameObject.GetChildControl<Button>("locked"));
                        por_farmer_select.Add(sss.GetChild(0));
                        por_farmer_employ.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/EmployBtn "));
                        por_farmer_uplv.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/UpgradeBtn"));
                        por_farmer_Max.Add(sss.gameObject.GetChildControl<Transform>("bar/Btn/LevelMax"));
                        por_farmer_efficiency.Add(sss.gameObject.GetChildControl<Text>("bar/efficiency"));
                        por_farmer_employ_num.Add(por_farmer_employ[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_farmer_uplv_num.Add(por_farmer_uplv[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_farmer_uplv_lv.Add(por_farmer_uplv[i - 1].gameObject.GetChildControl<Text>("lv"));
                        por_farmer_select2.Add(sss.GetChild(1).GetChild(1));
                        employFarmerControls.Add(new EmployFarmerControl(i));
                        i++;
                    }
                }
                else if (ListPage.Count == 2) {
                    int i = 1;
                    foreach (Transform sss in ss) {
                        por_worker.Add(sss);
                        por_worker_Lock.Add(sss.gameObject.GetChildControl<Button>("locked"));
                        por_worker_select.Add(sss.GetChild(0));
                        por_worker_employ.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/EmployBtn "));
                        por_worker_uplv.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/UpgradeBtn"));
                        por_worker_Max.Add(sss.gameObject.GetChildControl<Transform>("bar/Btn/LevelMax"));
                        por_worker_efficiency.Add(sss.gameObject.GetChildControl<Text>("bar/efficiency"));
                        por_worker_employ_num.Add(por_worker_employ[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_worker_uplv_num.Add(por_worker_uplv[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_worker_uplv_lv.Add(por_worker_uplv[i - 1].gameObject.GetChildControl<Text>("lv"));
                        por_worker_select2.Add(sss.GetChild(1).GetChild(1));
                        employPorterControls.Add(new EmployPorterControl(i));
                        i++;
                    }
                }
                else {
                    int i = 1;
                    foreach (Transform sss in ss) {
                        por_seller.Add(sss);
                        por_seller_Lock.Add(sss.gameObject.GetChildControl<Button>("locked"));
                        por_seller_select.Add(sss.GetChild(0));
                        por_seller_employ.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/EmployBtn "));
                        por_seller_uplv.Add(sss.gameObject.GetChildControl<Button>("bar/Btn/UpgradeBtn"));
                        por_seller_Max.Add(sss.gameObject.GetChildControl<Transform>("bar/Btn/LevelMax"));
                        por_seller_efficiency.Add(sss.gameObject.GetChildControl<Text>("bar/efficiency"));
                        por_seller_employ_num.Add(por_seller_employ[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_seller_uplv_num.Add(por_seller_uplv[i - 1].gameObject.GetChildControl<Text>("num"));
                        por_seller_uplv_lv.Add(por_seller_uplv[i - 1].gameObject.GetChildControl<Text>("lv"));
                        por_seller_select2.Add(sss.GetChild(1).GetChild(1));
                        employSellerControls.Add(new EmploySellerControl(i));
                        i++;
                    }
                }
                
            }
        }

        for (int i = 0; i < ListPage.Count; i++) {
            ListPage[i].gameObject.SetActive(false);
            Select[i].gameObject.SetActive(false);
            UnSelect[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < por_farmer_select.Count; i++) {
            //por_farmer_select[i].gameObject.SetActive(false);
            por_farmer_select2[i].gameObject.SetActive(false);
            int nowLv = employFarmerControls[i].FarmerEmployLv;
            int nowmoney = employFarmerControls[i].FarmerEmployMoney;
            int maxLv = employFarmerControls[i].LvMax;
            if (nowLv <= 0) {
                por_farmer_employ[i].gameObject.SetActive(true);
                por_farmer_uplv[i].gameObject.SetActive(false);
                por_farmer_Max[i].gameObject.SetActive(false);
                por_farmer_employ_num[i].text = nowmoney.ToString();

            }
            else if (nowLv < maxLv) {
                por_farmer_employ[i].gameObject.SetActive(false);
                por_farmer_uplv[i].gameObject.SetActive(true);
                por_farmer_Max[i].gameObject.SetActive(false);
                por_farmer_uplv_num[i].text = nowmoney.ToString();
                por_farmer_uplv_lv[i].text = nowLv.ToString();
                por_farmer_efficiency[i].text = "Efficiency:" + "\n" + ((int)(1f + nowLv * 0.25f) * 100).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_farmer_employ[i].gameObject.SetActive(false);
                por_farmer_uplv[i].gameObject.SetActive(false);
                por_farmer_Max[i].gameObject.SetActive(true);
            }
            
        }
        for (int i = 0; i < por_farmer_select.Count; i++) {
            if (i < LockFarmerNum) {
                por_farmer_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_farmer_Lock[i].gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < por_worker_select.Count; i++) {
            //por_worker_select[i].gameObject.SetActive(false);
            por_worker_select2[i].gameObject.SetActive(false);
            int nowLv = employPorterControls[i].PorterEmployLv;
            int nowmoney = employPorterControls[i].PorterEmployMoney;
            int maxLv = employPorterControls[i].LvMax;
            if (nowLv <= 0) {
                por_worker_employ[i].gameObject.SetActive(true);
                por_worker_uplv[i].gameObject.SetActive(false);
                por_worker_Max[i].gameObject.SetActive(false);
                por_worker_employ_num[i].text = nowmoney.ToString();

            }
            else if (nowLv < maxLv) {
                por_worker_employ[i].gameObject.SetActive(false);
                por_worker_uplv[i].gameObject.SetActive(true);
                por_worker_Max[i].gameObject.SetActive(false);
                por_worker_uplv_num[i].text = nowmoney.ToString();
                por_worker_uplv_lv[i].text = nowLv.ToString();
                por_worker_efficiency[i].text = "Efficiency:" + "\n" + ((int)(1f + nowLv * 0.25f) * 100).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_worker_employ[i].gameObject.SetActive(false);
                por_worker_uplv[i].gameObject.SetActive(false);
                por_worker_Max[i].gameObject.SetActive(true);
            }
            
        }
        for (int i = 0; i < por_worker_select.Count; i++) {
            if (i < LockPorterNum) {
                por_worker_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_worker_Lock[i].gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < por_seller_select.Count; i++) {
            //por_seller_select[i].gameObject.SetActive(false);
            por_seller_select2[i].gameObject.SetActive(false);
            int nowLv = employSellerControls[i].SellerEmployLv;
            int nowmoney = employSellerControls[i].SellerEmployMoney;
            int maxLv = employSellerControls[i].LvMax;
            if (nowLv <= 0) {
                por_seller_employ[i].gameObject.SetActive(true);
                por_seller_uplv[i].gameObject.SetActive(false);
                por_seller_Max[i].gameObject.SetActive(false);
                por_seller_employ_num[i].text = nowmoney.ToString();
            }
            else if (nowLv < maxLv) {

                por_seller_employ[i].gameObject.SetActive(false);
                por_seller_uplv[i].gameObject.SetActive(true);
                por_seller_Max[i].gameObject.SetActive(false);
                por_seller_uplv_num[i].text = nowmoney.ToString();
                por_seller_uplv_lv[i].text = nowLv.ToString();
                por_seller_efficiency[i].text = "Efficiency:" + "\n" + ((int)(1f + nowLv * 0.25f) * 100).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_seller_employ[i].gameObject.SetActive(false);
                por_seller_uplv[i].gameObject.SetActive(false);
                por_seller_Max[i].gameObject.SetActive(true);
            }
            
        }
        for (int i = 0; i < por_seller_select.Count; i++) {
            if (i < LockSellerNum) {
                por_seller_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_seller_Lock[i].gameObject.SetActive(true);
            }
        }

        Select[0].gameObject.SetActive(true);
        UnSelect[0].gameObject.SetActive(false);
        ListPage[0].gameObject.SetActive(true);
        WorkerStatus.gameObject.SetActive(false);

    }
    public void ReSetEmployUI() {
        for (int i = 0; i < por_farmer_select.Count; i++) {
            int nowLv = employFarmerControls[i].FarmerEmployLv;
            int nowmoney = employFarmerControls[i].FarmerEmployMoney;
            int maxLv = employFarmerControls[i].LvMax;
            if (nowLv <= 0) {
                por_farmer_employ[i].gameObject.SetActive(true);
                por_farmer_uplv[i].gameObject.SetActive(false);
                por_farmer_Max[i].gameObject.SetActive(false);
                por_farmer_employ_num[i].text = nowmoney.ToString();

            }
            else if (nowLv < maxLv) {
                por_farmer_employ[i].gameObject.SetActive(false);
                por_farmer_uplv[i].gameObject.SetActive(true);
                por_farmer_Max[i].gameObject.SetActive(false);
                por_farmer_uplv_num[i].text = nowmoney.ToString();
                por_farmer_uplv_lv[i].text = "Lv " + nowLv.ToString();
                por_farmer_efficiency[i].text = "Efficiency:" + "\n" + ((int)((1f + nowLv * 0.25f) * 100)).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_farmer_employ[i].gameObject.SetActive(false);
                por_farmer_uplv[i].gameObject.SetActive(false);
                por_farmer_Max[i].gameObject.SetActive(true);
            }
            
        }
        for (int i = 0; i < por_farmer_select.Count; i++) {
            if (i < LockFarmerNum) {
                por_farmer_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_farmer_Lock[i].gameObject.SetActive(true);
            }
        }
        //
        for (int i = 0; i < por_worker_select.Count; i++) {
            int nowLv = employPorterControls[i].PorterEmployLv;
            int nowmoney = employPorterControls[i].PorterEmployMoney;
            int maxLv = employPorterControls[i].LvMax;
            if (nowLv <= 0) {
                por_worker_employ[i].gameObject.SetActive(true);
                por_worker_uplv[i].gameObject.SetActive(false);
                por_worker_Max[i].gameObject.SetActive(false);
                por_worker_employ_num[i].text = nowmoney.ToString();

            }
            else if (nowLv < maxLv) {
                por_worker_employ[i].gameObject.SetActive(false);
                por_worker_uplv[i].gameObject.SetActive(true);
                por_worker_Max[i].gameObject.SetActive(false);
                por_worker_uplv_num[i].text = nowmoney.ToString();
                por_worker_uplv_lv[i].text = "Lv " +  nowLv.ToString();
                por_worker_efficiency[i].text = "Efficiency:" + "\n" + ((int)((1f + nowLv * 0.25f) * 100)).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_worker_employ[i].gameObject.SetActive(false);
                por_worker_uplv[i].gameObject.SetActive(false);
                por_worker_Max[i].gameObject.SetActive(true);
            }
            
        }
        for (int i = 0; i < por_worker_select.Count; i++) {
            if (i < LockPorterNum) {
                por_worker_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_worker_Lock[i].gameObject.SetActive(true);
            }
        }
        //
        for (int i = 0; i < por_seller_select.Count; i++) {
            int nowLv = employSellerControls[i].SellerEmployLv;
            int nowmoney = employSellerControls[i].SellerEmployMoney;
            int maxLv = employSellerControls[i].LvMax;
            if (nowLv <= 0) {
                por_seller_employ[i].gameObject.SetActive(true);
                por_seller_uplv[i].gameObject.SetActive(false);
                por_seller_Max[i].gameObject.SetActive(false);
                por_seller_employ_num[i].text = nowmoney.ToString();
            }
            else if (nowLv < maxLv) {

                por_seller_employ[i].gameObject.SetActive(false);
                por_seller_uplv[i].gameObject.SetActive(true);
                por_seller_Max[i].gameObject.SetActive(false);
                por_seller_uplv_num[i].text = nowmoney.ToString();
                por_seller_uplv_lv[i].text = "Lv " + nowLv.ToString();
                por_seller_efficiency[i].text = "Efficiency:" + "\n" + ((int)((1f + nowLv * 0.25f) * 100)).ToString() + "%";
            }
            else if (nowLv == maxLv) {
                por_seller_employ[i].gameObject.SetActive(false);
                por_seller_uplv[i].gameObject.SetActive(false);
                por_seller_Max[i].gameObject.SetActive(true);
            }
           
        }
        for (int i = 0; i < por_seller_select.Count; i++) {
            if (i < LockSellerNum) {
                por_seller_Lock[i].gameObject.SetActive(false);
            }
            else {
                por_seller_Lock[i].gameObject.SetActive(true);
            }
        }
    }
    public void ResetUI() {
        SourceTxt[0].text = CurrencyMgr.Instance.Gold.ToString();
        SourceObj[0].gameObject.SetActive(true);
        for (int i = 1; i < SourceObj.Count; i++) {
            if (SourceNum[i] > 0) {
                SourceTxt[i].text = SourceNum[i].ToString();
                SourceObj[i].gameObject.SetActive(true);
            }
            else {
                SourceObj[i].gameObject.SetActive(false);
            }
        }
    }

    protected override void OnPreOpen() {
        SDKBundle.OnGameStarted();
        SDKBundle.LevelStart(1, 1);
        PlayerMgr.Instance.initwidth = transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        PlayerMgr.Instance.initheight = transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        ResetUI();
    }

    protected override void OnOpen() {
    }

    protected override void OnPreClose() {
        base.OnPreClose();
    }

    protected override void OnClose() {
        base.OnClose();
    }

    protected override void InitMsg() {
        imgCtrl.AddListener(EventTriggerType.Drag, OnDrag);
        imgCtrl.AddListener(EventTriggerType.PointerUp, OnPointUp);
        imgCtrl.AddListener(EventTriggerType.PointerDown, OnPointDown);
        Send.RegisterMsg(SendType.GoldChange, ChangeGold);
        Send.RegisterMsg(SendType.ChocoaCarryChange, ChangeChocoaCarry);
        Send.RegisterMsg(SendType.ChocolateNumChange, ChangeChocolateNum);
        WorkerBtn.onClick.AddListener(()=> {
            if (WorkerStatus.gameObject.activeSelf == true) {
                WorkerStatus.gameObject.SetActive(false);
                ClearMsgList();
                CameraMgr.Instance.MoveToPlayer();
            }
            else {
                WorkerStatus.gameObject.SetActive(true);
            }
            //EmployerTip.gameObject.SetActive(false);

        });
        WorkerStatusClose.onClick.AddListener(()=> {
            WorkerStatus.gameObject.SetActive(false);
            ClearMsgList();
            CameraMgr.Instance.MoveToPlayer();
        });
        for (int i = 0; i < btnPage.Count; i++) {
            int temp = i;
            btnPage[i].onClick.AddListener(() => {
                for (int i = 0; i < ListPage.Count; i++) {
                    ListPage[i].gameObject.SetActive(false);
                    Select[i].gameObject.SetActive(false);
                    UnSelect[i].gameObject.SetActive(true);
                }
                Select[temp].gameObject.SetActive(true);
                UnSelect[temp].gameObject.SetActive(false);
                ListPage[temp].gameObject.SetActive(true);

            });
        }
        for (int i = 0; i < por_farmer.Count; i++) {
            int temp = i;
            por_farmer[i].GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(()=> {
                for (int j = 0; j < por_farmer_select.Count; j++) {
                    //por_farmer_select[j].gameObject.SetActive(false);
                    por_farmer_select2[j].gameObject.SetActive(false);
                }
                //por_farmer_select[temp].gameObject.SetActive(true);
                por_farmer_select2[temp].gameObject.SetActive(true);
            });
        }
        for (int i = 0; i < por_farmer_employ.Count; i++) {
            int temp = i;
            por_farmer_employ[i].onClick.AddListener(()=> {
                int needMoney = employFarmerControls[temp].FarmerEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employFarmerControls[temp].FarmerEmployLv = 1;
                }
                ReSetEmployUI();
            });
            por_farmer_uplv[i].onClick.AddListener(() => {
                if (employFarmerControls[temp].IsMax()) {
                    return;
                }
                int needMoney = employFarmerControls[temp].FarmerEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employFarmerControls[temp].FarmerEmployLv++;
                }
                ReSetEmployUI();
            });
        }
        //
        for (int i = 0; i < por_worker.Count; i++) {
            int temp = i;
            por_worker[i].GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => {
                for (int j = 0; j < por_worker_select.Count; j++) {
                    //por_worker_select[j].gameObject.SetActive(false);
                    por_worker_select2[j].gameObject.SetActive(false);
                }
                //por_worker_select[temp].gameObject.SetActive(true);
                por_worker_select2[temp].gameObject.SetActive(true);
            });
        }
        for (int i = 0; i < por_worker_employ.Count; i++) {
            int temp = i;
            por_worker_employ[i].onClick.AddListener(() => {
                int needMoney = employPorterControls[temp].PorterEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employPorterControls[temp].PorterEmployLv = 1;
                }
                ReSetEmployUI();
            });
            por_worker_uplv[i].onClick.AddListener(() => {
                if (employPorterControls[temp].IsMax()) {
                    return;
                }
                int needMoney = employPorterControls[temp].PorterEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employPorterControls[temp].PorterEmployLv++;
                }
                ReSetEmployUI();
            });
        }
        //
        
        for (int i = 0; i < por_seller.Count; i++) {
            int temp = i;

            por_seller[i].GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => {
                for (int j = 0; j < por_seller_select.Count; j++) {
                    //por_seller_select[j].gameObject.SetActive(false);
                    por_seller_select2[j].gameObject.SetActive(false);
                }
                //por_seller_select[temp].gameObject.SetActive(true);
                por_seller_select2[temp].gameObject.SetActive(true);
            });
        }
        for (int i = 0; i < por_seller_employ.Count; i++) {
            int temp = i;
            por_seller_employ[i].onClick.AddListener(() => {
                int needMoney = employSellerControls[temp].SellerEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employSellerControls[temp].SellerEmployLv = 1;
                }
                ReSetEmployUI();
            });
            por_seller_uplv[i].onClick.AddListener(() => {
                if (employSellerControls[temp].IsMax()) {
                    return;
                }
                int needMoney = employSellerControls[temp].SellerEmployMoney;
                int nowMoney = CurrencyMgr.Instance.Gold;
                if (nowMoney < needMoney) {
                    TipAc(0);
                    return;
                }
                else {
                    CurrencyMgr.Instance.Gold -= needMoney;
                    employSellerControls[temp].SellerEmployLv++;
                }
                ReSetEmployUI();
            });
        }
    }

    protected override void ClearMsg() {
        imgCtrl.RemoveListener(EventTriggerType.Drag, OnDrag);
        imgCtrl.RemoveListener(EventTriggerType.PointerUp, OnPointUp);
        imgCtrl.RemoveListener(EventTriggerType.PointerDown, OnPointDown);
        Send.UnregisterMsg(SendType.GoldChange, ChangeGold);
        Send.UnregisterMsg(SendType.ChocoaCarryChange, ChangeChocoaCarry);
        Send.UnregisterMsg(SendType.ChocolateNumChange, ChangeChocolateNum);
        WorkerBtn.onClick.RemoveListener(() => {
            if (WorkerStatus.gameObject.activeSelf == true) {
                WorkerStatus.gameObject.SetActive(false);
            }
            else {
                WorkerStatus.gameObject.SetActive(true);
            }
        });
        WorkerStatusClose.onClick.RemoveListener(() => {
            WorkerStatus.gameObject.SetActive(false);
        });
        for (int i = 0; i < btnPage.Count; i++) {
            int temp = i;
            btnPage[i].onClick.RemoveListener(() => {
                for (int i = 0; i < ListPage.Count; i++) {
                    ListPage[i].gameObject.SetActive(false);
                }
                ListPage[temp].gameObject.SetActive(true);

            });
        }
    }

    private void ClearMsgList() {
        for (int i = 0; i < btnPage.Count; i++) {
            int temp = i;
            btnPage[i].onClick.RemoveListener(() => {
                for (int i = 0; i < ListPage.Count; i++) {
                    ListPage[i].gameObject.SetActive(false);
                    Select[i].gameObject.SetActive(false);
                    UnSelect[i].gameObject.SetActive(true);
                }
                Select[temp].gameObject.SetActive(true);
                UnSelect[temp].gameObject.SetActive(false);
                ListPage[temp].gameObject.SetActive(true);

            });
        }
        WorkerStatusClose.onClick.RemoveListener(() => {
            WorkerStatus.gameObject.SetActive(false);
            CameraMgr.Instance.MoveToPlayer();
        });
    }
    private void OnDrag(BaseEventData arg0) {
        Send.SendMsg(SendType.CtrlDrag, arg0);
    }
    private void OnPointUp(BaseEventData arg0) {
        Send.SendMsg(SendType.CtrlUp, arg0);
    }
    private void OnPointDown(BaseEventData arg0) {
        Send.SendMsg(SendType.CtrlDown, arg0);
    }

    private void ChangeGold(object[] _objs) {
        int nowGold = (int)_objs[1];
        SourceNum[0] = nowGold;
        ResetUI();
    }

    private void ChangeChocoaCarry(object[] _objs) {
        int nowCarry = (int)_objs[0];
        SourceNum[1] = nowCarry;
        ResetUI();
    }
    public List<Transform> tip = new List<Transform>() { null, null};
    private List<string> objName = new List<string>() { "TipBG", "TipBG_Shelf" };
    public void TipAc(int bh) {
        if (tip[bh] == null) {
            tip[bh] = ObjectPool.Instance.Get("Fx", objName[bh], transform).transform;
            AutoRecycleObj autoRecycleObj = tip[bh].gameObject.GetComponent<AutoRecycleObj>();
            autoRecycleObj.OnStart();
        }
    }
    private void ChangeChocolateNum(object[] _objs) {
        for (int i = 2; i < 7; i++) {
            SourceNum[i] = 0;
        }

        for (int i = 0; i < 5; i++) {
            int num = (int)_objs[i];
            if (num == 0) {
                continue;
            }
            SourceNum[num + 1]++;
        }
        ResetUI();
    }

}
