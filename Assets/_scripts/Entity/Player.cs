using UnityEngine;
using System.Collections;

public class Player : UnitBase
{
    bool targeting = true;

    public string Name;

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
        
        Subscribe("GUI: Attack", UnitAttack);
        Subscribe("GUI: End", GoEND);
        base.UnitMain();
    }

    protected override void UnitAttack()
    {
        StartCoroutine("SelectTarget");
    }


    protected override void UnitEnd()
    {
        tempMat.color = fin;
        gameObject.GetComponent<Renderer>().material = tempMat;

        StopCoroutine("SelectTarget");
        UnSubscribe("GUI: Attack", UnitAttack);
        UnSubscribe("GUI: End", GoEND);
        base.UnitEnd();
    }
    protected override void GetHit()
    {
 
        UnSubscribe(this.ToString(), GoINIT);
        UnSubscribe(this.ToString() + "gethit", GetHit);
        base.GetHit();
    }

    IEnumerator SelectTarget()
    {
        targeting = true;
        tempMat.color = att;
        gameObject.GetComponent<Renderer>().material = tempMat;
        while (targeting)
        {GameObject target;

            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if ((hit.collider.gameObject.GetComponent<Player>() && hit.collider.gameObject != gameObject)
                        || hit.collider.gameObject.GetComponent<AIPlayer>() )
                    {
                        target = hit.collider.gameObject;
                        Publish(target.GetComponent<UnitBase>().ToString() + "gethit");
                        targeting = false;
                    }
                }

            }
            yield return null;
        }
        
        GoEND();

    }
}