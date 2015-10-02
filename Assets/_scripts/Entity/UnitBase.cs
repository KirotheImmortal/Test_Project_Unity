using UnityEngine;
using System.Collections;
public interface IUnit
{
    void EntityUpdate();
    void EntityStart();
    void EntityDestroy();

}
public class UnitBase : EventPubSub, IUnit
{
    [SerializeField]
    protected PartyManager partyManager;
    
    public float Health { get; protected set; }
    public float Resource { get; protected set; }
    public float Speed { get; protected set; }

    protected string StateINIT = "init";
    protected string StateMAIN = "main";
    protected string StateAttk = "attack";
    protected string stateEND = "end";

   protected QStateMachine fsm = new QStateMachine();

   

    public virtual void EntityDestroy()
    {
        Destroy(gameObject);
    }
    public virtual void EntityStart()
    {
        fsm.addState(StateINIT, null);
        fsm.addState(StateMAIN, StateINIT);
        fsm.addState(StateAttk, StateMAIN);
        fsm.addState(stateEND, null);

        Publish("Unit " + gameObject.name + " Started");
    }
    public virtual void EntityUpdate()
    {
        Publish("Unit " + gameObject.name + " Updated");
    }

    virtual protected void UnitInit()
    {
        Publish("Unit " + gameObject.name + " Init");
    }
    virtual protected void UnitMain()
    {
        Publish("Unit " + gameObject.name + " Main");
    }
    virtual protected void UnitAttack()
    {
        Publish("Unit " + gameObject.name + " Attack");
    }
    virtual protected void UnitEnd()
    {
        Publish("Unit " + gameObject.name + " End");
    }


}
