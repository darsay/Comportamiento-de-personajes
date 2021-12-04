
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBehaviour : MonoBehaviour
{
    // States Updates
    delegate void FsmUpdate();
    private FsmUpdate fsmUpdate;
    
    // State Machine Parameters
    #region StateMachineSetUp
        private StateMachineEngine guardStateMachine;
    
        private State patrolling;
        private State combat;
        private State searching;
        private State dead;
        private State scaped;

        private Perception playerSeenPerception;
    #endregion
    
    // NavMesh
    private NavMeshAgent _navMeshAgent;
    
    // Animator
    [SerializeField] private Animator _animator;
    
    // Patrol
    [SerializeField] private List<Transform> patrolCheckPoints;
    private int currentCheckpoint = 0;
    private bool checkPointReached;
    
    // Vision
    private List<Ray> visionRays = new List<Ray>();
    [SerializeField] private Transform head;


    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        fsmUpdate = PatrolUpdate;

        guardStateMachine = new StateMachineEngine();
        InitFSM(guardStateMachine);
    }

    private void Update() {
        fsmUpdate();
    }

    #region Patrolling

    void PatrolUpdate() {
        CastRays();
        if (Vector3.Distance(transform.position, patrolCheckPoints[currentCheckpoint].position) < 1) {
            checkPointReached = true;
            currentCheckpoint = (currentCheckpoint+1) % patrolCheckPoints.Count;
            StartCoroutine(WaitToNextMovement());
        }
        
        if (!checkPointReached) {
            _navMeshAgent.destination = patrolCheckPoints[currentCheckpoint].position;
        }
    }

    IEnumerator WaitToNextMovement() {
        var waitTime = Random.Range(3, 9);
        _animator.SetBool("isWalking", false);
        _navMeshAgent.enabled = false;
        yield return new WaitForSeconds(waitTime);
        _navMeshAgent.enabled = true;
        _animator.SetBool("isWalking", true);
        checkPointReached = false;
    }

    #endregion

    #region Combat
    void CombatUpdate() {
        print("AAAAAAAAAAAAAAAAAAAAA");
    }
    

    #endregion

    void InitFSM(StateMachineEngine fsm) {
        patrolling = fsm.CreateEntryState("Patrolling", (() => {
            fsmUpdate = PatrolUpdate;
        }));

        combat = fsm.CreateState("Combat", () => {
            fsmUpdate = CombatUpdate;
        });
        
        playerSeenPerception = fsm.CreatePerception<PushPerception>();

        fsm.CreateTransition("patrolToCombat", patrolling,
            playerSeenPerception, combat);
    }

    void CastRays() {
        visionRays.Clear();
        visionRays.Add(new Ray(head.position, transform.TransformDirection(Vector3.forward)));
        visionRays.Add(new Ray(head.position, transform.TransformDirection(Vector3.forward+Vector3.left*0.5f)));
        visionRays.Add(new Ray(head.position, transform.TransformDirection(Vector3.forward+Vector3.right*0.5f)));

        RaycastHit hit;

        foreach (var ray in visionRays) {
            if (Physics.Raycast(ray, out hit, 5)) {
                if (hit.collider.CompareTag("Player")) {
                    playerSeenPerception.Fire();
                }
               
            }
        }
    }

    
    
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(head.position, direction);
        direction = transform.TransformDirection(Vector3.forward + Vector3.left * 0.5f)*5;
        Gizmos.DrawRay(head.position, direction);
        direction = transform.TransformDirection(Vector3.forward + Vector3.right * 0.5f)*5;
        Gizmos.DrawRay(head.position, direction);
    }
}
