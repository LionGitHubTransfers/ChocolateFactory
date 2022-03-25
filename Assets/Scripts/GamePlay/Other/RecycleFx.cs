using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleFx : MonoBehaviour
{
    private float time;
    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private void Start() {

        gameObject.TryGetComponent(out ParticleSystem com); 
        {
            particleSystems.Add(com);
        }
        foreach (Transform ss in transform) {
            ss.gameObject.TryGetComponent(out ParticleSystem com2);
            {
                particleSystems.Add(com2);
            }
        }
        foreach (ParticleSystem ss in particleSystems) {
            ss.Play();
        }
    }
    public void OnceDemo(float _time) {
        time = _time;
        Invoke("RecycleDemo", time);
    }

    private void OnEnable() {
        foreach (ParticleSystem ss in particleSystems) {
            ss.Play();
        }
    }

    private void RecycleDemo() {
        CancelInvoke("RecycleDemo");
        transform.localScale = Vector3.one;
        ObjectPool.Instance.Recycle(gameObject, true);

    }

}
