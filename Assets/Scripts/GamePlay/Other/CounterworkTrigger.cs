using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterworkTrigger : MonoBehaviour
{
    CounterUnLockTrigger counterUnLockTrigger;

    // Start is called before the first frame update
    void Start() {
        counterUnLockTrigger = transform.parent.gameObject.GetComponent<CounterUnLockTrigger>();
    }

    private void OnTriggerEnter(Collider other) {
        counterUnLockTrigger?.OnTriggerEnterwork(other);
    }

    private void OnTriggerExit(Collider other) {
        counterUnLockTrigger?.OnTriggerExitwork(other);
    }
}
