using UnityEngine;
using System.Collections;

public class QGStateManager : MonoBehaviour
{

    QStateMachine FSM = new QStateMachine();

    public delegate void InitFunc();
    public delegate void RunFunc();
    public delegate void EndFunc();

    public string init = "INIT";
    public string run = "RUN";
    public string end = "END";

    public InitFunc init_func;
    public RunFunc run_func;
    public EndFunc end_func;

    void Start()
    {
        FSM.addState(init, null);
        FSM.addState(run, init);
        FSM.addState(end, null);
        init_func += randomInitFunction;
        run_func += RunControll;
        InvokeNextState();



    }


    void InvokeNextState()
    {
        if (FSM.currentState == null)
        {
            if (FSM.changeState(init))
                InvokeState();
        }

        else if (FSM.currentState.s_name == init)
        {
            if (FSM.changeState(run))
                InvokeState();
        }

        else if (FSM.currentState.s_name == run)
        {
            if (FSM.changeState(end))
                InvokeState();
        }
        else if (FSM.currentState.s_name == end)
        {
            if (FSM.changeState(init))
                InvokeState();
        }
    }
    void NextState(string state)
    {
        if (FSM.getState(state) != null)
            FSM.changeState(state);
    }
    void InvokeState()
    {
        if (init_func != null && FSM.currentState.s_name == init)
            init_func();
        else if (run_func != null && FSM.currentState.s_name == run)
            run_func();
        else if (end_func != null && FSM.currentState.s_name == end)
            end_func();
    }

    void randomInitFunction()
    {

        InvokeNextState();

    }

    void RunControll()
    {
        StartCoroutine(Controlls());
    }

    void randomEndFunction()
    {
        
    }

    IEnumerator Controlls()
    {
        bool play = true;
        while (play)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                play = !play;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Q button pressed.");
            }
            yield return null;
        }
        InvokeNextState();

    }


}
