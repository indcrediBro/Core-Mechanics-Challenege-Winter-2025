using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UpAndFade : MonoBehaviour
{
    public float moveUpDistance = 1f;
    public float duration = 1f;
    public Ease moveEase = Ease.OutQuad;

    private TMP_Text text;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        if (text == null)
        {
            text = GetComponentInChildren<TMP_Text>();
            text.DOFade(1, 0);
        }
    }

    private void Start()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.forward * moveUpDistance;

        // Create tween sequence
        Sequence seq = DOTween.Sequence();

        seq.Join(transform.DOMove(endPos, duration)
            .SetEase(moveEase));

        seq.Join(text.DOFade(0f, duration));

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
