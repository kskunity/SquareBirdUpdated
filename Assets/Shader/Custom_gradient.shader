Shader "Custom/gradient"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _ColorTop ("Top Color", Color) = (1,1,1,0.5)
    _ColorMid ("Mid Color", Color) = (1,1,1,0.5)
    _ColorBot ("Bot Color", Color) = (1,1,1,0.5)
    _Middle ("Middle", Range(0.001, 0.999)) = 1
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Background"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Background"
      }
      LOD 100
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _ColorTop;
      uniform float4 _ColorMid;
      uniform float4 _ColorBot;
      uniform float _Middle;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 c_1;
          float4 tmpvar_2;
          float _tmp_dvx_0 = (in_f.xlv_TEXCOORD0.y / _Middle);
          tmpvar_2 = (lerp(_ColorBot, _ColorMid, float4(_tmp_dvx_0, _tmp_dvx_0, _tmp_dvx_0, _tmp_dvx_0)) * float((_Middle>=in_f.xlv_TEXCOORD0.y)));
          c_1 = tmpvar_2;
          float4 tmpvar_3;
          float _tmp_dvx_1 = ((in_f.xlv_TEXCOORD0.y - _Middle) / (1 - _Middle));
          tmpvar_3 = lerp(_ColorMid, _ColorTop, float4(_tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1));
          float tmpvar_4;
          tmpvar_4 = float((in_f.xlv_TEXCOORD0.y>=_Middle));
          c_1.xyz = (c_1 + (tmpvar_3 * tmpvar_4)).xyz;
          c_1.w = 1;
          out_f.color = c_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
