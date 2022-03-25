using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterupLvTrigger : MonoBehaviour
{
    CounterUnLockTrigger counterUnLockTrigger;

    // Start is called before the first frame update
    void Start()
    {
        counterUnLockTrigger = transform.parent.gameObject.GetComponent<CounterUnLockTrigger>();
    }

    private void OnTriggerEnter(Collider other) {
        counterUnLockTrigger?.OnTriggerEnterupLv(other);
    }

    private void OnTriggerExit(Collider other) {
        counterUnLockTrigger?.OnTriggerExitupLv(other);
    }
}
