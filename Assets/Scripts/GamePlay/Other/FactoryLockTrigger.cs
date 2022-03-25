using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryLockTrigger : MonoBehaviour
{
    Factory0Control factory0Control;
    Text UnlockText;
    private void Start() {
        UnlockText = gameObject.GetChildControl<Text>("Unlock/num");
        factory0Control = transform.parent.gameObject.GetComponent<Factory0Control>();
        ReSetUI();
    }

    private void ReSetUI() {
        UnlockText.text = FactoryMgr.Instance.FactoryLockMoney.ToString();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            Invoke("DecreaseMoney", 0.25f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            CancelInvoke("DecreaseMoney");
        }
    }
    private void DecreaseMoney() {
        FactoryMgr.Instance.FactoryLv += 2;
        FactoryControl factoryControl0 = FactoryMgr.Instance.FactoryControls[0];
        factoryControl0.gameObject.SetActive(true);
        
        factoryControl0.IsZero = true;
        factory0Control.UnLockDemo();
    }
}
