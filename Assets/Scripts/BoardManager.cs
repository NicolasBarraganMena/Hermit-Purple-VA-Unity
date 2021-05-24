using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [Header ("Tamaño nivel")]
    public int columnas = 8; //numero de columnas del area en que se puede mover el personaje
    public int filas = 8; //numero de filas del area en que se puede mover el personaje

    [Header("Elementos generación nivel")]
    private Transform boardHolder; //Referencia al objeto Mapa
    [Header ("Baldosas")]
    public GameObject[] floors; 
    public GameObject[] outWalls;
    public GameObject[] walls;
    public GameObject[] items;
    public GameObject[] enemys;
    public GameObject exit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Funcion que genera el nivel
    public void SetupScene()
    {
        //Debug.Log("SetupScene!");
        //Generar Nivel
        BoardSetup();
    }

    void BoardSetup()
    {
        //Crear un objeto que guarde todos los objetos creados en el nivel
        boardHolder = new GameObject("Mapa").transform;

        //Desplazarme por el area de juego(incluyendo los bordes)
        //Desplazamiento a lo ancho
        for(int x = -1; x < columnas + 1; x++) // x=-1 y x<columnas + 1 para que incluya los bordes externos
        {
            //Desplazamiento a lo largo
            for(int y = -1; y < filas + 1; y++) // y=-1 y y<columnas + 1 para que incluya los bordes externos
            {
                //Tile (suelo) a instanciar en el nivel(mapa)
                GameObject element = GetElement(floors);

                //Cuando se encuentre en los bordes poner los muros
                if(x==-1 || y==-1 || x==columnas || y == filas)
                {
                    //Tile (muro) a instanciar en el nivel(mapa)
                    element = GetElement(outWalls);
                }

                //Poner el Tile en el nivel
                GameObject instance = Instantiate(element, new Vector2(x, y), Quaternion.identity);
                instance.transform.SetParent(boardHolder); //Guardar el objeto como hijo del objeto Mapa
            }
        }
    }

    GameObject GetElement(GameObject[] gameObjects)
    {
        return gameObjects[Random.Range(0, gameObjects.Length)];
    }
}
