using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Ignite,
    Chill,
    Shock
}

[System.Serializable]
public class EffectData
{
    public EffectType type;
    public GameObject prefab;
}
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Enemy enemy;

    [Header("Flash FX")] 
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;
    
    [Header("Ailment color")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
    
    [Header("Ailment FX")]
    [SerializeField] private List<EffectData> effects;
    

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    public void IgniteFXfor(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        
        Invoke("CancelRedBlink", _seconds);
    }
    
    public void ShockFXfor(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        
        Invoke("CancelRedBlink", _seconds);
    }
    
    public void ChillFXfor(float _seconds)
    {
        InvokeRepeating("ChillColorFx", 0, .3f);
        
        Invoke("CancelRedBlink", _seconds);
    }
    
    public void ShowEffect(EffectType effectType, float duration, Vector3 offset)
    {
        var effectData = effects.Find(e => e.type == effectType);
        if (effectData != null)
        {
            GameObject effect = Instantiate(effectData.prefab, transform.position + offset, Quaternion.identity);
            effect.transform.SetParent(transform);
            Destroy(effect, duration);
        }
    }

    

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
        
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    
    
}
