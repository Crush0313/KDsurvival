using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] string animalName;
    [SerializeField] int hp;
    [SerializeField] float walkSpeed;

    bool isAction;
    bool isWalking;

    [SerializeField] float walkTime;
    [SerializeField] float waitTime;
    float currentTime;
    Vector3 dir;

    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider boxCol;
    // Start is called before the first frame update
    void Start()
    {
        //액션 : 대기
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        ElaspeTime();
        Move();
        Rotation();
    }
    private void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
        }
        
        }

    void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, dir, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    void ElaspeTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                _Reset();
        }
    }
    void RandomAction()
    {
        int _rand = Random.Range(0, 4); //0,1,2,3
        switch (_rand)
        {
            case 0:
                Wait();
                break;
            case 1:
                Eat();
                break;
            case 2:
                Peek();
                break;
            case 3:
                TryWalk();
                break;
            default:
                break;
        }
    }
    private void _Reset()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        dir.Set(0, Random.Range(0f, 360f), 0);
        RandomAction();
    }
        


    void Wait()
    {
        currentTime = waitTime;
        anim.SetTrigger("Wait");
    }
    void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }
void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
    }
void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        anim.SetBool("Walking", isWalking);
    }

}
