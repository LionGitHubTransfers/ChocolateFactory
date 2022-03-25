using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaControl : MonoBehaviour
{
    bool CanShow;

    float nowTime = 0f;
    float maxTime = 3f;
    float onceTime = 1f;

    private void Start() {
        gameObject.SetActive(false);
        CanShow = false;
    }

    public void ToShow() {
        nowTime = 0f;
        CanShow = true;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (CanShow) {
            transform.rotation = Quaternion.Euler(0f, 90f, 50f);
            if (nowTime < maxTime) {
                nowTime += onceTime * Time.deltaTime;
            }
            else {
                CanShow = false;
                gameObject.SetActive(false);
                nowTime = 0f;
            }
        }
        
    }
}
