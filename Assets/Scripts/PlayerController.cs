using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    //ĳ���� ������ ����
    [SerializeField]
    private float walkSpeed;// �ȱ� �� �̵��ӵ�
    [SerializeField]
    private float runSpeed; // �޸��� �� �̵��ӵ�
    [SerializeField]
    private float crouchSpeed; // ���� �� �̵��ӵ�
    [SerializeField]
    private float applySpeed; // ���� ����� �̵��ӵ�
    [SerializeField]
    private float jumpForce; // ���� ����
    [SerializeField]
    private float decreaseJump; // ������ ���¹̳� �Ҹ� ��ġ

    // ī�޶� ����
    [SerializeField]
    private float lookSens; // ���콺 ����
    [SerializeField]
    private float lookLimit; // Y�� ����
    private float currentCameraRotationX;
    [SerializeField]
    private float crouchPosY; // �ɾ��� �� ���� ����
    private float defaultPosY; // �⺻ ���� ���� 
    private float applyCrouchPosY; // ���� ����� ����
    
    // ���� üũ
    public static bool isGround; 
    public static bool isRun;
    private bool isCrouch; 

   

    // ������Ʈ
    [SerializeField] private new Camera camera;
    [SerializeField] private Inventory inventory;
    private CapsuleCollider capsuleCol;
    private Rigidbody rigid;

    // Update is called once per frame
    void Start()
    {
        applySpeed = walkSpeed;
        capsuleCol = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        defaultPosY = camera.transform.localPosition.y;
        applyCrouchPosY = defaultPosY;
    }
    void Update()
    {
        Move();
        if (Inventory.inventoryActivated == false && Menu.isMenuOpen == false)
        {
            
            CameraRotation();
            PlayerRotation();
            Running();
            TryCrouch();
            IsGround();
            Jump();
            //Debug.Log(StatusManager.IsUsedStamina);
        }
    }
    // �÷��̾� �̵�
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirY = Input.GetAxisRaw("Vertical");

        Vector3 moveX = transform.right * moveDirX;
        Vector3 moveY = transform.forward * moveDirY;

        Vector3 velocity = (moveX + moveY).normalized * applySpeed;
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    
    // ī�޶� Y�� ���� �̵�
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSens;

        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -lookLimit, lookLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // ī�޶� �¿� ���� �̵��� ���� ĳ���� ȸ��
    private void PlayerRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 playerRotationY = new Vector3(0f, yRotation, 0f) * lookSens;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(playerRotationY));
    }
    // ����
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true && StatusManager.currentSp > 0 ) 
        {
            StatusManager.instance.DecreaseStamina(decreaseJump);
            rigid.velocity = transform.up * jumpForce;

        }
    }
    
    // �� üũ
    private void IsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * (capsuleCol.bounds.extents.y+ 0.1f), Color.black);
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCol.bounds.extents.y + 0.1f);
    }
    // �޸��� ���� �� ����
    public void Running()
    {
        if(Input.GetKey(KeyCode.LeftShift) && isGround == true&& StatusManager.CurrentSp > 0)
        {
            Run();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || StatusManager.CurrentSp <= 0)
        {
            RunCancel();
        }
    }
    public void Run()
    {
        if(StatusManager.currentSp > 0)
        {
            if (isCrouch)
                Crouch();
            StatusManager.IsUsedStamina = true;
            isRun = true;
            applySpeed = runSpeed;
        }
        if (StatusManager.currentSp < 0)
            isRun = false;
    }
    public void RunCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    // �ɱ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch; // ����
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = defaultPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    //  �ɱ� �� �ε巯�� ī�޶� �̵�
    IEnumerator CrouchCoroutine()
    {
        float posY = camera.transform.localPosition.y;
        int count = 0;
        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            camera.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 10)
                break;
            yield return null;
        }
        camera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
    

 
}
