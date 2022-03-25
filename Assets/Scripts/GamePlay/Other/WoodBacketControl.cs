using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBacketControl : MonoBehaviour
{
    List<Transform> cocoas = new List<Transform>();
    Transform cocoa;
    private void Awake() {
        cocoa = gameObject.GetChildControl<Transform>("cocoa");
        foreach (Transform ss in cocoa) {
            cocoas.Add(ss);
            ss.gameObject.SetActive(false);
        }
    }

    public void OnStart(int nowNum) {
        AllToDisable();
        for (int i = 0; i < nowNum; i++) {
            cocoas[i].gameObject.SetActive(true);
        }
    }
    private void AllToDisable() {
        foreach (Transform ss in cocoa) {
            ss.gameObject.SetActive(false);
        }
    }
}
