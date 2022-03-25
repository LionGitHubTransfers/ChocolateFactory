using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGoodsControl : MonoBehaviour
{
    Transform ChocolateRoot;
    Transform Model;
    Transform Sign;
    public DisplayGoodsCondition displayGoodsCondition;
    public Transform CustomerPos;
    public Transform PlayerPos;
    public Transform StandSign;
    public int bh;
    public DisplayGoodsRoot displayGoodsRoot; 
    public void Start() {
        ChocolateRoot = gameObject.GetChildControl<Transform>("chocolateRoot");
        CustomerPos = gameObject.GetChildControl<Transform>("CustomerPos");
        PlayerPos = gameObject.GetChildControl<Transform>("PlayerPos");
        StandSign = gameObject.GetChildControl<Transform>("StandSign");
        Model = gameObject.GetChildControl<Transform>("model");
        Sign = gameObject.GetChildControl<Transform>("sign");
        displayGoodsRoot = gameObject.GetChildControl<DisplayGoodsRoot>("chocolateRoot");
        SetView();
    }

    public void SetView() {
        if (displayGoodsCondition == DisplayGoodsCondition.None) {
            ChocolateRoot.gameObject.SetActive(false);
            Model.gameObject.SetActive(false);
            Sign.gameObject.SetActive(false);
            StandSign.gameObject.SetActive(false);
        }
        else if (displayGoodsCondition == DisplayGoodsCondition.Lock) {
            ChocolateRoot.gameObject.SetActive(false);
            Model.gameObject.SetActive(false);
            Sign.gameObject.SetActive(true);
            StandSign.gameObject.SetActive(true);
        }
        else {
            ChocolateRoot.gameObject.SetActive(true);
            Model.gameObject.SetActive(true);
            Sign.gameObject.SetActive(false);
            StandSign.gameObject.SetActive(true);
        }
        
    }
}

public enum DisplayGoodsCondition {
    None,
    Lock,
    UnLock,
}
