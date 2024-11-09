using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : GridAbstract, IPathFinding
{
    [Header("Breadth First Search")]
    public List<Node> queue = new List<Node>();
    public List<Node> path = new List<Node>();
    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    public List<NodeCameFrom> cameFromNodes = new List<NodeCameFrom>();
    public List<Node> visited = new List<Node>(); // kiểm tra xem đã đến node nào

    public virtual void FindPath(BlockCtrl startBlock, BlockCtrl targetBlock)
    {
        Debug.Log("FindPath");
        Node startNode = startBlock.blockData.node;
        Node targetNode = targetBlock.blockData.node;
        // thêm điểm bắt đầu vào queue
        this.Enqueue(startNode);
        ///  gán đích
        // this.cameFrom[startNode] = targetNode;

        this.cameFromNodes.Add(new NodeCameFrom(startNode, startNode)); 
        this.visited.Add(startNode);

        while(this.queue.Count > 0)
        {
            // Lấy phần tử đầu của queue
            Node current = this.Dequeue();

            // Đến đích rồi!
            if(current == targetNode)
            {
                ConstructPath(startNode, targetNode);
                break;
            }
            
            foreach(Node neighbor in current.Neighbors())
            {
                if(neighbor == null) continue;
                /// vị trí hợp lý + chưa phải là đích
                if(this.IsValidPosition(neighbor) && !this.visited.Contains(neighbor))
                {
                    this.Enqueue(neighbor);
                    this.visited.Add(neighbor);
                    // this.cameFrom[neighbor] = current;
                    this.cameFromNodes.Add(new NodeCameFrom(neighbor, current));
                }
            }
        }

        this.ShowVisited();
        this.ShowPath();
    }

    protected virtual void ShowVisited()
    {
        foreach (Node node in this.visited)
        {
            Vector3 pos = node.nodeObj.transform.position;
            Transform keyObj = this.ctrl.blockSpawner.Spawn(BlockSpawner.SCAN, pos, Quaternion.identity);
            keyObj.gameObject.SetActive(true);
        }
    }

    protected virtual void ConstructPath(Node startNode, Node targetNode)
    {
        Node currentCell = targetNode;
        while(currentCell != startNode)
        {
            path.Add(currentCell);
            currentCell = this.GetCameFrom(currentCell);
        }

        path.Add(startNode);
        path.Reverse();
    }

    protected virtual Node GetCameFrom(Node node)
    {
        return this.cameFromNodes.Find(item => item.node == node).cameFromNode;
    }

    protected virtual void ShowPath()
    {
        Vector3 pos;
        foreach (Node node in this.visited)
        {
            pos = node.nodeObj.transform.position;
            Transform linker = this.ctrl.blockSpawner.Spawn(BlockSpawner.LINKER, pos, Quaternion.identity);
            linker.gameObject.SetActive(true);
        }
    }

    protected virtual void Enqueue(Node blockCtrl)
    {
        this.queue.Add(blockCtrl);
    }

    protected virtual Node Dequeue()
    {
        Node node = this.queue[0];
        this.queue.RemoveAt(0);
        return node;
    }

    protected virtual void ShowCameFrom()
    {
        foreach(var pair in cameFrom)
        {
            Node key = pair.Key;
            Node value = pair.Value;
            Debug.Log("Left: " + key.ToString() + ", Right: " + value.ToString());  
        }
    }

    private bool IsValidPosition(Node node)
    {
        return !node.occupied;
    }

    // protected virtual void ShowStepsDebug(List<NodeStep> steps)
    // {
    //     Debug.LogError("Steps Count: " + steps.Count);

    //     foreach (NodeStep step in steps)
    //     {
    //         Debug.Log("stepsDebug: " + step.toNode.Name());
    //     }
    //     Debug.LogError("=========================");
    // }

    protected virtual void ShowScanStep(Node currentNode)
    {
        Vector3 pos = currentNode.nodeObj.transform.position;
        Transform obj = BlockSpawner.Instance.Spawn(BlockSpawner.SCAN_STEP, pos, Quaternion.identity);
        obj.gameObject.SetActive(true);
    }
}
