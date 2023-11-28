using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlurController : MonoBehaviour
{
    public float duration = 30f;
    private float timer = 0f;
    private bool isChanging = false;
    public float startValue = -1f;
    public float endValue = 1f;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (isChanging)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float newValue = Mathf.Lerp(startValue, endValue, t);

            _material.SetFloat("_MotionBlurAngle", newValue);

            if (timer >= duration)
            {
                isChanging = false;
            }
        }
        else
        {
            isChanging = true;
            timer = 0f;
        }
    }
}
