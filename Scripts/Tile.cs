using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{   
    //Cor
    [SerializeField] private Color clicked;
    [SerializeField] private SpriteRenderer sr;
    //Mina
    [SerializeField] private int isMina; // -1 bomba // qnt bomba em volta
    [SerializeField] private Text numBombas;

    //Auxiliares
    private GameManagerMina gm;
    private GridManager gridManager;

    //SpriteBomba  
    [SerializeField] GameObject bombSprite;

    //Posicao
    private int i, j;

    
    public void Start(){
        bombSprite.SetActive(false);
        gm = GameObject.Find("GameManager").GetComponent<GameManagerMina>();
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

    }

    

    public void Init(int mina, int a, int b){
        

        isMina = mina;
        numBombas.text = mina.ToString();
        this.i = a;
        this.j = b;

    }


    public void Clica(){
        if(!bombSprite.activeSelf && sr.color != clicked){
            if(isMina != 0) numBombas.enabled = true;            
            sr.color = clicked;
            if(isMina == -1) gridManager.LoseScene();
            if(isMina == 0) gridManager.ZeroClick(i,j);
            if(isMina != -1) gridManager.AtualizaTilesCertos();
            
        }
    }

    public void MarkBomb(){
        if(bombSprite.activeSelf){ // Se bomba ativa
            bombSprite.SetActive(false);
            if(isMina == -1) gridManager.AtualizaMinasCertas(-1);
            else gridManager.AtualizaMinasErradas(-1);
            
        }
        else if(!bombSprite.activeSelf && !numBombas.IsActive()){ // Se bomba desativada
            bombSprite.SetActive(true);
            if(isMina == -1) gridManager.AtualizaMinasCertas(+1);
            else gridManager.AtualizaMinasErradas(+1);
        }
    }

    public bool GetActive(){
        if(!(sr.color == clicked)) return true; //Nao foi ativada
        else return false;
    }

    public void Death(){
        Destroy(this.gameObject);
    }

    public void Reveal(){
        if(isMina == -1) bombSprite.SetActive(true);
        else numBombas.enabled = true;      
    }

}
