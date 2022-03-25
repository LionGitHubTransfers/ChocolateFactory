using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory0Control : MonoBehaviour
{
    Transform FxCocoaSmoke;
    Transform FxCocoa;
    
    Animator AniMachine;
    Transform cocoaEnd;

    int cocoaNumMax = 100;
    private const string COCOANUM = "COCOANUM";
    private int m_cocoaNum;
    public int cocoaNum {
        get {
            return m_cocoaNum;
        }
        set {
            int changeValue = value - m_cocoaNum;
            m_cocoaNum = value;
            LocalSave.SetInt(COCOANUM, value);
        }
    }
    Text cocoaText;
    float nowTime = 1f;
    float maxTime = 1f;
    float speedTime = 1f;
    public MachineCondition machineCondition;
    bool CanLongChocolate = false;
    int LongChocolateTimes = 2;
    int LongChocolateTimesMax = 2;
    float nowTime2 = 1.5f;
    float maxTime2 = 1.5f;
    float speedTime2 = 1f;
    Transform UnlockAnim;
    Transform UnlockMachine;
    Transform LockMachine;
    float speedRate = 1f;
    RoasterTrigger roasterTrigger;
    int ShutNowNum = 1;
    public void OnStart() {
        if (FactoryMgr.Instance.FactoryLv >= 0) {
            machineCondition = MachineCondition.Unlock;
        }
        else {
            machineCondition = MachineCondition.Lock;
        }
        m_cocoaNum = LocalSave.GetInt(COCOANUM, 0);
        //m_cocoaNum = 10;
        UnlockMachine = gameObject.GetChildControl<Transform>("UnlockMachine");
        LockMachine = gameObject.GetChildControl<Transform>("LockMachine");
        FxCocoaSmoke = UnlockMachine.gameObject.GetChildControl<Transform>("disintegrator/Smoke");
        FxCocoaSmoke.gameObject.SetActive(false);
        FxCocoa = UnlockMachine.gameObject.GetChildControl<Transform>("disintegrator/mech_1_bone/Mech_1_root/tube_root/tube_6/cocoa");
        FxCocoa.gameObject.SetActive(false);
        

        AniMachine = UnlockMachine.gameObject.GetChildControl<Animator>("disintegrator/mech_1_bone");
        AniMachine.SetBool("Working", false);
        cocoaEnd = UnlockMachine.gameObject.GetChildControl<Transform>("cocoaEnd");
        cocoaText = UnlockMachine.gameObject.GetChildControl<Text>("disintegrator/sign/num");
        UnlockAnim = gameObject.GetChildControl<Transform>("UnlockAnim");
        roasterTrigger = gameObject.GetChildControl<RoasterTrigger>("UnlockMachine/roaster/roaster");
        SetView();
    }
    public void SetView() {
        if (FactoryMgr.Instance.FactoryLv >= 0) {
            machineCondition = MachineCondition.Unlock;
        }
        else {
            machineCondition = MachineCondition.Lock;
        }
        if (machineCondition == MachineCondition.Lock) {
            LockMachine.gameObject.SetActive(true);
            UnlockMachine.gameObject.SetActive(false);
        }
        else{
            LockMachine.gameObject.SetActive(false);
            UnlockMachine.gameObject.SetActive(true);
        }
        UnlockAnim.gameObject.SetActive(false);
        ReSetUI();
    }
    public void UnLockDemo() {
        UnlockAnim.gameObject.SetActive(true);
    }

    public void OnTriggerEnterUnLock(Collider other) {
        if (other.gameObject.layer.Equals(3)) {
            if (other.gameObject.tag == "Player") {
                PlayerControl playerControl = other.gameObject.GetComponent<PlayerControl>();
                if (playerControl.playerCondition != PlayerCondition.CarryCocoa) {
                    if (playerControl.playerCondition == PlayerCondition.CarryFlyCocoa) {
                        playerControl.flybox.Complete();
                        playerControl.UseBox(cocoaEnd.position);
                        AddCocoaNum(playerControl.cocoaNum);
                    }
                    return;
                }
                else {
                    playerControl.UseBox(cocoaEnd.position);
                    AddCocoaNum(playerControl.cocoaNum);
                }
            }
        }
        else if (other.gameObject.layer.Equals(7)) {
            if (other.gameObject.tag == "Player") {
                FarmerControl farmerControl = other.gameObject.GetComponent<FarmerControl>();
                if (farmerControl.farmerCondition != FarmerCondition.MoveBox) {
                    return;
                }
                else {
                    farmerControl.UseBox(cocoaEnd.position);
                    AddCocoaNum(10);
                }
            }
        }
    }
    private void AddCocoaNum(int _cocoaNum) {
        cocoaNum += _cocoaNum;
        if (cocoaNum > cocoaNumMax) {
            cocoaNum = cocoaNumMax;
        }
        ReSetUI();
    }

    public void OnTriggerExitUnlock(Collider other) {
        if (other.gameObject.layer.Equals(3)) {
 
        }
        else if (other.gameObject.layer.Equals(7)) {

        }
    }
    private void ReSetUI() {
        if (machineCondition == MachineCondition.Lock) { 
            return;
        }
        cocoaText.text = cocoaNum.ToString();
        JudgeMachineWorkingCondition();
    }
    private void JudgeMachineWorkingCondition() {
        if (cocoaNum > 0) {
            StartMachine();
        }
        else {
            StopMachine();
        }
    }

    private void Update() {
        speedRate = 1f + roasterTrigger.GetSpeedRate();
        if (machineCondition == MachineCondition.StartDecrease) {
            if (nowTime < maxTime) {
                nowTime += speedRate * Time.deltaTime;
            }
            else {
                OnceMachine();
                nowTime = 0f;
            }
        }
        if (CanLongChocolate) {
            if (nowTime2 < maxTime2) {
                nowTime2 += speedRate * Time.deltaTime;
            }
            else {
                OnceMachineLongChocolate();
                nowTime2 = 0f;
            }
        }

    }


    private void StartMachine() {
        machineCondition = MachineCondition.StartDecrease;
    }
    private void OnceMachine() {
        
        AniMachine.SetBool("Working", true);
        FxCocoaSmoke.gameObject.SetActive(true);
        DecreaseCococa();
        LongChocolateTimes += 1;
        if (LongChocolateTimes > LongChocolateTimesMax) {
            LongChocolateTimes = LongChocolateTimesMax;
            CanLongChocolate = true;
        }
    }
    private void OnceMachineLongChocolate() {
        LongChocolateTimes -= 1;
        if (LongChocolateTimes <= 0) {
            CanLongChocolate = false;
            nowTime2 = 0f;
            LongChocolateTimes = 0;
            FxCocoa.gameObject.SetActive(false);
        }
        else {

            FxCocoa.gameObject.SetActive(true);
            CreateLongChocolate();
        }
    }
    Vector3 InitPos = new Vector3(6.6f, 0.4f, -17.17f);
    private void CreateLongChocolate() {
        Transform ObjectGo = ObjectPool.Instance.Get("Factory", "FollowObj").transform;
        ObjectGo.position = InitPos;
        SplineFollower splineFollower = ObjectGo.gameObject.GetComponent<SplineFollower>();
        splineFollower.spline = ConveyorMgr.Instance.SplineSum[0];
        splineFollower.SetDistance(0f);
        splineFollower.followSpeed = speedRate;
        

        Transform ObjectChocolate = ObjectPool.Instance.Get("Factory", "Chocolate").transform;
        ObjectChocolate.position = InitPos;
        ChocolateControl chocolateControl = ObjectChocolate.gameObject.GetComponent<ChocolateControl>();
        chocolateControl.OnStart(ObjectGo);
        chocolateControl.ShuntNum = ShutNowNum;
        ShutNowNum++;
        if (ShutNowNum > FactoryMgr.Instance.FactoryLv) {

            ShutNowNum = 1;
        }
    }

    private void DecreaseCococa() {
        cocoaNum -= 1;
        if (cocoaNum < 0) {
            cocoaNum = 0;
        }
        ReSetUI();
    }

    private void StopMachine() {
        nowTime = 0;
        machineCondition = MachineCondition.Unlock;
        AniMachine.SetBool("Working", false);
        FxCocoaSmoke.gameObject.SetActive(false);
    }
}

public enum MachineCondition {
    Lock,
    Unlock,
    StartDecrease,
}