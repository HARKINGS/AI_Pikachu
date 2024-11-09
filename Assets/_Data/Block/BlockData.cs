using UnityEngine;
using TMPro;

public class BlockData : BlockAbstract
{
    [Header("BlockData")]
    public TextMeshPro text;
    public Color textColor = Color.red;
    public Node node;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTextMeshPro();
    }

    protected virtual void LoadTextMeshPro()
    {
        if (this.text != null) return;
        this.text = transform.GetComponentInChildren<TextMeshPro>();
        Debug.LogWarning(transform.name + " LoadTextMeshPro", gameObject);
    }

    public virtual void SetNode(Node node)
    {
        this.node = node;
        // this.text.text = this.node.nodeId.ToString();
        this.text.text = this.node.x.ToString() + "\n" + this.node.y.ToString();
        this.SetColor(this.textColor);
    }

    public virtual void SetSprite(Sprite sprite)
    {
        this.ctrl.SetSprite(sprite);
        this.ctrl.blockID = sprite.name;
    }

    public virtual void SetColor(Color color)
    {
        this.text.color = color;
    }
}
