using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryControl : MonoBehaviour
{
    public int bh;

    private string ISLOCK;
    private bool m_isLock;
    public bool isLock {
        get {
            return m_isLock;
        }
        set {
            if (m_isLock != value) {
                if (value == false) {
                    BattleWindow.Instance.LockPorterNum++;
                    //BattleWindow.Instance.EmployerTipActive();
                    BattleWindow.Instance.ReSetEmployUI();
                }
                m_isLock = value;
                LocalSave.SetBool(ISLOCK, m_isLock);
            }
         
        }
    }
    Transform UnlockAnim;
    Transform UnlockMachine;
    Transform LockMachine;

    public Transform DisplaySign;
    public DisplayControl displayControl;
    public bool IsZero = false;

    private void Start() {
        ISLOCK = "ISLOCK" + bh.ToString();
        m_isLock = LocalSave.GetBool(ISLOCK, true);
        OnStart();
        SetView();
        if (IsZero) {
            SetView0();
        }
    }
    public void OnStart() {
        UnlockAnim = gameObject.GetChildControl<Transform>("UnlockAnim");
        UnlockMachine = gameObject.GetChildControl<Transform>("UnlockMachine");
        LockMachine = gameObject.GetChildControl<Transform>("LockMachine");

        DisplaySign = UnlockMachine.gameObject.GetChildControl<Transform>("productionDisplay/Display/signC");
        displayControl = UnlockMachine.gameObject.GetChildControl<DisplayControl>("productionDisplay/Display");

    }

    public void SetView() {
        if (isLock) {
            UnlockMachine.gameObject.SetActive(false);
            LockMachine.gameObject.SetActive(true);
        }
        else {
            UnlockMachine.gameObject.SetActive(true);
            LockMachine.gameObject.SetActive(false);
        }
        UnlockAnim.gameObject.SetActive(false);
    }
    public void SetView0() {
        UnlockMachine.gameObject.SetActive(true);
        LockMachine.gameObject.SetActive(false);
        UnlockAnim.gameObject.SetActive(false);
        isLock = false;
    }
    public void UnLockDemo() {
        isLock = false;
        UnlockAnim?.gameObject.SetActive(true);
    }
}
