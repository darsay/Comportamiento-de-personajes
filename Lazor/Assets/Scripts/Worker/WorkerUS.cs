using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerUS : MonoBehaviour {

    #region variables

    private UtilitySystemEngine NewUS_US;
    

    private Factor NewCurveNode;
    private Factor urgeToUrinate;
    private Factor NewCurveNode1;
    private Factor thirsty;
    private Factor NewCurveNode2;
    private Factor tiredness;
    private Factor NewCurveNode3;
    private Factor urgeTaskOne;
    private Factor NewCurveNode4;
    private Factor urgeTaskTwo;
    private UtilityAction GoToBathroom;
    private UtilityAction GoToCoffeShop;
    private UtilityAction GoToRestArea;
    private UtilityAction DoTaskOne;
    private UtilityAction DoTaskTwo;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        NewUS_US = new UtilitySystemEngine(false);
        

        CreateUtilitySystem();
    }
    
    
    private void CreateUtilitySystem()
    {
        // FACTORS
        NewCurveNode = new LinearCurve(urgeToUrinate, 1, 0);
        urgeToUrinate = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        NewCurveNode1 = new LinearCurve(thirsty, 1, 0);
        thirsty = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        NewCurveNode2 = new ExpCurve(tiredness, 1, 0, 0);
        tiredness = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        NewCurveNode3 = new LinearCurve(urgeTaskOne, 1, 0);
        urgeTaskOne = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        NewCurveNode4 = new LinearCurve(urgeTaskTwo, 1, 0);
        urgeTaskTwo = new LeafVariable(() => /*Reference to desired variable*/0.0f, 100, 0);
        
        // ACTIONS
        GoToBathroom = NewUS_US.CreateUtilityAction("GoToBathroom", GoToBathroomAction, NewCurveNode);
        GoToCoffeShop = NewUS_US.CreateUtilityAction("GoToCoffeShop", GoToCoffeShopAction, NewCurveNode1);
        GoToRestArea = NewUS_US.CreateUtilityAction("GoToRestArea", GoToRestAreaAction, NewCurveNode2);
        DoTaskOne = NewUS_US.CreateUtilityAction("DoTaskOne", DoTaskOneAction, NewCurveNode3);
        DoTaskTwo = NewUS_US.CreateUtilityAction("DoTaskTwo", DoTaskTwoAction, NewCurveNode4);
        
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        NewUS_US.Update();
    }

    // Create your desired actions
    
    private void GoToBathroomAction()
    {
        
    }
    
    private void GoToCoffeShopAction()
    {
        
    }
    
    private void GoToRestAreaAction()
    {
        
    }
    
    private void DoTaskOneAction()
    {
        
    }
    
    private void DoTaskTwoAction()
    {
        
    }
    
}