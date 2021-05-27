using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    private GameObject gameController;

    // Start is called before the first frame update
    void Awake()
    {
        //gameController = gameController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //Cuando se mueve la nariz
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadScene();
        }
    }

    //Carga la escena de juego
    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(GameController.instance);
    }
}
