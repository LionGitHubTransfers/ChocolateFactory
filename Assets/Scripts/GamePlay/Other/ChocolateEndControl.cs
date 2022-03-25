using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateEndControl : MonoBehaviour
{
    List<Transform> condition = new List<Transform>();
    public int bh;
    public void OnStart(int _bh) {
        bh = _bh;
        condition.Clear();
        foreach (Transform ss in transform) {
            condition.Add(ss);
            ss.gameObject.SetActive(false);
        }

        condition[bh].gameObject.SetActive(true);
    }
}
