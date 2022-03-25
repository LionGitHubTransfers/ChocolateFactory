using UnityEngine;
using System.Collections;

/// <summary>
/// 基础界面信息
/// </summary>
[SerializeField]
public class WindowInfo : MonoBehaviour {
    public WindowType windowType = WindowType.Normal;
    public OpenAnimType openAnimType = OpenAnimType.None;
    public OpenAnimType closeAnimType = OpenAnimType.None;
    public float animTime = 0.3f;
    public bool closeOnEmpty = false;
    public bool mask = false;
    public Vector3 defaultPos = Vector3.zero;
    public Vector3 openPos = Vector3.zero;
    public int group = 0;

}
