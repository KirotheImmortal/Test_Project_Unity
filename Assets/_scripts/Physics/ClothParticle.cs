using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClothParticle : MonoBehaviour
{

    static public ClothParticle instance
    {
        get; private set;
    }

    [SerializeField]
    private List<GameObject> Anchors;

    private class SpringDamper
    {
        float SpringConstant, DampingFactor;
        float RestLength;
        ClothParticle a, b;
        //public Vector3 ComputeForce()
        //{
        //    return null;
        //}

    }

    Vector3 pos, vel, accel, mom, force;

    [SerializeField]
    float _mass =  0;

    public float mass
    {
        get
        { return _mass; }
        private set
        {
            _mass = value;
        }
    }


    void Awake()
    {
        pos = gameObject.transform.position;
        vel = accel = mom = force = Vector3.zero;
        if (mass != 0)
        {
            if (gameObject.GetComponent<Collider>())
                mass = gameObject.GetComponent<Collider>().bounds.size.x
                    * gameObject.GetComponent<Collider>().bounds.size.y
                    * gameObject.GetComponent<Collider>().bounds.size.z;

            else if (gameObject.GetComponent<Renderer>())

                mass = gameObject.GetComponent<Renderer>().bounds.size.x
                    * gameObject.GetComponent<Renderer>().bounds.size.y
                    * gameObject.GetComponent<Renderer>().bounds.size.z;

            else
                mass = transform.lossyScale.x 
                    * transform.lossyScale.y 
                    * transform.lossyScale.z;
        }
    }

    Vector3 CalAccel()
    {
        return accel = (1 / mass) * force;
    }
    Vector3 CalVel()
    {
        return vel = vel + accel * Time.deltaTime;
    }
    Vector3 CalPos()
    {
        return pos = pos + CalVel() * Time.deltaTime;
    }

    Vector3 Gravity()
    {
        return mass * (new Vector3(0, -9.8f, 0) / (Time.deltaTime * Time.deltaTime));
    }


}
