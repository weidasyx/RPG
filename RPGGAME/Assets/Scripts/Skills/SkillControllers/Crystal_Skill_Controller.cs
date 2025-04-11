using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private float moveSpeed;
    private bool canMove;
    private bool canExplode;
    private Player player;

    private bool canGrow;
    [SerializeField] private float growSpeed;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canMove = _canMove;
        canExplode = _canExplode;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeSize();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25, whatIsEnemy);
        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1f)
            {
                FinishCrystal();
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), moveSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDirection(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if(equipmentAmulet != null)
                    equipmentAmulet.Effect(hit.transform);

            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestory();
        }
    }

    public void SelfDestory() => Destroy(gameObject);
}


