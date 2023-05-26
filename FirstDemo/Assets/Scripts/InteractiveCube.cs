using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.VFX;

public class InteractiveCube : MonoBehaviour
{
    [SerializeField] private float recoverTime = 20f;
    [SerializeField] private CubeType cubeType;
    
    private MeshRenderer meshRenderer;
    private Collider boxcollider;
    private VisualEffect visualEffect;

    private float currentDeadTime;

    private string TagName
    {
        get
        {
            if(cubeType == CubeType.Shoot)
                return "Bullet";
            else
                return "Hit";
        }
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxcollider = GetComponent<Collider>();
        visualEffect = GetComponent<VisualEffect>();
        visualEffect.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName))
        {
            meshRenderer.enabled = false;
            boxcollider.enabled = false;
            visualEffect.Play();
            currentDeadTime = recoverTime;
        }
    }

    private void Update()
    {
        if (currentDeadTime >= 0)
        {
            currentDeadTime -= Time.deltaTime;
            if (currentDeadTime <= 0)
            {
                meshRenderer.enabled = true;
                boxcollider.enabled = true;
            }
        }
    }
}

public enum CubeType
{
    Shoot,
    Hit
}