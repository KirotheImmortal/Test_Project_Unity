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

    //private bool changePlayer = false;
    private int nextPlayer;

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
        // if (partyMembers != null)
        //   {
        if (nextPlayer + 1 <= partyMembers.Count - 1)
        {

            if (nextPlayer >= 0)
                UnSubscribe(partyMembers[nextPlayer].ToString() + "End", ChangePlayer);



            nextPlayer += 1;
            //changePlayer = true;

            Publish(partyMembers[nextPlayer].ToString());
            Subscribe(partyMembers[nextPlayer].ToString() + "end", ChangePlayer);

            // changePlayer = false;
        }
        else
        {
            if (nextPlayer >= 0)
                UnSubscribe(partyMembers[nextPlayer].ToString() + "End", ChangePlayer);
            // nextPlayer = 0;
            ToEnd();
        }
        


        //  }
        //  else Publish("PartySlane");

    }
    void Dead()
    {
        UnSubscribe(this.ToString(), ToInit);
    }


    void StateInit()
    {
        if (partyMembers.Count > 0)
        {
            // changePlayer = true;
            nextPlayer = -1;
            ToMain();
        }
        else
        {
            ToEnd();
            Publish("PartySlane");
        }
    }
    void StateMain()
    {
        ChangePlayer();

    }
    void StateEnd()
    {

        nextPlayer = -1;
        Publish(this.ToString() + "end");

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
            if (nextPlayer >= 0 && (partyMembers[nextPlayer] == plr))
            {
                if (nextPlayer + 1 <= partyMembers.Count - 1)
                {
                    P
                    nextPlayer = -1;
                }

            }
            else if (nextPlayer + 1 > partyMembers.Count - 1)
            {
                nextPlayer -= 1;
            }
            partyMembers.Remove(plr);

        }
        else throw new System.ArgumentException("Does not exist in " + this, plr.gameObject.name);
    }
    ///<summer>
    /// Returns the list of PartyMembers
    /// </summer>
    public List<UnitBase> GetPartyList()
    {
        if (partyMembers.Count == 0)
            return null;
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