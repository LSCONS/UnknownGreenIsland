using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Runaway
}

public enum AITendency
{
    hostile,
    neutral,
    friendly
}

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float Health;
    public float curHealth;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropItem;

    [Header("Ai")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;
    public AITendency aiTendency;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    public float fieldOfView = 120f;

    private Transform Player;

    //private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;
    PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = "Player".GetComponentNameDFS<PlayerStatus>();
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        Player = "Player".GetComponentNameDFS<Transform>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, Player.transform.position); // Distance : a와 b 사이에 거리를 측정해 반환하는 함수

        //animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Runaway:
                RunawayUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        //animator.speed = agent.speed / walkSpeed;
    } //상태변화

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f) // ai상태가 랜더링이며, 랜더링 거리가 0.1f 이하일때
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if(playerDistance < attackDistance)
        {
            AiTendencyChk();
        }
    }

    void AiTendencyChk()
    {
        switch (aiTendency)
        {
            case AITendency.hostile:
                SetState(AIState.Attacking);
                break;
            case AITendency.neutral:
                if (curHealth < Health)
                {
                    SetState(AIState.Attacking);
                }
                break;
            case AITendency.friendly:
                SetState(AIState.Runaway);
                break;
        }
    }

    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            if (Time.time - lastAttackTime > attackRate) // 공격
            {
                agent.isStopped = true;
                lastAttackTime = Time.time;
                transform.position += new Vector3(0, 1, 0);
                playerStatus.HealthChange(-damage);
                Debug.Log(playerStatus.CurHealth);
            }
        }
        else
        {
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(Player.transform.position, path))
                {
                    agent.SetDestination(Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    void RunawayUpdate()
    {
        NavMeshHit hit;

        if (playerDistance < attackDistance)
        {
            transform.rotation = Player.transform.rotation;
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        }
        else
        {

        }
    }

    void WanderToNewLocation() // 새로운 위치로 이동
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation() // 탐지범위
    {
        NavMeshHit hit;

        // Random.onUnitSphere : 반지름이 1인 구 위의 점 위치를 Vector3로 가져옴
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    bool IsPlayerInFieldOfView() // 플레이어가 있는 방향
    {
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void HealthChange(int damage)
    {
        Debug.Log("시작");
        curHealth = Util.PlusAndClamp(curHealth, damage, Health);
        Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            death();
            //TODO: 사망처리 필요
            Debug.Log(name + " : 죽었다");
        }
    }

    private void death()
    {
        int RnadomNum = Random.RandomRange(0, dropItem.Length);
        for (int j = 0; j < dropItem.Length; j++)
        {
            Instantiate(dropItem[j], transform.position + Vector3.up, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
