using UnityEngine;
using System.Collections;

public class Player : UnitBase
{
    GameObject target;



    protected override void UnitInit()
    {
        Subscribe("GUI: Attack", UnitAttack);
        Subscribe("GUI: End", UnitEnd);
        fsm.changeState(StateMAIN);
        base.UnitInit();
    }

    protected override void UnitMain()
    {

        base.UnitMain();
    }

    protected override void UnitAttack()
    {

        StartCoroutine(SelectTarget());

    }
    private void Attacked()
    {
        fsm.changeState(stateEND);
        base.UnitAttack();
    }

    protected override void UnitEnd()
    {
        UnSubscribe("GUI: Attack", UnitAttack);
        UnSubscribe("GUI: End", UnitEnd);
        base.UnitEnd();
    }

    IEnumerator SelectTarget()
    {
        print("IEnum");
        while (target == null)
        {
            print("IEnum");

            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() || hit.collider.gameObject.GetComponent<AIPlayer>())
                    {
                        target = hit.collider.gameObject;
                        print(gameObject.name + " -attacking-> " + target.name);
                    }
                }

            }
            yield return null;

        }
        target = null;
        Attacked();

    }
}