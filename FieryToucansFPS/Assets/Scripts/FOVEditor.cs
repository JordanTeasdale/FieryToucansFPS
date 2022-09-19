using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(_EnemyAI))]

public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        _EnemyAI fov = (_EnemyAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.sightRange);

        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.sightRange);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.sightRange);

        if (fov.playerInSightRange)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
