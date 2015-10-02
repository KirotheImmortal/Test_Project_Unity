using UnityEngine;
using System.Collections;
using System;

public interface IEnt
{
    void EntityUpdate();
    void EntityStart();
    void EntityDestroy();

}


public class EntityBase : EventPubSub, IEnt
{
    public virtual void EntityDestroy()
    {
        throw new NotImplementedException();
    }

    public virtual void EntityStart()
    {
        throw new NotImplementedException();
    }

    public virtual void EntityUpdate()
    {
        throw new NotImplementedException();
    }


}
