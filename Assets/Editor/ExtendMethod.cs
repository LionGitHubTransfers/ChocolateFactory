using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendMethod {

    [MenuItem("Tools/AddMoney")]
    public static void AddMoney() {
        CurrencyMgr.Instance.Gold += 100000;
    }

    [MenuItem("Tools/AddGrade")]
    public static void AddGrade() {
        GradeMgr.Instance.CurGrade += 10;
    }

    [MenuItem("Tools/DeleteKeys")]
    public static void DeleteKeys() {
        LocalSave.DeleteAll();
    }
}