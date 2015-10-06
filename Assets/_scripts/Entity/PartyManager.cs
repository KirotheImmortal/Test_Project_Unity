using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyManager : EventPubSub
{
    QStateMachine fsm = new QStateMachine();

    List<UnitBase> partyMembers = new List<UnitBase>();

    public string init = "init";
    public string main = "main";
    public string end = "end";

    private bool changePlayer = false;
    UnitBase NextPlayer;

    void OnEnable()
    {
        fsm.addState(init, null);
        fsm.addState(main, init);
        fsm.addState(end, null);

        Subscribe(this.ToString(), ToInit);
    }

    void ToInit()
    {
        if (fsm.changeState(init))
            StateInit();
    }
    void ToMain()
    {
        if (fsm.changeState(main))
            StateMain();
    }
    void ToEnd()
    {
        if (fsm.changeState(end))
            StateEnd();

    }

    void ChangePlayer()
    {

        if (partyMembers.IndexOf(NextPlayer) + 1 <= partyMembers.Count - 1)
        {
            NextPlayer = partyMembers[partyMembers.IndexOf(NextPlayer) + 1];
            changePlayer = true;
            UnSubscribe(partyMembers[partyMembers.IndexOf(NextPlayer) - 1].ToString() + "End", ChangePlayer);
        }
        else
        {
            UnSubscribe(partyMembers[partyMembers.IndexOf(NextPlayer)].ToString() + "End", ChangePlayer);
            NextPlayer = partyMembers[0];
            ToEnd();
        }

    }


    void StateInit()
    {if (partyMembers != null)
        {
            changePlayer = true;
            NextPlayer = partyMembers[0];
            ToMain();
        }
        ToEnd();
    }
    void StateMain()
    {
        StartCoroutine(MainPhase());

    }
    void StateEnd()
    {
        StopCoroutine(MainPhase());
        Publish(this.ToString() + "end");

    }


    IEnumerator MainPhase()
    {
        while (partyMembers.Count > 0) 
        {
            if (changePlayer == true)
            {
                if (partyMembers != null)
                {
                    Publish(partyMembers[partyMembers.IndexOf(NextPlayer)].ToString());
                    Subscribe(partyMembers[partyMembers.IndexOf(NextPlayer)].ToString() + "end", ChangePlayer);

                    changePlayer = false;
                }
                else
                    ToEnd();
            }

            yield return null;
        }

    }





    ///<summer>
    /// When called it adds a UnitBase instance to the PartyMembers list.
    ///</summer>
    public void JoinParty(UnitBase plr)
    {
        if (!partyMembers.Contains(plr))
        {
            partyMembers.Add(plr);
        }
        else
            throw new System.ArgumentException("Already exists in " + this.ToString(), plr.gameObject.name);
    }
    ///<summer>
    /// Removes a UnitBase that has been passed in from the PartyMember list.
    /// </summer>
    public void LeaveParty(UnitBase plr)
    {
        if (partyMembers.Contains(plr))
        {
            if (NextPlayer == plr)
            {
                if (partyMembers.IndexOf(NextPlayer) + 1 <= partyMembers.Count - 1)
                { print("killed and was last in list");
                    NextPlayer = partyMembers[0];
                   
                }
                else if (partyMembers.IndexOf(NextPlayer) - 1 >= 0)
                {print("Killed and was first");
                    NextPlayer = partyMembers[partyMembers.IndexOf(NextPlayer) + 1];
                    
                }
                else if (partyMembers.IndexOf(NextPlayer) - 1 < 0)
                {
                   
                    NextPlayer = null; print("Last to die");
                }
            }
            partyMembers.Remove(plr);
            if (partyMembers.Count == 0)
                Publish("PartySlane");
        }
        else throw new System.ArgumentException("Does not exist in " + this, plr.gameObject.name);
    }
    ///<summer>
    /// Returns the list of PartyMembers
    /// </summer>
    public List<UnitBase> GetPartyList()
    {
        return partyMembers;
    }
    ///<summer>
    /// Returns all the Player instances in the PartyMember list.
    /// </summer>
    public List<Player> GetPlayerList()
    {
        List<Player> plrs = new List<Player>();
        foreach (Player plr in partyMembers)
            plrs.Add(plr);

        return plrs;
    }
    ///<summer>
    /// Returns all the AIPlayer instances in the PartyMember list. 
    /// </summer>
    public List<AIPlayer> GetAIPlayerList()
    {
        List<AIPlayer> plrs = new List<AIPlayer>();
        foreach (AIPlayer plr in partyMembers)
            plrs.Add(plr);

        return plrs;
    }
    ///<summer>
    /// Returns the UnitBase with the highst Speed variable
    /// </summer>
    public UnitBase QuickestMember()
    {
        UnitBase mem = null;

        foreach (UnitBase s in partyMembers)
            if (mem == null || mem.Speed < s.Speed)
                mem = s;

        return mem;

    }
    ///<summer>
    /// Returns the UnitBase with the lowest Speed Variable
    /// </summer>
    public UnitBase SlowestMember()
    {
        UnitBase mem = null;

        foreach (UnitBase s in partyMembers)
            if (mem == null || mem.Speed > s.Speed)
                mem = s;

        return mem;
    }
    ///<summer>
    /// Returns the UnitBase with the lowest Health Variable
    /// </summer>
    public UnitBase LessHp()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Health > ub.Health)
                mem = ub;

        return mem;
    }
    ///<summer>
    /// Returns the UnitBase with the highest Health Variable
    /// </summer>
    public UnitBase MostHp()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Health < ub.Health)
                mem = ub;

        return mem;
    }
    ///<summer>
    /// Returns the UnitBase with the highest Resource Variable
    /// </summer>
    public UnitBase MostResource()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Resource < ub.Resource)
                mem = ub;

        return mem;

    }
    ///<summer>
    /// Returns the UnitBase with the lowest Reource Variable
    /// </summer>
    public UnitBase LessResource()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Resource > ub.Resource)
                mem = ub;

        return mem;
    }
    ///<summer>
    /// Returns the total Speed of all the UnitBases in PartyMembers.
    /// </summer>
    public float TotalPartySpeed()
    {
        float speed = 0.0f;
        foreach (UnitBase ub in partyMembers)
            speed += ub.Speed;

        return speed;
    }
    ///<summer>
    /// Returns the Average Speed of all the UnitBases in the PartyMembers list.
    /// </summer>
    public float AvrPartySpeed()
    {
        return TotalPartySpeed() / partyMembers.Count;
    }
    ///<summer>
    /// Returns the fastest Speed of all the UnitBases in the PartyMembers list.
    /// </summer>
    public float FastestSpeed()
    {
        return QuickestMember().Speed;
    }



}