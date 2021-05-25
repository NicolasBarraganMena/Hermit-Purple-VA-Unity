using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Referencia a si mismo (this Game Controller)
    public static GameController instance;

    // Referencia al codigo generador de niveles
    [Header("Generador Niveles")]
    public BoardManager boardScript;

    [Header("Control Niveles")]
    public float turnDelay = 0.1f; //tiempo espera para hacer un movimiento
    public int playerEnergy = 100; //energia del jugador para continuar en el calabozo
    [HideInInspector] public bool playerTurn = true; //el jugador puede moverse

    //lista de enemigos para controlar
    private List<Enemy> enemies = new List<Enemy>();
    private bool enemiesMoving; //nos permite saber si los enemigos se estan moviendo

    void Awake()
    {
        //Singleton
        //esto permite tener una referencia al codigo mediante instance
        if(GameController.instance == null)
        {
            GameController.instance = this;
        }else if (GameController.instance != this)
        {
            Destroy(gameObject);
        }
        //evita que el objeto se destruye al cargar la escena
        DontDestroyOnLoad(gameObject);

        //Instanciar objeto con referencia al codigo generador de niveles
        boardScript = GetComponent<BoardManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn || enemiesMoving) return;

        StartCoroutine(MoveEnemies());
    }

    void InitGame()
    {
        //vaciar la lista de enemigos (cuando se carga un nuevo nivel)
        enemies.Clear();
        //llamar al metodo Setup Scene en el codigo Board Manager (para generar el nivel)
        boardScript.SetupScene(3);
    }

    public void GameOver()
    {
        enabled = false;
    }

    //Mover los enemigos en secuencia
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        //Movimiento enemigo por enemigo
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playerTurn = true; // el jugador ya puede moverse
        enemiesMoving = false;
    }

    //Agregar cada enemigo a la lista
    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }
}
