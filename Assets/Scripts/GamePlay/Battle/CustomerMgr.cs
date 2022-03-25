using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMgr : Singleton<CustomerMgr>
{
    public int CustomerNumMax = 12;
    List<Transform> CustomerSum = new List<Transform>();
    List<Transform> CustomerPosStart = new List<Transform>();
    List<Transform> CustomerPosEnd = new List<Transform>();
    float nowTime = 0;
    float maxTime = 5f;
    float waitTime = 5f;
    Transform CustomerPos;
    public List<Transform> MarketPos = new List<Transform>();
    public List<Transform> MarketPosTrue = new List<Transform>();


    public float speed;

    public void Init() {
        speed = 0f;
        CustomerPos = ObjectPool.Instance.Get("Market", "CustomerPos").transform;
        foreach (Transform ss in CustomerPos) {
            MarketPos.Add(ss);
        }
        foreach (Transform ss in BattleMgr.Instance.CustomerPosStart) {
            CustomerPosStart.Add(ss);
        }
        foreach (Transform ss in BattleMgr.Instance.CustomerPosEnd) {
            CustomerPosEnd.Add(ss);
        }
        InitMsg();
    }

    public void Clear() {
        ClearMsg();
    }

    public void InitMsg() {
    }

    public void ClearMsg() {
    }

    private void AddCustomer() {
        if (CustomerSum.Count >= CustomerNumMax) {
            return;
        }
        int randNum = Random.Range(0, CustomerPosStart.Count);
        Vector3 nowPos = CustomerPosStart[randNum].position;
        Transform ObjectGo = ObjectPool.Instance.Get("Market", "customer").transform;
        ObjectGo.position = nowPos;
        CustomerControl customerControl = ObjectGo.gameObject.GetComponent<CustomerControl>();
        int MarketMaxNeedBh = MarketMgr.Instance.DisplayChocolateNeedNum();
        if (!ArrowMgr.Instance.endnow) {
            MarketMaxNeedBh = 1;
            customerControl.ChangeNeed(MarketMaxNeedBh, 1);
        }
        else {
            if (MarketMaxNeedBh == -1) {
                MarketMaxNeedBh = 1;
            }
            if (CustomerNumMax - 4 > 4) {
                int nowMax = CustomerNumMax - 4;
                if (nowMax > 6) {
                    nowMax = 6;
                }
                customerControl.ChangeNeed(MarketMaxNeedBh, Random.Range(2, nowMax));
            }
            else {
                customerControl.ChangeNeed(MarketMaxNeedBh, Random.Range(2, 4));
            }
        }
       
        
        customerControl.StopPy();
        CustomerSum.Add(ObjectGo);
    }

    public void RemoveCustomer(Transform ObjectGo) {
        ObjectPool.Instance.Recycle(ObjectGo.gameObject);
        CustomerSum.Remove(ObjectGo);
    }

    public Transform GetEndPos() {
        int nowBh = Random.Range(0, CustomerPosEnd.Count);
        return CustomerPosEnd[nowBh];
    }

    public Transform GetMarketPos(Transform ObjectGo) {
        if (MarketPosTrue.Count < MarketPos.Count) {
            if (MarketPosTrue.Contains(ObjectGo)) {
                return MarketPos[MarketPosTrue.IndexOf(ObjectGo)];
            }
            MarketPosTrue.Add(ObjectGo);
            return MarketPos[MarketPosTrue.Count - 1];
        }
        return null;
    }

    //public void RemoveMarketPos(Transform ObjectGo) {
    //    MarketPosTrue.Remove(ObjectGo);
    //    Debug.LogError(MarketPosTrue.Count);
    //    foreach (Transform ss in MarketPosTrue) {
    //        CustomerControl customerControl = ss.gameObject.GetComponent<CustomerControl>();
    //        customerControl.ReSetStartPos();
    //    }
    //}

    public void OnUpdate() {
        int nowDisplay = MarketMgr.Instance.ReturnDisplayChocolateNum();
        CustomerNumMax = 3 + nowDisplay / 2;
        speed = 0.5f + nowDisplay * 0.1f;
        if (MarketPosTrue.Count >= MarketPos.Count) {
            return;
        }
        if (waitTime > 0) {
            waitTime -= Time.deltaTime;
            return;
        }
        if (nowTime < maxTime) {
            nowTime += speed * Time.deltaTime;
        }
        else {
            AddCustomer();
            nowTime = 0;
        }
    }

}
