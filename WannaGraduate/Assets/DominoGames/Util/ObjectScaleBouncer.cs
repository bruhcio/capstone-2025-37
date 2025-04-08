using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ObjectScaleBouncer : MonoBehaviour
{
    [SerializeField] float bounceScale = 1.25f;
    [SerializeField] float timeMultiplier = 1f;

    [Button]
    public void PlayEffect()
    {
        int instanceId = this.GetInstanceID() + 100000000;

        if (DOTween.IsTweening(instanceId))
        {
            return;
        }

        transform.DOScale(Vector3.one * bounceScale, 0.05f * timeMultiplier).SetId(instanceId).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.05f * timeMultiplier).SetId(instanceId);
        });
    }
}
