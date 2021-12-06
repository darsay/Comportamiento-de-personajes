using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerFSMUS : MonoBehaviour {

    #region variables

    private StateMachineEngine NewFSM_FSM;
    private UtilitySystemEngine Trabajando_SubUS;
    

    private PushPerception AlarmaTrabajandoPerception;
    private PushPerception AlarmaDandoLaAlarmaPerception;
    private PushPerception EscaparPerception;
    private PushPerception NoAlarmaPerception;
    private PushPerception MorirTrabajandoPerception;
    private PushPerception MorirHuyendoPerception;
    private PushPerception MorirDandoLaAlarmaPerception;
    private PushPerception JugadorOCadaverVistoPerception;
    private State Huyendo;
    private State DandoLaAlarma;
    private State Muerto;
    private State Escapando;
    private State Trabajando;
    private Factor NewCurveNode;
    private Factor NewCurveNode1;
    private Factor NewCurveNode2;
    private Factor NewCurveNode3;
    private Factor NewCurveNode4;
    private Factor ganasDeOrinar;
    private Factor sed;
    private Factor cansancio;
    private Factor ganasTarea1;
    private Factor ganasTarea2;
    private UtilityAction IrAlBaño;
    private UtilityAction IrALaCafeteria;
    private UtilityAction IrADescansar;
    private UtilityAction RealizarTarea1;
    private UtilityAction RealizarTarea2;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        NewFSM_FSM = new StateMachineEngine(false);
        Trabajando_SubUS = new UtilitySystemEngine(true);
        

        CreateTrabajando_SubUS();
        CreateStateMachine();
    }
    
    private void CreateTrabajando_SubUS()
    {
        // FACTORS
        NewCurveNode = new ExpCurve(ganasDeOrinar, 1, 0, 0);
        NewCurveNode1 = new LinearCurve(sed, 1, 0);
        NewCurveNode2 = new ExpCurve(cansancio, 1, 0, 0);
        NewCurveNode3 = new LinearCurve(ganasTarea1, 1, 0);
        NewCurveNode4 = new LinearCurve(ganasTarea2, 1, 0);
        ganasDeOrinar = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        sed = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        cansancio = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        ganasTarea1 = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        ganasTarea2 = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        
        // ACTIONS
        IrAlBaño = Trabajando_SubUS.CreateUtilityAction("IrAlBaño", IrAlBañoAction, NewCurveNode);
        IrALaCafeteria = Trabajando_SubUS.CreateUtilityAction("IrALaCafeteria", IrALaCafeteriaAction, NewCurveNode1);
        IrADescansar = Trabajando_SubUS.CreateUtilityAction("IrADescansar", IrADescansarAction, NewCurveNode2);
        RealizarTarea1 = Trabajando_SubUS.CreateUtilityAction("RealizarTarea1", RealizarTarea1Action, NewCurveNode3);
        RealizarTarea2 = Trabajando_SubUS.CreateUtilityAction("RealizarTarea2", RealizarTarea2Action, NewCurveNode4);
        
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }
    
    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        AlarmaTrabajandoPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        AlarmaDandoLaAlarmaPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        EscaparPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        NoAlarmaPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        MorirTrabajandoPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        MorirHuyendoPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        MorirDandoLaAlarmaPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        JugadorOCadaverVistoPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        
        // States
        Huyendo = NewFSM_FSM.CreateState("Huyendo", HuyendoAction);
        DandoLaAlarma = NewFSM_FSM.CreateState("DandoLaAlarma", DandoLaAlarmaAction);
        Muerto = NewFSM_FSM.CreateState("Muerto", MuertoAction);
        Escapando = NewFSM_FSM.CreateState("Escapando", EscapandoAction);
        Trabajando = NewFSM_FSM.CreateSubStateMachine("Trabajando", Trabajando_SubUS);
        
        // Transitions
        NewFSM_FSM.CreateTransition("AlarmaDandoLaAlarma", DandoLaAlarma, AlarmaDandoLaAlarmaPerception, Huyendo);
        NewFSM_FSM.CreateTransition("Escapar", Huyendo, EscaparPerception, Escapando);
        NewFSM_FSM.CreateTransition("NoAlarma", Escapando, NoAlarmaPerception, Trabajando);
        NewFSM_FSM.CreateTransition("MorirHuyendo", Huyendo, MorirHuyendoPerception, Muerto);
        NewFSM_FSM.CreateTransition("MorirDandoLaAlarma", DandoLaAlarma, MorirDandoLaAlarmaPerception, Muerto);
        
        // ExitPerceptions
        
        // ExitTransitions
        Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, AlarmaTrabajandoPerception, Huyendo);
        Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, MorirTrabajandoPerception, Muerto);
        Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, JugadorOCadaverVistoPerception, DandoLaAlarma);
        
    }

    // Update is called once per frame
    private void Update()
    {
        NewFSM_FSM.Update();
        Trabajando_SubUS.Update();
    }

    // Create your desired actions
    
    private void HuyendoAction()
    {
        
    }
    
    private void DandoLaAlarmaAction()
    {
        
    }
    
    private void MuertoAction()
    {
        
    }
    
    private void EscapandoAction()
    {
        
    }
    
    private void IrAlBañoAction()
    {
        
    }
    
    private void IrALaCafeteriaAction()
    {
        
    }
    
    private void IrADescansarAction()
    {
        
    }
    
    private void RealizarTarea1Action()
    {
        
    }
    
    private void RealizarTarea2Action()
    {
        
    }
    
}