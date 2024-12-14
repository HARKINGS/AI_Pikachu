using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : GridAbstract
{
    [Header("Block Handler")]
    public static int numBlockMatching = 0;
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
    protected bool nodeLinking = false;
    protected float freeNodesDelay = 1f;
    public AudioSource audioSource;
    public AudioClip matchingSound;
    public AudioClip unMatchingSound;
    public AudioClip clickSound;
    public AudioClip winSound;

    protected virtual void PlayMatchingSound()
    {
        audioSource.PlayOneShot(matchingSound);
    }

    protected virtual void PlayUnMatchingSound()
    {
        audioSource.PlayOneShot(unMatchingSound);
    }

    protected virtual void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    protected virtual void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public virtual void SetNode(BlockCtrl blockCtrl)
    {
        // Debug.Log("SetNode: " + blockCtrl.name);
        if (this.nodeLinking) return;
        if (this.IsBlockRemoved(blockCtrl)) return;

        this.PlayClickSound();

        Vector3 pos;
        Transform chooseObj;
        if (this.firstBlock == null)
        {
            this.ctrl.pathfinding.DataReset();
            this.firstBlock = blockCtrl;
            pos = blockCtrl.transform.position;
            chooseObj = this.ctrl.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
            chooseObj.gameObject.SetActive(true);
            return;
        }

        this.lastBlock = blockCtrl;
        pos = blockCtrl.transform.position;
        chooseObj = this.ctrl.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
        chooseObj.gameObject.SetActive(true);

        // Mặc định là chưa tới được vs nhau
        bool isPathFound = false;

        // Giống nhau thì tìm đường
        if (this.firstBlock != this.lastBlock
            && this.firstBlock.blockID == this.lastBlock.blockID)
        {
            isPathFound = this.ctrl.pathfinding.FindPath(this.firstBlock, this.lastBlock);
            if (isPathFound) 
            {
                numBlockMatching += 2;
                this.PlayMatchingSound();
                this.LinkNodes();

                if(numBlockMatching == 144)
                {
                    this.PlayWinSound();
                    return;
                }
            }
        }

        if(!isPathFound) 
        {
            this.PlayUnMatchingSound();
            Invoke(nameof(this.Unchoose),0.5f);
        }
        BlockDebug.Instance.ClearDebug();
    }

    protected virtual bool IsBlockRemoved(BlockCtrl blockCtrl)
    {
        Node node = blockCtrl.blockData.node;
        return !node.occupied && node.blockPlaced;
    }

    protected virtual void LinkNodes()
    {
        this.nodeLinking = true;
        this.ctrl.linesDrawer.Drawing(this.ctrl.pathfinding.PathNodes, this.freeNodesDelay);
        Invoke(nameof(this.FreeBlocks), this.freeNodesDelay);
    }

    protected virtual void FreeBlocks()
    {
        this.ctrl.gridSystem.NodeFree(this.firstBlock.blockData.node);
        this.ctrl.gridSystem.NodeFree(this.lastBlock.blockData.node);

        this.firstBlock = null;
        this.lastBlock = null;
        this.nodeLinking = false;
    }

    public virtual void Unchoose()
    {
        this.firstBlock = null;
        this.lastBlock = null;
    }
}
