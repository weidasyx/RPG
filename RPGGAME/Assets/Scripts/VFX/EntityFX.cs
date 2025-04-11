using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    protected Player player;
    protected SpriteRenderer sr;
    private Enemy enemy;
    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;
    
    

    

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

    [Header("Ailment particles")] [SerializeField]
    private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    
    [Header("Hit Fx")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;

    
    

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        sr = GetComponentInChildren<SpriteRenderer>();
        
        originalMat = sr.material;
    }

    

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);
        
        Vector3 posOffset = new Vector3(randomX, randomY, 0);
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + posOffset, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = _text;
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
        igniteFx.Play();
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
        chillFx.Play();
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
        
        igniteFx.Stop();
        chillFx.Stop();
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
    
    public void MakeTransParent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
        
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void CreateHitFx(Transform _target, bool _critical)
    {
        float ZRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);
        Vector3 hitFxRotation = new Vector3(0, 0, ZRotation);

        GameObject hitPrefab = hitFX;

        if (_critical)
        {
            hitPrefab = criticalHitFX;

            float yRotation = 0;
            ZRotation = Random.Range(-45, 45);
            if (GetComponent<Entity>().facingDir == -1)
            {
                yRotation = 180;
            }
            hitFxRotation = new Vector3(0, yRotation, ZRotation);
        }
        
        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        // if (_critical == false)
        // {
        //     newHitFx.transform.Rotate(new Vector3(0f, 0, ZRotation));
        // }
        // else
        // {
        //     newHitFx.transform.localScale = new Vector3(GetComponent<Entity>().facingDir, 1, 1);
        newHitFx.transform.Rotate(hitFxRotation);
        // }
        
        
        Destroy(newHitFx, .5f);
        
    }

    


}
