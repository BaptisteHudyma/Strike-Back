using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {
    public Weapon[] WeaponList;

    public int NumberOfEnemies = 1;
    public GameObject UnitToMove;
    public TileType[] tileTypes;
    PathFinding PathScript;

    public Tile[,] tiles;

    public int sizeMapX = 10;
    public int sizeMapZ = 10;

    public GameObject[] AllUnits;
    public int UnitsNb;

    public bool Endturn;
    public GameObject[] EnemiesTab;
    public int LevelSel = 1;

    public GameObject UnitAlly;
    public GameObject UnitEnemy;
    public int enemiesLeft;

    void Start()
    {
        enemiesLeft = NumberOfEnemies;
        WeaponList = new Weapon[2];
        SetWeapons();

        InitUnits();

        if (LevelSel == 3)
            sizeMapX = 13;

        PathScript = GetComponent<PathFinding>();
        InitiateMap(LevelSel);
        GenerateMapVisual();

        PathScript.AwakePathFinding();
    }

    void SetWeapons()
    {
        WeaponList[0] = new Weapon("CAC", 30, 1, 1, false, 1);
        WeaponList[1] = new Weapon("DTS", 20, 1, 5, false, 0);

    }

    void Update()
    {
        
        AllUnits = GameObject.FindGameObjectsWithTag("units");
        foreach (GameObject U in AllUnits)
        {
            U.GetComponent<Unit>().PosX = (int)U.transform.position.x;
            U.GetComponent<Unit>().PosZ = (int)U.transform.position.z;
        }
        FillEnemiesTab();
       /* if (UnitToMove != null)
        {
            UnitToMove.GetComponent<Unit>().PosX = (int)UnitToMove.transform.position.x;
            UnitToMove.GetComponent<Unit>().PosZ = (int)UnitToMove.transform.position.z;

            UnitToMove.GetComponent<Unit>().map = this;
        }*/
    }

    void InitUnits()    //initialize all units
    {
        AllUnits = GameObject.FindGameObjectsWithTag("units");
        UnitsNb = AllUnits.Length;
        print("Units number : " + UnitsNb);

        foreach (GameObject U in AllUnits)
        {
            Unit UnitScript = U.GetComponent<Unit>();
            UnitScript.PosX = (int)U.transform.position.x;
            UnitScript.PosZ = (int)U.transform.position.z;
            if (UnitScript.isEnemy)
                U.GetComponent<IAEnemy>().Map = this;

            
            if(U.name == "Ally1")
            {
                UnitScript.ActualLife = PlayerPrefs.GetInt("J1_life");
                UnitScript.TotalPM = PlayerPrefs.GetInt("J1_MP");
                UnitScript.TotalTP = PlayerPrefs.GetInt("J1_TP");
                UnitScript.WeaponID = PlayerPrefs.GetInt("J1_arme");
            }
            else if(U.name == "Ally2")
            {
                UnitScript.ActualLife = PlayerPrefs.GetInt("J2_life");
                UnitScript.TotalPM = PlayerPrefs.GetInt("J2_MP2");
                UnitScript.TotalTP = PlayerPrefs.GetInt("J2_arme");
                UnitScript.WeaponID = PlayerPrefs.GetInt("J2_TP2");
            }

            UnitScript.map = this;
            UnitScript.UnitStart();
        }
    }

    void InitiateMap(int Level)  //create a map array that will be use to instantiate the map. Generate obsacles too
    {
        tiles = new Tile[sizeMapX, sizeMapZ];

        for (int i = 0; i < sizeMapX; i++)
        {
            for (int y = 0; y < sizeMapZ; y++)
            {
                tiles[i, y]= new Tile();
                tiles[i, y].ActualTile = 0;
                tiles[i, y].UnitOnIt = UnitOnTile(i, y);
            }
        }

        if (Level == 1)
        {
            tiles[1, 3].ActualTile = 1;
            tiles[2, 3].ActualTile = 1;
            tiles[3, 3].ActualTile = 1;
            tiles[6, 3].ActualTile = 1;
            tiles[7, 3].ActualTile = 1;
            tiles[8, 3].ActualTile = 1;
            tiles[2, 5].ActualTile = 1;
            tiles[3, 5].ActualTile = 1;
            tiles[4, 5].ActualTile = 1;
            tiles[6, 5].ActualTile = 1;
            tiles[7, 5].ActualTile = 1;
            tiles[1, 7].ActualTile = 1;
            tiles[2, 7].ActualTile = 1;
            tiles[3, 7].ActualTile = 1;
            tiles[6, 7].ActualTile = 1;
            tiles[7, 7].ActualTile = 1;
            tiles[8, 7].ActualTile = 1;
        }
        else if(Level == 2)
        {
            tiles[3, 2].ActualTile = 1;
            tiles[7, 2].ActualTile = 1;
            tiles[4, 2].ActualTile = 1;
            tiles[3, 3].ActualTile = 1;
            tiles[3, 4].ActualTile = 1;
            tiles[4, 4].ActualTile = 1;
            tiles[5, 5].ActualTile = 1;
            tiles[6, 6].ActualTile = 1;
            tiles[7, 6].ActualTile = 1;
            tiles[7, 7].ActualTile = 1;
            tiles[3, 8].ActualTile = 1;
            tiles[6, 8].ActualTile = 1;
            tiles[8, 8].ActualTile = 1;
        }
        else if(Level == 3)
        {
            tiles[1, 2].ActualTile = 1;
            tiles[2, 2].ActualTile = 1;
            tiles[3, 2].ActualTile = 1;
            tiles[9, 2].ActualTile = 1;
            tiles[10, 2].ActualTile = 1;
            tiles[11, 2].ActualTile = 1;
            tiles[6, 3].ActualTile = 1;
            tiles[1, 4].ActualTile = 1;
            tiles[2, 4].ActualTile = 1;
            tiles[3, 4].ActualTile = 1;
            tiles[9, 4].ActualTile = 1;
            tiles[10, 4].ActualTile = 1;
            tiles[11, 4].ActualTile = 1;
            tiles[6, 5].ActualTile = 1;
            tiles[1, 6].ActualTile = 1;
            tiles[2, 6].ActualTile = 1;
            tiles[3, 6].ActualTile = 1;
            tiles[9, 6].ActualTile = 1;
            tiles[10, 6].ActualTile = 1;
            tiles[11, 6].ActualTile = 1;
        }
    }

    bool UnitOnTile(int X, int Z)   //if there is a unit on the tile [X,Z]
    {
        foreach(GameObject U in AllUnits)
        {
            int posx = U.GetComponent<Unit>().PosX;
            int posz = U.GetComponent<Unit>().PosZ;
            if (posx == X && posz == Z)
                return true;
        }
        return false;
    }

    void GenerateMapVisual()
    {
        for (int x = 0; x < sizeMapX; x++)
        {
            for (int z = 0; z < sizeMapZ; z++)
            {
                TileType tt = tileTypes[tiles[x, z].ActualTile];
                GameObject Tile = (GameObject)Instantiate(tt.TileVisual, new Vector3(x, tt.TileVisual.transform.localScale.y/2, z),  Quaternion.identity, this.transform);
                Clicable CLC =  Tile.GetComponent<Clicable>();
                
                CLC.TileX = x;
                CLC.TileZ = z;
                CLC.map = this;
                tiles[x, z].TileGO = Tile;
            }
        }
    }       //instantiate the map on the screen

    public Vector3 TileCoordToWorldCoor(int x, int z)
    {
        return new Vector3(tiles[x,z].TileGO.transform.position.x , 1, tiles[x, z].TileGO.transform.position.z);
    }       //give a vector3 that correspond to a tile position in world space

    public List<Node> GeneratePathTo(GameObject SelUnit, int TileX, int TileZ) {
        /*if (tiles[TileX, TileZ].UnitOnIt)
        {
            return null;
        }*/
        //UnitToMove.GetComponent<Unit>().currentPath = null;
        if (SelUnit != null)
        {
            Node source = PathScript.Grille[SelUnit.GetComponent<Unit>().PosX,
                            SelUnit.GetComponent<Unit>().PosZ];
            Node target = PathScript.Grille[TileX, TileZ];

            //Debug.Log("target : " + target.PosX + " " + target.PosZ + " walkable " + target.walkable);
            return PathScript.FindPath(source, target);
        }
        Debug.Log("JE SUIS PAS SENSE ETRE LA");
        return null;
    }       //generate a path from the tile where the actualUnit stand to the tile [X,Z]

    public void EndTurnPressed()
    {
        Endturn = true;
        //start the endOfTurn stage
        foreach(GameObject U in AllUnits)
        {   //move each units that needed to move
            Unit Helene = U.GetComponent<Unit>();
            Helene.currentPM = Helene.TotalPM;
            Helene.currentTP = Helene.TotalTP;
            if (Helene.isEnemy)
                U.GetComponent<IAEnemy>().ExecuteEnemyIA();
        }
        
        //end of the endOFTurn stage
        Endturn = false;
        print("Turn ended");
        //UnitToMove = null;
    }   //When the "end of turn" button is pressed, it launch this action

    public void FillEnemiesTab(){
        int Cmpt = 0;
        EnemiesTab = new GameObject[NumberOfEnemies];
        foreach (GameObject U in AllUnits)
        {
            Unit UScript = U.GetComponent<Unit>();
            if (UScript.isEnemy )
            {   //enemy
                EnemiesTab[Cmpt] = U;
                Cmpt++;
            }
        }
    }

    public bool CanShootAt(Vector2 CelLFrom, Vector2 CellToShoot, int MyTP, Weapon Weap)
    {
        float DistToEn = Mathf.Sqrt(Mathf.Pow(CelLFrom.x - CellToShoot.x, 2) + Mathf.Pow(CelLFrom.y - CellToShoot.y, 2));
        float Range = (float)Weap.MaxRange;
        if (DistToEn <= Range && MyTP>=Weap.Cost)
        {
            if (Weap.IsArea)
            {   //weapon shoot in area

                //if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            }
            else
            {   //shoot in line
                if (CelLFrom.x == CellToShoot.x)
                {
                    if (CelLFrom.y < CellToShoot.y)
                    {
                        float TempoTile = CelLFrom.y;
                        CelLFrom.y = CellToShoot.y;
                        CellToShoot.y = TempoTile;
                    }
                    for (int i = (int)CellToShoot.y; i < CelLFrom.y; i++)
                    {
                        if (tiles[(int)CelLFrom.x, i].ActualTile == 1)
                            return false;
                    }
                    return true;
                }
                else if (CelLFrom.y == CellToShoot.y)
                {
                    if (CelLFrom.x < CellToShoot.x)
                    {
                        float TempoTile = CelLFrom.x;
                        CelLFrom.x = CellToShoot.x;
                        CellToShoot.x = TempoTile;
                    }
                    for (int i = (int)CellToShoot.x; i < CelLFrom.x; i++)
                    {
                        if (tiles[i, (int)CelLFrom.y].ActualTile == 1)
                            return false;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void UseWeapon(Weapon ThisWeapon, GameObject Hitter, GameObject OnThisUnit)
    {
        Unit HitterScript = Hitter.GetComponent<Unit>();
        Unit HittedScript = OnThisUnit.GetComponent<Unit>();

        HitterScript.currentTP = HitterScript.currentTP - HitterScript.ActualWeap.Cost;

        HittedScript.ActualLife = HittedScript.ActualLife - HitterScript.ActualWeap.Damage;  //DAMAGES CALCULATIONS
    }
}