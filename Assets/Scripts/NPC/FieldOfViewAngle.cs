using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] float viewAngle;
    [SerializeField] float viewDistance;
    [SerializeField] LayerMask targetMask;

    Pig thePig;
    private void Start()
    {
        thePig = GetComponent<Pig>();
    }
    // Update is called once per frame
    void Update()
    {
        View();
    }
    void View()
    {
        //디버그용 레이를 만드는 코드
        Vector3 BounaryL = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 BounaryR = BoundaryAngle(viewAngle * 0.5f);
        Debug.DrawRay(transform.position + transform.up, BounaryL, Color.red);
        Debug.DrawRay(transform.position + transform.up, BounaryR, Color.red);

        //1차 : 영역 검사
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        //2차 : 시야 검사 (영역 내 객체들 중에서, 플레이어를 찾음)
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player") //플레이어라면
            {
                //Debug.Log("영역 내 Player");
                Vector3 _dir = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_dir, transform.forward); //0,0에서 '돼지에서 플레이어 방향의 벡터', '돼지 정면을 향하는 방향벡터(길이1)' 사이의 각
                if (_angle < viewAngle * 0.5f) //...이 시야각 내일 때
                {
                    //Debug.Log("시야 내 Player");
                    //3차 : 장애물 검사
                    RaycastHit _hit;
                    //플레이어 방향 벡터로, 거리만큼 레이를 쏴서
                    if(Physics.Raycast(transform.position+transform.up, _dir, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player") //장애물 없이 적중하면
                        {
                            thePig.Run(_hit.transform.position);
                            Debug.DrawRay(transform.position + transform.up, _dir, Color.blue);
                        }
                    }

                }
            }
        }
    }
    Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y; //객체의 회전에 종속됨
        return new Vector3(
            Mathf.Sin(_angle * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(_angle * Mathf.Deg2Rad));

    }
}
