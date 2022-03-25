using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorControl : MonoBehaviour
{
    Animator animator;
    int num;
    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        num = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer.Equals(3)) {
            animator.SetBool("Open", true);
            num++;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer.Equals(3)) {
            CancelInvoke("CloseDoor");
            num--;
            if (num == 0) {
                Invoke("CloseDoor", 3f);
            }
        }
    }

    private void CloseDoor() {
        animator.SetBool("Open", false);
    }

}
