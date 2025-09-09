using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level2Boss : MonoBehaviour
{
    
    public delegate void Boss2DefeatedAction();
    public static event Boss2DefeatedAction OnBoss2Defeated;
    
    public delegate void Boss2ShootsAction();
    public static event Boss2ShootsAction OnBoss2Shoots;
    
    public delegate void Boss2CallsAction();
    public static event Boss2CallsAction OnBoss2Calls;
    
    public delegate void Boss2StopsCallAction();
    public static event Boss2StopsCallAction OnBoss2StopsCall;
    
    public delegate void Boss2StageCompleteAction(int stage);
    public static event Boss2StageCompleteAction OnBoss2StageComplete;

    [Header("Siren Setup")] 
    [SerializeField] private GameObject lustBoss;
    [SerializeField] private float movementSpeed = 20f;
    [SerializeField] private float fallSpeed = 32;
    
    [Header("Siren Shoots Setup")] 
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject arrowPrefab;
    
    [Header("Siren Shoots Variables")] 
    [SerializeField] private float arrowSpeed;
    
    
    private bool isShooting;
    private bool isCalling;
    private bool alive = true;

    private Rigidbody rbB;
    private Vector3 currentDirection;

    private void Awake()
    {
        rbB = lustBoss.GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        alive = true;
        StartCoroutine(BossTiming());
        StartCoroutine(ChangeDirection());
    }

    private void FixedUpdate()
    {
        
        Vector3 fallingMove = transform.up * (-1 * fallSpeed * Time.fixedDeltaTime);
        
        if (alive) 
        {
            rbB.velocity = currentDirection * movementSpeed;
        }
        else
        {
            rbB.velocity = Vector3.zero;
            lustBoss.transform.localPosition = new Vector3(0, -55, 0);
        }
        
        rbB.MovePosition(rbB.position + fallingMove);
    }
    
    //----------Private Custom Methods----------//
    
    private void SetRandomDirection()
    {
        float randomAngle = Random.Range(30f, 330f);
        currentDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
    }
    
    private void SirenShoots()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody>().velocity = Vector3.up * arrowSpeed;
        Debug.Log("Shot Fired");
    }

    //----------Coroutines----------//
    
    private IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(.5f);
        SetRandomDirection();
        StartCoroutine(ChangeDirection());
    }

    private IEnumerator BossTiming()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(ShootingStage());
        yield return new WaitUntil(() => !isShooting);
        OnBoss2StageComplete?.Invoke(1);
        StartCoroutine(SirenCallsSequence());
        yield return new WaitUntil(() => !isCalling);
        OnBoss2StageComplete?.Invoke(2);
        
        StartCoroutine(ShootingStage());
        yield return new WaitUntil(() => !isShooting);
        OnBoss2StageComplete?.Invoke(3);
        StartCoroutine(SirenCallsSequence());
        yield return new WaitUntil(() => !isCalling);
        OnBoss2StageComplete?.Invoke(4);
        
        
        alive = false;
        yield return new WaitForSeconds(1f);
        
        lustBoss.SetActive(false);
        OnBoss2Defeated?.Invoke();
        Debug.Log("Lust Outlasted");
    }
    
    private IEnumerator ShootingStage()
    {
        isShooting = true;
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(.75f);
            OnBoss2Shoots?.Invoke();
            SirenShoots();
        }
        isShooting = false;
    }
    
    private IEnumerator SirenCallsSequence()
    {
        isCalling = true;
        OnBoss2Calls?.Invoke();
        yield return new WaitForSeconds(7f);
        OnBoss2StopsCall?.Invoke();
        isCalling = false;
    }
}
