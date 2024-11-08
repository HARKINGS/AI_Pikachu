using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : GridAbstract
{
    [Header("Grid System")]
    public int width = 18;
    public int height = 11;
    private float offsetX = 0.19f;
    public List<Node> nodes;
    public List<int> nodeIds;
    public BlocksProfile blocksProfile;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.InitGridSystem();
        this.LoadBlocksProfile();
    }

    protected virtual void LoadBlocksProfile()
    {
        if(this.blocksProfile != null) return;
        this.blocksProfile = Resources.Load<BlocksProfile>("Pikachu");
        Debug.LogWarning(transform.name + " LoadBlocksProfile", gameObject);
    }

    protected override void Start()
    {
        // this.SpawnNodes();
        this.SpawnBlocks();
    }

    protected virtual void InitGridSystem() 
    {
        if(this.nodes.Count > 0) return;

        int nodeId = 0;
        for(int x = 0; x < this.width; ++x)
            for(int y = 0; y < this.height; ++y)
            {
                Node node = new Node 
                {
                    x = x, 
                    y = y,
                    posX = x - (this.offsetX * x),
                    nodeId = nodeId,
                };
                this.nodes.Add(node);
                this.nodeIds.Add(nodeId);
                nodeId++;
            }
    }

    protected virtual void SpawnNodes() 
    {
        Vector3 pos = Vector3.zero;
        foreach(Node node in this.nodes)
        {
            if (node.x == 0 || 
                node.y == 0 || 
                node.x == this.width - 1 || 
                node.y == this.height - 1)
                continue;

            pos.x = node.posX;
            pos.y = node.y;
            Transform block = this.ctrl.blockSpawner.Spawn(BlockSpawner.BLOCK, pos, Quaternion.identity);
            BlockCtrl blockCtrl = block.GetComponent<BlockCtrl>();

            block.gameObject.SetActive(true);
        }
    }

    protected virtual void SpawnBlocks()
    {
        Vector3 pos = Vector3.zero;
        int blockCount = 4;
        foreach (Sprite sprite in this.blocksProfile.sprites)
        {
            for (int i = 0; i < blockCount; i++)
            {
                Node node = this.GetRandomNode();
                pos.x = node.posX;
                pos.y = node.y;

                Transform block = this.ctrl.blockSpawner.Spawn(BlockSpawner.BLOCK, pos, Quaternion.identity);
                BlockCtrl blockCtrl = block.GetComponent<BlockCtrl>();
                
                blockCtrl.blockData.SetSprite(sprite);

                // GridManagerCtrl.Instance.gridSystem.blocks.Add(blockCtrl);

                this.LinkNodeBlock(node, blockCtrl);
                block.name = "Block_" + node.x.ToString() + "_" + node.y.ToString();

                block.gameObject.SetActive(true);

                // this.NodeOccupied(node);
            }
        }
    }

    protected virtual Node GetRandomNode()
    {
        Node node;
        int randId;
        int nodeCount = this.nodes.Count;
        for(int i = 0; i < nodeCount; ++i)
        {
            randId = Random.Range(0, this.nodeIds.Count);
            node = this.nodes[this.nodeIds[randId]];
            this.nodeIds.RemoveAt(randId);

            if (node.x == 0 || 
                node.y == 0 || 
                node.x == this.width - 1 || 
                node.y == this.height - 1)
                continue;

            if(node.blockCtrl == null) 
                return node;
        }

        Debug.LogError("Node can't found, this should not happen");
        return null;
    }

    protected virtual void LinkNodeBlock(Node node, BlockCtrl blockCtrl)
    {
        blockCtrl.blockData.SetNode(node);
        node.blockCtrl = blockCtrl;
    }
}
