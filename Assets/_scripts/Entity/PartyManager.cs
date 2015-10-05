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
    private int nextPlayer = 0;

    void OnEnable()
    {
        fsm.addState(init, null);
        fsm.addState(main, init);
        fsm.addState(end, null);
       
        Subscribe(this.ToString(), ToInit);
    }

    void ToInit()
    {
        fsm.changeState(init);
        StateInit();
    }
    void ToMain()
    {
        if (fsm.changeState(main))
        {
            StateMain();
        }
    }
    void ToEnd()
    { 
        fsm.changeState(end);

        StateEnd();

    }

    void ChangePlayer()
    { UnSubscribe(partyMembers[nextPlayer - 1].ToString() + "End", ChangePlayer);
        if (nextPlayer > partyMembers.Count)
            ToEnd();
        else
         changePlayer = true;
    }


    void StateInit()
    {
        changePlayer = true;
        ToMain();
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
        while (nextPlayer < partyMembers.Count)
        {
            if (changePlayer == true)
            {
                if (partyMembers != null)
                {
                    Publish(partyMembers[nextPlayer].ToString());
                    Subscribe(partyMembers[nextPlayer].ToString() + "end", ChangePlayer);
                    nextPlayer++;
                    changePlayer = false;
                }
            }

            yield return null;
        }
       
    }







    public void JoinParty(UnitBase plr)
    {
        if (!partyMembers.Contains(plr))
        {
            partyMembers.Add(plr);
        }
        else
            throw new System.ArgumentException("Already exists in " + this.ToString(), plr.gameObject.name);
    }

    public void LeaveParty(UnitBase plr)
    {
        if (partyMembers.Contains(plr))
            partyMembers.Remove(plr);
        else throw new System.ArgumentException("Does not exist in " + this, plr.gameObject.name);
    }

    public List<UnitBase> GetPartyList()
    {
        return partyMembers;
    }

    public List<Player> GetPlayerList()
    {
        List<Player> plrs = new List<Player>();
        foreach (Player plr in partyMembers)
            plrs.Add(plr);

        return plrs;
    }
    public List<AIPlayer> GetAIPlayerList()
    {
        List<AIPlayer> plrs = new List<AIPlayer>();
        foreach (AIPlayer plr in partyMembers)
            plrs.Add(plr);

        return plrs;
    }

    public UnitBase QuickestMember()
    {
        UnitBase mem = null;

        foreach (UnitBase s in partyMembers)
            if (mem == null || mem.Speed < s.Speed)
                mem = s;

        return mem;

    }
    public UnitBase SlowestMember()
    {
        UnitBase mem = null;

        foreach (UnitBase s in partyMembers)
            if (mem == null || mem.Speed > s.Speed)
                mem = s;

        return mem;
    }

    public UnitBase LessHp()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Health > ub.Health)
                mem = ub;

        return mem;
    }
    public UnitBase MostHp()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Health < ub.Health)
                mem = ub;

        return mem;
    }

    public UnitBase MostResource()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Resource < ub.Resource)
                mem = ub;

        return mem;

    }
    public UnitBase LessResource()
    {
        UnitBase mem = null;

        foreach (UnitBase ub in partyMembers)
            if (mem == null || mem.Resource > ub.Resource)
                mem = ub;

        return mem;
    }

    public float TotalPartySpeed()
    {
        float speed = 0.0f;
        foreach (UnitBase ub in partyMembers)
            speed += ub.Speed;

        return speed;
    }
    public float AvrPartySpeed()
    {
        return  TotalPartySpeed() / partyMembers.Count;
    }
    public float FastestSpeed()
    {
        return QuickestMember().Speed;
    }



}