using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridManager : MonoBehaviour
{
    [SerializeField] private int largura, altura;
    [SerializeField] private Tile tiles;
    [SerializeField] private int numMinas;
    [SerializeField] private int[,] isMina; // -1 bomba // qnt bombas em volta
    private Tile[,] matrizTiles;
    private int numMinasCertas = 0;
    private int numMinasErradas = 0;
    private int numTilesCertos = 0;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] Text qntMinasRestantesText;
    public bool canButton = true;
    //Tela inicio
    [SerializeField] private GameObject telaInicio;
    [SerializeField] private GameObject explosaoInicio;
    private bool isTelaInicioSaiu = false;
    
    
    void Start(){

        isMina = new int[largura, altura];
        matrizTiles = new Tile[largura, altura];
        numMinas = 10;
        UpdateBombText();
        
        
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.A))StartCoroutine(PrimeiroZero());
        if(Input.anyKeyDown) TirarTelaInicio();
    }

    void CriarGrid(){

        for(int i = -9; i < largura-9; i++){
            for(int j = -5; j < altura-5; j ++){
                var aux = Instantiate(tiles, new Vector3(i,j,0), Quaternion.identity);
                aux.name = $"Tile {i} {j}";

                var ii = i + 9;
                var jj = j + 5;

                matrizTiles[ii,jj] = aux;
                
                aux.Init(isMina[ii,jj],ii,jj);
            }
        }
    }

    void SorteiaPosicoes(){

        
        int cont = 0;

        while(cont != numMinas){
            int a = Random.Range(0,largura);
            int b = Random.Range(0,altura);

            if(isMina[a,b] != -1){
                cont++;
                isMina[a,b] = -1;

                for(int i = 0; i < 3; i++){
                    for(int j = 0; j < 3; j ++){
                        
                        if( 0 <= i+a-1 && i+a-1 < largura && 0 <= j+b-1 && j+b-1 < altura && isMina[i+(a-1),j+(b-1)] != -1) isMina[i+(a-1),j+(b-1)] ++;

                    }
                }
            }
        }

    }

    public void ZeroClick(int i, int j){

        for(int x = 0; x < 3; x++){
            for(int y = 0; y < 3; y ++){

                if( 0 <= x+i-1 && x+i-1 < largura && 0 <= y+j-1 && y+j-1 < altura && matrizTiles[x+i-1,y+j-1].GetActive()) matrizTiles[x+i-1,y+j-1].Clica();

            }
        }

    }

    private IEnumerator  PrimeiroZero(){
        int a;
        int b;

        yield return new WaitForSeconds(0.1f);

        while(true){
            a = Random.Range(0,largura);
            b = Random.Range(0,altura);

            if(isMina[a,b] == 0) break;
        }

        ZeroClick(a,b);
    }

    public void AtualizaMinasCertas(int a){
        numMinasCertas += a;
        TryWinGame();
        
    }

    public void AtualizaTilesCertos(){
        numTilesCertos ++;
        TryWinGame();
    }

    public void AtualizaMinasErradas(int a){
        numMinasErradas += a;
    }

    private void TryWinGame(){
        if(numMinasCertas == numMinas && numTilesCertos == largura*altura - numMinas) winScene.SetActive(true);
    }

    private void UpdateBombText(){
        qntMinasRestantesText.text = numMinas.ToString();
    }

    public void IncrementaNumeroMinasInicial(){
        numMinas ++;
        UpdateBombText();
    }

    public void DecrementaNumeroMinasInicial(){
        if(numMinas <= 0) return;
        numMinas --;
        UpdateBombText();
    }

    public void StartGame(){
        if(!canButton) return;

        DeleteGrid();
        SorteiaPosicoes();
        CriarGrid();
        StartCoroutine(PrimeiroZero());
        winScene.SetActive(false);
        loseScene.SetActive(false);
        StartCoroutine(TimerButton());
    }

    public void LoseScene(){
        loseScene.SetActive(true);
        RevealAll();
    }

    private void RevealAll(){
        for(int i = 0; i < altura; i ++){
            for(int j = 0; j < largura; j++){
                matrizTiles[j,i].Reveal();
            }
        }
    }

    public void DeleteGrid(){
        if(!canButton) return;

        winScene.SetActive(false);
        loseScene.SetActive(false);
        for(int x = 0; x < largura; x++){
            for(int y = 0; y < altura; y ++){
                if(matrizTiles[x,y] == null) return;
                matrizTiles[x,y].Death();
                matrizTiles[x,y] = null;
            }   
        }
        StartCoroutine(TimerButton());
    }

    private IEnumerator TimerButton(){
        canButton = false;
        yield return new WaitForSeconds(2f);
        canButton = true;
    }

    private void TirarTelaInicio(){
        if(isTelaInicioSaiu) return; 
        telaInicio.gameObject.SetActive(false);
        var aux = Instantiate(explosaoInicio, new Vector3(0,0,0), Quaternion.identity);
        aux.transform.localScale = new Vector3(4f,4f,4f);
        isTelaInicioSaiu = true;
        
    }
    

}

