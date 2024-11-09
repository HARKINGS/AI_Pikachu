using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinding
{
    public abstract void FindPath(BlockCtrl startBlock, BlockCtrl targetBlock);
}
