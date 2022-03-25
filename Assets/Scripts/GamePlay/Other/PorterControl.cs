using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PorterControl : MonoBehaviour
{

    Animator animator;
    NavMeshAgent m_Agent;
    public PorterCondition porterCondition = PorterCondition.Chocolate;
    public PorterCarryCondition porterCarryCondition = PorterCarryCondition.None;
    float[] conditionTime = new float[] {5f, 10f, 15f};
    float nowTime = 0f;
    float maxTime;
    float nextTime = 50f;
    float speed = 1f;
    Transform ChocolateF;
    public List<Transform> ChocolateSum = new List<Transform>();
    List<ChocolateEndControl> chocolateEndControls = new List<ChocolateEndControl>();
    public int nowNum = 0;
    public int maxNum = 1;
    public int bh;
    Transform capacity;
    Text capatxt; 
    ParticleSystem fxhit;
    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        ChocolateF = gameObject.GetChildControl<Transform>("ChocolateEndF");
        fxhit = gameObject.GetChildControl<ParticleSystem>("hitFX");
        foreach (Transform ss in ChocolateF) {
            ChocolateSum.Add(ss);
            chocolateEndControls.Add(ss.gameObject.GetComponent<ChocolateEndControl>());
            ss.gameObject.SetActive(false);
        }
        capacity = gameObject.GetChildControl<Transform>("capacity");
        capatxt = gameObject.GetChildControl<Text>("capacity/num");
        ReSetUI();
    }
    private void ReSetUI() {
        capatxt.text = nowNum + "/" + maxNum;

    }
    public void AddFxLv(float waitTime) {
        Invoke("AddFxLvTrue", waitTime);
        ReSetUI();
    }

    private void AddFxLvTrue() {
        Transform ObjectGo = ObjectPool.Instance.Get("Fx", "levelUp", transform).transform;
        ObjectGo.localPosition = Vector3.zero;
        RecycleFx recycleFx = ObjectGo.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(2f);
    }

    private void Update() {
        if (capacity.gameObject.activeSelf == true) {
            capacity.rotation = Quaternion.Euler(50f, 0f, 0f);
        }

        float nowSpeedRate = m_Agent.velocity.magnitude / m_Agent.speed;
        animator.SetFloat("speed", nowSpeedRate);
        if (porterCondition == PorterCondition.Chocolate) {
            if (nowTime < conditionTime[(int)porterCondition]) {
                nowTime += speed * Time.deltaTime;
                if (porterCarryCondition == PorterCarryCondition.full) {
                    int sleepRate = Random.Range(1, 10);
                    if (sleepRate == 1) {
                        porterCondition = PorterCondition.Sleep;
                        AdddizzyFx();
                        nowTime = 0;
                    }
                    else {
                        porterCondition = PorterCondition.Display;
                        nowTime = nextTime;
                    }
                   
                }
            }
            else {
                if (porterCarryCondition != PorterCarryCondition.full) {
                    Transform nowPos = FactoryMgr.Instance.GetChococlatePos(bh);
                    if (nowPos == null) {

                    }
                    else {
                        m_Agent.SetDestination(nowPos.position);
                    }
                }
                else {
                    int sleepRate = Random.Range(1, 10);
                    Debug.Log(sleepRate);
                    if (sleepRate == 1) {
                        porterCondition = PorterCondition.Sleep;
                        AdddizzyFx();
                    }
                    else {
                        porterCondition = PorterCondition.Display;
                    }
                }
                nowTime = 0f;
            }
        }
        else if (porterCondition == PorterCondition.Display) {
            if (nowTime < conditionTime[(int)porterCondition]) {
                nowTime += speed * Time.deltaTime;
            }
            else {
                if (porterCarryCondition != PorterCarryCondition.None) {
                    Transform nowPos = MarketMgr.Instance.GetDisplayPos(bh);
                    if (nowPos == null) {

                    }
                    else {
                        m_Agent.SetDestination(nowPos.position);
                    }
                }
                else {
                    
                        porterCondition = PorterCondition.Chocolate;
                }
                nowTime = 0f;
            }
        }
        else if (porterCondition == PorterCondition.Sleep) {
            if (nowTime < conditionTime[(int)porterCondition]) {
                nowTime += speed * Time.deltaTime;
            }
            else {
  
                RemovedizzyFx();
                nowTime = 0;

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
        
        CancelInvoke("dizzyFxEnd");
        animator.SetBool("dizzy", false);
        fxhit.Play();
        Transform hitFx = ObjectPool.Instance.Get("Fx", "hitFX").transform;
        hitFx.position = transform.position + new Vector3(0f, 1f, 0f);
        RecycleFx recycleFx = hitFx.gameObject.GetComponent<RecycleFx>();
        recycleFx.OnceDemo(1f);
        ObjectPool.Instance.Recycle(FxDizzy.gameObject, true);
        Invoke("dizzyFxEnd", 1.5f);
        

    }
    private void dizzyFxEnd() {
        
        porterCondition = PorterCondition.Display;
        nowTime = nextTime;
    }

    public void IsCarryChocolate(int bh) {
        if (porterCarryCondition == PorterCarryCondition.full) {
            return;
        }
        porterCarryCondition = PorterCarryCondition.Carry;
        animator.SetBool("Carrying", true);

        chocolateEndControls[nowNum].OnStart(bh);
        nowNum++;
        ReSetUI();
        if (nowNum >= maxNum) {
            porterCarryCondition = PorterCarryCondition.full;
        }
    }
    public void RemoveChocolate() {
        if (nowNum > 1) {
            ChocolateSum[nowNum - 1].gameObject.SetActive(false);
            nowNum--;
            ReSetUI();
            return;
        }
        else if (nowNum == 1) {
            animator.SetBool("Carrying", false);
            porterCarryCondition = PorterCarryCondition.None;
            
            ChocolateSum[nowNum - 1].gameObject.SetActive(false);
            nowNum--;
            ReSetUI();
            porterCondition = PorterCondition.Chocolate;
        }
    }

    public int GetEndChocolatebh2D() {

        return chocolateEndControls[nowNum - 1].bh / 2;
    }
}

public enum PorterCondition {
    Chocolate,
    Display,
    Sleep,
}

public enum PorterCarryCondition {
    None,
    Carry,
    full,
}
