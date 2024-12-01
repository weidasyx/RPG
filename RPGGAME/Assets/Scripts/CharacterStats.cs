using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    
    
    [Header("Major Stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;      //法强
    public Stat vitality;

    [Header("Offensive Stats")] 
    public Stat critChance;
    public Stat damage;
    public Stat critPower;
    
    
    [Header("Defense Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;
    
    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited; //does damage over time
    public bool isChilled; //not move and reduce armor
    public bool isShocked; //in turor reduce 20% accuracy but I want to improve 30% suffered damage.
    
    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockTimer;
    
    private float ailmentTimer;
    
    private float ignitedDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [SerializeField] private float lightningDamageIncrease = .3f;
    

    private PlayerFX PFX;
    private EnemyFX EFX;
    
    
    
    
    
    public int currentHealth;
    
    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        PFX = GetComponent<PlayerFX>();
        EFX = GetComponent<EnemyFX>();
            
        
        critPower.SetDefaultValue(200);
        currentHealth = GetMaxHealthValue();
        ApplyMaxDamageFX();


    }

    private void ApplyMaxDamageFX()
    {
        // Get the damage values for fire, ice, and lightning
        int fireValue = fireDamage.GetValue();
        int iceValue = iceDamage.GetValue();
        int lightningValue = lightningDamage.GetValue();

        // Determine the maximum damage type
        EffectType maxEffectType;

        if (fireValue >= iceValue && fireValue >= lightningValue)
        {
            maxEffectType = EffectType.Ignite; 
        }
        else if (iceValue >= fireValue && iceValue >= lightningValue)
        {
            maxEffectType = EffectType.Chill; 
        }
        else
        {
            maxEffectType = EffectType.Shock; 
        }

        // Apply the corresponding effect to the player
        if (PFX != null) 
        {
            Vector3 offset = new Vector3(0, -0.5f, 0); 
            PFX.ShowEffect(maxEffectType, Mathf.Infinity, offset); 
        }                                                                                                         
    }

    public void ApplyShock(float duration)
    {
        if (!isShocked)
        {
            isShocked = true;
            shockTimer = duration;
        }
        else
        {
            shockTimer = duration;
        }
    }

    

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockTimer < 0)
        {
            isShocked = false;
        }

        if (igniteDamageTimer < 0 && isIgnited)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth <= 0)
            {
                Die();
            }
            igniteDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        
        int totalDamage = strength.GetValue() + damage.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            Debug.Log(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
            
        }
        return false;
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);
        
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;
        
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _lightningDamage && _iceDamage > _fireDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        
        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFXfor(ailmentsDuration);
            EFX.ShowEffect(EffectType.Ignite, ignitedTimer, new Vector3(-0.5f, -1f, 0));
        }

        if (_chill)
        {
            chilledTimer = 3;
            isChilled = _chill;
            fx.ChillFXfor(3);
            GetComponent<Entity>().SlowEntityBy(.25f, 3);
            EFX.ShowEffect(EffectType.Chill, chilledTimer, new Vector3(-0.3f, -.2f, 0));
        }

        if (_shock)
        {
            shockTimer = 8;
            isShocked = _shock;
            fx.ShockFXfor(8);
            EFX.ShowEffect(EffectType.Shock, shockTimer, new Vector3(-0.3f, -.2f, 0));
        }
        
        
        
    }

    public virtual void TakeDamage(int damage)
    {
        DecreaseHealthBy(damage);
        
        

        if (currentHealth <= 0)
        {
            Die();
        }

        
    }

    public virtual void DecreaseHealthBy(int _damage)
    {
        if (isShocked)
        {
            currentHealth -= Mathf.RoundToInt(_damage * (1 + lightningDamageIncrease));
        }
        else
        {
            currentHealth -= _damage;
        }
        
        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = totalCritPower * _damage;
        Debug.Log("crit damage is : " + critDamage);
        return Mathf.RoundToInt(critDamage);
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }



}
