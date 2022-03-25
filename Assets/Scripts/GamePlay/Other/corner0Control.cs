using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corner0Control : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            ChocolateControl chocolateControl = other.gameObject.GetComponent<ChocolateControl>();
            chocolateControl.ChangRotate(true);
            if (FactoryMgr.Instance.FactoryLv > 0) {
                chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[1]);
            }
            else {
                chocolateControl.RecObj();
            }
           
        }
    }
}
