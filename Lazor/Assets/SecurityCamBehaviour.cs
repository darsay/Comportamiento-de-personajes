using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SecurityCamBehaviour : MonoBehaviour
{
    // States Updates
    delegate void FsmUpdate();
    private FsmUpdate _fsmUpdate;
    
    // State Machine Parameters
    #region StateMachineSetUp
        private StateMachineEngine fsm;

        private State watch;
        private State lookingAtPlayer;
        private State destroyed;

        private Perception playerFoundPerception;
        private Perception playerLostPerception;
    #endregion
    
    // Camera Rotation
    private Vector3 initialRotation;
    [SerializeField] private float rotationAngle = 45;
    [SerializeField] private float rotationTime;
    [SerializeField] private float waitingTime;
    private bool isRotating;
    private bool rotatingLeft;
    private bool rotationInProgress;

    private Vector2 targets;
    
    // Player Detection

    [SerializeField] private Transform playerGO;
    [SerializeField] private CameraRayCaster cameraRayCaster;

    private void Awake() {
        initialRotation = transform.rotation.eulerAngles;
        playerGO = FindObjectOfType<PlayerController>().transform;
        InitFsm();
        InitCam();
    }

    private void Update() {
        _fsmUpdate();
    }

    void WatchUpdate() {
        CameraRotation();
        cameraRayCaster.LaunchRays();
        if (cameraRayCaster.LaunchRays()) {
            playerFoundPerception.Fire();
        }
        
    }

    void CameraRotation() {
        if (isRotating) {
            if (rotationInProgress) return;
            
            rotationInProgress = true;
            
            if(rotatingLeft) {
                transform.DORotate(new Vector3(initialRotation.x, targets.x, initialRotation.z), rotationTime)
                    .OnComplete(() => StartCoroutine(WaitingTime(waitingTime)));
            }
            else {
                transform.DORotate(new Vector3(initialRotation.x, targets.y, initialRotation.z), rotationTime)
                    .OnComplete(() => StartCoroutine(WaitingTime(waitingTime)));
            }
            
        }

        
    }

    IEnumerator WaitingTime(float t) {
        isRotating = false;
        rotatingLeft = !rotatingLeft;
        yield return new WaitForSeconds(t);
        isRotating = true;
        
        rotationInProgress = false;
        
    }

    void LookingUpdate() {

        var previousRotation = Quaternion.LookRotation(playerGO.position);
        print(previousRotation.y);
        
        if (previousRotation.eulerAngles.y < targets.y || previousRotation.eulerAngles.y > targets.x) {
            transform.DOLookAt(playerGO.position, 0, AxisConstraint.Y);
        }
    }

    void DestroyUpdate() {
        print("Destroyed");
    }

    void InitFsm() {
        fsm = new StateMachineEngine();

        watch = fsm.CreateEntryState("Watch", () => {
            _fsmUpdate = WatchUpdate;
        });

        lookingAtPlayer = fsm.CreateState("Looking", () => {
            _fsmUpdate = LookingUpdate;
        });

        destroyed = fsm.CreateState("Destroyed", (() => {
            _fsmUpdate = DestroyUpdate;
        }));

        playerFoundPerception = fsm.CreatePerception<PushPerception>();
        playerLostPerception = fsm.CreatePerception<PushPerception>();

        fsm.CreateTransition("PlayerFound", watch, playerFoundPerception, lookingAtPlayer);
        fsm.CreateTransition("PlayerLost", lookingAtPlayer, playerLostPerception, watch);
    }

    void InitCam() {
        targets.x = initialRotation.y - rotationAngle;
        targets.y = initialRotation.y + rotationAngle;
        isRotating = true;
    }

    
}
