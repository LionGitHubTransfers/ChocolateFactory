using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roasterControl : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            ChocolateControl chocolateControl = other.gameObject.GetComponent<ChocolateControl>();
            chocolateControl.ChangeCondition(1);
        }
    }
}
