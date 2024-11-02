using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridAbstract : SaiMonoBehaviour
{
    [Header("Grid Abstract")]
    public GridManagerCtrl ctrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCtrl();
    }

    protected virtual void LoadCtrl()
    {
        if (this.ctrl != null) return;
        this.ctrl = transform.parent.GetComponent<GridManagerCtrl>();
        Debug.LogWarning(transform.name + " LoadCtrl", gameObject);
    }

    // protected virtual void LoadCtrl()
    // {
    //     if (this.ctrl != null) return;
    //     if (transform.parent != null)
    //     {
    //         this.ctrl = transform.parent.GetComponent<GridManagerCtrl>();
    //         if (this.ctrl == null)
    //         {
    //             Debug.LogError(transform.name + " parent does not contain GridManagerCtrl", gameObject);
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError(transform.name + " has no parent", gameObject);
    //     }
    // }
}
