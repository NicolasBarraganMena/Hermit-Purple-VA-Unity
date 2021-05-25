using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Move
{

    [Header("Control Jugador")]
    public int wallDamage = 1;
    public int puntosEnergia = 20;
    public int puntosMascara = 10;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int energy;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        energy = GameController.instance.playerEnergy;
        base.Start();
    }

    private void OnDisable()
    {
        //Almacena el valor con el que el jugador acabo el nivel
        GameController.instance.playerEnergy = energy;
    }

    //Verificar si el jugador perdio o acabo el juego
    void CheckIfGameOver()
    {
        if (energy <= 0)
        {
            GameController.instance.GameOver();
        }
    }

    //Sobreescritura funcion que mira si puede moverse
    protected override void AttemptMove(int xDir, int yDir)
    {
        energy--; //la energia disminuye cuando el jugador se mueve
        base.AttemptMove(xDir, yDir);
        CheckIfGameOver(); //Comprobar si el jugador no ha perdido
        GameController.instance.playerTurn = false; //acabo el movimiento del jugador
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.playerTurn) return; //Si el jugador aun no se puede mover

        //Movimiento jugador
        int horizontal;
        int vertical;
        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");
        //Evitar  movimiento diagonal
        if (horizontal != 0) vertical = 0;
        //Comprobar si debo moverme
        if (horizontal != 0 || vertical != 0) AttemptMove(horizontal, vertical);
    }

    //Sobreescritura de la funcion de la clase Move
    protected override void OnCantMove(GameObject go)
    {
        //Codigo
        Wall hitWall = go.GetComponent<Wall>();
        if(hitWall != null)
        {
            hitWall.Daño(wallDamage);
            LoseEnergy(wallDamage);
        }
    }

    //Reiniciar la escena
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Perder energia al colisionar con un muro o cuando un enemigo lo ataca
    public void LoseEnergy(int damage)
    {
        energy -= damage;
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
    }

    //Cuando reciba energia por un item o llegue a la salida
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit")) //Cuando el jugador llega a la salida
        {
            Invoke("Restart", restartLevelDelay); //cambiar de nivel con un delay
            enabled = false; //Desactivar al jugador
        }else if (other.CompareTag("Food"))
        {
            energy += puntosMascara;
            other.gameObject.SetActive(false);
        }else if (other.CompareTag("Soda"))
        {
            energy += puntosEnergia;
            other.gameObject.SetActive(false);
        }
    }
}
