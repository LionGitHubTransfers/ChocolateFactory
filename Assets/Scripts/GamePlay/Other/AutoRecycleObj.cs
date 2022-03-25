using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecycleObj : MonoBehaviour
{
    public int bh;
    public float Time;
    public void OnStart() {
        Invoke("RecThis", Time);
    }

    private void RecThis() {
        ObjectPool.Instance.Recycle(gameObject);
        BattleWindow.Instance.tip[bh] = null;
    }
}
