using UnityEngine;
using System.Collections;
public interface IUnit
{
    void UnitUpdate();
    void UnitStart();
    void UnitDestroy();

}
public class UnitBase : EventPubSub, IUnit
{
    [SerializeField]
    protected PartyManager partyManager;
    [SerializeField]
    protected float _Health;
    public float Health { get { return _Health; } }
    [SerializeField]
    protected float _Resource;
    public float Resource { get { return _Resource; } }
    [SerializeField]
    protected float _Speed;
    public float Speed { get { return _Speed; } }


   protected Color att = Color.white;
   protected Color fin = Color.red;
   protected Color rdy = Color.blue;
   protected Color ded = Color.black;
   protected Material tempMat;

    protected string StateINIT = "init";
    protected string StateMAIN = "main";
    protected string stateEND = "end";

    protected QStateMachine fsm = new QStateMachine();



    public virtual void UnitDestroy()
    {
        Destroy(gameObject);
    }
    public virtual void UnitStart()
    {
        fsm.addState(StateINIT, null);
        fsm.addState(StateMAIN, StateINIT);
        fsm.addState(stateEND, null);

        Publish("Unit" + gameObject.name + "Started");
    }
    public virtual void UnitUpdate()
    {
        Publish("Unit" + gameObject.name + "Updated");
    }



    protected void PartyUp()
    {
        partyManager.JoinParty(this);
    }
    protected void LeaveParty()
    {
        partyManager.LeaveParty(this);
    }



    protected void InvokeStateFunction()
    {
        if (fsm.currentState == fsm.getState(StateINIT))
            UnitInit();
        else if (fsm.currentState == fsm.getState(StateMAIN))
            UnitMain();
        else if (fsm.currentState == fsm.getState(stateEND))
            UnitEnd();
    }
    protected void InvokeStateShift()
    {
        bool statechanged = false;
        if (fsm.currentState == fsm.getState(StateINIT))
        {
            fsm.changeState(StateMAIN);
            statechanged = true;
        }
        else if (fsm.currentState == fsm.getState(StateMAIN))
        {
            fsm.changeState(stateEND);
            statechanged = true;
        }
        else if (fsm.currentState == fsm.getState(stateEND))
        {
            fsm.changeState(StateINIT);
            statechanged = true;
        }


        if (statechanged)
            InvokeStateFunction();
    }
    protected void GoMAIN()
    {
        if (fsm.currentState == fsm.getState(StateINIT))
        {
            fsm.changeState(StateMAIN);
            UnitMain();
        }
            
    }
    protected void GoINIT()
    {
        if (fsm.currentState != fsm.getState(StateMAIN))
        {
            fsm.changeState(StateINIT);
            UnitInit();
        }
            
    }
    protected void GoEND()
    {
        fsm.changeState(stateEND);
        UnitEnd();
    }

    protected void GetHit()
    {
        _Health -= 5;
        if(_Health<=0)
        {
            LeaveParty();
            tempMat.color = ded;
            gameObject.GetComponent<Renderer>().material = tempMat;
        }
    }

    virtual protected void UnitInit()
    {
        Publish(this.ToString() + "Init");
    }
    virtual protected void UnitMain()
    {
        Publish(this.ToString() + "Main");
    }
    virtual protected void UnitAttack()
    {
        Publish(this.ToString() + "Attack");
    }
    virtual protected void UnitEnd()
    {
        Publish(this.ToString() + "End");
    }


}
