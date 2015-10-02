using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

//
// Publisher:
// Pranay Rana
//
// Location:
// http://www.codeproject.com/Articles/866547/Publisher-Subscriber-pattern-with-Event-Delegate-a
//

/*
The -readonly- keyword is different from the const keyword.
A const field can only be initialized at the declaration of the field. 
A readonly field can be initialized either at the declaration or in a constructor. 
Therefore, readonly fields can have different values depending on the constructor used. 
Also, while a const field is a compile-time constant, the readonly field can be used for runtime constants as in the following example:
*/

/// <summary>
/// /////////////////////////Event Message Class Object///////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageArgument<T> : EventArgs
{
    public T Message { get; private set; }
    public MessageArgument(T message)
    {
        Message = message;
    }
}
/// <summary>
/// //////////////////////////Interface for Publisher////////////////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPublisher<T>
{
    event EventHandler<MessageArgument<T>> DataPublisher;

    void PublishData(T data);
}

/// <summary>
/// ////////////////////Publisher Class///////////////////////////
/// </summary>
/// <typeparam name="T"></typeparam>
public class Publisher<T> : IPublisher<T>
{
    //Defined datapublisher event
    public event EventHandler<MessageArgument<T>> DataPublisher; //Creates an EventHandler for the MessageArgument<T> Class named as DataPublisher

    private void OnDataPublisher(MessageArgument<T> args) // Takes in MessageArgument<T> as 'args'
    {
        var handler = DataPublisher; // sets handler to DataPublisher
        if (handler != null) //Checks to make sure handler is not null
            handler(this, args); //takes in -this- as the Sender and 'args' as the TEventArgs
    }


    public void PublishData(T data)    // IPublish interface func. Takes in T as 'data'
    {
        MessageArgument<T> message = (MessageArgument<T>)Activator.CreateInstance(typeof(MessageArgument<T>), new object[] { data }); // creates a MessageArgument<T> as 'message' to an instance of a MessageArgument with a data type

        OnDataPublisher(message);
    }
}


public class Subscription<Tmessage> : IDisposable
{
    public readonly MethodInfo MethodInfo;
    private readonly EventAggregator EventAggregator;
    public readonly WeakReference TargetObjet;
    public readonly bool IsStatic;

    private bool isDisposed;
    public Subscription(Action<Tmessage> action, EventAggregator eventAggregator)
    {
        MethodInfo = action.Method;
        if (action.Target == null)
            IsStatic = true;
        TargetObjet = new WeakReference(action.Target);
        EventAggregator = eventAggregator;
    }

    ~Subscription()
    {
        if (!isDisposed)
            Dispose();
    }

    public void Dispose()
    {
        EventAggregator.UnSbscribe(this);
        isDisposed = true;
    }

    public Action<Tmessage> CreatAction()
    {
        if (TargetObjet.Target != null && TargetObjet.IsAlive)
            return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), TargetObjet.Target, MethodInfo);
        if (this.IsStatic)
            return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), MethodInfo);

        return null;
    }
}
public class EventAggregator
{
    private readonly object lockObj = new object(); //Creates a read only object as 'lockObj'
    private Dictionary<Type, IList> subscriber; //Creates a dictionary of Type and IList as 'subscriber'

    public EventAggregator()
    {
        subscriber = new Dictionary<Type, IList>(); //Sets subscriber = to a new Dictionary
    }
    /// <summary>
    /// is method used to publishing message. As in code this method does receives message as input than it filter out list of all subscriber by message type and publish message to Subscriber.
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    /// <param name="message"></param>
    public void Publish<TMessageType>(TMessageType message)
    {
        Type t = typeof(TMessageType); // sets t to the Type that Tmessage is 
        IList sublst;   // Initialize 'sublst';
        if (subscriber.ContainsKey(t)) // checks to see if subscriber has type t as a key
        {
            //The 'lock' keyword ensures that one thread does not enter a critical section of code while another thread is in the critical section.
            //If another thread tries to enter a locked code, it will wait, block, until the object is released.
            lock (lockObj) // locks lockObj
            {
                sublst = new List<Subscription<TMessageType>>(subscriber[t].Cast<Subscription<TMessageType>>());// sets 'sublst' to a new List based on dictionary 'subscriber'
            }

            foreach (Subscription<TMessageType> sub in sublst) // Loops threw 'sublist' as 'sub'
            {
                var action = sub.CreatAction(); // stores 'sub' as an action in 'action'
                if (action != null)  //Checks to make sure 'action' is not null
                    action(message); //adds message to action or sets. one of the two
            }
        }
    }
    /// <summary>
    ///  is method used to subscribe interested message type.
    ///  As in code this method receives Action delegate as input.
    ///  It maps Action to particular MessageType, i.e. it create entry for message type if not present in dictionary and maps Subscription object  (which waps Action) to message entry.
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public Subscription<TMessageType> Subscribe<TMessageType>(Action<TMessageType> action)
    {
        Type t = typeof(TMessageType); //sets 't' to type of 'TMessageType'
        IList actionlst; //Initialize 'actionlist'
        var actiondetail = new Subscription<TMessageType>(action, this); // sets 'actiondetail' as a new Subscription<TMessageType>(action,this)

        lock (lockObj) //locksObject
        {
            if (!subscriber.TryGetValue(t, out actionlst)) //Checks to see if a value 't' does not exist in 'actionlist'
            {
                actionlst = new List<Subscription<TMessageType>>(); //sets 'actionlst' to a new List<Subscription<TmessageType>>()
                actionlst.Add(actiondetail); // adds 'actiondetail' to the 'actionlst'
                subscriber.Add(t, actionlst); //adds 't' as key and 'actionlst' as value into 'subscriber'
            }
            else     //else
            {
                actionlst.Add(actiondetail); // adds 'actiondetail' to the 'actionlst'
            }
        }

        return actiondetail; //returns 'actiondetail'
    }
    /// <summary>
    /// is method used to unsubscribe form particular message type. It receives Subscription object as input and remove object from the dictionary.
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    /// <param name="subscription"></param>
    public void UnSbscribe<TMessageType>(Subscription<TMessageType> subscription)
    {
        Type t = typeof(TMessageType); //Sets 't' as type of 'TMessageType'
        if (subscriber.ContainsKey(t)) //checks to see if 'subscriber' holds key of 't'
        {
            lock (lockObj) // locks 'lockObj'
            {
                subscriber[t].Remove(subscription); //Removes 'subscription' from 'subscriber'['t']
            }
            subscription = null; // sets 'subscription' to null
        }
    }

}