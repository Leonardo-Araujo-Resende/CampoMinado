using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{   
    private Tile tile;


    

    
    void Update()
    {   
        Moviment();

        if(Input.GetMouseButton(0) && tile != null){
            tile.Clica();
        }

        if(Input.GetMouseButtonDown(1) && tile != null){
            tile.MarkBomb();
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Tile")){
            tile = other.GetComponent<Tile>();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Tile")){
            tile = null;
        }
    }


    void Moviment(){
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
    }
}
