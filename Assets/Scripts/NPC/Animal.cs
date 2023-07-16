using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;
    [SerializeField] protected int hp;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isDead;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float runTime;
    [SerializeField] protected float waitTime;
    protected float currentTime;
    protected Vector3 destination;

    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    [SerializeField] protected AudioClip[] SE_Normal;
    [SerializeField] protected AudioClip SE_Hurt;
    [SerializeField] protected AudioClip SE_Dead;

    // Start is called before the first frame update
    void Start()
    {
        nav=GetComponent<NavMeshAgent>();//�⺻������ ������ٵ� ��Ź���
        theAudio = GetComponent<AudioSource>();
        //�׼� : ���
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            ElaspeTime();
            Move();
            //Rotation();
        }
    }
    private void Move()
    {
        if (isWalking || isRunning)
        {

            //rigid.MovePosition(transform.position + (transform.forward * nav.speed * Time.deltaTime));
            nav.SetDestination(transform.position + destination * 5f);
        }

    }

    protected void ElaspeTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                _Reset();
        }
    }
    protected virtual void _Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        nav.speed = walkSpeed;
        nav.ResetPath();
        destination.Set(Random.Range(-0.2f, 0.2f), 0, Random.Range(0.5f, 1f));
        //RandomAction();
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {

            hp -= _dmg;
            if (hp <= 0)
                Dead();
            else
            {
                anim.SetTrigger("Hurt");
                PlaySE(SE_Hurt);
                //Run(_targetPos);
            }
        }
    }
    protected void Dead()
    {

        anim.SetTrigger("Dead");
        PlaySE(SE_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;

    }
    protected void Wait()
    {
        currentTime = waitTime;

    }
    protected void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        nav.speed = walkSpeed;
        anim.SetBool("Walking", isWalking);
    }

    protected void PlayRandomSE()
    {
        int _random = Random.Range(0, 3); //�ϻ� ���� 3��
        PlaySE(SE_Normal[_random]);
    }
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
