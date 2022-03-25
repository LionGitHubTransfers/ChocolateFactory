using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int cocoaNum = 0;
    Transform axe;
    SkinnedMeshRenderer skinnedMeshRenderer;
    DiaControl diaControl;
    Animator animator;
    Transform AttrackPos;
    public AttrackControl attrackControl;

    Transform Box;
    List<Transform> cocoaSum = new List<Transform>();

    public PlayerCondition playerCondition = PlayerCondition.CutTree;

    public Transform ChocolateSum;
    public List<Transform> ChocolateEnds = new List<Transform>();
    List<ChocolateEndControl> chocolateEndControls = new List<ChocolateEndControl>();
    List<int> chocoNum = new List<int>();
    private int m_nowChocolateNum = 0;
    public int nowChocolateNum {
        get {
            return m_nowChocolateNum;
        }
        set {
            if (m_nowChocolateNum == value) {
                return;
            }
            m_nowChocolateNum = value;
            int[] Chocolatebh = new int[] { 0, 0, 0, 0, 0};
            for (int i = 0; i < value; i++) {
                Chocolatebh[i] = chocolateEndControls[i].bh / 2;
            }
            Send.SendMsg(SendType.ChocolateNumChange, Chocolatebh[0], Chocolatebh[1], Chocolatebh[2], Chocolatebh[3], Chocolatebh[4]);
        }
    }
    public Tween flybox;
    private void Start() {
        axe = gameObject.GetChildControl<Transform>("Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/axe");
        AxeActive(true);
        skinnedMeshRenderer = gameObject.GetChildControl<SkinnedMeshRenderer>("Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/bag");
        diaControl = gameObject.GetChildControl<DiaControl>("dialog_root");
        animator = gameObject.GetComponent<Animator>();
        AttrackPos = gameObject.GetChildControl<Transform>("AttrackPos");
        attrackControl = AttrackPos.gameObject.GetComponent<AttrackControl>();
        Box = gameObject.GetChildControl<Transform>("BoxSum/woodBacket");
        Box.gameObject.SetActive(false);
        ChocolateSum = gameObject.GetChildControl<Transform>("BoxSum/ChocolateSum");
        foreach (Transform ss in ChocolateSum) {
            ChocolateEnds.Add(ss);
            ss.gameObject.SetActive(false);
            chocolateEndControls.Add(ss.gameObject.GetComponent<ChocolateEndControl>());
        }
        foreach (Transform ss in Box.GetChild(0)) {
            cocoaSum.Add(ss);
        }

    }
    public void AxeActive(bool active) {
        axe.gameObject.SetActive(active);
    }
    public bool SearchChocolateMax() {
        if (nowChocolateNum >= ChocolateEnds.Count) {
            return true;
        }
        else {
            return false;
        }
    }
    public void AniAttackCocoaTree() {
        animator.SetTrigger("getCocoa");

    }
    public void AniSlap() {
        animator.SetTrigger("slap");
    }
    private void AniSetLayerWeight(float num) {
        int bh = animator.GetLayerIndex("upper");
        animator.SetLayerWeight(bh, num);
    }
    private void JudgeAttrackTree() {
        if (attrackControl.IsIn) {
            attrackControl.AttackCocoaTree();
        }
    }
    public void StopAttrack() {
        attrackControl.CanAttrack = false;
        attrackControl.StopAttrack();
    }
    private void StartAttrack() {
        attrackControl.CanAttrack = true;
    }
    public void AddBox(Vector3 startPos, int nowNum) {
        Transform ObjectGo = ObjectPool.Instance.Get("garden", "woodBacket", transform).transform;
        ObjectGo.transform.position = startPos;
        WoodBacketControl woodBacketControl = ObjectGo.GetComponent<WoodBacketControl>();
        woodBacketControl.OnStart(nowNum);
        Vector3 StartPos = ObjectGo.localPosition;
        Vector3 EndPos = Box.localPosition;
        ObjectGo.DOLocalRotate(Box.localEulerAngles, 0.4f);
        flybox = DOTween.To(setter: value => {
            ObjectGo.localPosition = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f);
        flybox.OnComplete(()=> RecycleBox(ObjectGo, nowNum));
        playerCondition = PlayerCondition.CarryFlyCocoa;

    }
    private void RecycleBox(Transform ObjectGo, int nowNum) {
        ObjectGo.localEulerAngles = Vector3.one;
        ObjectPool.Instance.Recycle(ObjectGo.gameObject);
        WoodBacketControl woodBacketControl = Box.gameObject.GetComponent<WoodBacketControl>();
        woodBacketControl.OnStart(nowNum);
        cocoaNum = nowNum;
        Box.gameObject.SetActive(true);
        playerCondition = PlayerCondition.CarryCocoa;
        Send.SendMsg(SendType.ChocoaCarryChange, nowNum);
        animator.SetBool("Carrying", true);
        
    }
    public void UseBox(Vector3 EndPos) {
        for (int i = 0; i < cocoaNum; i++) {
            Transform ss = cocoaSum[i];
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
        ReSetCondition0();

    }
    public void ReSetCondition0() {
        if (playerCondition == PlayerCondition.CarryCocoa) {
            Send.SendMsg(SendType.ChocoaCarryChange, 0);
        }
        Box.gameObject.SetActive(false);
        animator.SetBool("Carrying", false);
        playerCondition = PlayerCondition.CutTree;
        AxeActive(true);
        StartAttrack();
    }
    public bool JudgeCarry() {
        if (playerCondition == PlayerCondition.CarryCocoa) {
            attrackControl.CanAttrack = false;
            attrackControl.StopAttrack();
            return true;
        }
        else if (playerCondition == PlayerCondition.CarryChocolate) {
            attrackControl.CanAttrack = false;
            attrackControl.StopAttrack();
            return true;
        }
        else {
            attrackControl.CanAttrack = true;
            return false;
        }
    }
    public void IsCarryChocolate(int bh) {
        if (SearchChocolateMax()) {
            return;
        }
        playerCondition = PlayerCondition.CarryChocolate;
        AxeActive(false);
        animator.SetBool("Carrying", true);
        chocoNum.Add(bh);
        //chocolateEndControls[nowChocolateNum].OnStart(bh);
        
        SetChocolateUI();
        nowChocolateNum++;

    }
    public void RemoveChocolate(int bh) {
        if (nowChocolateNum > 1) {
            chocoNum.Remove(bh);
            SetChocolateUI();
            //ChocolateEnds[nowChocolateNum - 1].gameObject.SetActive(false);
            nowChocolateNum--;
        }
        else if (nowChocolateNum == 1) {
            chocoNum.Remove(bh);
            animator.SetBool("Carrying", false);
            playerCondition = PlayerCondition.CutTree;
            SetChocolateUI();
            //ChocolateEnds[nowChocolateNum - 1].gameObject.SetActive(false);
            nowChocolateNum--;
        }
    }
    public void SetChocolateUI() {
        for (int i = 0; i < 5; i++) {
            if (i < chocoNum.Count) {
                chocolateEndControls[i].OnStart(chocoNum[i]);
            }
            else {
                ChocolateEnds[i].gameObject.SetActive(false);
            }
        }
    }

    public bool GetEndChocolatebh2D(int _bh) {
        foreach (int bh in chocoNum) {
            if (bh == _bh) {
                return true;
            }
        }
        return false;
    }
    
}

public enum PlayerCondition {
    CutTree,
    CarryFlyCocoa,
    CarryCocoa,
    CarryChocolate,
}

