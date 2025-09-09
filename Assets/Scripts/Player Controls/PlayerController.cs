using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerJumpAction();
    public static event PlayerJumpAction OnPlayerJump;
    
    [Header("Player Movement Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField]private float jumpHeight = 4;
    [SerializeField]private float jumpPeakTime = 0.75f;
    [SerializeField]private float jumpLandTime = 0.5f;
    [SerializeField] private float fallSpeed = 10f;
    
    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheckOrigin;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector3 groundCheckBoxSize = new Vector3(0.8f, 0.05f, 0.8f);

    [Header("Animation")]
    [SerializeField] private GameObject playerModel;
    
    private Rigidbody rb;
    private float moveInputHorizontal;
    private float moveInputVertical;
    
    private bool jumpPressed = false;
    private bool playerDead = false;
    private bool playerMissedPlatform = false;
    private bool bossModeMovment = false;
    private bool startingBossMode = false;
    private Vector3 sirenForce = Vector3.zero;

    
    //----------MonoBehaviour Methods----------//
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        ObstacleCollision.OnPlayerDied += OnPlayerDiedHandler;
        Obstacle.OnPlatformSideCollided += OnPlatformSideCollidedHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp += OnSpeedMultiplierPickUpHandler;
        Platform.OnBossModeTriggered += OnBossModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered += OnBossModeTriggeredHandler;
        Platform.OnLevel1ModeTriggered += OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered += OnLevelModeTriggeredHandler;
        Level2Boss.OnBoss2Calls += OnBoss2CallsHandler;
        Level2Boss.OnBoss2StopsCall += OnBoss2StopsCallHandler;
    }
    
    private void OnDisable()
    {
        ObstacleCollision.OnPlayerDied -= OnPlayerDiedHandler;
        Obstacle.OnPlatformSideCollided -= OnPlatformSideCollidedHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp -= OnSpeedMultiplierPickUpHandler;
        Platform.OnBossModeTriggered -= OnBossModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered -= OnBossModeTriggeredHandler;
        Platform.OnLevel1ModeTriggered -= OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered -= OnLevelModeTriggeredHandler;
        Level2Boss.OnBoss2Calls -= OnBoss2CallsHandler;
        Level2Boss.OnBoss2StopsCall -= OnBoss2StopsCallHandler;
    }
    
    void Update()
    {
        moveInputHorizontal = Input.GetAxis("Horizontal");
        moveInputVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround() && !bossModeMovment)
        {
            StartCoroutine(JumpSequence());
            jumpPressed = true;
            
        }
    }
    
    private void FixedUpdate()
    {
        if (!playerDead)
        {
            if (bossModeMovment)
            {
                if (startingBossMode)
                {
                    rb.velocity = Vector3.zero;
                    StartCoroutine(BossModeChangeFormSequence());
                    startingBossMode = false;
                }
                
                Vector3 fallingMove = transform.up * (-1 * fallSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + fallingMove);
                rb.velocity = new Vector3(moveInputHorizontal * 2 ,0,moveInputVertical * 2) * speed + sirenForce;
            }
            else
            {
                Vector3 forwardMove = transform.forward * (speed * Time.fixedDeltaTime);
                if (!playerDead && !playerMissedPlatform)
                {
                    rb.MovePosition(rb.position + forwardMove);
                }
        
                float verticalSpeed = rb.velocity.y;
                Vector3 verticalVelocity = Vector3.zero;
        
                if (jumpPressed)
                {
                    verticalVelocity = CalculateJumpVelocity() * Vector3.up;
           
                }
                else
                {
                    verticalVelocity += Vector3.up * (verticalSpeed - CalculateGravity() * Time.deltaTime);
                }
        
                jumpPressed = false;
        
                rb.velocity = new Vector3(moveInputHorizontal,0,0) * speed;
                rb.velocity += verticalVelocity;
            }
        }
        if (playerDead && bossModeMovment)
        {
            Vector3 fallingMove = transform.up * (-1 * fallSpeed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + fallingMove);
        }
    }
    
    
    //----------Private Custom Methods----------//
    
    private float CalculateJumpVelocity()
    {
        return (2 * jumpHeight) / jumpPeakTime;
    }
    
    private float CalculateGravity()
    {
        float time = rb.velocity.y > 0 ? jumpPeakTime : jumpLandTime;
        return (2 * jumpHeight) / (time * time);
    }
    
    private bool IsOnGround()
    {
        Collider[] colliders = Physics.OverlapBox(
            groundCheckOrigin.position,
            groundCheckBoxSize * 0.5f,
            Quaternion.identity,
            groundMask
        );
        
        return colliders.Length > 0;
    }
    
    
//----------Event Handlers----------//

    private void OnPlayerDiedHandler()
    {
        playerDead = true;
        rb.velocity = Vector3.zero;
    }

    private void OnPlatformSideCollidedHandler()
    {
        playerMissedPlatform = true;
        rb.velocity = Vector3.zero;
        playerModel.GetComponent<Animator>().Play("Falling 1");
    }

    private void OnSpeedMultiplierPickUpHandler()
    {
        StartCoroutine(SpeedPickupTimer());
    }

    private void OnBossModeTriggeredHandler()
    {
        bossModeMovment = true;
        startingBossMode = true;
    }

    private void OnLevelModeTriggeredHandler()
    {
        bossModeMovment = false;
        playerModel.GetComponent<Animator>().Play("Running");
        GetComponent<BoxCollider>().size = new Vector3(1f, 2f, 1);
        GetComponent<BoxCollider>().center = new Vector3(0, 0f, 0);
    }

    private void OnBoss2CallsHandler()
    {
        int randomForceDir = Random.Range(1, 4);

        switch (randomForceDir)
        {
            case 1: sirenForce = new Vector3(12, 0, 0);
                break;
            case 2: sirenForce = new Vector3(-12, 0, 0);
                break;
            case 3: sirenForce = new Vector3(0, 0, 12);
                break;
            default: sirenForce = new Vector3(0, 0, -12);
                break;
                
        }
        Debug.Log("Adding Force");
        
    }
    private void OnBoss2StopsCallHandler()
    {
        sirenForce = Vector3.zero;
        Debug.Log("Removing Force");
    }
    
    //----------Coroutines----------//

    private IEnumerator JumpSequence()
    {
        OnPlayerJump?.Invoke();
        playerModel.GetComponent<Animator>().Play("Running Forward Flip");
        yield return new WaitForSeconds(.8f);
        if (!playerDead)
        {
            playerModel.GetComponent<Animator>().Play("Running");
        }
    }
    
    private IEnumerator SpeedPickupTimer()
    {
        speed *= 1.5f;
        yield return new WaitForSeconds(5f);
        speed /= 1.5f;
    }
   
    private IEnumerator BossModeChangeFormSequence()
    {
        playerModel.GetComponent<Animator>().Play("Falling");
        
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider>().size = new Vector3(1.3f, .7f, 2);
        GetComponent<BoxCollider>().center = new Vector3(0, .35f, 0);
    }


}
