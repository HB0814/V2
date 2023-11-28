using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float time;
    private void Awake()
    {
        time = 1f;
    }
    void OnEnable()
    {
        Invoke("Off", time);
    }
    void Off()
    {
        gameObject.SetActive(false);
    }

}
