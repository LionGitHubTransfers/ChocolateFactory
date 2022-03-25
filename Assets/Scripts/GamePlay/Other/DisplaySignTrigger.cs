using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySignTrigger : MonoBehaviour
{
    DisplayControl displayControl;

    private void Start() {
        displayControl = transform.parent.gameObject.GetComponent<DisplayControl>();
    }
    private void OnTriggerEnter(Collider other) {
        displayControl.OnTriggerEnterSign(other);
    }
    private void OnTriggerExit(Collider other) {
        displayControl.OnTriggerExitSign(other);
    }
}
