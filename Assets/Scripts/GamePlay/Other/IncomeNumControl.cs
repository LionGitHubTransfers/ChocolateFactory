using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeNumControl : MonoBehaviour
{
    Animator animator;
    Text text;
    public void OnStart(int nowMoney) {
        animator = gameObject.GetComponent<Animator>();
        text = gameObject.GetComponent<Text>();
        text.text = nowMoney.ToString();
        animator.Play("appear", 0);
        Invoke("RecycleDemo", 1f);
    }
    private void RecycleDemo() {
        ObjectPool.Instance.Recycle(gameObject, true);
    }
}
