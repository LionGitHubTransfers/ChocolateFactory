using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 货币管理类
/// </summary>
public class CurrencyMgr : Singleton<CurrencyMgr> {
    private const string GOLD = "Gold";
    private int m_gold;
    public int Gold {
        get {
            return m_gold;
        }
        set {
            //if (value < 10) {
            //    ArrowMgr.Instance.Guide.gameObject.SetActive(false);
            //}
            //else {
            //    ArrowMgr.Instance.Guide.gameObject.SetActive(true);
            //}
            int changeValue = value - m_gold;
            m_gold = value;
            LocalSave.SetInt(GOLD, value);
            Send.SendMsg(SendType.GoldChange, changeValue, m_gold);
        }
    }

	public void Init(){
        m_gold = LocalSave.GetInt(GOLD, 50);

	}
	
	public void Clear(){
	}
}
