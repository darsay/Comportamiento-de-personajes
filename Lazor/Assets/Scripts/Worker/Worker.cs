using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour {

    #region variables

    private StateMachineEngine Worker_FSM;
    

    private PushPerception AlarmWorkingPerception;
    private PushPerception NotAlarmPerception;
    private PushPerception ExitReachedPerception;
    private PushPerception NotAliveWorkingPerception;
    private PushPerception NotAliveFireAlarmPerception;
    private PushPerception NotAliveRunAwayPerception;
    private PushPerception AlarmFireAlarmPerception;
    private PushPerception PlayerOrCorpseSeenPerception;
    private State Working;
    private State RunAway;
    private State FireAlarm;
    private State Dead;
    private State Scaping;

    private NavMeshAgent _navMeshAgent;
    // Animator
    [SerializeField] private Animator _animator;
    // Patrol
    [SerializeField] private List<Transform> workerCheckPoints;
    private int currentCheckpoint = 0;
    private bool checkPointReached;
    // Vision
    private List<Ray> visionRays = new List<Ray>();
    [SerializeField] private Transform head;
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        Worker_FSM = new StateMachineEngine(false);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        CreateStateMachine();
    }
    
    
    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        AlarmWorkingPerception = Worker_FSM.CreatePerception<PushPerception>();
        NotAlarmPerception = Worker_FSM.CreatePerception<PushPerception>();
        ExitReachedPerception = Worker_FSM.CreatePerception<PushPerception>();
        NotAliveWorkingPerception = Worker_FSM.CreatePerception<PushPerception>();
        NotAliveFireAlarmPerception = Worker_FSM.CreatePerception<PushPerception>();
        NotAliveRunAwayPerception = Worker_FSM.CreatePerception<PushPerception>();
        AlarmFireAlarmPerception = Worker_FSM.CreatePerception<PushPerception>();
        PlayerOrCorpseSeenPerception = Worker_FSM.CreatePerception<PushPerception>();
        
        // States
        Working = Worker_FSM.CreateEntryState("Working", WorkingAction);
        RunAway = Worker_FSM.CreateState("RunAway", RunAwayAction);
        FireAlarm = Worker_FSM.CreateState("FireAlarm", FireAlarmAction);
        Dead = Worker_FSM.CreateState("Dead", DeadAction);
        Scaping = Worker_FSM.CreateState("Scaping", ScapingAction);
        
        // Transitions
        Worker_FSM.CreateTransition("AlarmWorking", Working, AlarmWorkingPerception, RunAway);
        Worker_FSM.CreateTransition("NotAlarm", Scaping, NotAlarmPerception, Working);
        Worker_FSM.CreateTransition("ExitReached", RunAway, ExitReachedPerception, Scaping);
        Worker_FSM.CreateTransition("NotAliveWorking", Working, NotAliveWorkingPerception, Dead);
        Worker_FSM.CreateTransition("NotAliveFireAlarm", FireAlarm, NotAliveFireAlarmPerception, Dead);
        Worker_FSM.CreateTransition("NotAliveRunAway", RunAway, NotAliveRunAwayPerception, Dead);
        Worker_FSM.CreateTransition("AlarmFireAlarm", FireAlarm, AlarmFireAlarmPerception, RunAway);
        Worker_FSM.CreateTransition("PlayerOrCorpseSeen", Working, PlayerOrCorpseSeenPerception, FireAlarm);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        Worker_FSM.Update();
        _navMeshAgent.destination = (workerCheckPoints[0].position);
        
    }

    // Create your desired actions
    
    private void WorkingAction()
    {
        
    }
    
    private void RunAwayAction()
    {
        
    }
    
    private void FireAlarmAction()
    {
        
    }
    
    private void DeadAction()
    {
        
    }
    
    private void ScapingAction()
    {
        
    }
}