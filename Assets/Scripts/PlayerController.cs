using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;//�̵��ӵ�
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;
    [SerializeField]
    private float lookSensitivity;//���콺 �þ� ���� �ΰ���

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    float crouchPosY;
    float originPosY;
    float applyCrouchPosY;

    //���� ����
    bool isWalk = false;
    bool isRun = false;
    bool isGround = true;
    bool isCrouch = false;

    Vector3 LastPos;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    //�ʿ� ������Ʈ
    [SerializeField]
    Camera theCamera;
    Rigidbody myRigid;
    CapsuleCollider theCollider;
    GunController theGunController;
    CrossHair theCrossHair;
    StatusController theStatusController;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        theCollider = GetComponent<CapsuleCollider>();
        theGunController = FindAnyObjectByType<GunController>();
        theCrossHair=FindAnyObjectByType<CrossHair>();
        theStatusController = FindAnyObjectByType<StatusController>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {

        if (!Inventory.inventoryActivated)
        {

            TryCrouch();
            TryRun();
            IsGround();
            TryJump();

            Move();
            chkMove();
            CameraRotation();
            CharacterRotation();
        }
        
    }
    //�ɱ�
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();//�¿��� �� Ű �۵�
        }
    }
    void Crouch()
    {
        isCrouch = !isCrouch;
        theCrossHair.crouchAni(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());

    }
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY,applyCrouchPosY,0.3f);//����
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

    }



    //�޸���
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Runnung();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            runningCancel();
        }
    }
    void Runnung()
    {
        //���� ���¿��� �޸� �� �ɱ� ����
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrossHair.runAni(isRun);
        theStatusController.DecreaseSP(10);
        applySpeed = runSpeed;

    }
    void runningCancel()
    {
        isRun = false;
        theCrossHair.runAni(isRun);
        applySpeed = walkSpeed;

    }

    //����
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, theCollider.bounds.extents.y + 0.1f); //�̲������� ������ ����
        if(!isRun)
            theCrossHair.jumpAni(!isGround); //�����ϸ� �޸��� �Ͱ� ���� ���ڼ� �ִ� ���
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP()>0)
        {
            Jump();
        }
    }
    private void Jump()
    {
        //���� ���¿��� ������ �ɱ� ����
        if (isCrouch)
            Crouch();
        theStatusController.DecreaseSP(10);
        myRigid.velocity = transform.up * jumpForce;
    }

    //�̵�
    private void Move()
    {
        float _moveDriX = Input.GetAxisRaw("Horizontal");
        float _moveDriZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDriX;
        Vector3 _moveVertical = transform.forward * _moveDriZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    void chkMove()
    {
        if (!isRun&&!isCrouch&&isGround) //run�� �޸��ٴ� �Ҹ���, ���� �� ���� �ִ� ���״�, �����ϸ� �� ���� �ִ� ������
        {
            if (Vector3.Distance(LastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else
                isWalk = false;

            theCrossHair.walkAni(isWalk);
            LastPos = transform.position;
        }
    }
    //�þ�
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX,0,0);

    }
    private void CharacterRotation()
    {

        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0,_yRotation,0) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    public bool GetRun()
    {
        return isRun;
    }
}
