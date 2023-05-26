using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    private PlayerInput input;
    private CharacterController controller;
    private Animator animator;
    private CollisionState collisionState;
    private LineRenderer lineRenderer;
    private FireManager fireManager;
    private PlayableDirector director;

    [Header("Movement Settings")] 
    [SerializeField, Tooltip("水平移动速度")] 
    private float movementSpeed = 0.1f;
    [SerializeField, Tooltip("重力")] 
    private float gravity = -9.8f;
    [SerializeField, Tooltip("大跳的跳跃高度")] 
    private float jumpHeight = 10f;
    [SerializeField, Tooltip("开始加速下落时的速度")] 
    private float fallingSpeedup = 10f;
    [SerializeField, Tooltip("爬墙时向上跳的初速度")]
    private float climbingJumpSpeed = 10f;

    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Rig rightHandRig;
    [SerializeField] private Transform gunHandPosition;
    [SerializeField] private Rig gunHandRig;
    [SerializeField] private Transform gunAimCenter;
    [SerializeField, Tooltip("枪距离角色肩膀的距离")] private float gunToShoulderDistance = 2f;
    
    // input
    private Vector2 moveInput;
    private bool jumpPressed = false;
    private bool jumpPressDown = false;
    private bool aiming = false;
    private bool firePressDown = false;
    private bool hitPressDown = false;
    // member
    private float verticalVelocity;
    // state
    private bool moveAfterJump  = false;    // 移动中跳跃，还是跳跃后移动
    private bool keepJump = false;  // 保持滞空
    private bool jumping = false;   // 处于跳跃状态
    private bool climbing = false;  // TODO: 角色状态机
    private bool climbingJump = false;  // 抓墙时跳跃

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        collisionState = GetComponent<CollisionState>();
        lineRenderer = GetComponent<LineRenderer>();
        fireManager = GetComponent<FireManager>();
        director = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        #region 输入

        moveInput = input.actions["Move"].ReadValue<Vector2>();
        jumpPressDown = input.actions["Jump"].WasPressedThisFrame();
        jumpPressed = input.actions["Jump"].IsPressed();
        aiming = input.actions["Aim"].IsPressed();
        firePressDown = input.actions["Fire"].WasPressedThisFrame();
        hitPressDown = input.actions["Hit"].WasPressedThisFrame();

        #endregion
        
        #region 跳跃

        // 起跳
        if (jumpPressDown && collisionState.nearGround)
        {
            verticalVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
            moveAfterJump = Mathf.Abs(moveInput.normalized.x) < 0.3f;   // 判断起跳时移动速度
            keepJump = true;
            jumping = true;
            climbingJump = false;
            if (moveAfterJump)
            {
                animator.SetTrigger("Jump");
            }
            else
            {
                animator.SetTrigger("FastJump");
            }

        }

        // 滞空
        if (keepJump && verticalVelocity > -fallingSpeedup)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else if(!collisionState.onGround)
        {
            verticalVelocity += 3 * gravity * Time.deltaTime;
        }

        if (!jumpPressed)
        {
            keepJump = false;   // 松开后不能重新滞空
        }
        
        // 爬墙
        if (jumping && collisionState.climbWall && collisionState.GetPassable() && Mathf.Abs(moveInput.normalized.x) > 0.7 && !climbingJump)
        {
            climbing = true;
            verticalVelocity = 0;
            rightHandPosition.position = collisionState.GetClimbTouchPosition();
            rightHandPosition.rotation = Quaternion.LookRotation(moveInput.x > 0 ? Vector3.right : Vector3.left);
            rightHandRig.weight = 1;
            animator.SetBool("Climbing", climbing);
            if (jumpPressDown)
            {
                rightHandRig.weight = 0;
                verticalVelocity = climbingJumpSpeed;
                animator.SetTrigger("ClimbingUp");
                climbingJump = true;
                climbing = false;
            }
        }
        else
        {
            climbing = false;
            rightHandRig.weight = 0;
            animator.SetBool("Climbing", climbing);
        }
        
        // 落地
        if (verticalVelocity < 0 && collisionState.onGround)
        {
            if (moveAfterJump)
            {
                animator.SetTrigger("FallLand");
            }
            else
            {
                animator.SetTrigger("FastFallLand");
            }
            verticalVelocity = 0;
            moveAfterJump = false;
            keepJump = false;
            jumping = false;
            climbingJump = false;
        }
        
        // 运动
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);


        #endregion

        #region 水平移动

        bool isMoving = Mathf.Abs(moveInput.normalized.x) > 0.01f;
        if (isMoving && !climbing)
        {
            float herizontalSpeed = Mathf.Abs(moveInput.normalized.x * movementSpeed);
            
            float hDir = moveInput.x > 0 ? 1 : -1;
            if (moveAfterJump)
            {
                herizontalSpeed /= 2;   // 原地起跳，在空中动能很少，按理说应该走不动
            }

            if (!(aiming))
            {
                controller.Move(Vector3.right * hDir * herizontalSpeed * Time.deltaTime);
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveInput.x != 0 ? Vector3.right * moveInput.normalized.x : Vector3.right * hDir), 0.1f);
            
        }
        
        animator.SetFloat("MoveInput", Mathf.Abs(moveInput.normalized.x * movementSpeed), 0.2f, Time.deltaTime);
        #endregion

        #region 瞄准射击

        if (aiming)
        {
            #region 瞄准动作

            Vector3 targetPos;
            Vector3 deltaPos;
            if (moveInput.magnitude < 0.1f)
            {
                float hDir = controller.transform.forward.x > 0 ? 1 : -1;
                deltaPos = new Vector3(gunToShoulderDistance * hDir, 0, 0);
                targetPos = gunAimCenter.position + deltaPos;
                gunHandPosition.position = targetPos;
                gunHandPosition.rotation = Quaternion.LookRotation(new Vector3(1,-1,0));
            }
            else
            {
                float hDir = controller.transform.forward.x > 0 ? 1 : -1;
                deltaPos = new Vector3(gunToShoulderDistance * moveInput.normalized.x, gunToShoulderDistance * moveInput.normalized.y, 0);
                targetPos = gunAimCenter.position + deltaPos;
                gunHandPosition.position = targetPos;
                gunHandPosition.rotation = Quaternion.LookRotation(targetPos - gunAimCenter.position);
                gunHandPosition.Rotate(90f, 0f, 0f);
            }
            lineRenderer.SetPositions(new []{targetPos, targetPos + deltaPos * 12});
            lineRenderer.enabled = true;
            animator.SetBool("Aiming", true);
            gunHandRig.weight = 1;

            #endregion

            #region 射击

            if (firePressDown)
            {
                fireManager.FireCommonBullet(targetPos, Quaternion.LookRotation(targetPos - gunAimCenter.position));
            }


            #endregion
            
        }
        else
        {
            lineRenderer.enabled = false;
            gunHandRig.weight = 0;
            animator.SetBool("Aiming", false);
        }

        if (hitPressDown && collisionState.onGround)
        {
            director.Play();
        }

        #endregion
    }

}
