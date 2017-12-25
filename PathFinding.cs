using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {
    public Node [,]Grille;

    TileMap Map;

    int SizeMapX;
    int SizeMapZ;

    public void AwakePathFinding(){
        Map = GetComponent<TileMap>();
        SizeMapX = Map.sizeMapX;
        SizeMapZ = Map.sizeMapZ;

        Grille = new Node[SizeMapX, SizeMapZ];

        for (int X = 0; X < SizeMapX; X++)
        {
            for (int Z = 0; Z < SizeMapZ; Z++)
            {
                TileType TM = Map.tileTypes[Map.tiles[X, Z].ActualTile];
                Grille[X, Z] = new Node(TM.IsWalkable, TM.WalkCost);
                Grille[X, Z].PosX = X;
                Grille[X, Z].PosZ = Z;
                Grille[X, Z].UnitOnIt = Map.tiles[X, Z].UnitOnIt;
            }
        }
    }
    
    
    List<Node> GetPath(Node Start, Node End)
    {
        List<Node> path = new List<Node>();
        Node current = End;
        
        while (current != Start)
        {
            path.Add(current);
            current = current.ParentNode;
        }
        path.Add(Start);
        path.Reverse();
        return path;
    }


    public List<Node> FindPath(Node StartNode, Node EndNode)
    {
        for (int X = 0; X < SizeMapX; X++)
        {
            for (int Z = 0; Z < SizeMapZ; Z++)
            {
                Grille[X, Z].UnitOnIt = Map.tiles[X, Z].UnitOnIt;
            }
        }

        List<Node> OpenSet = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();

        OpenSet.Add(StartNode);

        //while we havent explored all the nodes
        while (OpenSet.Count > 0)
        {
            //Debug.Log(" ");
            Node Current = OpenSet[0];

            //Take the closest node to the end node
            for (int i = 0; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].FCost < Current.FCost
                    || OpenSet[i].FCost == Current.FCost
                    && OpenSet[i].HCost < Current.HCost)
                        Current = OpenSet[i];
            }
            //remove the closest node from open list and add it to close list
            OpenSet.Remove(Current);
            ClosedSet.Add(Current);

            //if we are at the end node, stop and give the path
            if (Current == EndNode)
            {
                return GetPath(StartNode, EndNode);
            }

            Current.neighbours = FillNeighbour(Current.PosX, Current.PosZ);
            //Debug.Log("voisins " + Current.neighbours.Count);

            foreach(Node NeighBour in Current.neighbours)
            {       //use the more effectiv neightbour to study it
                if (ClosedSet.Contains(NeighBour) || !NeighBour.walkable || (NeighBour.UnitOnIt && NeighBour!= EndNode))
                    continue;

                float DstAB = Current.GCost + DistanceBTW(Current, NeighBour) * NeighBour.DepCost;
                
                if ( DstAB < NeighBour.GCost || !OpenSet.Contains(NeighBour)){
                    NeighBour.GCost = (int)DstAB;
                    NeighBour.HCost = DistanceBTW(NeighBour, EndNode);
                    NeighBour.ParentNode = Current;
                    if (!OpenSet.Contains(NeighBour))
                        OpenSet.Add(NeighBour);
                }
            }

        }
        Debug.Log("No Path Found");
        return null;
    }

    int DistanceBTW(Node A, Node B){
        int dstX = Mathf.Abs( A.PosX - B.PosX);
        int dstZ = Mathf.Abs( A.PosZ - B.PosZ);

        if (dstX > dstZ)
            return 14 * dstX + 10 * (dstX - dstZ);
        return 14 * dstZ + 10 * (dstZ - dstX);
    }
    
    List<Node> FillNeighbour(int posX, int posZ)
    {
        List<Node> Neigh = new List<Node>();
        for (int X = -1; X <= 1; X++)
        {
            for (int Z = -1; Z <= 1; Z++)
            {
                int ObjX = X + posX;
                int ObjZ = Z + posZ;
                //if it's the current node or if the node is nt in the map
                if (X == 0 && Z == 0)
                {
                    continue;
                }
                //remove node that we don't want (here diagnoals)
                
                if (ObjX >= 0 && ObjX < SizeMapX && ObjZ >= 0 && ObjZ < SizeMapZ)
                {
                    if ((X == 1 && Z == -1) || (X == -1 && Z == -1)
                  || (X == 1 && Z == 1) || (X == -1 && Z == 1))
                        continue;

                    if (Grille[ObjX, ObjZ].walkable)
                        Neigh.Add(Grille[X + posX, Z + posZ]);
                }
            }
        }
        return Neigh;
    }

}