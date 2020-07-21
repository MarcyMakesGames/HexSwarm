﻿using System;
using UnityEngine;

public class PathfindingCell
{
    public PathfindingCell Parent { get; set; }
    public float FCost { get; set; }
    public float GCost { get; set; }
    public float HCost { get; set; }    
    public Vector3Int Location { get; }

    public PathfindingCell (Vector3Int location)
    {
        Location = location;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
}
