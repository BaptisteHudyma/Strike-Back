using UnityEngine;
using System.Collections.Generic;

public class IAEnemy : MonoBehaviour {

    public TileMap Map;
    public GameObject[] EnemyTab;

    Unit ThisUnitScript;
	
	void Start () {
        ThisUnitScript = gameObject.GetComponent<Unit>();
        GameObject [] AllUnits = GameObject.FindGameObjectsWithTag("units");
	}

    void EnemiesList()
    {
        GameObject []AllUnitsList = GameObject.FindGameObjectsWithTag("units");
        EnemyTab = new GameObject[(int)Map.UnitsNb - Map.NumberOfEnemies];
        int Parseur = 0;
        foreach(GameObject U in AllUnitsList)
        {
            if(!U.GetComponent<Unit>().isEnemy)
            {
                EnemyTab[Parseur] = U;
                Parseur++;
            }
        }
    }   //get enemy list
	
    GameObject GetNearestEnemy(GameObject [] EnemyList){
        //Vector2 ThisUnitPos = new Vector2(ThisUnitScript.PosX, ThisUnitScript.PosZ);
        int NearestPath = 1000;

        GameObject NearestEnemy = null;
        if (EnemyList != null)
        {
            foreach (GameObject U in EnemyList)
            {
                if (U != null)
                {
                    Unit Script = U.GetComponent<Unit>();

                    List<Node> Tempo = new List<Node>();
                    Tempo = Map.GeneratePathTo(this.gameObject, Script.PosX, Script.PosZ);

                    //Debug.Log("Path to Nearest "+Tempo);
                    if (Tempo.Count < NearestPath)
                    {
                        NearestEnemy = U;
                        NearestPath = Tempo.Count;
                    }
                }
            }
            
        }
        else
            print("No enemies");
        return NearestEnemy;
    }   //return nearest enemy

    public void ExecuteEnemyIA()
    {
        EnemiesList();

        Vector2 Mypos;
        Vector2 HisPos;
        GameObject NearEn = GetNearestEnemy(EnemyTab);
        Debug.Log("Nearest enemy "+EnemyTab[0]);
        if (NearEn == null) //no more player on the current play
            print("Defeat");
        else
        {
            Mypos = new Vector2(ThisUnitScript.PosX, ThisUnitScript.PosZ);
            HisPos = new Vector2(NearEn.GetComponent<Unit>().PosX, NearEn.GetComponent<Unit>().PosZ);
            if (Map.CanShootAt(Mypos, HisPos, ThisUnitScript.currentTP, ThisUnitScript.ActualWeap))
            //if(false)
            {   //shoot at him
                while(ThisUnitScript.currentTP>=ThisUnitScript.ActualWeap.Cost && NearEn.GetComponent<Unit>().ActualLife>0){
                    Map.UseWeapon(ThisUnitScript.ActualWeap, this.gameObject, NearEn);
                    Debug.LogWarning(gameObject.name + " use his weapon " + ThisUnitScript.ActualWeap.Name + " on " + NearEn.name);
                }
                if (NearEn.GetComponent<Unit>().ActualLife <= 0)
                {
                    Debug.LogError(gameObject.name + " destroy your unit " + NearEn.name);
                    Destroy(NearEn);
                    EnemiesList();
                    if (EnemyTab == null)
                    {
                        Debug.LogError("DEFEATED");
                    }
                }
            }
            else
            {   //move until I can shoot or dont have any tp
                Mypos = new Vector2(ThisUnitScript.PosX, ThisUnitScript.PosZ);
                HisPos = new Vector2(NearEn.GetComponent<Unit>().PosX, NearEn.GetComponent<Unit>().PosZ);
                ThisUnitScript.currentPath = Map.GeneratePathTo(this.gameObject, (int)HisPos.x, (int) HisPos.y);
                ThisUnitScript.FollowPath();
                
                Mypos = new Vector2(ThisUnitScript.PosX, ThisUnitScript.PosZ);

                Debug.LogError(Mypos);
                if (Map.CanShootAt(Mypos, HisPos, ThisUnitScript.currentTP, ThisUnitScript.ActualWeap))
                {   //shoot at him
                    while (ThisUnitScript.currentTP >= ThisUnitScript.ActualWeap.Cost && NearEn.GetComponent<Unit>().ActualLife > 0)
                    {
                        Map.UseWeapon(ThisUnitScript.ActualWeap, this.gameObject, NearEn);
                        Debug.LogWarning(gameObject.name + " use his weapon " + ThisUnitScript.ActualWeap.Name + " on " + NearEn.name);
                    }
                    if (NearEn.GetComponent<Unit>().ActualLife <= 0)
                    {
                        Debug.LogError(gameObject.name + " destroy your unit " + NearEn.name);
                        Destroy(NearEn);
                        EnemiesList();
                        if (EnemyTab == null)
                        {
                            Debug.LogError("DEFEATED");
                        }
                    }
                }
            }
        }
        
    }
}



// 100 - 120 pv

//
