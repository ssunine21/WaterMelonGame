using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl init = null;
    private void Awake()
    {
        if (init == null)
        {
            init = this;

        }
        else if (init != this)
        {
            Destroy(this.gameObject);
        }
    }

    private float _currAudioVolum;
    public float currAudioVolum
    {
        get { return _currAudioVolum; }
        set { _currAudioVolum = value; }
    }
}
