#define DIRECT_READING //需要延迟读表注释这行

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Globalization;

/// <summary>
/// 游戏启动
/// </summary>
public class Launch : SingletonMonoBehavior<Launch> {

    void Awake() {
        Debug.Log(" Launch Awake Begin......");
        Debug.Log(" HWFrameWork Version:0.1.7");
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        QualitySettings.vSyncCount = 0;
        gameObject.AddMissingComponent<CoDelegator>();
        gameObject.AddMissingComponent<SoundMgr>();
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
#if DIRECT_READING
        InitClient();
#else
        CoDelegator.Coroutine(InitClient());
#endif
    }

#if DIRECT_READING
    private void InitClient() {
        GameStateMgr.Instance.SwitchState(GameState.Loading);
        //直读表模块
        RefDataMgr.Instance.InitBasic();
        InitLogic();
    }
#else
    //延迟读表模块
    IEnumerator InitClient() {
        RefDataMgr.Instance.InitBasic();
        GameStateMgr.Instance.SwitchState(GameState.Loading);
        yield return CoDelegator.Coroutine(RefDataMgr.Instance.Init());
        InitLogic();
    }
#endif

    private void InitLogic() {
        //逻辑模块的初始化
        ObjectPool.Instance.Init();
        BattleMgr.Instance.Init();
        PlayerMgr.Instance.Init();
        CameraMgr.Instance.Init();
        GardenMgr.Instance.Init();
        ConveyorMgr.Instance.Init();
        FactoryMgr.Instance.Init();
        MarketMgr.Instance.Init();
        CustomerMgr.Instance.Init();
        ArrowMgr.Instance.Init();
        GradeMgr.Instance.Init();
        CurrencyMgr.Instance.Init();
        ScoreMgr.Instance.Init();
        ShopMgr.Instance.Init();
        TaskMgr.Instance.Init();
        SignMgr.Instance.Init();
        SettingsMgr.Instance.Init();
        //初始化完成切换到主界面
        GameStateMgr.Instance.SwitchState(GameState.Battle);
        //初始化完成切换到主场景
        //LoadSceneMgr.Instance.LoadScene("Main", GameState.Main);
    }

    private void Update() {
#if UNITY_EDITOR
        //空格暂停游戏功能
        if (Input.GetKeyDown(KeyCode.Space)) {
            EditorApplication.isPaused = true;
        }
#endif

        PlayerMgr.Instance.OnUpdate();
    }
    private void LateUpdate() {
#if UNITY_EDITOR
        //空格暂停游戏功能
        if (Input.GetKeyDown(KeyCode.Space)) {
            EditorApplication.isPaused = true;
        }
#endif

        CameraMgr.Instance.OnLateUpdate();
        CustomerMgr.Instance.OnUpdate();
    }

}