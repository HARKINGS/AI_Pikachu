using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// đảm bảo có xoá đi trong object thì khi add lại số liệu được đảm bảo như này
[RequireComponent(typeof(BoxCollider))]
public class BlockClickable : BlockAbstract
{
    [Header("Block Clickable")]
    public BoxCollider _collider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
    }

    protected virtual void LoadCollider()
    {
        if (this._collider != null) return;
        this._collider = GetComponent<BoxCollider>();
        this._collider.isTrigger = true;
        this._collider.size = new Vector3(0.7f, 0.9f, 0.5f);
        Debug.Log(transform.name + " LoadSpawner", gameObject);
    }

    protected void OnMouseUp()
    {
        GridManagerCtrl.Instance.SetNode(this.ctrl);
        Debug.Log(transform.name + " Block Clickable", gameObject);
    }
}
