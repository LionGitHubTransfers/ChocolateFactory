using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corner1Control : MonoBehaviour
{
    public int bh;
    Transform corner1;
    Transform corner2;
    private void Start() {
        bh = int.Parse(transform.parent.parent.parent.gameObject.name);
        corner1 = gameObject.GetChildControl<Transform>("mech_corner_1");
        corner2 = gameObject.GetChildControl<Transform>("mech_corner_2");
        ReSetUI();
        InitMsg();
    }

    private void InitMsg() {
        Send.RegisterMsg(SendType.FactoryLvChange, ReSetUI);
    }

    bool shunt = false;
    private void ReSetUI(object[] _objs) {
        if (FactoryMgr.Instance.FactoryLv == bh) {
            corner1.gameObject.SetActive(true);
            corner2.gameObject.SetActive(false);
        }
        else {
            corner1.gameObject.SetActive(false);
            corner2.gameObject.SetActive(true);
        }
    }
    public void ReSetUI() {
        if (FactoryMgr.Instance.FactoryLv == bh) {
            corner1.gameObject.SetActive(true);
            corner2.gameObject.SetActive(false);
        }
        else {
            corner1.gameObject.SetActive(false);
            corner2.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "chocolate") {
            ChocolateControl chocolateControl = other.gameObject.GetComponent<ChocolateControl>();
            int ShuntNum = chocolateControl.ShuntNum;
            if (ShuntNum == bh) {
                shunt = true;
            }
            else {
                shunt = false;
            }
            if (FactoryMgr.Instance.FactoryLv > bh) {
                if (shunt) {
                    chocolateControl.ChangRotate(false);
                    chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh]);
                }
                else {
                    if (bh == 5) {
                        chocolateControl.ChangRotate(false);
                        chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh]);
                        shunt = !shunt;
                    }
                    else {
                        chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh + 1]);
                        shunt = !shunt;
                    }
                }
            }
            else if (FactoryMgr.Instance.FactoryLv > bh - 1) {
                chocolateControl.ChangRotate(false);
                chocolateControl.ChangeSpline(ConveyorMgr.Instance.SplineSum[2 * bh]);
            }
            else {
                chocolateControl.RecObj();
            }
        }
    }
}
