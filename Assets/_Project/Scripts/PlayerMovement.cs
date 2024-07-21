using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider _collider;


    [Header("Variable")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;

    [SerializeField] private LayerMask whatIsWalkable;

    [Header("Refrence")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private CinemachineFreeLook playerCineCam; 

    private float moveX;
    private float moveY;
    private Vector3 moveDirection;
    private float turnSmoothTime = 0.1f;
    //private float turnSmoothTimeforGravityRotation = 0.2f;
    private float turnSmoothVelocity;

    private bool isGrounded;

    private Camera mainCam;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _collider = GetComponent<CapsuleCollider>();
        mainCam = Camera.main;
        Physics.gravity = new Vector3(0, -3, 0);


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InputMovement();
        Rotation();
        GroundCheck();
        SpeedLimit();
        StartCoroutine(GravityController());
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        else if (Input.GetKey(KeyCode.S)) { moveY = -1; }
        else moveY = 0;

        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
        }
        else if (Input.GetKey(KeyCode.A)) { moveX = -1; }
        else moveX = 0;
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(moveX, 0, moveY).normalized;
        if(direction.magnitude > 0.1)
        {
            //moveDirection = mainCam.transform.right * moveX + mainCam.transform.forward * moveY;
            moveDirection = groundCheckTransform.right * moveX + groundCheckTransform.forward * moveY;
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.1f, whatIsWalkable);

    }
    private void Rotation()
    {
        Vector3 direction = new Vector3(moveX, 0, moveY).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float setAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, setAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,angle, transform.eulerAngles.z);
        }
        
    }
    private void SpeedLimit()
    {
        Vector3 flatVelo = new Vector3(rb.velocity.x,0,rb.velocity.z);
        if(flatVelo.magnitude > moveSpeed)
        {
            Vector3 limitVelo = flatVelo.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVelo.x,rb.velocity.y,limitVelo.z);
        }
    }

    private IEnumerator GravityController()
    {
        // collider direction 0,1,2 curresponding X,Y,Z
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Physics.gravity = new Vector3(0,-3,0);

            yield return new WaitForSecondsRealtime(1f);
            playerCineCam.m_Lens.Dutch = 0;
            //_collider.direction = 1;
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Physics.gravity = new Vector3(0, 3, 0);
            
            yield return new WaitForSecondsRealtime(1f);
            playerCineCam.m_Lens.Dutch = 180;
            //_collider.direction = 1;
            transform.DORotate(new Vector3(0, 0, 180), 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Physics.gravity = new Vector3(-3, 0, 0);

            yield return new WaitForSecondsRealtime(1f);
            playerCineCam.m_Lens.Dutch = 90;
            //_collider.direction = 0;
            transform.DORotate(new Vector3(0, 0, 90), 0.25f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Physics.gravity = new Vector3(3, 0, 0);

            yield return new WaitForSecondsRealtime(1f);
            playerCineCam.m_Lens.Dutch = -90;
            //_collider.direction = 0;
            transform.DORotate(new Vector3(0, 0, -90), 0.25f);
        }
    }
}
