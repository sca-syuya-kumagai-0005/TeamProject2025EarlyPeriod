﻿Shader "Custom/RaindropStableFix"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _DropSize("Drop Size", Range(0.1, 2.0)) = 1.0
        _DropAmount("Static Drop Amount", Range(0.0, 2.0)) = 1.0
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _DropSize;
                float _DropAmount;

                #define S(a, b, t) smoothstep(a, b, t)

                float3 N13(float p)
                {
                    float3 p3 = frac(float3(p, p, p) * float3(.1031, .11369, .13787));
                    p3 += dot(p3, p3.yzx + 19.19);
                    return frac(float3(
                        (p3.x + p3.y) * p3.z,
                        (p3.x + p3.z) * p3.y,
                        (p3.y + p3.z) * p3.x));
                }

                float2 DropLayer2(float2 uv, float t, float aspect)
                {
                    float2 UV = uv;
                    float2 grid = float2(12.0, 2.0);
                    uv.y += t * 0.75;

                    float2 id = floor(uv * grid);
                    float colShift = frac(sin(dot(id, float2(12.9898, 78.233))) * 43758.5453);
                    uv.y += colShift;

                    id = floor(uv * grid);
                    float3 n = N13(id.x * 35.2 + id.y * 2376.1);
                    float2 st = frac(uv * grid) - float2(0.5, 0.0);
                    float2 adjusted_st = float2(st.x * aspect, st.y);

                    float y = UV.y * 15.0;
                    float wiggleFreq = 0.5 + n.z * 1.0;
                    float wiggleAmp = 0.03 + n.y * 0.07;

                    float pauseFreq = 5.0 + n.x * 10.0;
                    float pause = sin(y * pauseFreq) * 0.5 + 0.5;
                    float pauseEffect = S(0.1, 0.8, pause);

                    float speed = lerp(0.3, 0.9, n.z) * (1.0 - pauseEffect * 0.5);
                    float ti = frac(t * speed + n.z);
                    y = 1.0 - ti;

                    float x = n.x - 0.5;
                    float wave = sin(y * wiggleFreq + sin(y * 0.5)) * wiggleAmp;
                    x += wave * (0.5 - abs(x)) * 0.8;

                    float2 p = float2(x * aspect, y);
                    float d = length((adjusted_st - p) * grid.yx) / _DropSize;
                    float mainDrop = S(0.4, 0.0, d);

                    float r = sqrt(S(1.0, y, adjusted_st.y));
                    float cd = abs(adjusted_st.x - p.x);
                    float trail = S(0.23 * r, 0.15 * r * r, cd);
                    float trailFront = S(-0.02, 0.02, adjusted_st.y - y);
                    trail *= trailFront * r * r;

                    float dropletY = frac(UV.y * 10.0) + (adjusted_st.y - 0.5);
                    float dd = length(adjusted_st - float2(x * aspect, dropletY)) / _DropSize;
                    float droplets = S(0.3, 0.0, dd);

                    float m = mainDrop + droplets * r * trailFront;
                    return float2(m, trail);
                }

                float StaticDrops(float2 uv, float t, float aspect)
                {
                    uv *= 40.0;
                    float2 id = floor(uv);
                    float2 st = frac(uv) - 0.5;
                    st.x *= aspect;

                    float3 n = N13(id.x * 107.45 + id.y * 3543.654);
                    float2 p = (n.xy - 0.5) * 0.7;
                    float d = length(st - p) / _DropSize;
                    float fade = S(0.0, 1.0, sin((t + n.z) * 6.28));
                    return S(0.3, 0.0, d) * frac(n.z * 10.0) * fade;
                }

                float2 Drops(float2 uv, float t, float l0, float l1, float l2, float aspect)
                {
                    float s = StaticDrops(uv, t, aspect) * l0 * _DropAmount;
                    float2 m1 = DropLayer2(uv, t, aspect) * l1;
                    float2 m2 = DropLayer2(uv * 1.85, t, aspect) * l2;
                    float c = s + m1.x + m2.x;
                    c = S(0.3, 1.0, c);
                    return float2(c, max(m1.y * l0, m2.y * l1));
                }

                fixed4 frag(v2f_img i) : SV_Target
                {
                    float2 screenUV = i.uv;
                    float2 resolution = _ScreenParams.xy;
                    float aspect = resolution.x / resolution.y;

                    float2 uv = (screenUV * resolution - 0.5 * resolution) / resolution.y;
                    float T = _Time.y;
                    float t = T * 0.2;

                    float rainAmount = 1.0;
                    float maxBlur = lerp(3.0, 6.0, rainAmount);
                    float minBlur = 2.0;

                    float staticDrops = S(-0.5, 1.0, rainAmount) * 2.0;
                    float layer1 = S(0.25, 0.75, rainAmount);
                    float layer2 = S(0.0, 0.5, rainAmount);

                    float2 c = Drops(uv, t, staticDrops, layer1, layer2, aspect);
                    float2 e = float2(0.001, 0.0);
                    float cx = Drops(uv + e, t, staticDrops, layer1, layer2, aspect).x;
                    float cy = Drops(uv + e.yx, t, staticDrops, layer1, layer2, aspect).x;
                    float2 n = float2(cx - c.x, cy - c.x);

                    float focus = lerp(maxBlur - c.y, minBlur, S(0.1, 0.2, c.x));
                    float4 texCoord = float4(screenUV + n, 0, focus);
                    float3 col = tex2Dlod(_MainTex, texCoord).rgb;

                    float2 vignetteUV = screenUV - 0.5;
                    col *= 1.0 - dot(vignetteUV, vignetteUV);

                    float alpha = saturate(c.x);
                    return fixed4(col, alpha);
                }

                ENDCG
            }
        }
            FallBack "Transparent/Diffuse"
}
