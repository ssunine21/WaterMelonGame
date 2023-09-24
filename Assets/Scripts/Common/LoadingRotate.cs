using System;
using System.Collections;
using UnityEngine;

public class LoadingRotate : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(IeRatate());
    }

    private IEnumerator IeRatate()
    {
        while (true)
        {
            transform.Rotate(new Vector3(0, 0, 10 * Time.deltaTime));
            yield return null;
        }
    }
}
