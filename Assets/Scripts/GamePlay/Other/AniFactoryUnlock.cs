using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniFactoryUnlock : MonoBehaviour
{
    FactoryControl factoryControl;
    private void Start() {
        factoryControl = transform.parent.gameObject.GetComponent<FactoryControl>();
    }



    private void SetUseActive() {
        factoryControl.SetView();
    }

}
