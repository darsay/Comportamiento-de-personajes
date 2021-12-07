
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WorkerFSMUS : MonoBehaviour {

    #region variables

    delegate void FsmUpdate();
    private FsmUpdate fsmUpdate;
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
    private PushPerception EmpezarPerception;
    private PushPerception PruebaPerception;
    private State Huyendo;
    private State DandoLaAlarma;
    private State Muerto;
    private State Escapando;
    private State Trabajando;
    private State Inicio;
    private Factor NewCurveNode;
    private Factor NewCurveNode1;
    private Factor NewCurveNode2;
    private Factor NewCurveNode3;
    private Factor NewCurveNode4;
    private float ganasDeOrinar;
    private float sed;
    private float cansancio;
    private float ganasTarea1;
    private float ganasTarea2;
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
        fsmUpdate = TrabajandoAction;

        NewFSM_FSM = new StateMachineEngine(false);
        Trabajando_SubUS = new UtilitySystemEngine(true);
        

        CreateTrabajando_SubUS();
        InitFSM(NewFSM_FSM);
        //fsmUpdate = Inicio;
    }
    
    private void CreateTrabajando_SubUS()
    {
        // FACTORS
        
        //NewCurveNode = new ExpCurve(ganasDeOrinar, 1, 0, 0);
        //NewCurveNode1 = new LinearCurve(sed, 1, 0);
        //NewCurveNode2 = new ExpCurve(cansancio, 1, 0, 0);
        //NewCurveNode3 = new LinearCurve(ganasTarea1, 1, 0);
        //NewCurveNode4 = new LinearCurve(ganasTarea2, 1, 0);
        //ganasDeOrinar = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        //sed = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        //cansancio = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        //ganasTarea1 = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        //ganasTarea2 = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        
        // ACTIONS
        //IrAlBaño = Trabajando_SubUS.CreateUtilityAction("IrAlBaño", IrAlBañoAction, NewCurveNode);
        //IrALaCafeteria = Trabajando_SubUS.CreateUtilityAction("IrALaCafeteria", IrALaCafeteriaAction, NewCurveNode1);
        //IrADescansar = Trabajando_SubUS.CreateUtilityAction("IrADescansar", IrADescansarAction, NewCurveNode2);
        //RealizarTarea1 = Trabajando_SubUS.CreateUtilityAction("RealizarTarea1", RealizarTarea1Action, NewCurveNode3);
        //RealizarTarea2 = Trabajando_SubUS.CreateUtilityAction("RealizarTarea2", RealizarTarea2Action, NewCurveNode4);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }
    
    private void InitFSM(StateMachineEngine NewFSM_FSM)
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
        EmpezarPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        PruebaPerception = NewFSM_FSM.CreatePerception<PushPerception>();
        // States
        Huyendo = NewFSM_FSM.CreateState("Huyendo", (() => {
            fsmUpdate = HuyendoAction;
        }));

        DandoLaAlarma = NewFSM_FSM.CreateState("DandoLaAlarma", (() => {
            fsmUpdate = DandoLaAlarmaAction;
        }));
        
        Muerto = NewFSM_FSM.CreateState("Muerto", (() => {
            fsmUpdate = MuertoAction;
        }));

        Escapando = NewFSM_FSM.CreateState("Escapando", (() => {
            fsmUpdate = HuyendoAction;
        }));

        /*Inicio = NewFSM_FSM.CreateEntryState("Inicio", (() => {
            fsmUpdate = InicioAction;
        }));*/

        Trabajando = NewFSM_FSM.CreateEntryState("Trabajando", (() => {
            fsmUpdate = TrabajandoAction;
        }));
        
        /*Huyendo = NewFSM_FSM.CreateState("Huyendo", HuyendoAction);
        DandoLaAlarma = NewFSM_FSM.CreateState("DandoLaAlarma", DandoLaAlarmaAction);
        Muerto = NewFSM_FSM.CreateState("Muerto", MuertoAction);
        Escapando = NewFSM_FSM.CreateState("Escapando", EscapandoAction);
        Inicio = NewFSM_FSM.CreateEntryState("Inicio", InicioAction);
        Trabajando = NewFSM_FSM.CreateSubStateMachine("Trabajando", Trabajando_SubUS);*/
        
        // Transitions
        NewFSM_FSM.CreateTransition("AlarmaDandoLaAlarma", DandoLaAlarma, AlarmaDandoLaAlarmaPerception, Huyendo);
        NewFSM_FSM.CreateTransition("Escapar", Huyendo, EscaparPerception, Escapando);
        NewFSM_FSM.CreateTransition("NoAlarma", Escapando, NoAlarmaPerception, Trabajando);
        NewFSM_FSM.CreateTransition("MorirHuyendo", Huyendo, MorirHuyendoPerception, Muerto);
        NewFSM_FSM.CreateTransition("MorirDandoLaAlarma", DandoLaAlarma, MorirDandoLaAlarmaPerception, Muerto);
        //NewFSM_FSM.CreateTransition("Empezar", Inicio, EmpezarPerception, Trabajando);
        NewFSM_FSM.CreateTransition("Prueba", Trabajando, PruebaPerception, Escapando);
        
        // ExitPerceptions
        
        // ExitTransitions
        Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, AlarmaTrabajandoPerception, Huyendo);
        //Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, MorirTrabajandoPerception, Muerto);
        //Trabajando_SubUS.CreateExitTransition("Trabajando_SubUSExit", Trabajando, JugadorOCadaverVistoPerception, DandoLaAlarma);
        
    }

    // Update is called once per frame
    private void Update()
    {
        fsmUpdate();
        
        Debug.Log("Update");
        
        /*Debug.Log(sed.getValue());
        Debug.Log(NewCurveNode1.getValue());
        Debug.Log(ganasDeOrinar.getValue());
        Debug.Log(NewCurveNode.getValue());
        */
    }

    // Create your desired actions
    
    private void HuyendoAction()
    {
        Debug.Log("Huyendo");
    }
    
    private void DandoLaAlarmaAction()
    {
        Debug.Log("Dando la alarma");
    }
    
    private void MuertoAction()
    {
        Debug.Log("Muerto");
    }
    
    private void EscapandoAction()
    {
        Debug.Log("Escapando");
    }

    /*private void InicioAction()
    {
        Debug.Log("Inicio");
        PruebaPerception.Fire();
    }*/

    private void TrabajandoAction()
    {
        Debug.Log("Trabajando");
        Trabajando_SubUS.Update();
        sed = 0;
        //PruebaPerception.Fire();

        
    }
    
    private void IrAlBañoAction()
    {
        Debug.Log("Ir al baño");
    }
    
    private void IrALaCafeteriaAction()
    {
        Debug.Log("Ir a la cafeteria");
    }
    
    private void IrADescansarAction()
    {
        Debug.Log("Ir a descansar");
    }
    
    private void RealizarTarea1Action()
    {
        Debug.Log("Realizar Tarea 1");
    }
    
    private void RealizarTarea2Action()
    {
        Debug.Log("Realizar tarea 2");
    }
    
}