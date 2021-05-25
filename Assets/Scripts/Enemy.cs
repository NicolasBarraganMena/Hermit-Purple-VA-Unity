using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Move
{

    [Header("Control Enemigo")]
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        //Agregarse a la lista de enemigos
        GameController.instance.AddEnemyToList(this);
        //Referencia a la posicion del jugador
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    //Sobreescritura funcion que mira si puede moverse
    protected override void AttemptMove(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove(xDir, yDir);
        skipMove = true;
        
    }

    //Movimiento Enemigo
    public void MoveEnemy()
    {
        int xDir = 0; 
        int yDir = 0;
        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) //Si el enemigo esta en la misma columna que el jugador
        {
            yDir = target.position.y > transform.position.y ? 1 : -1; //El enemigo se movera en el eje y hacia el jugador
        }
        else 
        {
            xDir = target.position.x > transform.position.x ? 1 : -1; //El enemigo se movera en el eje x hacia el jugador
        }
        AttemptMove(xDir, yDir);
    }

    //Sobreescritura de la funcion de la clase Move
    protected override void OnCantMove(GameObject go)
    {
        //Codigo
        Player hitPlayer = go.GetComponent<Player>(); //Referencia al objeto del jugador
        if(hitPlayer != null)
        {
            hitPlayer.LoseEnergy(playerDamage);
            animator.SetTrigger("enemyAttack");
        }
    }

}
