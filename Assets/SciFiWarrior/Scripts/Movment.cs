﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    private float normalSpeed = 4;
    private float runSpeed = 7;
    private float rotationSpeed = 80;
    private float rot = 0;
    private float gravity = 8;
    private Dictionary<string, int> movementConditions = new Dictionary<string, int>();

    private Vector3 moveDir = Vector3.zero;

    public Animator anim;
    public CharacterController characterController;

    private void Start()
    {
        movementConditions.Add("Idle", 0);
        movementConditions.Add("ShootSingleBullet", 1);
        movementConditions.Add("ShootAuto", 2);
        movementConditions.Add("WalkForward", 3);
        movementConditions.Add("Run", 4);
        
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
        GetInput();
    }

    
    private void Movement()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDir = applyMoveForce("WalkForward", normalSpeed);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                moveDir = idle("Idle");
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDir = applyMoveForce("Run", runSpeed);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveDir = idle("Idle");
            }
 
            rotation(moveDir);
            
        }
    }
    
    private Vector3 applyMoveForce(string typeOfAction, float speed)
    {
        anim.SetInteger("Condition", movementConditions[typeOfAction]);
        moveDir = new Vector3(0, 0, 1);
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
        return moveDir;
    }

    private Vector3 idle(string typeOfAction)
    {
        anim.SetInteger("Condition", movementConditions[typeOfAction]);
        moveDir = new Vector3(0, 0, 0);
        return moveDir;
    }

    private void rotation(Vector3 finalMoveDir)
    {
        rot += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);
        finalMoveDir.y -= gravity * Time.deltaTime;
        characterController.Move(finalMoveDir * Time.deltaTime);
    }


    private void GetInput()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attacking();
            }
        }
    }

    private void Attacking()
    {
        anim.SetInteger("Condition", 3);
    }
}