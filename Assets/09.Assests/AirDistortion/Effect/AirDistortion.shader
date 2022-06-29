// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AirDistortion"
{
    Properties
    {
	    _MainTex("Base", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    uniform sampler2D   _MainTex;
    uniform sampler2D   _NoiseTex;
    uniform sampler2D   _MaskTex;
    uniform float       _Intensity;
    uniform float       _NoiseScale;
    uniform float       _NoiseSpeed;
    uniform float       _MaskWeight;
    uniform float4      _MaskTex_TexelSize;

    struct v2f
    {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
        float2 noise_uv0 : TEXCOORD1;
        float2 noise_uv1 : TEXCOORD2;
    };

    v2f vert(appdata_img v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        o.noise_uv0 = float2(o.uv.x * _NoiseScale, o.uv.y * _NoiseScale + _Time.x * 0.5 * _NoiseSpeed);
        o.noise_uv1 = float2(o.uv.x * _NoiseScale, o.uv.y * _NoiseScale * 1.1 + _Time.x * _NoiseSpeed);
        return o;
    }

    half4 heatAir(v2f i) : COLOR
    {
        float noise0 = tex2D(_NoiseTex, i.noise_uv0);
        float noise1 = tex2D(_NoiseTex, i.noise_uv1);
        float4 noise = noise0 * noise1;

        float2 mask_uv = i.uv;
        #if UNITY_UV_STARTS_AT_TOP
            mask_uv.y = 1 - mask_uv.y;
        #endif

        float mask = tex2D(_MaskTex, mask_uv);
        mask = lerp(1, mask, _MaskWeight);

        return tex2D(_MainTex, i.uv + noise * _Intensity * mask);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment heatAir

            ENDCG
        }
    }

    Fallback off
}