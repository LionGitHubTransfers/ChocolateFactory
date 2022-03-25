using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessControl : MonoBehaviour {
    Animator animator;
    Transform nowChocolate;
    ChocolateControl chocolateControlNow;
    bool isWorking = false;
    float startOffset;
    float middleOffset;
    int bh;

    Transform UIRoot;

    bool isDecreaseBgPrecent = true;
    private string PROCESSEMPLOYMONEY = "PROCESSEMPLOYMONEY";
    private int m_ProcessEmployMoney;
    bool endLv = false;

    public int ProcessEmployMoney {
        get {
            return m_ProcessEmployMoney;
        }
        set {
            int changeValue = value - m_ProcessEmployMoney;
            m_ProcessEmployMoney = value;
            LocalSave.SetInt(PROCESSEMPLOYMONEY, value);
        }
    }

    private string PROCSEEERMPLOYLV = "PROCSEEERMPLOYLV";
    private int m_ProcessEmployLv;

    public int ProcessEmployLv {
        get {
            return m_ProcessEmployLv;
        }
        set {
            if (value > 4) {
                return;
            }
            else if (value == 4) {
                endLv = true;
                CancelInvoke("DecreaseGold");
                OnceMoney = 0;
                maxUI.gameObject.SetActive(true);
            }
            if (m_ProcessEmployLv < value) {

                AddFxLv(0.7f);
                ReSetUI();
                moneyMax = LvMoney[value];
                ProcessEmployMoney = moneyMax;
            }
            m_ProcessEmployLv = value;
            LocalSave.SetInt(PROCSEEERMPLOYLV, value);
        }
    }
    int moneyMax;
    int[] LvMoney = new int[] { 5, 10, 15, 20, 25, 0 };
    bool OnceTrigger;
    Text moneyText;
    Image moneyImage;
    Text LvText;
    Text speedText;
    Transform maxUI;
    float speedRate = 1f;
    List<Transform> lvSum = new List<Transform>();
    Animator ani;
    private void Start() {
        ani = transform.parent.gameObject.GetComponent<Animator>();
        bh = int.Parse(transform.parent.parent.parent.gameObject.name);
        animator = gameObject.GetChildControl<Animator>("mech_6_root");
        animator.speed = 0;

        UIRoot = gameObject.GetChildControl<Transform>("UIRoot");
        moneyText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/base/num");
        moneyImage = gameObject.GetChildControl<Image>("UIRoot/upgradeUI/bg/base/percent");
        LvText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/level");
        maxUI = gameObject.GetChildControl<Transform>("UIRoot/upgradeUI/bg/Max");
        maxUI.gameObject.SetActive(false);
        speedText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/speed");
        for (int i = 1; i <= 5; i++) {
            lvSum.Add(gameObject.GetChildControl<Transform>("lv" + i));
        }
        UIRoot.gameObject.SetActive(false);
        PROCESSEMPLOYMONEY = PROCESSEMPLOYMONEY + bh;
        PROCSEEERMPLOYLV = PROCSEEERMPLOYLV + bh;
        m_ProcessEmployMoney = LocalSave.GetInt(PROCESSEMPLOYMONEY, LvMoney[0]);
        m_ProcessEmployLv = LocalSave.GetInt(PROCSEEERMPLOYLV, 0);
        moneyMax = LvMoney[m_ProcessEmployLv];
        if (m_ProcessEmployLv == 4) {
            endLv = true;
            CancelInvoke("DecreaseGold");
            OnceMoney = 0;
            maxUI.gameObject.SetActive(true);
        }
        ReSetUI();
    }
    private void ReSetUI() {
        moneyText.text = ProcessEmployMoney.ToString();
        moneyImage.fillAmount = (moneyMax - ProcessEmployMoney) * 1.0f / moneyMax;
        LvText.text = "lv " + ProcessEmployLv;
        foreach (Transform ss in lvSum) {
            ss.gameObject.SetActive(false);
        }
        lvSum[ProcessEmployLv].gameObject.SetActive(true);
        if (ProcessEmployLv == 0) {
            speedText.text = "Speed:\n100%";
            speedRate = 1f;
        }
        else {
            speedRate = ProcessEmployLv * 0.2f + 1f;
            int speedRatetxt = (int)(speedRate * 100);
            speedText.text = "Speed:\n" + speedRatetxt.ToString() + "%";
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            nowChocolate = other.gameObject.transform;
            chocolateControlNow = other.gameObject.GetComponent<ChocolateControl>();
            startOffset = nowChocolate.position.x;
            middleOffset = nowChocolate.position.x - transform.position.x;
            isWorking = true;
        }
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = false;
            OnceTrigger = true;
            UIRoot.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            isWorking = false;
        }
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = true;
            OnceTrigger = false;
            CancelInvoke("DecreaseGold");
            OnceMoney = 0;
            UIRoot.gameObject.SetActive(false);
        }
    }
    private void Update() {
        if (isWorking) {
            float endOffset = startOffset - nowChocolate.position.x;
            float rate = endOffset / (2f * middleOffset);
            if (rate > 1) {
                rate = 1;
            }
            animator.Play("working", 0, rate);
            if (rate > 0.5f) {
                chocolateControlNow.ChangeCondition(2 * bh + 1);
            }
        }
        if (isDecreaseBgPrecent) {
         
        }
        else {
            
                if (OnceTrigger) {
                    if (!endLv) {
                    OnceMoney = ProcessEmployMoney / 4;
                        InvokeRepeating("DecreaseGold", 0.25f, 0.25f);
                    }
                    OnceTrigger = false;
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
        ObjectGo.localScale = Vector3.one * 2f;
        RecycleFx recycleFx = ObjectGo.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(2f);
    }
    int OnceMoney = 0;
    private void DecreaseGold() {
        int nowMoney = CurrencyMgr.Instance.Gold;
        if (nowMoney - OnceMoney < 0) {
            CancelInvoke("DecreaseMoney");
            OnceMoney = 0;
        }
        else {
            if (ProcessEmployMoney - OnceMoney < 0) {
                CurrencyMgr.Instance.Gold -= ProcessEmployMoney;
                ProcessEmployMoney = 0;
                CancelInvoke("DecreaseMoney");
                OnceMoney = 0;
                ProcessEmployLv++;
                ani.SetTrigger("Up");
                ReSetUI();
                return;
            }
            CurrencyMgr.Instance.Gold -= OnceMoney;
            ProcessEmployMoney -= OnceMoney;
            if (OnceMoney != 0) {
                PlayerMgr.Instance.ShowDecreaseGold(transform.position);
            }
            OnceMoney++;
            if (ProcessEmployMoney <= 0) {
                ProcessEmployLv++;
                ani.SetTrigger("Up");
                CancelInvoke("DecreaseMoney");
                OnceMoney = 0;
            }
            ReSetUI();
        }

    }
}
