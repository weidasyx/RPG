using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")] [SerializeField]
    private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Space] [Header("Clone attack")] [SerializeField]
    private UI_SkillTreeSlot cloneAttackUnlockButton;

    [SerializeField] private float cloneAttackMultipiler;
    [SerializeField] private bool canAttack;
    
    [Header("Aggresive clone")]
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneMultiplier;

    public bool canApplyOnHitEffect { get; private set; }



    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();
        
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            
            return;
        }
        
        
        GameObject newClone = Instantiate(clonePrefab);
        
        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    #region Unlock Region

    protected override void CheckUnlock()
    {
        UnlockAggresiveClone();
        UnlockMultipleClone();
        UnlockCloneAttack();
        UnlockCrystalInsteadOfClone();
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultipiler;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneMultiplier;
        }
    }

    private void UnlockMultipleClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion

   

    public void CreateCloneWithDelay(Transform _clonePosition)
    {
       
        StartCoroutine(CloneDelayCorotine(_clonePosition, new Vector3(2 * player.facingDir, 0)));
            
    }

    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
