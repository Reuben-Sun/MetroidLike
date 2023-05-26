using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("子弹销毁时间")] private float lifeTime = 5f;

    private float remainingTime;
    void Start()
    {
        remainingTime = lifeTime;
    }
    
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
