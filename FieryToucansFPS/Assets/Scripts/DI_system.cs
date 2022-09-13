using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DI_system : MonoBehaviour
{
    [Header("References")]
    [SerializeField]  DamageIndicator indicatorPrefab;
    [SerializeField]  RectTransform holder;
    [SerializeField]  new Camera camera;
    [SerializeField]  Transform player;

    private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

    void Create(Transform target)  {
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target);

        Indicators.Add(target, newIndicator);
    }

}
