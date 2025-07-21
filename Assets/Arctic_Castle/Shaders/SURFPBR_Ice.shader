Shader "ArcticCastle/SURFPBR_Ice" {
    Properties {
        _MainTex ("Base Albedo (RGB)", 2D) = "white" {}
        _Smoothness("Smoothness", range(0,1)) = 0.5
        [Normal]_BumpMap ("Base Normal", 2D) = "bump" {} 
        [NoScaleOffset][Normal]_DetailBump ("Detail Normal", 2D) = "bump" {}  
        _DetailInt ("DetailNormal Intensity", Range(0,1)) = 0.4
        _DetailTiling("DetailNormal Tiling", float) = 2 
        _SelfIlumination ("Self Illumination", Color) = (0,0,0,0)
        [NoScaleOffset]_AO("AO", 2D) = "white" {}
    }


    SubShader {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry" }
        LOD 500
 
        CGPROGRAM
        #pragma surface surf Standard interpolateview 
        #pragma target 3.0
       

        sampler2D _MainTex, _BumpMap, _DetailBump;
        half _DetailInt, _DetailTiling, _Smoothness;
        half4 _SelfIlumination;


        struct Input {
            half2 uv_MainTex; 
            half2 uv_BumpMap; 
        };


        void surf (Input IN, inout SurfaceOutputStandard o) {      

            //Texture Inputs
            half3 main = tex2D (_MainTex, IN.uv_MainTex);            
            half3 norm = UnpackNormal(tex2D (_BumpMap, IN.uv_BumpMap));
            half ao = tex2D(_MainTex, IN.uv_BumpMap);
            half2 detnorm = UnpackNormal(tex2D (_DetailBump, IN.uv_MainTex * _DetailTiling));

            half3 blendedNormal = norm + half3(detnorm.r * _DetailInt, detnorm.g * _DetailInt, 0);

       

            o.Albedo = main;
            o.Smoothness = _Smoothness;
            o.Emission = ao * _SelfIlumination;
            o.Metallic = 0;
            o.Normal = blendedNormal.rgb;          

        }
        ENDCG
    } 
    FallBack "Standard"
}