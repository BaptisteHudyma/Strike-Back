using UnityEngine;
using System.Collections.Generic;

public class Clicable : MonoBehaviour {

    public int TileX;
    public int TileZ;
    public TileMap map;
    public PathFinding ScriptFinding;
    

	void OnMouseUp()
    {   
        
        if (map.tiles[TileX, TileZ].UnitOnIt)
        {   //we clicked on a tile with a ally so if it's an ally we switch current ally
            GameObject SelUnit = SearchWithUnitWeClick(TileX, TileZ);
            if (SelUnit != null && !(SelUnit.GetComponent<Unit>().isEnemy))
            {
                map.UnitToMove = SelUnit;
                print("Selected unit changed : " + map.UnitToMove.name + " at coord " + TileX + " " + TileZ);
            }
            else if (SelUnit != null){  //it's an enemy, try to shoot at it
                //print(SelUnit.name + " IsEnemy  " + SelUnit.GetComponent<Unit>().isEnemy);
                Unit MyScript = map.UnitToMove.GetComponent<Unit>();
                Vector2 Mypos = new Vector2(MyScript.PosX, MyScript.PosZ);
                Vector2 HisPos = new Vector2(SelUnit.GetComponent<Unit>().PosX, SelUnit.GetComponent<Unit>().PosZ);

                if (map.CanShootAt(Mypos, HisPos, MyScript.currentTP, MyScript.ActualWeap) && MyScript.currentTP >= MyScript.ActualWeap.Cost)
                {
                    map.UseWeapon(MyScript.ActualWeap, map.UnitToMove, SelUnit);
                    Debug.LogWarning(map.UnitToMove.name + " use weapon on " + SelUnit.name + " with weapon "+MyScript.ActualWeap.Name);
                }

                if (SelUnit.GetComponent<Unit>().ActualLife <= 0)
                {
                    Debug.LogError(SelUnit.name + " get himslef killed by "+map.UnitToMove.name);
                    map.enemiesLeft--;
                    Destroy(SelUnit);
                    SelUnit = null;
                    if(map.enemiesLeft<=0)
                    {
                        Debug.LogError("Congratulation you win !");
                    }
                }
            }
        }
        else    //prepare to go to the clicked tile
        {
            map.UnitToMove.GetComponent<Unit>().currentPath = map.GeneratePathTo(map.UnitToMove, TileX, TileZ);
            Unit Script = map.UnitToMove.GetComponent<Unit>();
            print("Got to " + TileX + " " + TileZ + " path lgth : " + (Script.currentPath.Count-1));
            if (Script.currentPath != null)
                Script.FollowPath();
        }
        //map.moveUnitTo(TileX, TileZ);
    }

    GameObject SearchWithUnitWeClick(int X, int Z)
    {
        foreach(GameObject U in map.AllUnits)
        {
            //if ( !U.GetComponent<Unit>().isEnemy){
                int posx = U.GetComponent<Unit>().PosX;
                int posz = U.GetComponent<Unit>().PosZ;
                if (posx == X && posz == Z)
                    return U;
            //}
        }
        return null;
    }
}
