using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterUnLockTrigger : MonoBehaviour
{
    public int bh;
    Transform InCome;
    public CounterUnLockCondition counterUnLockCondition = CounterUnLockCondition.None;
    int num = 0;
    List<Transform> CustomerSum = new List<Transform>();
    public Transform CustomerPos;
    float nowtime = 0f;
    float maxtime = 5f;
    public float speed = 1f;
    bool canUp = false;
    Transform percent;
    Image fill;

    Transform UIRoot;

    bool isDecreaseBgPrecent = true;

    private string COUNTEREMPLOYMONEY = "COUNTEREMPLOYMONEY";
    private int m_CountEmployMoney;
    bool endLv = false;

    public int CountEmployMoney {
        get {
            return m_CountEmployMoney;
        }
        set {
            int changeValue = value - m_CountEmployMoney;
            m_CountEmployMoney = value;
            LocalSave.SetInt(COUNTEREMPLOYMONEY, value);
        }
    }

    private string COUNTERMPLOYLV = "COUNTERMPLOYLV";
    private int m_CountEmployLv;

    public int CountEmployLv {
        get {
            return m_CountEmployLv;
        }
        set {
            if (value > 4) {
                return;
            }
            else if (value == 4) {
                endLv = true;
                CancelInvoke("DecreaseGold");
                maxUI.gameObject.SetActive(true);
            }
            if (m_CountEmployLv < value) {

                AddFxLv(0.7f);
                ReSetUI();
                moneyMax = LvMoney[value];
                CountEmployMoney = moneyMax;
            }
            m_CountEmployLv = value;
            LocalSave.SetInt(COUNTERMPLOYLV, value);
        }
    }
    int moneyMax;
    int[] LvMoney = new int[] { 200, 300, 400, 500, 600, 0 };
    bool OnceTrigger;
    Text moneyText;
    Image moneyImage;
    Text LvText;
    Text speedText;
    Transform maxUI;
    float priceRate = 1f;
    List<Transform> lvSum = new List<Transform>();
    CounterControl counterControl;
    Animator ani;
    public bool onceAddMoney = false;
    private void Start() {
        ani = transform.parent.gameObject.GetComponent<Animator>();
        InCome = gameObject.GetChildControl<Transform>("InCome");
        CustomerPos = gameObject.GetChildControl<Transform>("CustomerPos");
        percent = gameObject.GetChildControl<Transform>("percent");
        fill = gameObject.GetChildControl<Image>("percent/BG/fill");
        percent.gameObject.SetActive(false);
        ReSetUIfill();
        UIRoot = gameObject.GetChildControl<Transform>("UIRoot");
        moneyText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/base/num");
        moneyImage = gameObject.GetChildControl<Image>("UIRoot/upgradeUI/bg/base/percent");
        LvText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/level");
        maxUI = gameObject.GetChildControl<Transform>("UIRoot/upgradeUI/bg/Max");
        maxUI.gameObject.SetActive(false);
        speedText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/speed");
        for (int i = 1; i <= 5; i++) {
            lvSum.Add(gameObject.GetChildControl<Transform>("model/lv" + i));
        }
        BattleWindow.Instance.employSellerControls[bh].counterUnLockTrigger = this;
        counterControl = MarketMgr.Instance.counterControl;
        UIRoot.gameObject.SetActive(false);
        COUNTEREMPLOYMONEY = COUNTEREMPLOYMONEY + bh;
        COUNTERMPLOYLV = COUNTERMPLOYLV + bh;
        m_CountEmployMoney = LocalSave.GetInt(COUNTEREMPLOYMONEY, LvMoney[0]);
        m_CountEmployLv = LocalSave.GetInt(COUNTERMPLOYLV, 0);
        moneyMax = LvMoney[m_CountEmployLv];
        if (BattleWindow.Instance.employSellerControls[bh].SellerEmployLv > 0) {
            num++;
            counterUnLockCondition = CounterUnLockCondition.Working;
        }
        if (m_CountEmployLv == 4) {
            endLv = true;
            CancelInvoke("DecreaseGold");
            maxUI.gameObject.SetActive(true);
        }
        ReSetUI();
    }
    private void ReSetUIfill() {
        fill.fillAmount = nowtime / maxtime;
    }
    private void ReSetUI() {
        int thenmoney = int.Parse(moneyText.text);
        int money = CountEmployMoney;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            moneyText.text = thenmoney.ToString();
        });
        float thenamount = moneyImage.fillAmount;
        float mount = (moneyMax - CountEmployMoney) * 1.0f / moneyMax;
        Tween tween2 = DOTween.To(() => thenamount, x => thenamount = x, mount, 0.5f).OnUpdate(() => {
            moneyImage.fillAmount = thenamount;
        });
        LvText.text = "lv " + (CountEmployLv + 1);
        foreach (Transform ss in lvSum) {
            ss.gameObject.SetActive(false);
        }
        lvSum[CountEmployLv].gameObject.SetActive(true);
        if (CountEmployLv == 0) {
            speedText.text = "Price:\n100%";
            priceRate = 1f;
        }
        else {
            priceRate = CountEmployLv * 0.5f + 1f;
            int speedRatetxt = (int)(priceRate * 100);
            speedText.text = "Price:\n" + speedRatetxt.ToString() + "%";
        }
    }



    public void OnTriggerEnterwork(Collider other) {
        if (other.gameObject.layer == 3) {
            if (other.gameObject.tag == "Player") {
                num++;
                if (num > 0) {
                    counterUnLockCondition = CounterUnLockCondition.Working;
                }
            }
            else if (other.gameObject.tag == "Customer") {
                CustomerControl customerControl = other.gameObject.GetComponent<CustomerControl>();
                if (customerControl.customerSearchCdt == CustomerSearchCdt.Counter) {
                    CustomerSum.Add(other.gameObject.transform);
                    if (CustomerSum.Count == 1) {
                        InvokeRepeating("ToUp", 0.25f, 0.25f);
                    }
                }
             
            }
        }
        else if (other.gameObject.layer == 7) {
            if (other.gameObject.tag == "Player") {
                num++;
                if (num > 0) {
                    counterUnLockCondition = CounterUnLockCondition.Working;
                }
            }
        }
    }
    public void OnTriggerEnterupLv(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = false;
            OnceTrigger = true;
            UIRoot.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExitwork(Collider other) {
        if (other.gameObject.layer == 3) {
            if (other.gameObject.tag == "Player") {
                num--;
                if (num == 0) {
                    counterUnLockCondition = CounterUnLockCondition.None;
                }
            }
            else if (other.gameObject.tag == "Customer") {
                CustomerControl customerControl = other.gameObject.GetComponent<CustomerControl>();
                if (customerControl.customerSearchCdt == CustomerSearchCdt.Counter) {
                    CustomerSum.Remove(other.gameObject.transform);
                    if (CustomerSum.Count <= 0) {
                        CancelInvoke("ToUp");
                        percent.gameObject.SetActive(false);
                        nowtime = 0f;
                        canUp = false;
                    }
                }
                   
            }
        }
    }
    public void OnTriggerExitupLv(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = true;
            OnceTrigger = false;
            CancelInvoke("DecreaseGold");
            UIRoot.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (canUp == true && counterUnLockCondition == CounterUnLockCondition.Working) {
            if (nowtime < maxtime) {
                nowtime += speed * Time.deltaTime;
            }
            else {
                GetMoney();
                nowtime = 0;
            }
            ReSetUIfill();
        }
        else {
            percent.gameObject.SetActive(false);
            canUp = false;
        }
        if (isDecreaseBgPrecent) {
        }
        else {

                if (OnceTrigger) {
                    if (!endLv) {
                        InvokeRepeating("DecreaseGold", 0.25f, 0.25f);
                    }
                    OnceTrigger = false;
                }

        }

    }
    private void ToUp() {
        if (counterUnLockCondition == CounterUnLockCondition.Working) {
            percent.gameObject.SetActive(true);
            canUp = true;
        }
            
    }

    private void GetMoney() {
        foreach (Transform ss in CustomerSum) {
            CustomerControl _customerControl = ss.gameObject.GetComponent<CustomerControl>();
            if (_customerControl == counterControl.GetTop(bh)) {
                CustomerControl customerControl = counterControl.RemoveCustomerPos(bh);
                if (customerControl == null) {

                }
                else {
                    int GetMoneyNum = (int)(customerControl.PayMoney() * priceRate);
                    CurrencyMgr.Instance.Gold += GetMoneyNum;
                    Transform ObjectGo = ObjectPool.Instance.Get("Market", "incomeNum", InCome).transform;
                    IncomeNumControl incomeNumControl = ObjectGo.gameObject.GetComponent<IncomeNumControl>();
                    onceAddMoney = true;
                    incomeNumControl.OnStart(GetMoneyNum);
                    CustomerSum.Remove(ss);
                    if (CustomerSum.Count <= 0) {
                        CancelInvoke("ToUp");
                        percent.gameObject.SetActive(false);
                        nowtime = 0f;
                        canUp = false;
                    }
                    return;
                }
            }
        }
    }
    private void AddFxLv(float waitTime) {
        //Invoke("AddFxLvTrue", waitTime);
        AddFxLvTrue();
    }

    private void AddFxLvTrue() {
        Transform ObjectGo = ObjectPool.Instance.Get("Fx", "levelUp", transform).transform;
        ObjectGo.localPosition = Vector3.zero;
        ObjectGo.localScale = Vector3.one * 5f;
        RecycleFx recycleFx = ObjectGo.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(2f);
    }
    private void DecreaseGold() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowMoney = CurrencyMgr.Instance.Gold;
        int OnceMoney = CountEmployMoney;
        if (OnceMoney != 0 && nowMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position);
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            CountEmployMoney -= nowMoney;
        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            CountEmployMoney = 0;
            CountEmployLv++;
            ani.SetTrigger("Up");
            CancelInvoke("DecreaseMoney");
            
        }
        ReSetUI();
    }

}

public enum CounterUnLockCondition {
    None,
    Working,
}
