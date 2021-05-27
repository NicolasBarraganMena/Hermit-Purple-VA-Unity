using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Referencia a si mismo (this Game Controller)
    public static GameController instance;

    // Referencia al codigo generador de niveles
    [Header("Generador Niveles")]
    public BoardManager boardScript;

    [Header("Control Niveles")]
    public int restartDelay = 10; //tiempo para reiniciar el juego
    public float levelDelay = 2f; //tiempo visualización pantalla
    public float turnDelay = 0.1f; //tiempo espera para hacer un movimiento
    public int playerEnergy = 100; //energia del jugador para continuar en el calabozo
    [HideInInspector] public bool playerTurn = true; //el jugador puede moverse
    public bool doingSetup; //Se esta mostrando la pantalla de inicio

    //lista de enemigos para controlar
    private List<Enemy> enemies = new List<Enemy>();
    private bool enemiesMoving; //nos permite saber si los enemigos se estan moviendo

    //Control del nivel
    private int level = 1;
    private GameObject levelImage; //Referencia a la imagen inicial
    private Image background; // fondo de la imagen
    private Text levelText; //Referencia al texto con el indicador del piso
    private bool startLevel = true; // Se verifica si se esta en el piso 1


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


    // Update is called once per frame
    void Update()
    {
        if (playerTurn || enemiesMoving || doingSetup) return;

        StartCoroutine(MoveEnemies());
    }

    void InitGame()
    {
        //mostrar la pantalla de inicio
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        background = GameObject.Find("LevelImage").GetComponent<Image>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        levelText.text = "Piso " + level;
        levelImage.SetActive(true);
        //reiniciar la musica de fondo de ser necesario
        if (!SoundManager.instance.musicSource.isPlaying)
        {
            SoundManager.instance.musicSource.Play();
        }
        //vaciar la lista de enemigos (cuando se carga un nuevo nivel)
        enemies.Clear();
        //llamar al metodo Setup Scene en el codigo Board Manager (para generar el nivel)
        boardScript.SetupScene(level);

        Invoke("HideLevelImage", levelDelay);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "En el piso " + level + "\n ¡has muerto!";
        background.color = Color.red;
        levelImage.SetActive(true);
        Invoke("CountdownRestart", restartDelay);
        enabled = false;
    }

    //Espera para volver a empezar la escena
    void CountdownRestart()
    {
        SceneManager.LoadScene(0);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(startLevel)
        {
            InitGame();
            startLevel = false;
        }
        else
        {
            level++;
            InitGame();
        }
    }
}
