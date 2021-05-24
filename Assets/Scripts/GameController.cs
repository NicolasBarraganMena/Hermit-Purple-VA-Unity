using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Referencia al codigo generador de niveles
    [Header("Generador Niveles")]
    public BoardManager boardScript;

    void Awake()
    {
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
        boardScript.SetupScene();
    }
}
