using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour
{
    List<int> ints = new List<int>();
   
    void Start()
    {
        ints.Add(1);
        ints.Add(2);
        ints.Add(3);

        print(ints[1].ToString());

        ints.RemoveAt(1);

        print(ints[1].ToString());
    }

}
