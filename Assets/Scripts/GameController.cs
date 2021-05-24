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
        
    }

    void InitGame()
    {
        //llamar al metodo Setup Scene en el codigo Board Manager (para generar el nivel)
        boardScript.SetupScene(7);
    }
}
