using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour, IUIRaycastable
{
    [SerializeField] private Transform _entryPoint;
    [SerializeField] private Image _buttonImage; // Reference to the button's Image component
    [SerializeField] private Room _room;

    public event Action<JoinRoom> OnRequestRoomEntered;

    private Coroutine _blinkCoroutine;
    private bool _isBlinking = false;

    public Transform EntryPoint
    {
        get => _entryPoint;
        set => _entryPoint = value;
    }

    public Room Room => _room;

    private void Awake()
    {
        if (_buttonImage == null)
        {
            _buttonImage = GetComponent<Image>();
            if (_buttonImage == null)
            {
                Debug.LogError("Button Image component is not assigned or found on the Room GameObject.");
            }
        }
    }

    private void OnEnable()
    {
        StartBlink();
    }

    private void OnDisable()
    {
        StopBlink();
    }

    public void StartBlink()
    {
        if (_buttonImage != null && !_isBlinking)
        {
            _blinkCoroutine = StartCoroutine(BlinkEffect());
            _isBlinking = true;
        }
    }

    public void StopBlink()
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
            _isBlinking = false;
            if (_buttonImage != null)
            {
                var color = _buttonImage.color;
                color.a = 1f;
                _buttonImage.color = color;
            }
        }
    }

    private IEnumerator BlinkEffect()
    {
        float duration = 1f;
        float minAlpha = 0.3f;
        float maxAlpha = 1f;
        while (true)
        {
            // Fade out
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(maxAlpha, minAlpha, t / duration);
                SetImageAlpha(alpha);
                yield return null;
            }
            // Fade in
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(minAlpha, maxAlpha, t / duration);
                SetImageAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetImageAlpha(float alpha)
    {
        if (_buttonImage != null)
        {
            var color = _buttonImage.color;
            color.a = alpha;
            _buttonImage.color = color;
        }
    }

    public void OnRaycastHit()
    {
        Debug.Log("Room UI Button Clicked!" + gameObject.name);
        OnRequestRoomEntered?.Invoke(this);
    }
}