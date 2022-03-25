using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cocoaControl : MonoBehaviour
{
    Transform cocoaTree;
    Animator animator;
    Transform Unlock;
    int num;
    public bool isUnlock = false;
    Text text;

    int money;
    List<Transform> cocoaSum = new List<Transform>();
    
    public int nowCocoaNum = 0;
    int CocoaNumMax = 10;
    bool isCocoaMax = false;
    float CreateCocoaStart = 0f;
    float CreateCocoaEnd = 2f;
    float CreateCocoaOnce = 1.5f;


    List<Transform> BoxCocoaSum = new List<Transform>();
    public int nowBoxNumTrue = 0;
    public int nowBoxNum = 0;
    int BoxNumMax = 10;
    public Transform Box;

    public bool isFull = false;

    List<Transform> nowInTreeAttrack = new List<Transform>();
    public List<Transform> nowInTreePlayer = new List<Transform>();
    public int bh;

    BoxControl boxControl;
    Text cocoatxt;
    public bool isMoveCocoa = false;
    List<Transform> flycocoa = new List<Transform>();
    List<Tween> flyTween = new List<Tween>();
    public void OnStart(bool active, int _money, int _bh) {
        money = _money;
        bh = _bh;
        text = gameObject.GetChildControl<Text>("Unlock/num");
        Box = gameObject.GetChildControl<Transform>("woodBacket/cocoa");
        foreach (Transform ss in Box) {
            BoxCocoaSum.Add(ss);
            ss.gameObject.SetActive(false);
        }
        SetUnlockUI();
        cocoaTree = transform.GetChild(0);
        Unlock = transform.GetChild(1);
        animator = cocoaTree.gameObject.GetComponent<Animator>();
        cocoatxt = gameObject.GetChildControl<Text>("woodBacket/sign/num");
        boxControl = gameObject.GetChildControl<BoxControl>("woodBacket");
        foreach (Transform ss in cocoaTree) {
            cocoaSum.Add(ss);
            ss.gameObject.SetActive(false);
        }
        ResetCocoaUI();
        
        isUnlock = active;
        Init(active);
    }
    private void Init(bool active) {
        cocoaTree.gameObject.SetActive(active);
        Unlock.gameObject.SetActive(!active);
        Box.gameObject.SetActive(active);
    }
    private void ResetCocoaUI() {
        if (nowBoxNumTrue >= BoxNumMax) {
            cocoatxt.text = "Max";
        }
        else {
            cocoatxt.text = nowBoxNum.ToString();
        }
        
    }
    private void SetUnlockUI() {
        int thenmoney = int.Parse(text.text);
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, money, 0.5f).OnUpdate(()=> {
            text.text = thenmoney.ToString();
        });
    }
    private void SetUnlockUI0() {
        int thenmoney = int.Parse(text.text);
        Tween tween = DOTween.To(() => thenmoney, x => thenmoney = x, 0, 0.5f).OnUpdate(() => {
            text.text = thenmoney.ToString();
        });
    }
    public void OnTriggerEnterDemo(Collider other) {
        if (other.gameObject.layer.Equals(3)) {
            if (other.gameObject.tag == "Attrack") {
                nowInTreeAttrack.Add(other.gameObject.transform);
            }
            else if (other.gameObject.tag == "Player") {
                nowInTreePlayer.Add(other.gameObject.transform);
                if (isUnlock == false) {
                    InvokeRepeating("DecreaseGold", 0.25f, 0.25f);
                }
            }
        }
    }
    public void OnTriggerExitDemo(Collider other) {
        if (other.gameObject.layer.Equals(3)) {

            if (other.gameObject.tag == "Attrack") {
                nowInTreeAttrack.Remove(other.gameObject.transform);
            }
            else if (other.gameObject.tag == "Player") {
                nowInTreePlayer.Remove(other.gameObject.transform);
                if (isUnlock == false) {
                    CancelInvoke("DecreaseGold");
                }
                if (nowBoxNum > 0) {
                    PlayerControl playerControl = PlayerMgr.Instance.Player.gameObject.GetComponent<PlayerControl>();
                    if (playerControl.JudgeCarry()) {
                        return;
                    }
                    playerControl.StopAttrack();
                    playerControl.AddBox(transform.position, nowBoxNumTrue);
                    playerControl.AxeActive(false);
                    BoxClear(nowBoxNum);
                }
            }
        }
        else if (other.gameObject.layer.Equals(7)) {
            nowInTreePlayer.Remove(other.gameObject.transform);
        }
    }
    private void DecreaseGold() {
        if (PlayerMgr.Instance.playerMoveCondition == PlayerMoveCondition.Move) {
            return;
        }
        int nowGold = CurrencyMgr.Instance.Gold;
        int OnceMoney = money;
        if (nowGold != 0 && OnceMoney != 0) {
            PlayerMgr.Instance.ShowDecreaseGold(transform.position);
        }
        if (nowGold - OnceMoney >= 0) {
            CurrencyMgr.Instance.Gold -= money;
            money = 0;
            SetUnlockUI0();
            GardenMgr.Instance.LockMoney = money;
            isUnlock = true;
            CancelInvoke("DecreaseGold");
            cocoaTree.localScale = Vector3.one * 0.1f;
            Init(true);
            animator.SetBool("IsCreate", true);
            GardenMgr.Instance.AddCocoa();
            foreach (Transform ss in nowInTreeAttrack) {
                AttrackControl attrackControl = ss.gameObject.GetComponent<AttrackControl>();
                attrackControl?.OnTriggerEnterDemo(this);
            }
            SetUnlockUI0();
        }
        else {
            if (nowGold == 0) {
                return;
            }
            CurrencyMgr.Instance.Gold = 0;
            money -= nowGold;
            GardenMgr.Instance.LockMoney = money;
            SetUnlockUI();
        }
    }
    
    private void Update() {
        if (isCocoaMax == false && isUnlock) {
            if (CreateCocoaStart < CreateCocoaEnd) {
                CreateCocoaStart += CreateCocoaOnce * Time.deltaTime;
            }
            else {
                cocoaSum[nowCocoaNum].gameObject.SetActive(true);
                nowCocoaNum++;
                if (nowCocoaNum >= CocoaNumMax) {
                    isCocoaMax = true;
                }
                CreateCocoaStart = 0;
            }
        }
    }
    public void DecreaseCocoa(Vector3 AttrackPos) {
        if (nowCocoaNum - 1 < 0) {
            return;
        }
        Vector3 AttrackDir = -(AttrackPos - transform.position).normalized * 15f;
        Tween tween = cocoaTree.DORotate(AttrackDir, 0.15f);
        tween.OnComplete(()=> {
            cocoaTree.DORotate(Vector3.zero, 0.2f);
        });
        if (isFull) {
            return;
        }
        int bh = nowCocoaNum - 1;
        cocoaSum[bh].gameObject.SetActive(false);
        nowCocoaNum--;
        isCocoaMax = false;
        Transform ObjectGo = ObjectPool.Instance.Get("garden", "cocoa").transform;
        ObjectGo.transform.position = cocoaSum[bh].position;
        flycocoa.Add(ObjectGo);
        MoveToBox(ObjectGo.transform);
    }
    private void MoveToBox(Transform ObjectGo) {
        if (isFull || nowBoxNumTrue < 0 || nowBoxNumTrue >= 10) {
            return;
        }
        Vector3 StartPos = ObjectGo.position;
        Vector3 EndPos = BoxCocoaSum[nowBoxNumTrue].position;
        Tween tween = DOTween.To(setter: value => {
            ObjectGo.position = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f);
        tween.OnComplete(()=> MoveBoxEnd(ObjectGo.transform, tween));
        flyTween.Add(tween);
        ObjectGo.DOScale(0.65f, 0.4f);
        ObjectGo.DORotate(BoxCocoaSum[nowBoxNumTrue].localEulerAngles, 0.4f);
        isMoveCocoa = true;
        nowBoxNumTrue++;

        if (nowBoxNumTrue >= BoxNumMax) {
            isFull = true;
        }
    }

    private void MoveBoxEnd(Transform ObjectGo, Tween tween) {
        ObjectGo.localEulerAngles = Vector3.zero;
        ObjectGo.localScale = Vector3.one;
        ObjectPool.Instance.Recycle(ObjectGo.gameObject);
        flyTween.Remove(tween);
        flycocoa.Remove(ObjectGo);
        BoxCocoaSum[nowBoxNum].gameObject.SetActive(true);
        isMoveCocoa = false;
        if (nowBoxNum + 1 > 0) {
            BoxDisEmpty(nowBoxNum + 1);
        }
        if (nowBoxNum >= BoxNumMax - 1) {
            BoxFull();
        }
        nowBoxNum++;
        ResetCocoaUI();
    }
    private void BoxFull() {
        boxControl.isFull = true;
        boxControl.isEmpty = false;
        boxControl.BoxMoveOnce = true;
    }
    private void BoxEmpty() {
        isFull = false;
        boxControl.isFull = false;
        boxControl.isEmpty = true;
        boxControl.BoxMoveOnce = false;
        boxControl.nowNum = 0;
    }
    private void BoxDisEmpty(int nownum) {
        boxControl.isEmpty = false;
        boxControl.BoxMoveOnce = true;
        boxControl.nowNum = nownum;
    }
    public void BoxClear(int nowNum) {
        List<Tween> flynew = new List<Tween>();
        foreach (Tween ss in flyTween) {
            flynew.Add(ss);
        }
        foreach (Tween ss in flynew) {
            ss.Complete();
        }
        foreach (Transform ss in BoxCocoaSum) {
            ss.gameObject.SetActive(false);
        }
        nowBoxNumTrue = 0;
        nowBoxNum = 0;
        
        BoxEmpty();
        ResetCocoaUI();
    }
    private void BoxClearTo0() {
        foreach (Transform ss in BoxCocoaSum) {
            ss.gameObject.SetActive(false);
        }
        nowBoxNum = 0;
        nowBoxNumTrue = 0;
        BoxEmpty();
        ResetCocoaUI();
    }
    public bool BoxToPlayer(Transform Person, int nowNum) {

        int nowLayer = Person.gameObject.layer;
        FarmerControl farmerControl = Person.gameObject.GetComponent<FarmerControl>();
        if (farmerControl.farmerCondition != FarmerCondition.GetBox) {
            return false;
        }
        farmerControl.AddBox(Box.position);
        BoxClearTo0();
        isFull = false;
        return true;

    }

}
