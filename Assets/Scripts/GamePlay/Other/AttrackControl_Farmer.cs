using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrackControl_Farmer : MonoBehaviour
{
    FarmerControl farmerControl;
    cocoaControl _cocoaControl;
    bool IsIn;
    public int OnceAttrackTimes = 1;
    public bool IsMove = false;
    private void Start() {
        farmerControl = transform.parent.gameObject.GetComponent<FarmerControl>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "cocoaTree") {
            _cocoaControl = other.transform.parent.gameObject.GetComponent<cocoaControl>();
            if (!_cocoaControl.isUnlock) {
                return;
            }
            else {
                InvokeRepeating("AniAttackCocoaTree", 0.25f, 0.25f);
            }
 

            IsIn = true;

        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "cocoaTree") {
            CancelInvoke("AniAttackCocoaTree");
            IsIn = false;
            _cocoaControl = null;
        }
    }
    private void AniAttackCocoaTree() {
        if (_cocoaControl == null || IsMove == true) {
            return;
        }
        
        if (farmerControl.farmerCondition != FarmerCondition.AttrackTree) {
            return;
        }
        if (_cocoaControl.isFull || _cocoaControl.nowCocoaNum == 0) {
            farmerControl.NextTime(); 
        }
        else {
            farmerControl.AniAttackCocoaTree();
        }

    }

    public void AttackCocoaTree() {
        if (_cocoaControl != null) {

            int times = Random.Range(1, OnceAttrackTimes + 1);
            for (int i = 0; i < times; i++) {
                _cocoaControl.DecreaseCocoa(transform.parent.position);
            }
        }
    }
}
