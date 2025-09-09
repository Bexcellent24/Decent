using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level1Boss : MonoBehaviour
{
   //----------Events----------//
   public delegate void BossHitAction(int bossLife);
   public static event BossHitAction OnBossHit;
   
   public delegate void BossDefeatedAction();
   public static event BossDefeatedAction OnBossDefeated;
   
   //----------Variables----------//

   [SerializeField] private GameObject limboBoss;
   [SerializeField] private float speed = 8;
   [SerializeField] private float fallSpeed = 32;
   
   private float moveInputHorizontal;
   private float moveInputVertical;
   private Rigidbody rbB;
   private int bossLife = 0;

   
   
   //----------MonoBehaviour Methods----------//
   private void Awake()
   {
      rbB = limboBoss.GetComponent<Rigidbody>();
   }

   private void Update()
   {
      moveInputHorizontal = Input.GetAxis("Horizontal");
      moveInputVertical = Input.GetAxis("Vertical");
   }

   private void FixedUpdate()
   {
      Vector3 fallingMove = transform.up * (-1 * fallSpeed * Time.fixedDeltaTime);
      rbB.velocity = new Vector3(moveInputHorizontal * -2 ,0,moveInputVertical * -2) * speed;
      rbB.MovePosition(rbB.position + fallingMove);
      
   }

   void OnEnable()
   {
      PickUpTrigger.OnBossPickUp += OnBossPickUpHandler;
   }
   private void OnDisable()
   {
      PickUpTrigger.OnBossPickUp -= OnBossPickUpHandler;
   }
   
   
   //----------Event Handlers----------//
   private void OnBossPickUpHandler()
   {
      Debug.Log("OnBossPickUpHandler");

      bossLife++;
      Debug.Log("Boss Life Number" + bossLife);
      OnBossHit?.Invoke(bossLife);
      
      
      if (bossLife == 5)
      {
         bossLife = 0;
         OnBossDefeated?.Invoke();
         limboBoss.SetActive(false);
         Debug.Log("Limbo Defeated");
      }
   }
   
}
