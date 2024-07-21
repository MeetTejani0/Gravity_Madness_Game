using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Variables")]
    [SerializeField] private float speed = 5;
    [SerializeField] private LayerMask whatIsWalkable;

    [Header("Refrence")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private CinemachineFreeLook playerFreeLookCamera;

    private CharacterController controller;
    private Camera mainCamera;
    private float turnSmoothTime = 0.08f;
    private float turnSmoothVelocity;

    private float moveX;
    private float moveY;
    private bool canMove;
    private bool isOnY;

    private bool isGrounded;

    private Vector3 gravityVelocity;

    public static Action<int> onPointBoxCollected;
    public static Action onGameOver;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        canMove = true;
        Physics.gravity = new Vector3(0, -3, 0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Gravity();
        GravityController();
        GroundCheck();




        if (canMove)
        {
            Movement();

        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.1f, whatIsWalkable);
        
    }

    private void Movement()
    {

        if(Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        else if(Input.GetKey(KeyCode.S)) { moveY = -1;}
        else moveY = 0;

        
        if(Input.GetKey(KeyCode.D)) 
        {
            moveX = isOnY? -1:1;
        }
        else if(Input.GetKey(KeyCode.A)) { moveX = isOnY ? 1 : -1; }
        else moveX = 0;

        /*if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravityVelocity = groundCheckTransform.up * 100;
            gravityVelocity += Physics.gravity * Time.deltaTime;
            controller.Move(gravityVelocity * Time.deltaTime);
            //Debug.Log(groundCheckTransform.up);
        }*/


        Vector3 direction = new Vector3(moveX, 0, moveY).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float setAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, setAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);

            Vector3 moveDirection = Quaternion.Euler(0,setAngle,0) * Vector3.forward;
            controller.Move(moveDirection * speed * Time.deltaTime);

            playerAnimator.SetBool("Walk", true);

        }
        else
        {
            playerAnimator.SetBool("Walk", false);

        }

    }

    private void Gravity()
    {

        

        if (isGrounded)
        {
            gravityVelocity = Physics.gravity;
            playerAnimator.SetBool("Falling", false);
        }
        else
        {
            gravityVelocity += Physics.gravity * Time.deltaTime;
            playerAnimator.SetBool("Falling", true);
        }

        controller.Move(gravityVelocity * Time.deltaTime);
    }

    private void GravityController()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)|| (Input.GetKeyDown(KeyCode.Q) && isGrounded))
        {
            Physics.gravity = new Vector3(0,3,0);
            gravityVelocity = Vector3.zero;
            transform.DORotate(new Vector3(0, 0, 180), 0.5f);
            playerFreeLookCamera.m_Lens.Dutch = 180;
            //CameraInputInvert();
            playerFreeLookCamera.m_XAxis.m_InvertInput = true;
            playerFreeLookCamera.m_YAxis.m_InvertInput = false;

            isOnY = true;

        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)|| (Input.GetKeyDown(KeyCode.E) && isGrounded))
        {
            Physics.gravity = new Vector3(0, -3, 0);
            gravityVelocity = Vector3.zero;
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            playerFreeLookCamera.m_Lens.Dutch = 0;
            //CameraInputInvert();
            playerFreeLookCamera.m_XAxis.m_InvertInput = false;
            playerFreeLookCamera.m_YAxis.m_InvertInput = true;

            isOnY = false;

        }

        /*if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Physics.gravity = new Vector3(-3, 0, 0);

            gravityVelocity = Vector3.zero;
            transform.DORotate(new Vector3(0, 0, -90), 0.25f);
            playerFreeLookCamera.m_Lens.Dutch = -90;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Physics.gravity = new Vector3(3, 0, 0);

            gravityVelocity = Vector3.zero;
            transform.DORotate(new Vector3(0, 0, 90), 0.25f);
            playerFreeLookCamera.m_Lens.Dutch = 90;
        }*/

    }
    private void CameraInputInvert()
    {
        playerFreeLookCamera.m_XAxis.m_InvertInput = !playerFreeLookCamera.m_XAxis.m_InvertInput;
        playerFreeLookCamera.m_YAxis.m_InvertInput = !playerFreeLookCamera.m_YAxis.m_InvertInput;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<ICollectable>(out ICollectable collector))
        {
            onPointBoxCollected?.Invoke(collector.CollectPoint());
            collector.CollectEffect();
        }

        if(other.gameObject.TryGetComponent<DeadZone>(out DeadZone zone))
        {
            Debug.Log("GAME OVER");
            onGameOver?.Invoke();

        }

    }
}
