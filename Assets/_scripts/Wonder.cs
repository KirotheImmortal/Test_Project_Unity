using UnityEngine;
using System.Collections;

public class Wonder : MonoBehaviour
{
    [SerializeField]
    private Vector3 nextPos;
    private Vector3 originPos;

    public float speed;

    [SerializeField]
    private float range;
    [SerializeField]
    private Vector3 view;

    void Start()
    {
        originPos = transform.position;
    }


    void Update()
    {
        if (.1f < range)
        {
            range = 0;
            Vector2 temp = Random.insideUnitSphere;
            nextPos = new Vector3(temp.x + transform.forward.x + transform.position.x, 0, temp.y + transform.forward.z + transform.position.z);
        }


          //transform.forward = nextPos;
          //Debug.DrawLine(transform.forward + transform.position, nextPos,Color.red);
          //Debug.DrawLine(transform.position, transform.forward + transform.position,Color.blue);

        transform.forward = Vector3.Lerp(transform.forward, nextPos, Time.deltaTime);

        Debug.DrawLine(transform.position, (transform.forward + transform.position) * speed * Time.deltaTime, Color.cyan);
        
        view = transform.position = (transform.forward + transform.position) *speed * Time.deltaTime;
       //Debug.DrawLine(transform.position, transform.forward + transform.position, Color.black);

        range += Time.deltaTime;
    }


}
