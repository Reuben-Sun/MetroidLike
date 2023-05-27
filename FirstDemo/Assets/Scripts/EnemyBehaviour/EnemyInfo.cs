using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
}
