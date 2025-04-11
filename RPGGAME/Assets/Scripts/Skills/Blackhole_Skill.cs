using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;
    
    Blackhole_Skill_Controller currentBlackhole;

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackHoleUnlocked = true;
        }
    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackholeDuration);
        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;
        
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        
        return false;
    }

    public float GetBlackholeSize()
    {
        return maxSize/2 ;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackHole();
    }
}
