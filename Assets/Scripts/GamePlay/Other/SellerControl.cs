using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SellerControl : MonoBehaviour
{
    Animator animator;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
    }

}

