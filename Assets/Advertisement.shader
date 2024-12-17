Shader "Toon/Basic Outline With Texture" {
    Properties {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
        _MainTex ("Base Texture (RGB)", 2D) = "white" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float2 uv : TEXCOORD0; // Add UV coordinates
    };

    struct v2f {
        float4 pos : SV_POSITION;
        fixed4 color : COLOR;
        float2 uv : TEXCOORD0; // Pass UVs to the fragment shader
    };

    uniform float _Outline;
    uniform float4 _OutlineColor;
    uniform sampler2D _MainTex; // Texture sampler

    v2f vert(appdata v) {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);

        // Calculate outline offset
        float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
        float2 offset = TransformViewToProjection(norm.xy);

        #ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE
            o.pos.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;
        #else
            o.pos.xy += offset * o.pos.z * _Outline;
        #endif

        o.color = _OutlineColor;
        o.uv = v.uv; // Pass UVs to fragment shader
        return o;
    }
    ENDCG

    SubShader {
        Tags { "RenderType" = "Opaque" }
        UsePass "Toon/Basic/BASE"

        // Add texture rendering Pass
        Pass {
            Name "TEXTURE"
            Tags { "LightMode" = "ForwardBase" }
            Cull Back
            ZWrite On
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Pass UVs
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv); // Sample texture
                return texColor * _Color; // Combine texture with color tint
            }
            ENDCG
        }

        // Outline Pass (unchanged)
        Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}