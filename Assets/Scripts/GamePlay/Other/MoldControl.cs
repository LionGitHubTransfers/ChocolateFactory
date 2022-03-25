using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoldControl : MonoBehaviour
{
    Animator animator;
    Transform nowChocolate;
    ChocolateControl chocolateControlNow;
    bool isWorking = false;
    float startOffset;
    float middleOffset;
    int bh;

    Transform UIRoot;

    bool isDecreaseBgPrecent = true;

    private string MOLDEREMPLOYMONEY = "MOLDEREMPLOYMONEY";
    private int m_MoldEmployMoney;
    bool endLv = false;

    public int MoldEmployMoney {
        get {
            return m_MoldEmployMoney;
        }
        set {
            int changeValue = value - m_MoldEmployMoney;
            m_MoldEmployMoney = value;
            LocalSave.SetInt(MOLDEREMPLOYMONEY, value);
        }
    }

    private string MOLDERMPLOYLV = "MOLDERMPLOYLV";
    private int m_MoldEmployLv;

    public int MoldEmployLv {
        get {
            return m_MoldEmployLv;
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
            if (m_MoldEmployLv < value) {

                AddFxLv(0.7f);
                ReSetUI();
                moneyMax = LvMoney[value];
                MoldEmployMoney = moneyMax;
            }
            m_MoldEmployLv = value;
            LocalSave.SetInt(MOLDERMPLOYLV, value);
        }
    }
    int moneyMax;
    int[] LvMoney = new int[] { 250, 300, 400, 1000, 2000, 0 };
    bool OnceTrigger;
    Text moneyText;
    Image moneyImage;
    Text LvText;
    Text speedText;
    Transform maxUI;
    float speedRate = 1f;
    List<Transform> lvSum = new List<Transform>();
    List<Transform> lvSum2 = new List<Transform>();
    Animator ani;
    ParticleSystem smokeFx;
    private void Start() {
        ani = transform.parent.gameObject.GetComponent<Animator>();
        bh = int.Parse(transform.parent.parent.parent.gameObject.name);
        animator = gameObject.GetChildControl<Animator>("mech_5_root");
        animator.speed = 0;

        UIRoot = gameObject.GetChildControl<Transform>("UIRoot");
        moneyText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/base/num");
        moneyImage = gameObject.GetChildControl<Image>("UIRoot/upgradeUI/bg/base/percent");
        LvText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/level");
        maxUI = gameObject.GetChildControl<Transform>("UIRoot/upgradeUI/bg/Max");
        maxUI.gameObject.SetActive(false);
        speedText = gameObject.GetChildControl<Text>("UIRoot/upgradeUI/bg/speed");
        smokeFx = gameObject.GetChildControl<ParticleSystem>("moldFX");
        for (int i = 1; i <= 5; i++) {
            lvSum.Add(gameObject.GetChildControl<Transform>("mech_5_root/lv" + i));
        }
        for (int i = 1; i <= 5; i++) {
            lvSum2.Add(gameObject.GetChildControl<Transform>("Base/lv" + i));
        }
        UIRoot.gameObject.SetActive(false);
        MOLDEREMPLOYMONEY = MOLDEREMPLOYMONEY + bh;
        MOLDERMPLOYLV = MOLDERMPLOYLV + bh;
        m_MoldEmployMoney = LocalSave.GetInt(MOLDEREMPLOYMONEY, LvMoney[0]);
        m_MoldEmployLv = LocalSave.GetInt(MOLDERMPLOYLV, 0);
        moneyMax = LvMoney[m_MoldEmployLv];
        if (m_MoldEmployLv == 4) {
            endLv = true;
            CancelInvoke("DecreaseGold");
            maxUI.gameObject.SetActive(true);
        }
        ReSetUI();
    }
    private void ReSetUI() {
        int thenmoney = int.Parse(moneyText.text);
        int money = MoldEmployMoney;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            moneyText.text = thenmoney.ToString();
        });
        float thenamount = moneyImage.fillAmount;
        float mount = (moneyMax - MoldEmployMoney) * 1.0f / moneyMax;
        Tween tween2 = DOTween.To(() => thenamount, x => thenamount = x, mount, 0.5f).OnUpdate(() => {
            moneyImage.fillAmount = thenamount;
        });
        LvText.text = "lv " + (MoldEmployLv + 1);
        foreach (Transform ss in lvSum) {
            ss.gameObject.SetActive(false);
        }
        foreach (Transform ss in lvSum2) {
            ss.gameObject.SetActive(false);
        }
        lvSum[MoldEmployLv].gameObject.SetActive(true);
        lvSum2[MoldEmployLv].gameObject.SetActive(true);
        if (MoldEmployLv == 0) {
            speedText.text = "Speed:\n100%";
            speedRate = 1f;
        }
        else {
            speedRate = MoldEmployLv * 0.2f + 1f;
            int speedRatetxt = (int)(speedRate * 100);
            speedText.text = "Speed:\n" + speedRatetxt.ToString() + "%";
        }
    }

    public float GetSpeed() {
        return m_MoldEmployLv * 0.15f;
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
            UIRoot.gameObject.SetActive(false);
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
    private void DecreaseGold() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowMoney = CurrencyMgr.Instance.Gold;
        int OnceMoney = MoldEmployMoney;
        if (OnceMoney != 0 && nowMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position);
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            MoldEmployMoney -= nowMoney;

        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            MoldEmployMoney = 0;
            MoldEmployLv++;
            ani.SetTrigger("Up");
            CancelInvoke("DecreaseMoney");

        }
        ReSetUI();
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
                smokeFx.Play();
                chocolateControlNow.ChangeCondition(2 * bh + 1);
            }
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

}
