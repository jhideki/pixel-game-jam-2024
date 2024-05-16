using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public float fadeDuration = 5f;
    public float shakeDuration;
    public float shakeMagnitude = 5f;
    public Image miniGameImage;

    private bool isShaking;

    private RectTransform rectTransform;
    void Start()
    {
        miniGameImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetImage(Sprite imageSprite)
    {
        if (miniGameImage == null)
        {
            miniGameImage = GetComponent<Image>();
        }
        miniGameImage.sprite = imageSprite;
    }

    public bool HasImage()
    {
        return miniGameImage.sprite != null;
    }

    public void ShakeImage()
    {
        if (!isShaking)
            StartCoroutine(Shake());
    }
    private IEnumerator Shake()
    {
        isShaking = true;
        Vector3 originalPosition = rectTransform.localPosition;
        float elaspedTime = 0.0f;

        Debug.Log(shakeDuration);
        while (elaspedTime < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            rectTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, 0f);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }

    public void ToggleImage()
    {
        if (miniGameImage.enabled)
        {
            miniGameImage.enabled = false;
        }
        else
        {
            miniGameImage.enabled = true;
        }
    }
    public IEnumerator FadeImage(bool fadeIn, Action OnImageLoaded)
    {
        ToggleImage();
        Color color = miniGameImage.color;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elaspedTime = 0f;
        color.a = startAlpha;
        miniGameImage.color = color;
        while (elaspedTime < fadeDuration)
        {
            elaspedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elaspedTime / fadeDuration);
            miniGameImage.color = color;
            yield return null;
        }
        color.a = endAlpha;
        miniGameImage.color = color;
        OnImageLoaded?.Invoke();
    }
}
