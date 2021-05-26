using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Move
{

    [Header ("Sonidos jugador")]
    public AudioClip maskSound1, maskSound2, ajaSound3, ajaSound4, gameOverSound;

    [Header("Control Jugador")]
    public int wallDamage = 1;
    public int puntosEnergia = 20;
    public int puntosMascara = 10;
    public float restartLevelDelay = 1f;
    public Text energyText; //texto con la energia restante que se ira actualizando en el Canvas

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
        //Mostrar la energia actual
        energyText.text = "Energia: " + energy;
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
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameController.instance.GameOver();
        }
    }

    //Sobreescritura funcion que mira si puede moverse
    protected override void AttemptMove(int xDir, int yDir)
    {
        energy--; //la energia disminuye cuando el jugador se mueve
        //Mostrar la energia actual
        energyText.text = "Energia: " + energy;
        base.AttemptMove(xDir, yDir);
        CheckIfGameOver(); //Comprobar si el jugador no ha perdido
        GameController.instance.playerTurn = false; //acabo el movimiento del jugador
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.playerTurn || GameController.instance.doingSetup) return; //Si el jugador aun no se puede mover

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
        //Mostrar la energia actual
        energyText.text = "Energia: " + energy;
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
            SoundManager.instance.RandomizeSfx(maskSound1, maskSound2);
            //Mostrar la energia actual
            energyText.text = "Energia: " + energy;
            animator.SetTrigger("playerHeal");
            other.gameObject.SetActive(false);
        }else if (other.CompareTag("Soda"))
        {
            energy += puntosEnergia;
            SoundManager.instance.RandomizeSfx(ajaSound3, ajaSound4);
            //Mostrar la energia actual
            energyText.text = "Energia: " + energy;
            animator.SetTrigger("playerHeal");
            other.gameObject.SetActive(false);
        }
    }
}
