using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClickable : BlockAbstract
{
    public BoxCollider _collider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
    }

    protected virtual void LoadCollider()
    {
        if (this._collider == null) return;
        this._collider = GetComponent<BoxCollider>();
        this._collider.isTrigger = true;
        this._collider.size = new Vector3(0.7f, 0.9f, 0.5f);
        Debug.Log(transform.name + " LoadSpawner", gameObject);
    }

    protected void OnMouseUp()
    {
        Debug.Log(transform.name + " BlockClickable", gameObject);
    }
}
