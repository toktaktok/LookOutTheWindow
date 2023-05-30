using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine;
using DG.Tweening;

public class WiggleUI : MonoBehaviour
{
    // Assign your UI element here
    public RectTransform uiElement;

    // Set your wiggle speed and range here
    public float rotateSpeed = 1f;
    public float rotateRadius = 50f;

    private Vector2 initialPosition;

    void Start()
    {
        initialPosition = uiElement.anchoredPosition;
    }

    void Update()
    {
        float angle = Time.time * rotateSpeed;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * rotateRadius;
        uiElement.anchoredPosition = initialPosition + offset;

    }
}