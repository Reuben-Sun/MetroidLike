using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireManager : MonoBehaviour
{
    [SerializeField, Tooltip("子弹速度")] private float bulletSpeed = 10f;
    [SerializeField, Tooltip("普通子弹")] private GameObject commonBullet;

    public void FireCommonBullet(Vector3 initPosition, Quaternion initRotation)
    {
        GameObject bullet = Instantiate(commonBullet, initPosition, initRotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
    }
}
