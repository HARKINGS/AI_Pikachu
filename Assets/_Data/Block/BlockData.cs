using UnityEngine;
using TMPro;

public class BlockData : BlockAbstract
{
    [Header("BlockData")]
    public Node node;

    // protected override void LoadComponents()
    // {
    //     base.LoadComponents();
    //     this.LoadTextMeshPro();
    // }

    // protected virtual void LoadTextMeshPro()
    // {
    //     if (this.text != null) return;
    //     this.text = transform.GetComponentInChildren<TextMeshPro>();
    //     Debug.LogWarning(transform.name + " LoadTextMeshPro", gameObject);
    // }

    public virtual void SetNode(Node node)
    {
        this.node = node;
    }

    public virtual void SetSprite(Sprite sprite)
    {
        this.ctrl.SetSprite(sprite);
        this.ctrl.blockID = sprite.name;
    }
}
