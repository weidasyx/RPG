using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")] 
    [SerializeField] private UI_SkillTreeSlot UnlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    
    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked;
    
    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    
    [Header("Multi Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canMultiCrystal;
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private int amountOfCrystals;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
        UnlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
    }

    #region Unlock Skill Region

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();
        UnlockCrystalMirage();
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
            canMultiCrystal = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }

    private void UnlockCrystalMirage()
    {
        if (UnlockCloneInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }
    

    #endregion
    

    public override void UseSkill()
    {
        base.UseSkill();
        if (canUseMultiCrystals())
            return;
        
        
        

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
        
    }

    public void CurrentCrystalChooseRandomTarget() =>
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
    

    private bool canUseMultiCrystals()
    {
        if (canMultiCrystal)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfCrystals)
                    Invoke("ResetAbility", useTimeWindow);
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;
                    RefillCrystal();
                }
                return true;
            }
            
            
            
            
        }
        
        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfCrystals - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }

        cooldownTimer = multiCrystalCooldown;
        RefillCrystal();
    }
}
