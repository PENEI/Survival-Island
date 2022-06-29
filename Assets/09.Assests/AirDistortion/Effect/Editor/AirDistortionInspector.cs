using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AirDistortion))]
public class AirDistortionInspector : Editor
{
    SerializedObject serObj;
    SerializedProperty intensity;
    SerializedProperty noiseScale;
    SerializedProperty noiseSpeed;
    SerializedProperty maskWeight;
    SerializedProperty maskTexture;
    SerializedProperty noiseTexture;
    /*SerializedProperty showGeneratedNormals;
    SerializedProperty offsetScale;
    SerializedProperty blurRadius;
    SerializedProperty dlaaSharp;
    SerializedProperty edgeThresholdMin;
    SerializedProperty edgeThreshold;
    SerializedProperty edgeSharpness;*/

    void OnEnable()
    {
        serObj = new SerializedObject(target);

        intensity = serObj.FindProperty("intensity");
        noiseScale = serObj.FindProperty("noiseScale");
        noiseSpeed = serObj.FindProperty("noiseSpeed");
        maskWeight = serObj.FindProperty("maskWeight");
        maskTexture = serObj.FindProperty("maskTexture");
        noiseTexture = serObj.FindProperty("noiseTexture");
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();

        intensity.floatValue = EditorGUILayout.FloatField("Intensity", intensity.floatValue);
        noiseScale.floatValue = EditorGUILayout.FloatField("Scale", noiseScale.floatValue);
        noiseSpeed.floatValue = EditorGUILayout.FloatField("Speed", noiseSpeed.floatValue);
        maskWeight.floatValue = EditorGUILayout.Slider("Mask weight", maskWeight.floatValue, 0, 1);
        noiseTexture.objectReferenceValue = EditorGUILayout.ObjectField("Noise", noiseTexture.objectReferenceValue, typeof(Texture2D));
        maskTexture.objectReferenceValue = EditorGUILayout.ObjectField("Mask (optional)", maskTexture.objectReferenceValue, typeof(RenderTexture));

        serObj.ApplyModifiedProperties();
    }
}