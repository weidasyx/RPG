using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    [Header("Enemy Effects")]
    [SerializeField] private GameObject igniteEffectPrefab;
    [SerializeField] private GameObject chillEffectPrefab;
    [SerializeField] private GameObject shockEffectPrefab;

    public void ShowEffect(EffectType effectType, float duration, Vector3 offset)
    {
        GameObject effectPrefab = null;

        switch (effectType)
        {
            case EffectType.Ignite:
                effectPrefab = igniteEffectPrefab;
                break;
            case EffectType.Chill:
                effectPrefab = chillEffectPrefab;
                break;
            case EffectType.Shock:
                effectPrefab = shockEffectPrefab;
                break;
        }

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position + offset, Quaternion.identity);
            effect.transform.SetParent(transform);
            Destroy(effect, duration);
        }
    }
}
