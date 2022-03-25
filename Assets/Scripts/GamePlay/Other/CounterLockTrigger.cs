using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterLockTrigger : MonoBehaviour
{
    Text txtMoney;
    CounterOne counterOne;
    Transform modelA;
    private void Start() {
        counterOne = transform.parent.gameObject.GetComponent<CounterOne>();
        txtMoney = gameObject.GetChildControl<Text>("Sign/Unlock/num");
        modelA = gameObject.GetChildControl<Transform>("modelA");
        modelA.gameObject.SetActive(false);
        ReSetUI();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            InvokeRepeating("DecreaseMoney", 0.25f, 0.25f);
            modelA.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            CancelInvoke("DecreaseMoney");
            modelA.gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        modelA?.gameObject.SetActive(false);
    }
    private void ReSetUI() {
        int thenmoney = int.Parse(txtMoney.text);
        int money = MarketMgr.Instance.counterMoney;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            txtMoney.text = thenmoney.ToString();
        });
    }
    private void ReSetUI0() {
        int thenmoney = int.Parse(txtMoney.text);
        int money = 0;
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(() => {
            txtMoney.text = thenmoney.ToString();
        });
    }
    private void DecreaseMoney() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowMoney = CurrencyMgr.Instance.Gold;
        int OnceMoney = MarketMgr.Instance.counterMoney;
        if (nowMoney != 0 && OnceMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position);
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            MarketMgr.Instance.counterMoney -= nowMoney;
            ReSetUI();
        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            MarketMgr.Instance.counterMoney = 0;
            counterOne.counterCondition = CounterCondition.UnLock;
            MarketMgr.Instance.counterLv++;
            MarketMgr.Instance.ReSetCounterView();
            CancelInvoke("DecreaseMoney");
            ReSetUI0();
        }
    }
}
