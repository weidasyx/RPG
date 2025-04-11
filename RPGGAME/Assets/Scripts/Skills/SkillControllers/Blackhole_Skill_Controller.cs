using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyprefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    public float maxSize;
    public float growspeed;
    public float Shrinkspeed;
    [SerializeField] private bool canGrow = true;
    [SerializeField] private bool canShrink = false;
    private float blackholeTimer = 5;

    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private bool playerCanDisappear;
    
    // If there is public then it will auto Instantiate.
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growspeed, float _shrinkspeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growspeed = _growspeed;
        Shrinkspeed = _shrinkspeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        
        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisappear = false;
    }

    private void Update()
    {
        
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0 )
        {
            blackholeTimer = Mathf.Infinity;
            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        
        
        CloneAttackLogic();
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growspeed); 
        }
        

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * Shrinkspeed);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
            
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;
        DestoryHotKey();
        cloneAttackReleased = true;
        canCreateHotKey = false;
        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.MakeTransParent(true);
        }
        
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased  && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randonIndex = Random.Range(0, targets.Count);
            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
            {
                xOffset = -2;
            }

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randonIndex], new Vector3(xOffset, 0));
            }
            
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }

        }
    }

    private void FinishBlackHoleAbility()
    {
        playerCanExitState = true;
        cloneAttackReleased = false;
        canShrink = true;
    }

    private void DestoryHotKey()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测进入触发器的碰撞体是否具有`Enemy`组件
        if (collision.GetComponent<Enemy>() != null)
        {
            // 使敌人时间冻结
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;
        if (!canCreateHotKey)
            return;
        GameObject newHotkey = Instantiate(hotKeyprefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotkey);
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        Blackhole_HotKey_Controller newHotKeyScript = newHotkey.GetComponent<Blackhole_HotKey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);



}
