using UnityEngine;
using System.Collections;

public delegate void CallBack();
public class UIHoverBase : MonoBehaviour
{    
    protected CallBack vdCallBack = null;

    protected virtual void OnMouseOver()
    {
        if(vdCallBack != null)
        vdCallBack();
    }
    public virtual void AddToCall(CallBack cb)
    {
        vdCallBack += cb;
    }
    public virtual void RemoveCall(CallBack cb)
    {
        vdCallBack -= cb;
    }



}
