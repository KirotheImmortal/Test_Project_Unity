using UnityEngine;
using System.Collections;

public class UnitGUI : EventPublisher
{

    public void Attk()
    {
        Publish("GUI: Attack");
    }
    public void End()
    {
        Publish("GUI: End");
    }

}
