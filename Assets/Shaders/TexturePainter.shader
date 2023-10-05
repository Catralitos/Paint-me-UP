// Code from https://github.com/mixandjam/Splatoon-Ink
// Comments mine to help me understand/modify code

// Also checked out https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
Shader "TNTC/TexturePainter"{   

    Properties{
        // Default color of the material
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
    }

    SubShader{
        Cull Off ZWrite Off ZTest Off

        Pass{
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            #include "UnityCG.cginc"
            
			sampler2D _MainTex;
            float4 _MainTex_ST;

            //Position of the paint origin
            float3 _PainterPosition;
            // Paint splat radius
            float _Radius;
            // Paint splat hardness (I assume how deep the color is)
            float _Hardness;
            // Paint splat strenght (paint ammount)
            float _Strength;
            // Color of the paint being shot
            float4 _PainterColor;
            // If the UV has been prepared to be painted or painted
            float _PrepareUV;

            // vertex shader inputs
            // - vertex position
            // - an uv coordinate
            struct appdata{
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
            };

            // vertex shader outputs ("vertex to fragment")
            // - the pixel position on screen
            // - an uv coordinate
            // - a world position in Unity
            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            // signed distance function
            float mask(float3 position, float3 center, float radius, float hardness){
                // Get distance from the painter to the coordinate in world space being painted
                float m = distance(center, position);
                
                // smoothstep is used to obtain a smooth Hermite interpolation between 0 and 1
                // https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-smoothstep
                // To get the size of the circular splat of paint we interpolate the value of distance between
                // radius * hardness and radius
                // I assume only that hardness is the paint intensity and goes from 0 to 1, so it's smaller than radius
                // And we return 1 - the interpolation because the bigger the distance, the smaller the splat of paint
                return 1 - smoothstep(radius * hardness, radius, m);    
            }

            // vertex shader
            v2f vert (appdata v){
                v2f o;
                
                // To get world position, multiply the current model matrix (translation, scaling, rotation)
                // by the position of the object
                // https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                // The uv coordinates will be the same
                o.uv = v.uv;

                // Set up a default uvw map with a full alpha
				float4 uv = float4(0, 0, 0, 1);
                
                // The rasterizer expects (-1,1) coordinates as that's how clip space works, but the uv map goes from (0,1)
                // So the uv value is multiplied by 2 and has 1 subtracted.
                // So (0,1) becomes (0,2) and finally becomes (-1,1), for example
                // https://www.youtube.com/watch?v=YUWfHX_ZNCw
                
                // That value is multiplied by the projection parameters so the UV is flipped/mirrored if required
                // _ProjectionParams.x is 1.0 (or –1.0 if currently rendering with a flipped projection matrix)
                // https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
                uv.xy = float2(1, _ProjectionParams.x) * (v.uv.xy * float2( 2, 2) - float2(1, 1));

                // Set the pixel position according to the uv we calculated
				o.vertex = uv; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                // _prepareUV is changed in the PaintManager class
                // if it's larger than 0, the surface has not been painted
                // so no changes have to occur
                if(_PrepareUV > 0 ){
                    return float4(0, 0, 1, 1);
                }         

                // Sample the texture of the object
                float4 col = tex2D(_MainTex, i.uv);
                // Get the distance of the paint splat
                float f = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
                // Check the edge of the paint by multiplying the distance by the strength
                float edge = f * _Strength;

                //Returns the inbetween of the default color, the painted color according to the edge of the paint
                return lerp(col, _PainterColor, edge);
            }
            ENDCG
        }
    }
}