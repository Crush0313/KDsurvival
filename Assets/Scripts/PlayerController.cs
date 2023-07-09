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
    private bool isRun = false;
    private bool isGround = true;
    bool isCrouch = false;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    //�ʿ� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    CapsuleCollider collider;
    GunController theGunController;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        theGunController = FindAnyObjectByType<GunController>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        TryCrouch();
        TryRun();
        IsGround();
        TryJump();

        Move();
        CameraRotation();
        CharacterRotation();
        
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
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Runnung();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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
        applySpeed = runSpeed;

    }
    void runningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;

    }

    //����
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, collider.bounds.extents.y + 0.1f);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        //���� ���¿��� ������ �ɱ� ����
        if (isCrouch)
            Crouch();
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
}
