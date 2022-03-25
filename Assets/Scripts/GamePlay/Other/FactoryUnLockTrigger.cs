using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnLockTrigger : MonoBehaviour
{
    private Factory0Control factory0Control;
    private void Start() {
        factory0Control = transform.parent.parent.gameObject.GetComponent<Factory0Control>();
    }

    private void OnTriggerEnter(Collider other) {
        factory0Control.OnTriggerEnterUnLock(other);
    }

    private void OnTriggerExit(Collider other) {
        factory0Control.OnTriggerExitUnlock(other);
    }
}
