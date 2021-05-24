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
    private Transform itemHolder; //Referencia a los items
    [Header ("Elementos del nivel")]
    public GameObject[] floors; 
    public GameObject[] outWalls;
    public GameObject[] walls;
    public GameObject[] items;
    public GameObject[] enemys;
    public GameObject exit;

    //Posiciones disponibles para instanciar objetos
    private List<Vector2> gridPositions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Lista con las posiciones para instanciar items del juego
    void InitializeList()
    {
        gridPositions.Clear();
        for(int x=1; x<columnas-1; x++)
        {
            for (int y = 1; y < columnas - 1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
            }
        }
        //Debug.Log("Ya se guardaron las posiciones de los items");
        //Debug.Log("Posiciones: " + gridPositions.Count);
    }

    //Obtener una posicion aleatoria para poner un item en ella
    Vector2 RandomPosition()
    {
        int index = Random.Range(0, gridPositions.Count);
        Vector2 position = gridPositions[index];
        //Eliminar la posicion de la lista para que no vuelva a poner un objeto en esa misma posicion
        gridPositions.RemoveAt(index);
        return position;
    }

    //Poner el objeto en la posicion escogida anteriormente
    void LayoutObject(GameObject[] items, int min, int max, Transform itemHolder)
    {
        int objectCount = Random.Range(min, max + 1);
        for(int i = 0; i < objectCount; i++)
        {
            Vector2 randomPosition = RandomPosition();
            GameObject itemChoice = GetElement(items);
            GameObject instance = Instantiate(itemChoice, randomPosition, Quaternion.identity);
            instance.transform.SetParent(itemHolder); //Guardar el objeto como hijo del objeto Items
        }
    }

    //Funcion que genera el nivel
    public void SetupScene(int nivel)
    {
        //Debug.Log("SetupScene!");
        //Generar Nivel
        BoardSetup();
        //Generar items en el nivel
        InitializeList();
        //Crear un objeto que guarde todos los items creados en el nivel
        itemHolder = new GameObject("Elementos").transform;
        //Generar muros en el nivel
        LayoutObject(walls, 5, 9, itemHolder);
        //Generar items en el nivel
        LayoutObject(items, 1, 5, itemHolder);
        //Generar enemigos en el nivel
        int numberEnemys = (int)Mathf.Log(nivel, 2);
        LayoutObject(enemys, numberEnemys, numberEnemys, itemHolder);
        //Generar la puerta de salida
        Instantiate(exit, new Vector2(columnas - 1, filas - 1), Quaternion.identity);

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
