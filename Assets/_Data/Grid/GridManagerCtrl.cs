using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerCtrl : SaiMonoBehaviour
{
    [Header("Grid Manager Ctrl")]
    private static GridManagerCtrl instance;
    public static GridManagerCtrl Instance => instance;

    public BlockSpawner blockSpawner;
    public IPathFinding pathFinding;
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
    

    protected override void Awake()
    {
        base.Awake();
        if (GridManagerCtrl.instance != null) Debug.LogError("Only 1 GridManagerCtrl allow to exist");
        GridManagerCtrl.instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawner();
        this.LoadPathFinding();
    }

    protected virtual void LoadSpawner()
    {
        if (this.blockSpawner != null) return;
        this.blockSpawner = transform.Find("BlockSpawner").GetComponent<BlockSpawner>();
        Debug.LogWarning(transform.name + " LoadSpawner", gameObject);
    }

    protected virtual void LoadPathFinding()
    {
        if (this.pathFinding != null) return;
        this.pathFinding = transform.GetComponentInChildren<IPathFinding>();
        Debug.LogWarning(transform.name + " LoadPathFinding", gameObject);
    }

    public virtual void SetNode(BlockCtrl blockCtrl)
    {
        if(this.firstBlock != null && this.lastBlock != null)   
        {
            this.pathFinding.FindPath(this.firstBlock, this.lastBlock);
            this.firstBlock = null;
            this.lastBlock = null;
            Debug.Log("Reset Block");
            return;
        }

        if(this.firstBlock == null) 
        {
            this.firstBlock = blockCtrl;
            return;
        }

        this.lastBlock = blockCtrl;        
    }

    // protected virtual void LoadGridSystem()
    // {
    //     if (this.gridSystem != null) return;
    //     this.gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();
    //     Debug.LogWarning(transform.name + " LoadGridSystem", gameObject);
    // }
}
