using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战场管理类，管理战场的进行逻辑
/// </summary>
public class BattleMgr : Singleton<BattleMgr> {

    public BattleState state = BattleState.Wait;
    Transform MainRoad;
    public Transform CustomerPosStart;
    public Transform CustomerPosEnd;
    public void Init() {
        if (MainRoad == null) {
           MainRoad = new GameObject("MainRoad").transform;
        }
        InitMainRoad();
        InitMsg();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }

    public void StartBattle() {
        PlayerMgr.Instance.StartBattle();
        state = BattleState.Game;
    }
    private void InitMainRoad() {
        ObjectPool.Instance.Get("MainRoad", "BG", MainRoad);
        Transform floor = ObjectPool.Instance.Get("MainRoad", "floor", MainRoad).transform;
        CustomerPosStart = floor.gameObject.GetChildControl<Transform>("CustomerPosStart");
        CustomerPosEnd = floor.gameObject.GetChildControl<Transform>("CustomerPosEnd");
        //ObjectPool.Instance.Get("MainRoad", "wall", MainRoad);
        ObjectPool.Instance.Get("MainRoad", "door", MainRoad);
    }

}

public enum BattleState {
    Wait,
    Game,
    Pause,
    WaitRevive,
    GameOver,
}