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
        //����׿� ���̸� ����� �ڵ�
        Vector3 BounaryL = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 BounaryR = BoundaryAngle(viewAngle * 0.5f);
        Debug.DrawRay(transform.position + transform.up, BounaryL, Color.red);
        Debug.DrawRay(transform.position + transform.up, BounaryR, Color.red);

        //1�� : ���� �˻�
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        //2�� : �þ� �˻� (���� �� ��ü�� �߿���, �÷��̾ ã��)
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player") //�÷��̾���
            {
                //Debug.Log("���� �� Player");
                Vector3 _dir = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_dir, transform.forward); //0,0���� '�������� �÷��̾� ������ ����', '���� ������ ���ϴ� ���⺤��(����1)' ������ ��
                if (_angle < viewAngle * 0.5f) //...�� �þ߰� ���� ��
                {
                    //Debug.Log("�þ� �� Player");
                    //3�� : ��ֹ� �˻�
                    RaycastHit _hit;
                    //�÷��̾� ���� ���ͷ�, �Ÿ���ŭ ���̸� ����
                    if(Physics.Raycast(transform.position+transform.up, _dir, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player") //��ֹ� ���� �����ϸ�
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
        _angle += transform.eulerAngles.y; //��ü�� ȸ���� ���ӵ�
        return new Vector3(
            Mathf.Sin(_angle * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(_angle * Mathf.Deg2Rad));

    }
}
