using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disintegratorControl : MonoBehaviour
{
    Transform UIRoot;

    bool isDecreaseBgPrecent = true;

    private string DISEREMPLOYMONEY = "DISEREMPLOYMONEY";
    private int m_DisEmployMoney;
    bool endLv = false;

    public int DisEmployMoney {
        get {
            return m_DisEmployMoney;
        }
        set {
            int changeValue = value - m_DisEmployMoney;
            m_DisEmployMoney = value;
            LocalSave.SetInt(DISEREMPLOYMONEY, value);
        }
    }

    private string DISERMPLOYLV = "DISERMPLOYLV";
    private int m_DisEmployLv;

    public int DisEmployLv {
        get {
            return m_DisEmployLv;
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
            if (m_DisEmployLv < value) {

                AddFxLv(0.7f);
                ReSetUI();
                moneyMax = LvMoney[value];
                DisEmployMoney = moneyMax;
            }
            m_DisEmployLv = value;
            LocalSave.SetInt(DISERMPLOYLV, value);
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
        ani = gameObject.GetComponent<Animator>();
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
        m_DisEmployMoney = LocalSave.GetInt(DISEREMPLOYMONEY, LvMoney[0]);
        m_DisEmployLv = LocalSave.GetInt(DISERMPLOYLV, 0);
        moneyMax = LvMoney[m_DisEmployLv];
        if (m_DisEmployLv == 4) {
            endLv = true;
            CancelInvoke("DecreaseGold");
            OnceMoney = 0;
            maxUI.gameObject.SetActive(true);
        }
        ReSetUI();
    }

    public float GetSpeedRate() {
        return DisEmployLv * 0.2f;
    }

    private void ReSetUI() {
        moneyText.text = DisEmployMoney.ToString();
        moneyImage.fillAmount = (moneyMax - DisEmployMoney) * 1.0f / moneyMax;
        LvText.text = "lv " + DisEmployLv;
        foreach (Transform ss in lvSum) {
            ss.gameObject.SetActive(false);
        }
        lvSum[DisEmployLv].gameObject.SetActive(true);
        if (DisEmployLv == 0) {
            speedText.text = "Speed:\n100%";
            speedRate = 1f;
        }
        else {
            speedRate = DisEmployLv * 0.2f + 1f;
            int speedRatetxt = (int)(speedRate * 100);
            speedText.text = "Speed:\n" + speedRatetxt.ToString() + "%";
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = false;
            OnceTrigger = true;
            UIRoot.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {

            isDecreaseBgPrecent = true;
            OnceTrigger = false;
            CancelInvoke("DecreaseGold");
            OnceMoney = 0;
            UIRoot.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (isDecreaseBgPrecent) {
        
        }
        else {
            if (OnceTrigger) {
                if (!endLv) {
                    OnceMoney = DisEmployMoney / 4;
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
        ObjectGo.localScale = Vector3.one * 5f;
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
            if (DisEmployMoney - OnceMoney < 0) {
                CurrencyMgr.Instance.Gold -= DisEmployMoney;
                DisEmployMoney = 0;
                CancelInvoke("DecreaseMoney");
                OnceMoney = 0;
                DisEmployLv++;
                ani.SetTrigger("Up");
                ReSetUI();
                return;
            }
            CurrencyMgr.Instance.Gold -= OnceMoney;
            DisEmployMoney -= OnceMoney;
            if (OnceMoney != 0) {
                PlayerMgr.Instance.ShowDecreaseGold(transform.position);
            }
            OnceMoney++;
            if (DisEmployMoney <= 0) {
                DisEmployLv++;
                ani.SetTrigger("Up");
                CancelInvoke("DecreaseMoney");
                OnceMoney = 0;
            }
            ReSetUI();
        }

    }
}
