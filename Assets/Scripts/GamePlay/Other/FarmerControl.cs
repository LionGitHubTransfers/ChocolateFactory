using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerControl : MonoBehaviour {
    Animator animator;
    Transform Box;
    List<Transform> CocoaSum = new List<Transform>();
    int bh;
    public bool hasBox = false;

    NavMeshAgent m_Agent;
    public AttrackControl_Farmer attrackControl_Farmer;
    public FarmerCondition farmerCondition;
    float nowTime = 0f;
    float[] ConditionTime = new float[] {0.5f, 10f, 1f, 1f, 15f};
    float behaviourSpeed = 1f;
    bool IsBoxEmpty = false;
    Vector3 machine = new Vector3(2f, 0f, -17f);
    ParticleSystem fxhit;
    public void OnStart(int _bh) {
        bh = _bh;
        m_Agent = GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        fxhit = gameObject.GetChildControl<ParticleSystem>("hitFX");
        Box = gameObject.GetChildControl<Transform>("Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/woodBacket");
        foreach (Transform ss in Box.GetChild(0)) {
            CocoaSum.Add(ss);
            ss.gameObject.SetActive(false);
        }
        Box.gameObject.SetActive(false);
        attrackControl_Farmer = gameObject.GetChildControl<AttrackControl_Farmer>("AttrackPos");
        farmerCondition = FarmerCondition.None;
    }

    private void CocoaSumActive(bool active) {
        foreach (Transform ss in CocoaSum) {
            ss.gameObject.SetActive(active);     
        }
    }
    private bool GetTreePos() {
        Transform treeTran = GardenMgr.Instance.GetMoreCocoasTree(bh);
        if (treeTran == null) {
            return false;
        }
        Vector3 treePos = treeTran.position + new Vector3(1.5f, 0f, 0f);
        m_Agent.SetDestination(treePos);
        return true;
    }
    private bool GetBoxPos() {
        Transform BoxTran = GardenMgr.Instance.GetMoreCocoasBox(bh);
        if (BoxTran == null) {
            return false;
        }
        Vector3 boxPos = BoxTran.position;
        m_Agent.SetDestination(boxPos);
        return true;
    }
    private bool GetMachinePos() {
        if (hasBox == false) {
            return false;
        }
        m_Agent.SetDestination(machine);
        return true;
    }

    private bool GetReturnBox() {
        if (hasBox) {
            return false;
        }
        return true;
    }
    public void NextTime() {
        nowTime += 10f;
    }

    private void Update() {
        float nowSpeedRate = m_Agent.velocity.magnitude / m_Agent.speed;
        if (nowSpeedRate > 0.1f) {
            attrackControl_Farmer.IsMove = true;
        }
        else {
            attrackControl_Farmer.IsMove = false;
        }
        animator.SetFloat("speed", nowSpeedRate);
        if (farmerCondition == FarmerCondition.None) {
            if (nowTime < ConditionTime[((int)farmerCondition)]) {
                nowTime += behaviourSpeed * Time.deltaTime;
            }
            else {
                if (GetTreePos()) {
                    farmerCondition = FarmerCondition.AttrackTree;
                    nowTime = 0f;

                }
                if (GetBoxPos()) {
                    farmerCondition = FarmerCondition.GetBox;
                    nowTime = 0f;
                }
            }
        }
        else if (farmerCondition == FarmerCondition.AttrackTree) {
            if (nowTime < ConditionTime[(int)farmerCondition]) {
                nowTime += behaviourSpeed * Time.deltaTime;
            }
            else {
                if (GetBoxPos()) {
                    farmerCondition = FarmerCondition.GetBox;
                    nowTime = 0f;
                }
                else {
                    farmerCondition = FarmerCondition.None;
                    nowTime = 0f;
                }
            }
        }
        else if (farmerCondition == FarmerCondition.GetBox) {
            if (nowTime < ConditionTime[(int)farmerCondition]) {
                nowTime += behaviourSpeed * Time.deltaTime;
            }
            else {
                if (GetMachinePos()) {
                    farmerCondition = FarmerCondition.MoveBox;
                    nowTime = 0f;
                }
                else {
                    farmerCondition = FarmerCondition.None;
                    nowTime = 0f;
                }
            }
        }
        else if (farmerCondition == FarmerCondition.MoveBox) {
            if (nowTime < ConditionTime[(int)farmerCondition]) {
                nowTime += behaviourSpeed * Time.deltaTime;
            }
            else {
                if (GetReturnBox()) {
                    int sleepRate = Random.Range(1, 10);
                    if (sleepRate == 1) {
                        farmerCondition = FarmerCondition.Sleep;
                        AdddizzyFx();
                    }
                    else {
                        farmerCondition = FarmerCondition.None;
                    }

                    nowTime = 0f;
                }
                else {
                    farmerCondition = FarmerCondition.MoveBox;
                }
            }
        }
        else if (farmerCondition == FarmerCondition.Sleep) {
            if (nowTime < ConditionTime[(int)farmerCondition]) {
                nowTime += behaviourSpeed * Time.deltaTime;
            }
            else {
                
                
                RemovedizzyFx();
                
            }
        }
    }
    Transform FxDizzy;
    private void AdddizzyFx() {
        animator.SetBool("dizzy", true);
        fxhit.Stop();
        FxDizzy = ObjectPool.Instance.Get("Fx", "SleepFX", transform).transform;
        ParticleSystem particleSystem = FxDizzy.gameObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
        FxDizzy.localPosition = new Vector3(0f, 0f, 0f);
    }
    public void RemovedizzyFx() {
        animator.SetBool("dizzy", false);
        fxhit.Play();
        ObjectPool.Instance.Recycle(FxDizzy.gameObject, true);
        farmerCondition = FarmerCondition.None;
        nowTime = 0f;
    }

    public void AniAttackCocoaTree() {
        animator.SetTrigger("getCocoa");
    }
    private void AniSetLayerWeight(float num) {
        int bh = animator.GetLayerIndex("upper");
        animator.SetLayerWeight(bh, num);
    }
    private void JudgeAttrackTree() {
        attrackControl_Farmer.AttackCocoaTree();

    }

    public void AddBox(Vector3 startPos) {
        hasBox = true;
        Transform ObjectGo = ObjectPool.Instance.Get("garden", "woodBacket", transform).transform;
        ObjectGo.transform.position = startPos;
        Vector3 StartPos = ObjectGo.localPosition;
        Vector3 EndPos = Box.localPosition;
        ObjectGo.DOLocalRotate(Box.localEulerAngles, 0.4f);
        Tween tween = DOTween.To(setter: value => {
            ObjectGo.localPosition = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f);
        tween.OnComplete(() => RecycleBox(ObjectGo));
    }
    private void RecycleBox(Transform ObjectGo) {
        ObjectGo.localEulerAngles = Vector3.one;
        ObjectPool.Instance.Recycle(ObjectGo.gameObject);
        Box.gameObject.SetActive(true);
        CocoaSumActive(true);
        animator.SetBool("Carrying", true);
        IsBoxEmpty = false;
        if (farmerCondition == FarmerCondition.GetBox) {
            nowTime = ConditionTime[(int)farmerCondition];
        }
        
    }
    public void ReturnBox() {
        Box.gameObject.SetActive(false);
        animator.SetBool("Carrying", false);
    }

    public void UseBox(Vector3 EndPos) {
        if (IsBoxEmpty) {
            return;
        }
        foreach (Transform ss in CocoaSum) {
            Transform ObjectGo = ObjectPool.Instance.Get("garden", "cocoa").transform;
            ObjectGo.localEulerAngles = ss.localEulerAngles;
            ObjectGo.localScale = ss.localScale;
            Vector3 StartPos = ss.position;
            Tween tween = DOTween.To(setter: value => {
                ObjectGo.position = EMath.Parabola(StartPos, EndPos, 1f, value);
            }, startValue: 0, endValue: 1, duration: 0.5f);
            tween.OnComplete(() => {
                ObjectGo.localEulerAngles = Vector3.zero;
                ObjectGo.localScale = Vector3.one;
                ObjectPool.Instance.Recycle(ObjectGo.gameObject);
            });
            ss.gameObject.SetActive(false);
        }
        IsBoxEmpty = true;
        hasBox = false;
        ReturnBox();
    }
    public void AddFxLv(float waitTime) {
        Invoke("AddFxLvTrue", waitTime);
    }

    private void AddFxLvTrue() {
        Transform ObjectGo = ObjectPool.Instance.Get("Fx", "levelUp", transform).transform;
        ObjectGo.localPosition = Vector3.zero;
        RecycleFx recycleFx = ObjectGo.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(2f);
    }
}
public enum FarmerCondition {
    None,
    AttrackTree,
    GetBox,
    MoveBox,
    Sleep,
}
