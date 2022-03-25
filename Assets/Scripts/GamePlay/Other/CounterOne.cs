using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterOne : MonoBehaviour
{
    public CounterCondition counterCondition;
    Transform Lock;
    Transform UnLock;
    public int bh;
    public CounterUnLockTrigger counterUnLockTrigger;
    private void Start() {
        Lock = gameObject.GetChildControl<Transform>("Lock");
        UnLock = gameObject.GetChildControl<Transform>("UnLock");
        counterUnLockTrigger = gameObject.GetChildControl<CounterUnLockTrigger>("UnLock");
        SetView();
    }

    public void SetView() {
        if (counterCondition == CounterCondition.None) {
            Lock.gameObject.SetActive(false);
            UnLock.gameObject.SetActive(false);
        }
        else if (counterCondition == CounterCondition.Lock) {
            if (bh == 1) {
                int disnum = MarketMgr.Instance.ReturnDisplayChocolateNum();
                if (disnum >= 6) {
                    Lock.gameObject.SetActive(true);
                }
                else {
                    Lock.gameObject.SetActive(false);
                }
                UnLock.gameObject.SetActive(false);
                return;
            }
            Lock.gameObject.SetActive(true);
            UnLock.gameObject.SetActive(false);
        }
        else {
            Lock.gameObject.SetActive(false);
            UnLock.gameObject.SetActive(true);
        }
    }

}

public enum CounterCondition {
    None,
    Lock,
    UnLock,
}
