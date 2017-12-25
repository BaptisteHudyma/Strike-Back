using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileType{
    public string Name;
    public GameObject TileVisual;
    public bool IsWalkable = true;
    public float WalkCost;
}

public class Weapon
{
    public GameObject WeaponVisal;
    public int Damage;
    public int Cost;
    public int MaxRange;
    public bool IsArea;
    public string Name;
    public int WeaponID;
    public Weapon(string name, int dammage, int cost, int range, bool Area, int ID)
    {
        Name = name;
        Damage = dammage;
        Cost = cost;
        MaxRange = range;
        IsArea = Area;
        WeaponID = ID;
    }
}

public class Tile
{
    public int ActualTile;
    public bool UnitOnIt;
    public GameObject TileGO;
}

public class Node{
    public bool walkable;
    public List<Node> neighbours;
    public float DepCost;
    public int PosX;
    public int PosZ;
    public int GCost;
    public int HCost;
    public Node ParentNode;
    public bool UnitOnIt;

    public Node(bool _walkable, float _DepCost)
    {
        walkable = _walkable;
        neighbours = new List<Node>();
        ParentNode = null;
        DepCost = _DepCost;
        
    }

    public int FCost{
        get { return GCost + HCost;  }
    }
}