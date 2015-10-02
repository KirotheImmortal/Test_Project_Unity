using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyManager : EventPubSub
{
    QStateMachine fsm = new QStateMachine();

    List<UnitBase> partyMembers = new List<UnitBase>();

    public string init = "init";
    public string start = "start";
    public string main = "main";
    public string end = "end";

    private bool changePlayer = false;
    private int nextPlayer = 0;

    void Start()
    {
        fsm.addState(init, null);
        fsm.addState(start, init);
        fsm.addState(main, start);
        fsm.addState(end, null);
    }

    void ToInit()
    {
        fsm.changeState(init);
    }

    void ChangePlayer()
    {
        changePlayer = true;
    }

    [ContextMenu("Hell")]
    void ToMain()
    {
        StartCoroutine(MainPhase());
    }



    IEnumerator MainPhase()
    {
        while (nextPlayer < partyMembers.Count)
        {
            if (changePlayer == true)
            {
                if (partyMembers != null)
                Publish(partyMembers[nextPlayer].ToString());
                nextPlayer++;
                changePlayer = false;
            }
           
            yield return null;
        }

        fsm.changeState(end);
        Publish(this.ToString() + "End");
    }
















    public void JoinParty(Player plr)
    {
        if (partyMembers.Contains(plr))
            partyMembers.Add(plr);
        else
            throw new System.ArgumentException("Already exists in " + this, plr.gameObject.name);
    }
    public void JoinPart(AIPlayer aiplr)
    {
        if (partyMembers.Contains(aiplr))
            partyMembers.Add(aiplr);
        else throw new System.ArgumentException("Already exists in " + this, aiplr.gameObject.name);
    }

    public void LeaveParty(Player plr)
    {
        if (partyMembers.Contains(plr))
            partyMembers.Remove(plr);
        else throw new System.ArgumentException("Does not exist in " + this, plr.gameObject.name);
    }
    public void LeaveParty(AIPlayer aiplr)
    {
        if (partyMembers.Contains(aiplr))
            partyMembers.Remove(aiplr);
        else throw new System.ArgumentException("Does not exist in " + this, aiplr.gameObject.name);
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


}