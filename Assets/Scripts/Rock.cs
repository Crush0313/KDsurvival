using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    int hp; //바위 체력
    [SerializeField]
    float destroyTime; //파편 제거 시간
    [SerializeField]
    SphereCollider col;

    [SerializeField]
    GameObject go_rock;
    [SerializeField]
    GameObject go_debris;
    [SerializeField]
    GameObject go_effect_prefabs;
    [SerializeField]
    GameObject go_item_prefabs;

    [SerializeField]
    int count;

    [SerializeField]
    string strike_SE;
    [SerializeField]
    string destroy_SE;

    public void Mine()
    {
        SoundManager.instance.PlaySE(strike_SE);

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if (hp <= 0)
            Destruction();
    }
    void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_SE);

        col.enabled=false;
        for (int i = 0; i < count; i++)
        {

            Instantiate(go_item_prefabs, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
