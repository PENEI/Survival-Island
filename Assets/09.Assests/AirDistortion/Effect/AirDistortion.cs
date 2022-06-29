using UnityEngine;
using System.Collections;

public class AirDistortion : MonoBehaviour
{
    public Shader shader;

    private Material m_material;
    protected Material material
    {
        get
        {
            if (m_material == null)
            {
                m_material = new Material(shader);
                m_material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_material;
        }
    }

    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        if (!shader.isSupported)
        {
            enabled = false;
            return;
        }
    }

    public float intensity = 1;
    public float noiseScale = 0.1f;
    public float noiseSpeed = 0.5f;
    public float maskWeight = 1;
    public Texture maskTexture = null;
    public Texture noiseTexture = null;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_NoiseTex", noiseTexture);
        if (maskTexture != null)
            material.SetTexture("_MaskTex", maskTexture);

        material.SetFloat("_Intensity", intensity * 0.01f);
        material.SetFloat("_NoiseScale", noiseScale);
        material.SetFloat("_NoiseSpeed", noiseSpeed);
        material.SetFloat("_MaskWeight", maskWeight);
        material.SetFloat("_Intensity", intensity * 0.01f);

        Graphics.Blit(source, destination, material);
	}
}
