using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunScript))]
public class GunDrawerScript : Editor
{
    SerializedProperty enableExperimentalOptions;
    SerializedProperty cameraCenterRotationSpeed;
    SerializedProperty pushbackGunPosition;
    SerializedProperty distanceBeforePushback;

    void OnEnable()
    {
        enableExperimentalOptions = serializedObject.FindProperty("enableExperimentalOptions");
        cameraCenterRotationSpeed = serializedObject.FindProperty("cameraCenterRotationSpeed");
        pushbackGunPosition = serializedObject.FindProperty("pushbackGunPosition");
        distanceBeforePushback = serializedObject.FindProperty("distanceBeforePushback");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // If we call base the default inspector will get drawn.
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("<b>Experimental Options</b> - <i>Enable before playing</i> ", GUIStyle.none);
        
        enableExperimentalOptions.boolValue = EditorGUILayout.Toggle(" - Gun Movement", enableExperimentalOptions.boolValue);
        if (enableExperimentalOptions.boolValue)
        {
            cameraCenterRotationSpeed.floatValue = EditorGUILayout.FloatField("To Center Speed", cameraCenterRotationSpeed.floatValue);
            pushbackGunPosition.floatValue = EditorGUILayout.Slider("Push Back", pushbackGunPosition.floatValue, 0f, -1f);
            distanceBeforePushback.floatValue = EditorGUILayout.Slider("Distance", distanceBeforePushback.floatValue, 0f, 10f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}