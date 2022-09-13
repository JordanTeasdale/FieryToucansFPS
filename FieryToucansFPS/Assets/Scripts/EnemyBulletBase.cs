using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    public Transform shooter;

    public void SetShooter(Transform _shooter) {
        shooter = _shooter;
    }
}
