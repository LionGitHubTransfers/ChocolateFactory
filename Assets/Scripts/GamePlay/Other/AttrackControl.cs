using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrackControl : MonoBehaviour
{
    PlayerControl playerControl;
    float AttrackRadius = 0.5f;
    public bool IsIn = false;
    cocoaControl _cocoaControl;

    public bool CanAttrack = true;
    public bool IsMove = false;
    private void Start() {
        playerControl = transform.parent.gameObject.GetComponent<PlayerControl>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "cocoaTree" && CanAttrack) {
            _cocoaControl = other.transform.parent.gameObject.GetComponent<cocoaControl>();
            if (!_cocoaControl.isUnlock) {
                return;
            }
            if (_cocoaControl.isFull) {
                CancelInvoke("AniAttackCocoaTree");
            }
            else {
                    InvokeRepeating("AniAttackCocoaTree", 0.25f, 0.25f);
                
            }

            IsIn = true;
        }
        if (other.gameObject.layer == 7) {
            if (other.gameObject.TryGetComponent<FarmerControl>(out FarmerControl com)) {
                if (com.farmerCondition == FarmerCondition.Sleep) {
                    playerControl.AniSlap();
                    com.RemovedizzyFx();
                }
            }
            else if (other.gameObject.TryGetComponent<PorterControl>(out PorterControl com1)) {
                if (com1.porterCondition == PorterCondition.Sleep) {
                    playerControl.AniSlap();
                    com1.RemovedizzyFx();
                }
            }
            //else if (other.gameObject.TryGetComponent<SellerControl>(out PorterControl com2)) {
            
                
            //}
        }
    }
    public void OnTriggerEnterDemo(cocoaControl __cocoaControl) {
        if (CanAttrack) {
            _cocoaControl = __cocoaControl;
                InvokeRepeating("AniAttackCocoaTree", 0.25f, 0.25f);
            
            IsIn = true;
        }
        
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "cocoaTree" && CanAttrack) {
            CancelInvoke("AniAttackCocoaTree");
            IsIn = false;
            _cocoaControl = null;
        }
    }

    private void AniAttackCocoaTree() {
        if (_cocoaControl == null || IsMove == true || _cocoaControl.nowInTreePlayer.Count == 0) {
            return;
        }
        if (_cocoaControl.isFull || _cocoaControl.nowCocoaNum == 0) {
        }
        else {
            playerControl.AniAttackCocoaTree();
        }
        
    }


    public void AttackCocoaTree() {
        if (_cocoaControl != null) {
            int times = Random.Range(1, 4);
            for (int i = 0; i < times; i++) {
                _cocoaControl.DecreaseCocoa(transform.parent.position);
            }
            
        }
    }


    public void StopAttrack() {
        CancelInvoke("AniAttackCocoaTree");
    }

    public void StartAttrack() {
        InvokeRepeating("AniAttackCocoaTree", 0.25f, 0.25f);
    }
}
