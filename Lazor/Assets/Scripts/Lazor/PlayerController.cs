using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    // States Updates
    delegate void FsmUpdate();
    private FsmUpdate _fsmUpdate;
        
    // Player animator
    [SerializeField] private Animator _animator;
    
    // RigidBody
    private Rigidbody _rigidbody;

    // InputController
    private PlayerInput _playerInput;
    private Vector3 inputVector;

    // State Machine Parameters
    #region StateMachineSetUp
        private StateMachineEngine playerStateMachine;

        private State standar;
        private State stealth;
        private State running;
        private State hidden;
        private State aiming;
        private State dead;
        
        
        private Perception crouchPerception;
        private Perception standUpPerception;
        private Perception runningPerception;
        private Perception stopRunningPerception;
        private Perception crouchRunningPerception;
        
        
        
        private Transition toStealth;
    

    #endregion
    
    //Camera Stuff
    [SerializeField] private Transform cameraTransform;
   
    
    // Player parameters
    [SerializeField] private bool isCrouched;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isRunning;
    
    // Player stats

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float stealthSpeed;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        
        _fsmUpdate = StandardUpdate;

        playerStateMachine = new StateMachineEngine();
        InitFSM(playerStateMachine);
    }
    

    private void Update() {
        playerStateMachine.Update();
        _fsmUpdate();
        HandleCamera();
        print(playerStateMachine.actualState.Name);
    }

    private void HandleCamera() {
    }


    public void OnMove(InputValue value) {
        var moveInput = value.Get<Vector2>();
        inputVector = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveInput.magnitude > 0) {
            isMoving = true;
            _animator.SetBool("isWalking" ,true);
            HandleRotation();
        }
        else {
            isMoving = false;
            _animator.SetBool("isWalking" ,false);
        }
    }

    public void OnCrouch(InputValue value) {
        if (isRunning) {
            isCrouched = true;
            _animator.SetBool("isRunning", false);
        }
        
        if (!isCrouched) {
            crouchPerception.Fire();
        }
        else { 
            standUpPerception.Fire();
        }
        
        
    }

    public void OnRun(InputValue value) {
        if (!isMoving) {
            stopRunningPerception.Fire();
            return;
        }
        
        
        if (value.isPressed && isMoving) {
            _animator.SetBool("isCrouched", false);
            isRunning = true;
            
            runningPerception.Fire();
            
        }
        else {
            isRunning = false;
            _animator.SetBool("isRunning", false);
        }
        
    }

    private void InitFSM(StateMachineEngine fsm) {
        standar = fsm.CreateEntryState("Standard", (() => {
            _fsmUpdate = StandardUpdate;
            isRunning = false;
            isCrouched = false;
            _animator.SetBool("isCrouched", false);
        }));
        
        stealth = fsm.CreateState("Stealth", (() => {
            _fsmUpdate = StealthUpdate;
            isRunning = false;
            isCrouched = true;
            _animator.SetBool("isCrouched", true);
        }));

        running = fsm.CreateState("Running", () => {
            isRunning = true;
            isCrouched = false;
            _fsmUpdate = RunningUpdate;
            _animator.SetBool("isRunning", true);
        });
        
        
        crouchPerception = fsm.CreatePerception<PushPerception>();
        standUpPerception = fsm.CreatePerception<PushPerception>();
        runningPerception = fsm.CreatePerception <PushPerception>();
        stopRunningPerception = fsm.CreatePerception <ValuePerception>(() => !isRunning);
        crouchRunningPerception = fsm.CreatePerception <ValuePerception>(() => isCrouched);
        

        fsm.CreateTransition("ToStealth", standar,
            crouchPerception, stealth);
        fsm.CreateTransition("ToStandard", stealth,
            standUpPerception, standar);
        
        fsm.CreateTransition("ToRunFromStealth", stealth,
            runningPerception, running);
        fsm.CreateTransition("ToRunFromStandard", standar,
            runningPerception, running);
        
        fsm.CreateTransition("ToStandardFromRun", running,
            stopRunningPerception, standar);
        fsm.CreateTransition("ToStealthFromRun", running,
            crouchRunningPerception, stealth);

    }

    private void RunningUpdate() {
        MoveCharacter(runSpeed);
    }


    void StandardUpdate() {
        MoveCharacter(walkSpeed);
    }

    void StealthUpdate() {
        MoveCharacter(stealthSpeed);
    }

    bool CheckIfRunning() {
        return isRunning;
    }


    void MoveCharacter(float speed) {
        if (inputVector.magnitude <= 0) return;
        HandleRotation();

        Vector3 move = new Vector3(inputVector.x, 0, inputVector.z);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;

        _rigidbody.velocity = move.normalized * speed + _rigidbody.velocity.y * Vector3.up ;
        
    }

    void  HandleRotation() {
        transform.DOLookAt(transform.position + _rigidbody.velocity, 0.2f, AxisConstraint.Y);
    }
}
