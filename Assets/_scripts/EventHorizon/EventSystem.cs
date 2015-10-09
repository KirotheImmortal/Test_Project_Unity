using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void Callback();

public class EventSystem : MonoBehaviour
{

    /// <summary>
    /// List of "Published" strings
    /// </summary>


    /// <summary>
    /// Dictionary of Subscribers
    /// String value is what message the "Subscriber" is looking for
    /// OnEvent is the function the "Subscriber" wants played in its corresponding event
    /// </summary>
    Dictionary<string, Callback> Subscribers = new Dictionary<string, Callback>();




    /// <summary>
    /// Adds a String and OnEvent to the Susbribers Dictionary
    /// </summary>
    /// <param name="sPub"> As stated before. The awaited message from publisher</param>
    /// <param name="onEvent">OnEvent functions to be called when message is later resieved</param>
    public void Subsribe(string sPub, Callback onEvent)
    {
        if (Subscribers != null && Subscribers.ContainsKey(sPub.ToLower()))
            Subscribers[sPub.ToLower()] += onEvent;
        else
            Subscribers.Add(sPub.ToLower(), onEvent);
    }
    /// <summary>
    /// Adds message to the List to later be used to invoke Subscriber funcitons
    /// </summary>
    /// <param name="sPubEvent"></param>
    public void AddPublishedEvent(string sPubEvent)
    {
        //  Publishes.Add(sPubEvent.ToLower());

        if (Subscribers.ContainsKey(sPubEvent.ToLower()) && Subscribers[sPubEvent.ToLower()] != null)
            Subscribers[sPubEvent.ToLower()]();

    }

    /// <summary>
    /// Removes the Callback delegate associated with the string msg
    /// Removes the Key if the delegate becomes empty
    /// </summary>
    /// <param name="sPub"></param>
    /// <param name="onEvent"></param>
    public void RemoveSubscription(string sPub, Callback onEvent)
    {
        sPub = sPub.ToLower();
        if (Subscribers.ContainsKey(sPub))
        {
           
            Subscribers[sPub] -= onEvent;
            if (Subscribers[sPub] == null)
                Subscribers.Remove(sPub);
        }
        else throw new ArgumentException("Subscription to '" + sPub.ToLower(), this.ToString());
    }





    
}
