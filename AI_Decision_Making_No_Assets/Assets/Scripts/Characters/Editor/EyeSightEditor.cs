using Characters.Senses;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EyeSight))]
public class EyeSightEditor : Editor
{
    private void OnSceneGUI()
    {
        var sight = (EyeSight)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(sight.transform.position, Vector3.up, Vector3.forward, 360, sight.Radius);

        Vector3 viewAngle1 = DirectionFromAngle(sight.transform.eulerAngles.y, -sight.Angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(sight.transform.eulerAngles.y, sight.Angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(sight.transform.position, sight.transform.position + viewAngle1 * sight.Radius);
        Handles.DrawLine(sight.transform.position, sight.transform.position + viewAngle2 * sight.Radius);

        if (!sight.TryGetTarget(out var enemy))
            return;
        
        Handles.color = Color.red;
        Handles.DrawLine(sight.transform.position, enemy.transform.position);
    }

    private Vector3 DirectionFromAngle(float euleryY, float angleInDegrees)
    {
        angleInDegrees += euleryY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
