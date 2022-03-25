using UnityEngine;
using System.Collections;

/// <summary>
/// 窗口根节点
/// </summary>
public class UIRootTwoD : MonoBehaviour {

    public static UIRootTwoD Instance = null;

    public Canvas normalCanvas;
    public Canvas modalCanvas;
    public Canvas tipCanvas;
    public Canvas systemCanvas;

    void Awake() {
        // 保证只有一个， 重复创建的对象直接删掉
        if (null != Instance) {
            GameObject.DestroyImmediate(gameObject);
            Debug.Log(" UIRoot2D Already Awaked !!! ");
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SortWindow(Transform winTransform, WindowType winType) {
        switch (winType) {
            case WindowType.Normal:
                winTransform.SetParent(normalCanvas.transform, false);
                break;
            case WindowType.Modal:
                winTransform.SetParent(modalCanvas.transform, false);
                break;
            case WindowType.Tips:
                winTransform.SetParent(tipCanvas.transform, false);
                break;
            case WindowType.System:
                winTransform.SetParent(systemCanvas.transform, false);
                break;
            default:
                break;
        }
    }
}
