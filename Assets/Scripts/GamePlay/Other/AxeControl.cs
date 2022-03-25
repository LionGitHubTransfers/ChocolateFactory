using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeControl : MonoBehaviour
{
    float startPos;
    float endPos;
    PlayerControl playerControl;
    private void Start() {
        playerControl = PlayerMgr.Instance.Player.gameObject.GetComponent<PlayerControl>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            if (playerControl.JudgeCarry()) {

            }
            else {
                startPos = other.transform.position.z;
            }
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Player") {
            if (playerControl.JudgeCarry()) {

            }
            else {
                endPos = other.transform.position.z;
                if (startPos - endPos > 0) {
                    playerControl.AxeActive(true);
                }
                else if (startPos - endPos < 0) {
                    playerControl.AxeActive(false);
                }
                else {
                    
                }
            }

        }
    }

}
