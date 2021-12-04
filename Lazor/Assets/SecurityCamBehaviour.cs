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

    [SerializeField]private GameObject cameraObject;
    
    // Player Detection
    [SerializeField] private Transform lens;
    private List<Ray> rays;

    [SerializeField] private Transform playerGO;

    private void Awake() {
        initialRotation = cameraObject.transform.rotation.eulerAngles;
        playerGO = FindObjectOfType<PlayerController>().transform;
        rays = new List<Ray>();
        InitFsm();
        InitCam();
    }

    private void Update() {
        _fsmUpdate();
    }

    void WatchUpdate() {
        CameraRotation();
        LaunchRays();
    }

    void CameraRotation() {
        if (isRotating) {
            if (rotationInProgress) return;
            
            rotationInProgress = true;
            
            if(rotatingLeft) {
                cameraObject.transform.DORotate(new Vector3(initialRotation.x, targets.x, initialRotation.z), rotationTime)
                    .OnComplete(() => StartCoroutine(WaitingTime(waitingTime)));
            }
            else {
                cameraObject.transform.DORotate(new Vector3(initialRotation.x, targets.y, initialRotation.z), rotationTime)
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
        if (transform.rotation.eulerAngles.y > targets.y || transform.rotation.eulerAngles.y < targets.x) return;
        
        cameraObject.transform.DOLookAt(playerGO.position, 0.1f, AxisConstraint.Y);
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

    void LaunchRays() {
        rays.Clear();
        rays.Add(new Ray(lens.position, lens.forward));
        rays.Add(new Ray(lens.position, lens.forward+Vector3.left*0.5f));
        rays.Add(new Ray(lens.position, lens.forward+Vector3.right*0.5f));
        
        RaycastHit hit;

        foreach (var ray in rays) {
            if (Physics.Raycast(ray, out hit, 5)) {
                if (hit.collider.CompareTag("Player")) {
                    playerFoundPerception.Fire();
                }
               
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(lens.position, direction);
        direction = transform.TransformDirection(Vector3.forward + Vector3.left * 0.5f)*5;
        Gizmos.DrawRay(lens.position, direction);
        direction = transform.TransformDirection(Vector3.forward + Vector3.right * 0.5f)*5;
        Gizmos.DrawRay(lens.position, direction);
    }
}
