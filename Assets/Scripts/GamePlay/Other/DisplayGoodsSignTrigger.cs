using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayGoodsSignTrigger : MonoBehaviour
{
    Text txtMoney;
    int bh;
    DisplayGoodsControl displayGoodsControl;
    Transform modelA;
    private void Start() {
        displayGoodsControl = transform.parent.gameObject.GetComponent<DisplayGoodsControl>();
        bh = displayGoodsControl.bh;

        modelA = transform.parent.gameObject.GetChildControl<Transform>("modelA");
        txtMoney = gameObject.GetChildControl<Text>("Unlock/num");
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
        int money = MarketMgr.Instance.chocolateMoney[bh - 1];
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
        int OnceMoney = MarketMgr.Instance.chocolateMoney[bh - 1];
        if (nowMoney != 0 && OnceMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position);
        }
        if (nowMoney - OnceMoney < 0) {
            CurrencyMgr.Instance.Gold = 0;
            MarketMgr.Instance.ChangeChocolateMoney(bh - 1, OnceMoney - nowMoney);
            ReSetUI();
        }
        else {
            CurrencyMgr.Instance.Gold -= OnceMoney;
            MarketMgr.Instance.ChangeChocolateMoney(bh - 1, 0);
            displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.UnLock;
            MarketMgr.Instance.ChangeChocolateNum(bh - 1, MarketMgr.Instance.chocolateNum[bh - 1] + 1);
            PlayerMgr.Instance.characterController.enabled = false;
            PlayerMgr.Instance.Player.position = displayGoodsControl.PlayerPos.position;
            PlayerMgr.Instance.characterController.enabled = true;
            displayGoodsControl.SetView();
            CancelInvoke("DecreaseMoney");
            ReSetUI0();
        }

    }
}
