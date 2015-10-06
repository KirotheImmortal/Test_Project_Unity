using UnityEngine;
using System.Collections;

public class Player : UnitBase
{

    bool targeting = true;

    void Awake()
    {
        tempMat = gameObject.GetComponent<Renderer>().material;
        PartyUp();
        Subscribe(this.ToString(), GoINIT);
        Subscribe(this.ToString() + "gethit", GetHit);
        base.UnitStart();
    }



    protected override void UnitInit()
    {
      
        GoMAIN();
        base.UnitInit();
    }

    protected override void UnitMain()
    {   tempMat.color = rdy;
        gameObject.GetComponent<Renderer>().material = tempMat;
        Publish("unitmain");
        Subscribe("GUI: Attack", UnitAttack);
        Subscribe("GUI: End", GoEND);
        base.UnitMain();
    }

    protected override void UnitAttack()
    {
        StartCoroutine(SelectTarget());
    }
    private void Attacked()
    {
        GoEND();
        base.UnitAttack();
    }

    protected override void UnitEnd()
    {
        tempMat.color = fin;
        gameObject.GetComponent<Renderer>().material = tempMat;


        UnSubscribe("GUI: Attack", UnitAttack);
        UnSubscribe("GUI: End", GoEND);
        base.UnitEnd();
    }

    IEnumerator SelectTarget()
    {

        targeting = true;
        tempMat.color = att;
        gameObject.GetComponent<Renderer>().material = tempMat;
        while (targeting)
        {GameObject target;

            
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if ((hit.collider.gameObject.GetComponent<Player>()&& hit.collider.gameObject != gameObject) || hit.collider.gameObject.GetComponent<AIPlayer>() )
                    {
                        target = hit.collider.gameObject;
                        Publish(target.GetComponent<UnitBase>().ToString() + "gethit");
                        targeting = false;
                    }
                }

            }
            yield return null;
        }
        
        Attacked();

    }
}