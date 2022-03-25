using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    public bool isFull = false;
    public bool isEmpty = true;
    public int nowNum = 0;
    cocoaControl _cocoaControl;
    List<Transform> person = new List<Transform>();
    public bool BoxMoveOnce = true;
    private void Start() {
        _cocoaControl = transform.parent.gameObject.GetComponent<cocoaControl>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.layer == 7) {
            person.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (other.gameObject.layer == 7) {
                person.Remove(other.gameObject.transform);
            }
            
        }
   
    }
    private void ClearPerson() {
        person.Clear();
    }

    private void Update() {
        if (_cocoaControl.isMoveCocoa) {
            return;
        }
        if (BoxMoveOnce && !isEmpty) {
            if (person.Count > 0) {
                for (int i = 0; i < person.Count; i++) {
                    if (_cocoaControl.BoxToPlayer(person[i], nowNum)) {
                        ClearPerson();
                        BoxMoveOnce = false;
                        return;
                    }
                    else {
                        
                    }
                }
            }
        }
    }

}
