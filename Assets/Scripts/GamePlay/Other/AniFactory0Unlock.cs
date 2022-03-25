using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniFactory0Unlock : MonoBehaviour
{
    Factory0Control factory0Control;
    private void Start() {
        factory0Control = transform.parent.gameObject.GetComponent<Factory0Control>();
    }



    private void SetUseActive() {
        factory0Control.SetView();
    }


}
