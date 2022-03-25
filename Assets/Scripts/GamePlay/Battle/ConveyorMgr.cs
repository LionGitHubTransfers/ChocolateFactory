using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMgr : Singleton<ConveyorMgr> {

    Transform ConveyorF;
    Transform Conveyor;

    public List<SplineComputer> SplineSum = new List<SplineComputer>();
    Vector3 InitPos = new Vector3(10.48f, 0.3544f, -17.17f);
    public void Init() {
        if (ConveyorF == null) {
            ConveyorF = new GameObject("ConveyorF").transform;
        }
        InitConveyor();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }

    private void InitConveyor() {
        Conveyor = ObjectPool.Instance.Get("Factory", "conveyor",ConveyorF).transform;
        Conveyor.position = InitPos;
        foreach (Transform ss in Conveyor) {
            SplineSum.Add(ss.gameObject.GetComponent<SplineComputer>());
        }
    }
}

