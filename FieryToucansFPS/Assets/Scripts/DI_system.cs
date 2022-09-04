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

    #region Delegates
    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable() {
        CreateIndicator += Create;
      //CheckIfObjectInSight += InSight;

    }
    private void OnDisable() {
        CreateIndicator -= Create;
      //  CheckIfObjectInSight -= InSight;
    }

    void Create(Transform target)  {
        if (Indicators.ContainsKey(target)) {
            Indicators[target].Restart();
            return; 
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, GameManager.instance.player.transform, new Action( () => { Indicators.Remove(target); } ));

        Indicators.Add(target, newIndicator);

    }

    //bool InSight(Transform t) {
    //    Vector3 screenPoint = GameManager.instance.player.GetC.WorldToViewportPoint(t.position);
    //    return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    //}

}
