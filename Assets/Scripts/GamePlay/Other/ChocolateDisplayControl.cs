using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateDisplayControl : MonoBehaviour
{
    List<Transform> condition = new List<Transform>();
    public int nowbh;
    private void Start() {
        foreach (Transform ss in transform) {
            condition.Add(ss);
            ss.gameObject.SetActive(false);
        }
        int bh = int.Parse(transform.parent.parent.parent.parent.parent.gameObject.name);
        nowbh = 2 * bh + 1;
        condition[nowbh].gameObject.SetActive(true);
        
    }
}
