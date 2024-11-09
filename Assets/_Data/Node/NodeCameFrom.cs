using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeCameFrom 
{
    // kiểm tra xem node đến từ cameFromNode (cameFromNode -> node)
    public Node node; 
    public Node cameFromNode;

    public NodeCameFrom(Node node, Node cameFromNode)
    {
        this.node = node;
        this.cameFromNode = cameFromNode;
    }
}
