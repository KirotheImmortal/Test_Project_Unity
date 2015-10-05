using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombatManager : EventPubSub
{
    [SerializeField]
    public List<PartyManager> parties = new List<PartyManager>();

    int currentParty = 0;

    void Start()
    { begin(); }

    void begin()
    {
        OrderParties();
        QueueFights();
    }

    void QueueFights()
    {
        Publish(parties[currentParty].ToString()); print(parties[currentParty].ToString());
        Subscribe(parties[currentParty].ToString() + "end", NextParty);
    }
    void NextParty()
    {
        UnSubscribe(parties[currentParty].ToString() + "end", NextParty);
        
        currentParty++;
        if (currentParty > parties.Count - 1)
        {
            OrderParties();
            currentParty = 0;
        }
                QueueFights();
    }
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


}
