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
        CheckIfObjectInSight += InSight;

    }
    private void OnDisable() {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= InSight;
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

    bool InSight(Transform t) {
        float angle = Vector3.Angle(new Vector3(GameManager.instance.player.transform.position.x, 0, GameManager.instance.player.transform.position.z), new Vector3(t.forward.x, 0, t.forward.z));
        return angle < 60;
    }

}
