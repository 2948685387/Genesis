using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death
}

[Serializable]
public class Parameter
{
    public float MaxHP;
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public float attackDamage;
    public Animator animator;
    public bool getHit;
    public float beHitDamage;
    public bool isAttack;
}
public class FSM : MonoBehaviour
{

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    public Parameter parameter;

    private void Awake()
    {
        parameter.MaxHP = 100f;
        parameter.health = parameter.MaxHP;
    }
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);

        parameter.animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    public void FlipTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }
    public void Hurt()
    {
        var damageText = GetComponentInChildren<EnemyHealthBar>();
        damageText.Text_Display();
    }

    public void AttackOver()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (Vector2.Distance(transform.position, player.transform.position) < .8f && player.layer != LayerMask.NameToLayer("Flashing"))
        {
            //Debug.Log(player);
            var playerController = player.GetComponent<PlayerController>();
            playerController.Hurt((player.transform.position - transform.position).normalized, parameter.attackDamage * playerController.beHitDamageConversion);
        }
    }

    public void Death()
    {
        Destroy(this);
        gameObject.layer = LayerMask.NameToLayer("Background");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}