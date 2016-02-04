using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombatManager : EventPubSub
{
    [SerializeField]
    public List<PartyManager> parties = new List<PartyManager>();

    int nextParty;

    void Start()
    { begin(); }
    void PartySlane()
    {


        //for (int i = 0; i < parties.Count; i++)
        //    if (parties[i].GetPartyList() == null)
        //    {

        //        //if(i == nextParty)
        //        //{

        //        //    //NextParty();
        //        //    UnSubscribe(parties[nextParty].ToString() + "end", NextParty);

        //        //}

        //        parties.Remove(parties[i]);
        //        OrderParties();
    //}

        //List<PartyManager> temp = new List<PartyManager>();
        //temp = parties;
        //foreach (PartyManager p in temp)
        //{
        //    if (p.GetPartyList() == null)
        //    {
                
        //        //print(this.ToString());
        //        temp.Remove(p);print("slut");
        //        OrderParties();
        //    }
        //}
        
        //parties = temp;
    }


    bool PartyCheck()
    {
        for (int i = 0; i < parties.Count; i++)
            if (parties[i].GetPartyList() == null)
           
             parties.Remove(parties[i]);
        //List<PartyManager>  temp = parties;

        //foreach (PartyManager p in temp)
        //{
        //    if (p.GetPartyList() == null)
        //    {
        //        parties.Remove(p);
        //        return false;
        //    }
        //}
        return true;
    }

    //Start up functions
    void begin()
    {
        OrderParties();
        nextParty = 0;
        QueueFights();

    }

    //Subscribes to listen for the currentPartie's end call. and publishes that parties call.
    void QueueFights()
    {

        Subscribe(parties[nextParty].ToString() + "end", NextParty);
        Publish(parties[nextParty].ToString());

    }
    //When called unsubscribes to the call of the previous party. Incramments currentParty. And the calls QueueFights();
    void NextParty()
    {
       UnSubscribe(parties[nextParty].ToString() + "end", NextParty);

       
        if (nextParty + 1 >= parties.Count)
        {
          

            nextParty = 0;

          
        }
        else if (nextParty + 1 < parties.Count)
        {           

            nextParty += 1;

        }
        QueueFights();
    }

    //Reorders the list based on the average speed of each party
    void OrderParties()
    {
   
        parties.Sort((x, y) => x.TotalPartySpeed().CompareTo(y.TotalPartySpeed()));
        

    }
        void Awake()
            {
            Subscribe("PartySlane", PartySlane);
            }
    }

