using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [Header("Player Effects")]
    [SerializeField] private GameObject canigniteEffectPrefab;
    [SerializeField] private GameObject canchillEffectPrefab;
    [SerializeField] private GameObject canshockEffectPrefab;

    public void ShowEffect(EffectType effectType, float duration, Vector3 offset)
    {
        GameObject effectPrefab = null;

        switch (effectType)
        {
            case EffectType.Ignite:
                effectPrefab = canigniteEffectPrefab;
                break;
            case EffectType.Chill:
                effectPrefab = canchillEffectPrefab;
                break;
            case EffectType.Shock:
                effectPrefab = canshockEffectPrefab;
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
