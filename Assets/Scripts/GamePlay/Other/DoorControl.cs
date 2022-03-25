using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    Animator animator;
    public int num;
    bool isOpen = false;
    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        num = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if ((other.gameObject.layer.Equals(3) && other.gameObject.tag == "Player") || (other.gameObject.layer.Equals(7) && other.gameObject.tag == "porter")) {
            CancelInvoke("closeDoor");
            
            if (isOpen) {
                num++;
                return;
            }
            if (num == 0) {
                if (other.gameObject.transform.position.z < transform.position.z) {
                    animator.SetBool("UpOpen", true);
                    isOpen = true;
                }
                else {
                    animator.SetBool("DownOpen", true);
                    isOpen = true;
                }
            }

            num++;

        }
    }

    private void OnTriggerExit(Collider other) {
        if ((other.gameObject.layer.Equals(3) && other.gameObject.tag == "Player") || (other.gameObject.layer.Equals(7) && other.gameObject.tag == "porter")) {
            CancelInvoke("closeDoor");
            num--;
            if (num == 0) {
                    Invoke("closeDoor", 3f);
            }
           
            
        }
    }
    private void closeDoor() {
        animator.SetBool("UpOpen", false);
        animator.SetBool("DownOpen", false);
        isOpen = false;
    }
}
