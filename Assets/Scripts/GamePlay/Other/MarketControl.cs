using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketControl : MonoBehaviour
{
    List<Transform>[] Chocolate = new List<Transform>[5];
    private void Start() {
        for (int i = 0; i < 5; i++) {
            Chocolate[i] = new List<Transform>();
        }
        InitMarket();
    }

    private void InitMarket() {

        for (int i = 0; i < 5; i++) {
            foreach (Transform ss in gameObject.GetChildControl<Transform>((i + 1).ToString())) {
                
                Chocolate[i].Add(ss);
            }
            for (int j = 0; j < Chocolate[i].Count; j++) {
                DisplayGoodsControl displayGoodsControl = Chocolate[i][j].gameObject.GetComponent<DisplayGoodsControl>();
                displayGoodsControl.bh = i + 1;
                if (j < MarketMgr.Instance.chocolateNum[i]) {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.UnLock;
                }
                else if (j == MarketMgr.Instance.chocolateNum[i]) {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.Lock;
                }
                else {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.None;
                }
            }
        }
    }

    public void SetView() {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < Chocolate[i].Count; j++) {
                DisplayGoodsControl displayGoodsControl = Chocolate[i][j].gameObject.GetComponent<DisplayGoodsControl>();
                if (j < MarketMgr.Instance.chocolateNum[i]) {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.UnLock;
                }
                else if (j == MarketMgr.Instance.chocolateNum[i]) {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.Lock;
                }
                else {
                    displayGoodsControl.displayGoodsCondition = DisplayGoodsCondition.None;
                }
                displayGoodsControl.SetView();
            }
        }
    }
}
