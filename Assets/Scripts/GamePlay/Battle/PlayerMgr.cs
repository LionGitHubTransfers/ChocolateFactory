using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 玩家控制器 
/// </summary>
public class PlayerMgr : Singleton<PlayerMgr> {

    Transform PlayerF;
    public Transform Player;
    public CharacterController characterController;
    Animator animator;
    Vector3 MouseEnter;
    Vector3 MouseDrag;
    Vector3 nowDirection;
    public Transform MouseControl;
    public RectTransform In;
    public RectTransform Out;
    public bool canMove;
    float speed = 0.2f;
    Transform ArrowRoot;
    public PlayerControl playerControl;
    AttrackControl attrackControl;
    public PlayerMoveCondition playerMoveCondition = PlayerMoveCondition.Idle;
    public float initwidth;
    public float initheight;
    //初始化
    public void Init() {
        canMove = false;
        if (PlayerF == null) {
            PlayerF = new GameObject("PlayerF").transform;
        }
        InitPlayer();
        InitMsg();
    }

    //清除数据
    public void Clear() {
        ClearMsg();
    }

    //注册消息
    public void InitMsg() {
        Send.RegisterMsg(SendType.CtrlUp, OnCtrlUp);
        Send.RegisterMsg(SendType.CtrlDown, OnCtrlDown);
        Send.RegisterMsg(SendType.CtrlDrag, OnCtrlDrag);

    }

    //反注册消息
    public void ClearMsg() {
        Send.UnregisterMsg(SendType.CtrlUp, OnCtrlUp);
        Send.UnregisterMsg(SendType.CtrlDown, OnCtrlDown);
        Send.UnregisterMsg(SendType.CtrlDrag, OnCtrlDrag);


    }
    private void OnCtrlUp(object[] _objs) {
        PointerEventData data = (PointerEventData)_objs[0];
        Vector3 point = data.position;

        nowDirection = Vector3.zero;
        MouseControl.gameObject.SetActive(false);

    }
    private void OnCtrlDown(object[] _objs) {
        PointerEventData data = (PointerEventData)_objs[0];
        Vector3 point = data.position;
        MouseEnter = point;
        MouseControl.gameObject.SetActive(true);
        In.SetPosX(MouseEnter.x / Screen.width * initwidth);
        In.SetPosY(MouseEnter.y / Screen.height * initheight);
    }
    private void OnCtrlDrag(object[] _objs) {
        PointerEventData data = (PointerEventData)_objs[0];
        Vector3 point = data.position;

        MouseDrag = point;
        Vector3 nowDrag = MouseDrag - MouseEnter;
        if (nowDrag.magnitude < 40f) {
            nowDirection = nowDrag;
        }
        else {
            nowDirection = nowDrag.normalized * 40f;
        }
        
        Out.SetPosX(nowDirection.x );
        Out.SetPosY(nowDirection.y );
    }

    //开始游戏时调用，根据需求实现，需要在Battle.StartBattle()中调用
    public void StartBattle() {
        canMove = true;
    }

    //Update函数，根据需求实现，需要在Launch.Update()中调用
    public void OnUpdate() {
        if (canMove == false) {
            return;
        }
        if (ArrowMgr.Instance.IsEnd()) {
        }
        else {
            ArrowMgr.Instance.ArrowBh();
            ArrowRoot.gameObject.SetActive(true);
            int bh = ArrowMgr.Instance.numnow;
            if (ArrowMgr.Instance.IsEnd()) {
                ArrowRoot.gameObject.SetActive(false);
                ArrowMgr.Instance.endnow = true;
            }
            else {
                Transform ArrowNow = ArrowMgr.Instance.ArrowSum[bh];
                if ((ArrowNow.position - ArrowRoot.position).magnitude < 2f) {
                    ArrowRoot.gameObject.SetActive(false);
                }
                else {
                    ArrowRoot.gameObject.SetActive(true);
                    ArrowRoot.LookAt(ArrowMgr.Instance.ArrowSum[bh]);
                }
            }
        }
        
        float moveDirLen = nowDirection.sqrMagnitude;
        float moveDirtrue = moveDirLen / 1600f;
        animator.SetFloat("speed", moveDirtrue);
        if (moveDirtrue > 0.1f) {
            playerMoveCondition = PlayerMoveCondition.Move;
            attrackControl.IsMove = true;
        }
        else {
            playerMoveCondition = PlayerMoveCondition.Idle;
            attrackControl.IsMove = false;
        }
        Vector3 nowPos = new Vector3(nowDirection.x, 0f, nowDirection.y);
        
        characterController.Move(nowPos * speed * Time.deltaTime);
        //Player.position = new Vector3(Player.position.x, 0f, Player.position.z);
        float radians = Mathf.Acos(Vector3.Dot(new Vector3(0f, 1f, 0f), (MouseDrag - MouseEnter).normalized));//--弧度
        float degrees;
        if (MouseDrag.x > MouseEnter.x) {
            degrees = radians * Mathf.Rad2Deg;//--角度
        }
        else {
            degrees = -radians * Mathf.Rad2Deg;//--角度
        }
        Player.localEulerAngles = new Vector3(0f, degrees, 0f);
    }

    private void InitPlayer() {
        Player = ObjectPool.Instance.Get("Player", "Char", PlayerF).transform;
        Player.transform.position = new Vector3(0f, 0f, -21f);
        animator = Player.gameObject.GetComponent<Animator>();
        playerControl = Player.gameObject.GetComponent<PlayerControl>();
        attrackControl = Player.gameObject.GetChildControl<AttrackControl>("AttrackPos");
        characterController = Player.gameObject.GetComponent<CharacterController>();
        ArrowRoot = Player.gameObject.GetChildControl<Transform>("GuideArrowRoot");
        ArrowRoot.gameObject.SetActive(false);
    }


    public void ShowDecreaseGold(Vector3 EndPos) {
        Vector3 StartPos = Player.position + new Vector3(0f, 0.7f, 0f);
        Transform GoldObject = ObjectPool.Instance.Get("MainRoad", "money").transform;
        Transform GoldObject1 = ObjectPool.Instance.Get("MainRoad", "money").transform;
        Transform GoldObject2 = ObjectPool.Instance.Get("MainRoad", "money").transform;
        Transform GoldObject3 = ObjectPool.Instance.Get("MainRoad", "money").transform;
        GoldObject.position = StartPos;
        GoldObject1.position = StartPos;
        GoldObject2.position = StartPos;
        GoldObject3.position = StartPos;
        Tween tween = GoldObject.DOMove(EndPos, 0.5f);
        tween.OnComplete(()=>RecycleGold(GoldObject));
        Tween tween1 = GoldObject1.DOMove(EndPos, 0.7f);
        tween1.OnComplete(() => RecycleGold(GoldObject1));
        Tween tween2 = GoldObject2.DOMove(EndPos, 0.9f);
        tween2.OnComplete(() => RecycleGold(GoldObject2));
        Tween tween3 = GoldObject3.DOMove(EndPos, 1.2f);
        tween3.OnComplete(() => RecycleGold(GoldObject3));
    }

    private void RecycleGold(Transform _object) {
        ObjectPool.Instance.Recycle(_object.gameObject);
    }



}

public enum PlayerMoveCondition {
    Idle,
    Move,
}