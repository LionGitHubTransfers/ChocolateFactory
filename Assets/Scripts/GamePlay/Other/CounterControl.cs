using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterControl : MonoBehaviour
{
    public List<Transform> counterSum = new List<Transform>();
    public List<CounterOne> counterOnes = new List<CounterOne>(); 
    public List<Transform> CustomerPos1 = new List<Transform>();
    public Queue<CustomerControl> customerControl1 = new Queue<CustomerControl>();
    public int Pos1Now = 0;
    public List<Transform> CustomerPos2 = new List<Transform>();
    public Queue<CustomerControl> customerControl2 = new Queue<CustomerControl>();
    public int Pos2Now = 0;
    private void Start() {
        Transform ss = transform.GetChild(0);
        counterSum.Add(ss);
        counterOnes.Add(ss.gameObject.GetComponent<CounterOne>());
        Transform posF = ss.gameObject.GetChildControl<Transform>("UnLock/CustomerPos");
        foreach (Transform sss in posF) {
            CustomerPos1.Add(sss);
        }
        ss = transform.GetChild(1);
        counterOnes.Add(ss.gameObject.GetComponent<CounterOne>());
        counterSum.Add(ss);
        posF = ss.gameObject.GetChildControl<Transform>("UnLock/CustomerPos");
        foreach (Transform sss in posF) {
            CustomerPos2.Add(sss);
        }
        for (int i = 0; i < counterSum.Count; i++) {
            CounterOne counterOne = counterOnes[i];
            if (i < MarketMgr.Instance.counterLv) {
                counterOne.counterCondition = CounterCondition.UnLock;
            }
            else if (i == MarketMgr.Instance.counterLv) {
                counterOne.counterCondition = CounterCondition.Lock;
            }
            else {
                counterOne.counterCondition = CounterCondition.None;
            }
        }
    }
    public void SetView() {
        for (int i = 0; i < counterSum.Count; i++) {
            CounterOne counterOne = counterOnes[i];
            if (i < MarketMgr.Instance.counterLv) {
                counterOne.counterCondition = CounterCondition.UnLock;
                counterOne.bh = i;
            }
            else if (i == MarketMgr.Instance.counterLv) {
                counterOne.counterCondition = CounterCondition.Lock;
            }
            else {
                counterOne.counterCondition = CounterCondition.None;
            }
            counterOne.SetView();
        }
    }
    public Transform GetCustomerPos(CustomerControl customerControl) {
        if (MarketMgr.Instance.counterLv >= 2) {
            if (Pos1Now >= 3 && Pos2Now >= 3) {
                return null;
            }
            else if (Pos1Now >= 3 && Pos2Now <= 3) {
                if (customerControl1.Contains(customerControl) || customerControl2.Contains(customerControl)) {
                    return null;
                }
                customerControl2.Enqueue(customerControl);
                SetQuenePos();
                Pos2Now++;
                return CustomerPos2[Pos2Now - 1];
            }
            else {
                if (customerControl1.Contains(customerControl) || customerControl2.Contains(customerControl)) {
                    return null;
                }
                customerControl1.Enqueue(customerControl);
                SetQuenePos();
                Pos1Now++;
                return CustomerPos1[Pos1Now - 1];
            }
        }
        else if (MarketMgr.Instance.counterLv >= 1) {
            if (Pos1Now >= 3) {
                return null;
            }
            else {
                if (customerControl1.Contains(customerControl)) {
                    return null;
                }
                customerControl1.Enqueue(customerControl);
                SetQuenePos();
                Pos1Now++;
                return CustomerPos1[Pos1Now - 1];
            }
        }
        else {
            return null;
        }
        
    }
    public CustomerControl RemoveCustomerPos(int bh) {
        if (bh == 0) {
            if (Pos1Now <= 0) {
                return null;
            }
            else {
                CustomerControl customerControl = customerControl1.Dequeue();
                customerControl.ToHome();
                SetQuenePos();
                Pos1Now--;
                return customerControl;
            }
        }
        else if (bh == 1) {
            if (Pos2Now <= 0) {
                return null;
            }
            else {
                CustomerControl customerControl = customerControl2.Dequeue();
                customerControl.ToHome();
                SetQuenePos();
                Pos2Now--;
                return customerControl;
            }
        }
        else {
            return null;
        }
    }

    public CustomerControl GetTop(int bh) {
        if (bh == 0) {
            if (Pos1Now <= 0) {
                return null;
            }
            else {
                return customerControl1.Peek();
            }
        }
        else if (bh == 1) {
            if (Pos2Now <= 0) {
                return null;
            }
            else {
                return customerControl2.Peek();
            }
        }
        else {
            return null;
        }
        
    }

    private void SetQuenePos() {
        int i = 0;
        foreach (CustomerControl ss in customerControl1) {
            ss.SetDis(CustomerPos1[i]);
            i++;
        }
        i = 0;
        foreach (CustomerControl ss in customerControl2) {
            ss.SetDis(CustomerPos2[i]);
            i++;
        }
    }

}
