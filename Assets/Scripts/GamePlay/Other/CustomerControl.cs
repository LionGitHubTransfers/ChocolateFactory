using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerControl : MonoBehaviour
{
    Transform DialogNeed;
    List<Transform> DialogNeedList = new List<Transform>();
    Text DialogNeedNum;
    Transform DialogPoint;
    NavMeshAgent m_Agent;
    Animator animator;
    public CustomerHandCdt customerHandCdt = CustomerHandCdt.None;
    public CustomerSearchCdt customerSearchCdt = CustomerSearchCdt.Chocolate;
    Transform ChocolateEndF;
    List<Transform> ChocolateEnd = new List<Transform>();
    public int chocolateBh;
    int chocolateMaxNum;
    ParticleSystem happy;
    ParticleSystem sad;

    int chocolateNowNum;

    float nextNum = 50f;
    float nowNum = 0f;
    float speed = 1f;
    float[] TimeList = new float[] {3f, 5f, 5f};

    List<Vector3> MarketPos = new List<Vector3>() { new Vector3(-1.7f, 0f, 10f), new Vector3(1.7f, 0f, 10f) };
    Vector3 StartMarketPos;

    int GiveMoneyNum;
    int waitTimes;

    int NeedBh;
    int NeedNum = 2;
    public CustomerFace customerFace = CustomerFace.None;
    private void Start() {
        DialogNeed = gameObject.GetChildControl<Transform>("Dialog");    
        foreach (Transform ss in DialogNeed.gameObject.GetChildControl<Transform>("dialogBG/List")) {
            DialogNeedList.Add(ss);
        }
        happy = gameObject.GetChildControl<ParticleSystem>("EmojiHappy");
        
        sad = gameObject.GetChildControl<ParticleSystem>("EmojiDisappointed");
        StopPy();
        DialogPoint = gameObject.GetChildControl<Transform>("dialog_root");      
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        ChocolateEndF = gameObject.GetChildControl<Transform>("ChocolateEndF");
        foreach (Transform ss in ChocolateEndF) {
            ChocolateEnd.Add(ss);
        }
        Init();
        ReSetUI();
    }

    public void StopPy() {
        happy?.Stop();
        sad?.Stop();
    }

    private void Init() {
        
        DialogNeed.gameObject.SetActive(false);
        DialogPoint.gameObject.SetActive(false);
        foreach (Transform ss in DialogNeedList) {
            ss.gameObject.SetActive(false);
        }
        foreach (Transform ss in ChocolateEnd) {
            ss.gameObject.SetActive(false);
        }
        customerHandCdt = CustomerHandCdt.None;
        customerSearchCdt = CustomerSearchCdt.Chocolate;
        customerFace = CustomerFace.None;
        chocolateBh = NeedBh;//Random.Range(1, 6);

        chocolateMaxNum = NeedNum;//Random.Range(2, 6);

        chocolateNowNum = 0;
        customerHandCdt = CustomerHandCdt.None;
        animator?.SetBool("carrying", false);
        StartMarketPos = MarketPos[Random.Range(0, 2)];
        waitTimes = 0;
        foreach (Transform ss in ChocolateEnd) {
            ChocolateEndControl chocolateEndControl = ss.gameObject.GetComponent<ChocolateEndControl>();
            chocolateEndControl.OnStart(chocolateBh * 2 + 1);
        }
        GiveMoneyNum = chocolateBh * chocolateMaxNum * 10;
        nowNum = 0f;
        ReSetUI();
    }
    public void ChangeNeed(int _NeedBh, int _NeedNum) {
        NeedBh = _NeedBh;
        NeedNum = _NeedNum;
        chocolateBh = NeedBh;
        if (ChocolateEnd.Count > 0) {
            foreach (Transform ss in ChocolateEnd) {
                ChocolateEndControl chocolateEndControl = ss.gameObject.GetComponent<ChocolateEndControl>();
                chocolateEndControl.OnStart(chocolateBh * 2 + 1);
            }
            GiveMoneyNum = chocolateBh * chocolateMaxNum * 10;
        }
    }
    private void ReSetUI() {
        if (customerSearchCdt == CustomerSearchCdt.Chocolate) {
            foreach (Transform ss in DialogNeedList) {
                ss.gameObject.SetActive(false);
            }
            DialogNeedNum = DialogNeedList[chocolateBh - 1].gameObject.GetChildControl<Text>("num");
            DialogNeedNum.text = chocolateNowNum + "/" + chocolateMaxNum;
            DialogNeedList[chocolateBh - 1].gameObject.SetActive(true);
            DialogNeed.gameObject.SetActive(true);
        }
        if (customerSearchCdt == CustomerSearchCdt.Home) {
            
            foreach (Transform ss in DialogNeedList) {
                ss.gameObject.SetActive(false);
            }
            if (customerHandCdt == CustomerHandCdt.full) {
                happy.Play();
                sad.Stop();
                customerFace = CustomerFace.Happy;
            }
            else {
                sad.Play();
                customerFace = CustomerFace.Sad;
                happy.Stop();
            }
                
            DialogNeed.gameObject.SetActive(false);
        }
    }
    public bool IsChocolateMax() {
        if (chocolateNowNum >= chocolateMaxNum) {
            return true;
        }
        else {
            return false;
        }
    }

    public void GetChocolate(Transform startPos) {
        Transform ObjectGo = ObjectPool.Instance.Get("Factory", "ChocolateEnd", transform).transform;
        ObjectGo.position = startPos.position;
        ObjectGo.rotation = startPos.rotation;
        ChocolateEndControl chocolateEndControl = ObjectGo.gameObject.GetComponent<ChocolateEndControl>();
        chocolateEndControl.OnStart(chocolateBh * 2 + 1);
        Vector3 StartPos = ObjectGo.localPosition;
        Vector3 EndPos = ChocolateEnd[chocolateNowNum].localPosition;
        int thenNum = chocolateNowNum;
        Tween tween = DOTween.To(setter: value => {
            ObjectGo.localPosition = EMath.Parabola(StartPos, EndPos, 1f, value);
        }, startValue: 0, endValue: 1, duration: 0.5f).OnComplete(()=> {
            ObjectPool.Instance.Recycle(ObjectGo.gameObject);
            ChocolateEnd[thenNum].gameObject.SetActive(true);
        });
        ObjectGo.DORotate(ChocolateEnd[chocolateNowNum].rotation.eulerAngles, 0.4f);
        chocolateNowNum++;
        ReSetUI();
        if (chocolateNowNum >= chocolateMaxNum) {
            customerHandCdt = CustomerHandCdt.full;
            animator?.SetBool("carrying", true);
        }
        else if (chocolateNowNum > 0) {
            customerHandCdt = CustomerHandCdt.Carry;
            animator?.SetBool("carrying", true);
        }
    }
    private void Update() {
        if (DialogNeed.gameObject.activeSelf == true) {
            DialogNeed.rotation = Quaternion.Euler(50f, 0f, 0f);
        }
        if (DialogPoint.gameObject.activeSelf == true) {
            DialogPoint.rotation = Quaternion.Euler(50f, 0f, 0f);
        }
        float nowSpeedRate = m_Agent.velocity.magnitude / m_Agent.speed;
        animator.SetFloat("speed", nowSpeedRate);

        if (customerSearchCdt == CustomerSearchCdt.Chocolate) {
            if (nowNum < TimeList[(int)customerSearchCdt]) {
                nowNum += speed * Time.deltaTime;
                if (customerHandCdt == CustomerHandCdt.full) {
                    customerSearchCdt = CustomerSearchCdt.Counter;
                    nowNum = nextNum;
                }
            }
            else {
                Transform numPos = MarketMgr.Instance.GetChocolateBh2GoodsPos(chocolateBh);
                if (numPos == null) {
                    numPos = MarketMgr.Instance.GetChocolateBh2GoodsPosNull(chocolateBh);
                    if (numPos == null) {

                    }
                    else {
                        if (customerHandCdt == CustomerHandCdt.None) {
                            m_Agent.SetDestination(numPos.position);
                            if (transform.position.z < 5f) {
                                waitTimes++;
                                if (waitTimes > 1) {
                                    customerSearchCdt = CustomerSearchCdt.Home;
                                    nowNum = nextNum;
                                    ReSetUI();
                                    waitTimes = 0;
                                    return;
                                }

                            }
                        }
                        else {
                            waitTimes = 0;
                        }
                    }
                }
                else {
                    m_Agent.SetDestination(numPos.position);
                }
                
                //waitTimes++;
                //if (waitTimes > 3) {
                //    if (chocolateNowNum > 0) {

                //    }
                //    else {
                //        customerSearchCdt = CustomerSearchCdt.Home;
                //        nowNum = nextNum;
                //        ReSetUI();
                //    }
                //}
                nowNum = 0f;
            }
        }
        else if (customerSearchCdt == CustomerSearchCdt.Counter) {
            if (nowNum < TimeList[(int)customerSearchCdt]) {
                nowNum += speed * Time.deltaTime;
            }
            else {
                Transform numPos = MarketMgr.Instance.GetChechOutPos(this);
                nowNum = 0f;
            }
        }
        else if (customerSearchCdt == CustomerSearchCdt.Home) {
            if (nowNum < TimeList[(int)customerSearchCdt]) {
                nowNum += speed * Time.deltaTime;
                if (customerHandCdt == CustomerHandCdt.Carry) {
                    customerSearchCdt = CustomerSearchCdt.Chocolate;
                    nowNum = nextNum;
                }
            }
            else {
                Vector3 numPos = new Vector3(35f, 0f, 0f);
                if ((transform.position - numPos).magnitude > 3f) {
                    m_Agent.SetDestination(numPos);
                }
                else {
                    RecycleCustomer();
                }
                nowNum = 0f;
            }
        }
    }

    public void SetDis(Transform now) {
        m_Agent.SetDestination(now.position);
    }

    public int PayMoney() {

        ToHome();
        return GiveMoneyNum;
    }

    public void ToHome() {
        customerSearchCdt = CustomerSearchCdt.Home;
        nowNum = nextNum;
        ReSetUI();
    }


    private void RecycleCustomer() {
        Init();
        CustomerMgr.Instance.RemoveCustomer(transform);
    }
}

public enum CustomerFace {
    None,
    Happy,
    Sad,
}

public enum CustomerHandCdt {
    None,
    Carry,
    full,
}

public enum CustomerSearchCdt {
    Chocolate,
    Counter,
    Home,
}
