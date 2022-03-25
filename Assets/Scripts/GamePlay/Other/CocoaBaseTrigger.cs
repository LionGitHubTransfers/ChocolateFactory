using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocoaBaseTrigger : MonoBehaviour
{
    cocoaControl _cocoaControl;
    private void Start() {
        _cocoaControl = transform.parent.gameObject.GetComponent<cocoaControl>();
    }


    private void OnTriggerEnter(Collider other) {
        _cocoaControl?.OnTriggerEnterDemo(other);
    }

    private void OnTriggerExit(Collider other) {
        _cocoaControl?.OnTriggerExitDemo(other);
    }
}
