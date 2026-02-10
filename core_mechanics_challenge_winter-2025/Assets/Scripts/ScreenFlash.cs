using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField] private Image flashImage;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private Color deathFlashColor = Color.red;
    [SerializeField] private float flashDuration;

    private void OnEnable()
    {
        GameManager.Instance.OnBombExplode += TriggerFlash;
        GameManager.Instance.OnGameOver += TriggerDeathFlash;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnBombExplode -= TriggerFlash;
        GameManager.Instance.OnGameOver -= TriggerDeathFlash;
    }

    void Start()
    {
        if (flashImage != null)
        {
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        }
    }

    public void TriggerFlash()
    {
        if (SettingsManager.Instance.screenFlashEnabled)
        {
            flashColor = Color.white;
            StartCoroutine(FlashCoroutine(flashDuration));
        }
    }

    public void TriggerDeathFlash()
    {
        if (SettingsManager.Instance.screenFlashEnabled)
        {
            flashColor = deathFlashColor;
            StartCoroutine(FlashCoroutine(flashDuration * 1.5f));
        }
    }

    private IEnumerator FlashCoroutine(float flashDuration)
    {
        float elapsedTime = 0f;
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1);

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }
}
