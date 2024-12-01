using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthBar;
        UpdateHealthBar();
    }

    // private void Update()
    // {
    //     UpdateHealthBar();
    // }

    private void FlipUI()
    {
        myTransform.Rotate(0f, 180f, 0f);
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
    }
}
