using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
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
    protected override void _Reset()
    {
        base._Reset();
        RandomAction();
    }
    void RandomAction()
    {

        PlayRandomSE();
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
}
