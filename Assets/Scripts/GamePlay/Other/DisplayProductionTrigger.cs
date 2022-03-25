using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayProductionTrigger : MonoBehaviour
{
    DisplayControl displayControl;
    private void Start() {
        displayControl = transform.parent.gameObject.GetComponent<DisplayControl>();
    }
    private void OnTriggerEnter(Collider other) {
        displayControl.OnTriggerEnterProduction(other);
    }
}
