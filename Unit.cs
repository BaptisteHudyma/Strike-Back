using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Unit : MonoBehaviour {
    public Weapon ActualWeap;
    public bool isEnemy;
    public int ActualLife=100;

    public int PosX;
    public int PosZ;
    public TileMap map;
    public int TotalPM = 4;
    public int TotalTP = 4;

    public List<Node> currentPath;

    private int currentNode;
    public int currentPM;
    public int currentTP;
    public int PathLength;
    public int WeaponID;

    public void UnitStart()
    {

        currentPM = TotalPM;
        currentTP = TotalTP;
        SetWeapon(WeaponID);
    }

    void SetWeapon(int ID)
    {
        foreach( Weapon Weap in map.WeaponList )
        {
            if(Weap.WeaponID == ID)
            {
                ActualWeap = Weap;
                return;
            }

        }
    }

    void Update(){
        //if (ActualLife <= 0)
        //    Destroy(this.gameObject);

        PathLength = 0;
        if (currentPath != null)
        {
            //Debug.Log("Longueur du chemin de " + transform.name + " " + ((currentPath.Count) - 1));
            PathLength = ((currentPath.Count) - 1);
        }
    }

    public void FollowPath()
    {
        if (currentPath != null)
        {
            float TIMECOUNT = 0;
            List<Node> Tempo = new List<Node>();
            Tempo = currentPath;
            int PathLght = Tempo.Count;
            currentNode = 0;
            if (isEnemy && map.tiles[Tempo[PathLght-1].PosX, Tempo[PathLght-1].PosZ].UnitOnIt)
                PathLght--;
            while (currentNode < PathLght - 1 && currentPM > 0)
            {
                    
                    //Debug.Log("Begin");
                    Vector3 Start = map.TileCoordToWorldCoor(Tempo[currentNode].PosX, Tempo[currentNode].PosZ)
                         + new Vector3(0, 0.7f, 0);
                    Vector3 End = map.TileCoordToWorldCoor(Tempo[currentNode + 1].PosX, Tempo[currentNode + 1].PosZ)
                         + new Vector3(0, 0.7f, 0);
                    
                    TIMECOUNT += Time.deltaTime;
                    //Debug.Log(TIMECOUNT);
                    //if (TIMECOUNT >= 1000)
                    {
                        this.transform.position = End;// Vector3.SmoothDamp(Start, End, ref CurrentSpeed, SmoothTime);
                        map.tiles[Tempo[currentNode].PosX, Tempo[currentNode].PosZ].UnitOnIt = false;
                        map.tiles[Tempo[currentNode + 1].PosX, Tempo[currentNode + 1].PosZ].UnitOnIt = true;
                        currentNode++;
                        currentPM--;
                        TIMECOUNT = 0;
                        PosX = Tempo[currentNode].PosX;
                        PosZ = Tempo[currentNode].PosZ;
                    }
                    Debug.DrawLine(Start, End, Color.red);
                    
                    //Debug.Log(Start + "  " + End);
                
            }
            currentPath = null;
            Tempo = null;
        }
    }
    
}
