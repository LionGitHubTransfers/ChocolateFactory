using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : BaseWindowWrapper<MainWindow> {

    private Button btnShop;
    private Button btnSign;

    protected override void InitCtrl() {
        btnShop = gameObject.GetChildControl<Button>("btnShop");
        btnSign = gameObject.GetChildControl<Button>("btnSign");
    }

    protected override void OnPreOpen() {
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnShop.onClick.AddListener(OnBtnShopClick);
        btnSign.onClick.AddListener(OnBtnSignClick);
    }

    protected override void ClearMsg() {
        btnShop.onClick.RemoveListener(OnBtnShopClick);
        btnSign.onClick.RemoveListener(OnBtnSignClick);
    }

    private void OnBtnSignClick() {
        WindowMgr.Instance.OpenWindow<SignWindow>();
    }

    private void OnBtnShopClick() {
        WindowMgr.Instance.OpenWindow<ShopWindow>();
    }
}
