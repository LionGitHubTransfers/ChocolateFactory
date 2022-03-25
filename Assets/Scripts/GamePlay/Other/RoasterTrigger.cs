using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoasterTrigger : MonoBehaviour
{
    Transform FxSmoke;
    Transform Fxlight;
    int num = 0;
    Transform UIRoot;


    bool isDecreaseBgPrecent = true;

    private string ROASTEREMPLOYMONEY = "ROASTEREMPLOYMONEY";
    private int m_RoaterEmployMoney;
    bool endLv = false;
    Animator ani;

    public int RoasterEmployMoney {
        get {
            return m_RoaterEmployMoney;
        }
        set {
            int changeValue = value - m_RoaterEmployMoney;
            m_RoaterEmployMoney = value;
            LocalSave.SetInt(ROASTEREMPLOYMONEY, value);
        }
    }

    private string ROASTERMPLOYLV = "ROASTERMPLOYLV";
    private int m_RoasterEmployLv;

    public int RoasterEmployLv {
        get {
            return m_RoasterEmployLv;
        }
        set {
            if (value > 4) {
                return;
            } else if (value == 4) {
                endLv = true;
                CancelInvoke("DecreaseGold");
                maxUI.gameObject.SetActive(true);
            }
            if (m_RoasterEmployLv < value) {

                AddFxLv(0.7f);
                ReSetUI();
                moneyMax = LvMoney[value];
                RoasterEmployMoney = moneyMax;
            }
            m_RoasterEmployLv = value;
            LocalSave.SetInt(ROASTERMPLOYLV, value);
        }
    }
    int moneyMax;
    int[] LvMoney = new int[] { 500, 600, 1000, 5000, 10000, 0};
    bool OnceTrigger;
    Text moneyText;
    Image moneyImage;
    Text LvText;
    Text speedText;
    Transform maxUI;
    float speedRate = 1f;
    List<Transform> lvSum = new List<Transform>();
    List<Transform> lvSum2 = new List<Transform>();
    private void Start() {
        ani = transform.parent.gameObject.GetComponent<Animator>();
        FxSmoke = gameObject.GetChildControl<Transform>("Smoke");
        Fxlight = gameObject.GetChildControl<Transform>("light");
        UIRoot = gameObject.GetChildControl<Transform>("UIRoot");
        moneyText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/base/num");
        moneyImage = gameObject.GetChildControl<Image>("UIRoot/upgradeUI/bg/base/percent");
        LvText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/level");
        maxUI = gameObject.GetChildControl<Transform>("UIRoot/upgradeUI/bg/Max");
        maxUI.gameObject.SetActive(false);
        speedText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/speed");
        for (int i = 1; i <= 5;i++) {
            lvSum.Add(gameObject.GetChildControl<Transform>("lv" + i));
        }
        Transform dis = transform.parent.parent.gameObject.GetChildControl<Transform>("disintegrator");
        foreach (Transform ss in dis) {
            if (ss.gameObject.name.Contains("lv")) {
                lvSum2.Add(ss);
            }
        }
        UIRoot.gameObject.SetActive(false);
        FxActive(false);
        m_RoaterEmployMoney = LocalSave.GetInt(ROASTEREMPLOYMONEY, LvMoney[0]);
        m_RoasterEmployLv = LocalSave.GetInt(ROASTERMPLOYLV, 0);
        moneyMax = LvMoney[m_RoasterEmployLv];
        if (m_RoasterEmployLv == 4) {
            endLv = true;
            CancelInvoke("DecreaseGold");
            maxUI.gameObject.SetActive(true);
        }
        ReSetUI();
    }

    public float GetSpeedRate() {
        return RoasterEmployLv * 0.8f;
    }

    private void FxActive(bool active) {
        FxSmoke.gameObject.SetActive(active);
        Fxlight.gameObject.SetActive(active);
    }
    private void ReSetUI() {
        int thenmoney = int.Parse(moneyText.text);
        int money = RoasterEmployMoney;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            moneyText.text = thenmoney.ToString();
        });
        float thenamount = moneyImage.fillAmount;
        float mount = (moneyMax - RoasterEmployMoney) * 1.0f / moneyMax;
        Tween tween2 = DOTween.To(() => thenamount, x => thenamount = x, mount, 0.5f).OnUpdate(() => {
            moneyImage.fillAmount = thenamount;
        });
        LvText.text = "lv " + (RoasterEmployLv + 1);
        foreach (Transform ss in lvSum) {
            ss.gameObject.SetActive(false);
        }
        foreach (Transform ss in lvSum2) {
            ss.gameObject.SetActive(false);
        }
        lvSum[RoasterEmployLv].gameObject.SetActive(true);
        lvSum2[RoasterEmployLv].gameObject.SetActive(true);
        if (RoasterEmployLv == 0) {
            speedText.text = "Speed:\n100%";
            speedRate = 1f;
        }
        else {
            speedRate = RoasterEmployLv * 0.2f + 1f;
            int speedRatetxt = (int)(speedRate * 100);
            speedText.text = "Speed:\n" + speedRatetxt.ToString() + "%";
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            num++;
            
            FxActive(true);
        }
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
           
            isDecreaseBgPrecent = false;
            OnceTrigger = true;
            UIRoot.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            num--;
            if (num == 0) {
                FxActive(false);
            }
        }
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
           
            isDecreaseBgPrecent = true;
            OnceTrigger = false;
            CancelInvoke("DecreaseGold");
            UIRoot.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (isDecreaseBgPrecent) {
        }
        else {

                if (OnceTrigger) {
                    if (!endLv) {
                        InvokeRepeating("DecreaseGold", 0.25f, 1.5f);
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

        Transform ObjectGo = ObjectPool.Instance.Get("Fx", "levelUpFX", transform).transform;
        ObjectGo.localPosition = new Vector3(0f, 0f, -4.45f);
        //ObjectGo.localScale = new Vector3(3f, 7f, 2f);
        //ObjectGo.localEulerAngles = new Vector3(-90f, 0f, 0f);
        RecycleFx recycleFx = ObjectGo.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(2f);
    }
    private void DecreaseGold() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowMoney = CurrencyMgr.Instance.Gold;
        int OnceMoney = RoasterEmployMoney;
        if (nowMoney != 0 && OnceMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position + new Vector3(-5f, 0f, 0f));
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            RoasterEmployMoney -= nowMoney;
        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            RoasterEmployMoney = 0;
            RoasterEmployLv++;
            ani.SetTrigger("Up");
            CancelInvoke("DecreaseMoney");
        }
        ReSetUI();
    }
}
