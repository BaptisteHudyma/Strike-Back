using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour {
    public float Speed = 0.5f;

    public int MaxX=100;    //Positions Max in game space;
    public int MaxY=100;
    private Vector3 OriginalPos;

    public int PixFormSide; //number of pixel from the side of the screen that make the camera move

    Vector3 NextPosition;
	
    void Start()
    {
        OriginalPos = Input.mousePosition;
    }


	// Update is called once per frame
	void Update () {
        //Debug.Log(Input.mousePosition.x + " " + Input.mousePosition.y);
        if (MouseOnMoveArea(PixFormSide))
        {
            
            this.transform.position = NextPosition;
        }
	}


    
    bool MouseOnMoveArea(int PixToMove)
    {
        if ((Input.mousePosition.x < PixToMove && Input.mousePosition.x >= 0) || (Input.mousePosition.x >= Screen.width - PixToMove && Input.mousePosition.x <= Screen.width)
            )
        {
            //if (NextPosition.x < OriginalPos.x + MaxX && NextPosition.x > OriginalPos.x - MaxX){
                if (Input.mousePosition.x < PixToMove)
                    NextPosition = this.transform.position + new Vector3(-Speed, 0, 0);
                else
                {
                    NextPosition = this.transform.position + new Vector3(Speed, 0, 0);
                }
                return true;
            //}
            return false;
            
        }
        if ((Input.mousePosition.y < PixToMove && Input.mousePosition.y >= 0) || (Input.mousePosition.y >= Screen.height - PixToMove && Input.mousePosition.y <= Screen.height)
            )
        {
           // if (NextPosition.y < OriginalPos.y + MaxY && NextPosition.y > OriginalPos.y - MaxY) {
                if (Input.mousePosition.y < PixToMove)
                    NextPosition = this.transform.position + new Vector3(0, 0, -Speed);
                else
                    NextPosition = this.transform.position + new Vector3(0, 0, Speed);
                return true;
            //}
            return false;
        }

        return false;
    }
}
