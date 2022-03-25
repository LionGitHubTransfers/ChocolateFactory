using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryLockTriggerNormal : MonoBehaviour
{
    FactoryControl factoryControl;
    Text UnlockText;
    Transform MoneyEndPos;
    private void Start() {
        UnlockText = gameObject.GetChildControl<Text>("newlineMesh/Unlock/num");
        factoryControl = transform.parent.gameObject.GetComponent<FactoryControl>();
        MoneyEndPos = gameObject.GetChildControl<Transform>("MoneyEndPos");
        ReSetUI();
    }

    private void ReSetUI() {
        int thenmoney = int.Parse(UnlockText.text);
        int money = FactoryMgr.Instance.FactoryLockMoney;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            UnlockText.text = thenmoney.ToString();
        });
    }
    private void ReSetUI0() {
        int thenmoney = int.Parse(UnlockText.text);
        int money = 0;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            UnlockText.text = thenmoney.ToString();
        });
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            InvokeRepeating("DecreaseMoney", 0.25f, 0.25f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            CancelInvoke("DecreaseMoney");
        }
    }
    private void DecreaseMoney() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowMoney = CurrencyMgr.Instance.Gold;
        int OnceMoney = FactoryMgr.Instance.FactoryLockMoney;
        if (nowMoney != 0 && OnceMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(MoneyEndPos.position);
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            FactoryMgr.Instance.FactoryLockMoney -= nowMoney;
            ReSetUI();
        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            FactoryMgr.Instance.FactoryLockMoney = 0;
            CancelInvoke("DecreaseMoney");
            FactoryMgr.Instance.FactoryLv++;
            factoryControl.UnLockDemo();
            ReSetUI0();
        }
    }
}
