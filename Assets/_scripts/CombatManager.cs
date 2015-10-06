using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombatManager : EventPubSub
{
    [SerializeField]
    public List<PartyManager> parties = new List<PartyManager>();

    PartyManager nextParty;

    void Start()
    { begin(); }
    void PartySlane()
    {
        
        foreach (PartyManager p in parties)
        {
            if (p.GetPartyList() == null)
                parties.Remove(p);
        }
    }


    bool PartyCheck()
    {
        
        foreach(PartyManager p in parties)
        {
            if (p.GetPartyList() == null)
                parties.Remove(p);
        }
        if(parties.Count <= 1)
        {
            return false;
        }
        return true;
    }

    //Start up functions
    void begin()
    {
        OrderParties();
        nextParty = parties[0];
        QueueFights();
    }

    //Subscribes to listen for the currentPartie's end call. and publishes that parties call.
    void QueueFights()
    {
        if (parties.Contains(nextParty))
        {
            Subscribe(parties[parties.IndexOf(nextParty)].ToString() + "end", NextParty);
            Publish(parties[parties.IndexOf(nextParty)].ToString());
        }
        
    }
    //When called unsubscribes to the call of the previous party. Incramments currentParty. And the calls QueueFights();
    void NextParty()
    {
        UnSubscribe(parties[parties.IndexOf(nextParty)].ToString() + "end", NextParty);
        if (PartyCheck() == true)
        {
            print("It happend after PartyCheck().");
            if (parties.IndexOf(nextParty) + 1 > parties.Count - 1)
            {
                OrderParties();
                nextParty = parties[0];
            }
            else nextParty = parties[parties.IndexOf(nextParty) + 1];
            print("It happend after next party check.");
            QueueFights();
        }
        else
            Publish("GameOver");
                
    }
    //Reorders the list based on the average speed of each party
    void OrderParties()
    {
        List<PartyManager> templ = new List<PartyManager>();
        PartyManager last = null;
        PartyManager slower = null;
        for (int i = 0; i < parties.Count; i++)
        {
            foreach (PartyManager pm in parties)
            {


                if (templ.Count != 0)
                    last = templ[templ.Count - 1];

                if (last != null)
                {
                    if (slower == null && !templ.Contains(pm))
                        slower = pm;
                    else if (slower.AvrPartySpeed() >= pm.AvrPartySpeed() && pm.AvrPartySpeed() <= last.AvrPartySpeed() && !templ.Contains(pm))
                        slower = pm;

                   
                }
                else
                {
                    if (slower == null && !templ.Contains(pm))
                        slower = pm;
                    else if (slower.AvrPartySpeed() <= pm.AvrPartySpeed() && !templ.Contains(pm))
                        slower = pm;
                }
            }
            templ.Add(slower);
          
        }
        parties = templ;
    }
    
    void Awake()
    {
        Subscribe("PartySlane", PartySlane);
    }

}
