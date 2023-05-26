using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState : MonoBehaviour
{
    [Header("Statge")]
    public bool onGround;
    public bool nearGround;
    public bool climbWall;

    [Header("Settings")]
    [SerializeField, Tooltip("地表的碰撞层")] private LayerMask groundLayer;
    [SerializeField, Tooltip("在地表上的检测距离")] private float testDistance = 0f;
    [SerializeField, Tooltip("靠近地表的检测距离")] private float nearDistance = 0.2f;
    [SerializeField, Tooltip("爬墙检测点")] private Transform climbWallPoint;
    [SerializeField, Tooltip("爬墙检测距离")] private float climbTestDistance = 0.2f;
    [SerializeField, Tooltip("可通行检测点")] private Transform passablePoint;
    [SerializeField, Tooltip("通行检测距离")] private float passableTestDistance = 1f;
    
    private float offset = 0.05f;
    private float lookRight;
    private void Update()
    {
        onGround = Physics.Raycast(transform.position + Vector3.up * offset, Vector3.down, testDistance + offset, groundLayer);
        // onGround = Physics.SphereCast(transform.position, testDistance, Vector3.down, out RaycastHit hit);
        nearGround = Physics.Raycast(transform.position + Vector3.up * offset, Vector3.down, nearDistance + offset, groundLayer);
        lookRight = (climbWallPoint.position.x - transform.position.x) > 0 ? 1 : -1;
        climbWall = Physics.Raycast(climbWallPoint.position, Vector3.right * lookRight, climbTestDistance, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.05f, transform.position + Vector3.up * 0.05f + Vector3.down * testDistance);
        Gizmos.DrawLine(climbWallPoint.position, climbWallPoint.position + Vector3.right * lookRight * climbTestDistance);
    }

    public bool GetPassable()
    {
        bool passable = !Physics.Raycast(passablePoint.position, Vector3.right * lookRight, passableTestDistance, groundLayer);
        return passable;
    }

    public Vector3 GetClimbTouchPosition()
    {
        Ray ray = new Ray(climbWallPoint.position, Vector3.right * lookRight);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }
}
