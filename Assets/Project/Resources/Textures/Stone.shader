Shader "Custom/NewSurfaceShader"
{
	Properties {
		texture_1 ("Texture 1", 2D) = "white" {}
		texture_2 ("Texture 2", 2D) = "white" {}
		texture_3 ("Texture 3", 2D) = "white" {}
		texture_4 ("Texture 4", 2D) = "white" {}
		_MainTex ("Foo", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		
		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)
		
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
// int vector funtions
inline int2 toint2(int x) {
    return int2(x, x);
}
inline int2 toint2(int x, int y) {
    return int2(x, y);
}
inline int3 toint3(int x) {
    return int3(x, x, x);
}
inline int3 toint3(int x, int y, int z) {
    return int3(x, y, z);
}
inline int3 toint3(int2 xy, int z) {
    return int3(xy.x, xy.y, z);
}
inline int3 toint3(int x, int2 yz) {
    return int3(x, yz.x, yz.y);
}
inline int4 toint4(int x, int y, int z, int w) {
    return int4(x, y, z, w);
}
inline int4 toint4(int x) {
    return int4(x, x, x, x);
}
inline int4 toint4(int x, int3 yzw) {
    return int4(x, yzw.x, yzw.y, yzw.z);
}
inline int4 toint4(int2 xy, int2 zw) {
    return int4(xy.x, xy.y, zw.x, zw.y);
}
inline int4 toint4(int3 xyz, int w) {
    return int4(xyz.x, xyz.y, xyz.z, w);
}
inline int4 toint4(int2 xy, int z, int w) {
    return int4(xy.x, xy.y, z, w);
}
// float vector funtions
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float dot2(float2 x) {
	return dot(x, x);
}
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float param_rndi(float minimum, float maximum, float seed) {
	return floor(param_rnd(minimum, maximum + 1.0, seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
static const float4 p_o5161158192115_albedo_color = tofloat4(0.068359375, 0.062217712, 0.062217712, 1.000000000);
static const float p_o5161158192115_metallic = 0.000000000;
static const float p_o5161158192115_roughness = 0.660000000;
static const float p_o5161158192115_emission_energy = 0.000000000;
static const float p_o5161158192115_normal = 4.640000000;
static const float p_o5161158192115_ao = 8.900000000;
static const float p_o5161158192115_depth_scale = 1.000000000;
static const float p_o5175771147486_amount1 = 0.500000000;
static const float p_o5175653706991_amount1 = 0.500000000;
static const float p_o5174462524596_amount1 = 0.500000000;
static const float p_o5174479301817_gradient_pos[5] = {  0.065928452, 0.401067615, 0.550661266, 0.658485711, 0.853142202  };
static const float4 p_o5174479301817_gradient_col[5] = {  tofloat4(0.030541420, 0.028991699, 0.048828125, 1.000000000), tofloat4(0.261840820, 0.267481804, 0.406250000, 1.000000000), tofloat4(0.143353641, 0.127792358, 0.195312500, 1.000000000), tofloat4(0.273651123, 0.280473709, 0.382812500, 1.000000000), tofloat4(0.537528992, 0.554844856, 0.714843750, 1.000000000)  };
static const float p_o5174177311921_value = 0.320000000;
static const float p_o5174177311921_width = 0.480000000;
uniform sampler2D texture_1;
static const float texture_1_size = 1024.000000000;
static const float p_o5174194089142_gradient_pos[5] = {  0.065928452, 0.406577319, 0.550661266, 0.658485711, 0.853142202  };
static const float4 p_o5174194089142_gradient_col[5] = {  tofloat4(0.000000000, 0.000000000, 0.000000000, 1.000000000), tofloat4(0.361637115, 0.362998337, 0.396484375, 1.000000000), tofloat4(0.185232982, 0.185232982, 0.185232982, 1.000000000), tofloat4(0.352264404, 0.353929520, 0.378906250, 1.000000000), tofloat4(1.000000000, 1.000000000, 1.000000000, 1.000000000)  };
static const float p_o5175385271511_gradient_pos[5] = {  0.000000000, 0.254545003, 0.537299275, 0.772726953, 1.000000000  };
static const float4 p_o5175385271511_gradient_col[5] = {  tofloat4(0.341377258, 0.343234539, 0.371093750, 1.000000000), tofloat4(0.119972229, 0.128245652, 0.166015625, 1.000000000), tofloat4(0.376850128, 0.416287005, 0.435546875, 1.000000000), tofloat4(0.075702667, 0.076952696, 0.095703125, 1.000000000), tofloat4(0.342620850, 0.366334558, 0.425781250, 1.000000000)  };
static const float p_o5175100058835_curve_0_x = 0.000000000;
static const float p_o5175100058835_curve_0_y = 0.000000000;
static const float p_o5175100058835_curve_0_ls = 0.000000000;
static const float p_o5175100058835_curve_0_rs = 0.457632878;
static const float p_o5175100058835_curve_1_x = 0.240816325;
static const float p_o5175100058835_curve_1_y = 0.330022097;
static const float p_o5175100058835_curve_1_ls = -1.483444434;
static const float p_o5175100058835_curve_1_rs = -2.905435427;
static const float p_o5175100058835_curve_2_x = 1.000000000;
static const float p_o5175100058835_curve_2_y = 1.000000000;
static const float p_o5175100058835_curve_2_ls = 1.000000000;
static const float p_o5175100058835_curve_2_rs = 0.000000000;
uniform sampler2D texture_2;
static const float texture_2_size = 2048.000000000;
static const float p_o5165486713930_amount = 1.190000000;
uniform sampler2D texture_4;
static const float texture_4_size = 1024.000000000;
static const float p_o5177398537494_amount1 = 0.530000000;
static const float p_o5177398537494_amount2 = 0.270000000;
static const float p_o5177348205812_cx = 0.000000000;
static const float p_o5177348205812_cy = 0.000000000;
static const float p_o5177348205812_scale_x = -1.000000000;
static const float p_o5177348205812_scale_y = -1.000000000;
static const float p_o5177364983029_cx = 0.000000000;
static const float p_o5177364983029_cy = 0.000000000;
static const float p_o5177364983029_scale_x = 1.000000000;
static const float p_o5177364983029_scale_y = -1.000000000;
static const float p_o5177314651378_cx = 0.000000000;
static const float p_o5177314651378_cy = 0.000000000;
static const float p_o5177314651378_scale_x = -1.000000000;
static const float p_o5177314651378_scale_y = 1.000000000;
static const float4 p_o5177415314709_color = tofloat4(0.109622955, 0.147678569, 0.201171875, 1.000000000);
// #globals: invert (o5165386050587)
// #globals: blend
float3 blend_normal(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1 + (1.0-opacity)*c2;
}
float3 blend_dissolve(float2 uv, float3 c1, float3 c2, float opacity) {
	if (rand(uv) < opacity) {
		return c1;
	} else {
		return c2;
	}
}
float3 blend_multiply(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1*c2 + (1.0-opacity)*c2;
}
float3 blend_screen(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*(1.0-(1.0-c1)*(1.0-c2)) + (1.0-opacity)*c2;
}
float blend_overlay_f(float c1, float c2) {
	return (c1 < 0.5) ? (2.0*c1*c2) : (1.0-2.0*(1.0-c1)*(1.0-c2));
}
float3 blend_overlay(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_overlay_f(c1.x, c2.x), blend_overlay_f(c1.y, c2.y), blend_overlay_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hard_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*0.5*(c1*c2+blend_overlay(uv, c1, c2, 1.0)) + (1.0-opacity)*c2;
}
float blend_soft_light_f(float c1, float c2) {
	return (c2 < 0.5) ? (2.0*c1*c2+c1*c1*(1.0-2.0*c2)) : 2.0*c1*(1.0-c2)+sqrt(c1)*(2.0*c2-1.0);
}
float3 blend_soft_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_soft_light_f(c1.x, c2.x), blend_soft_light_f(c1.y, c2.y), blend_soft_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_burn_f(float c1, float c2) {
	return (c1==0.0)?c1:max((1.0-((1.0-c2)/c1)),0.0);
}
float3 blend_burn(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_burn_f(c1.x, c2.x), blend_burn_f(c1.y, c2.y), blend_burn_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_dodge_f(float c1, float c2) {
	return (c1==1.0)?c1:min(c2/(1.0-c1),1.0);
}
float3 blend_dodge(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_dodge_f(c1.x, c2.x), blend_dodge_f(c1.y, c2.y), blend_dodge_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_lighten(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*max(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_darken(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*min(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_difference(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*clamp(c2-c1, tofloat3(0.0), tofloat3(1.0)) + (1.0-opacity)*c2;
}
float3 blend_additive(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + c1 * oppacity;
}
float3 blend_addsub(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + (c1 - .5) * 2.0 * oppacity;
}
// #globals: adjust_hsv
float3 rgb_to_hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv_to_rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
// #globals: blend2_7 (o5175771147486)
float blend_linear_light_f(float c1, float c2) {
	return (c1 + 2.0 * c2) - 1.0;
}
float3 blend_linear_light(float2 uv, float3 c1, float3 c2, float opacity) {
return opacity*tofloat3(blend_linear_light_f(c1.x, c2.x), blend_linear_light_f(c1.y, c2.y), blend_linear_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_vivid_light_f(float c1, float c2) {
	return (c1 < 0.5) ? 1.0 - (1.0 - c2) / (2.0 * c1) : c2 / (2.0 * (1.0 - c1));
}
float3 blend_vivid_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_vivid_light_f(c1.x, c2.x), blend_vivid_light_f(c1.y, c2.y), blend_vivid_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_pin_light_f( float c1, float c2) {
	return (2.0 * c1 - 1.0 > c2) ? 2.0 * c1 - 1.0 : ((c1 < 0.5 * c2) ? 2.0 * c1 : c2);
}
float3 blend_pin_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_pin_light_f(c1.x, c2.x), blend_pin_light_f(c1.y, c2.y), blend_pin_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_hard_lerp_f(float c1, float c2) {
	return floor(c1 + c2);
}
float3 blend_hard_lerp(float2 uv, float3 c1, float3 c2, float opacity) {
		return opacity*tofloat3(blend_hard_lerp_f(c1.x, c2.x), blend_hard_lerp_f(c1.y, c2.y), blend_hard_lerp_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_exclusion_f(float c1, float c2) {
	return c1 + c2 - 2.0 * c1 * c2;
}
float3 blend_exclusion(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_exclusion_f(c1.x, c2.x), blend_exclusion_f(c1.y, c2.y), blend_exclusion_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hue(float2 uv, float3 c1, float3 c2, float opacity) {
	float3 outcol = c2;
	float3 hsv, hsv2, tmp;
	hsv2 = rgb_to_hsv(c1);
	if (hsv2.y != 0.0) {
		hsv = rgb_to_hsv(outcol);
		hsv.x = hsv2.x;
		tmp = hsv_to_rgb(hsv);
		outcol = lerp(outcol, tmp, opacity);
	}
	return outcol;
}
float3 blend_saturation(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 outcol = c2;
	float3 hsv, hsv2;
	hsv = rgb_to_hsv(outcol);
	if (hsv.y != 0.0) {
		hsv2 = rgb_to_hsv(c1);
		hsv.y = facm * hsv.y + opacity * hsv2.y;
		outcol = hsv_to_rgb(hsv);
	}
	return outcol;
}
float3 blend_color(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 outcol = c2;
	float3 hsv, hsv2, tmp;
	hsv2 = rgb_to_hsv(c1);
	if (hsv2.y != 0.0) {
		hsv = rgb_to_hsv(outcol);
		hsv.x = hsv2.x;
		hsv.y = hsv2.y;
		tmp = hsv_to_rgb(hsv);
		outcol = lerp(outcol, tmp, opacity);
	}
	return outcol;
}
float3 blend_value(float2 uv, float3 c1, float3 c2, float opacity) {
	float facm = 1.0 - opacity;
	float3 hsv, hsv2;
	hsv = rgb_to_hsv(c2);
	hsv2 = rgb_to_hsv(c1);
	hsv.z = facm * hsv.z + opacity * hsv2.z;
	return hsv_to_rgb(hsv);
}
// #globals: scale_2 (o5177348205812)
float2 scale(float2 uv, float2 center, float2 scale) {
	uv -= center;
	uv /= scale;
	uv += center;
	return uv;
}
float4 o5174479301817_gradient_gradient_fct(float x) {
  if (x < p_o5174479301817_gradient_pos[0]) {
    return p_o5174479301817_gradient_col[0];
  } else if (x < p_o5174479301817_gradient_pos[1]) {
    return lerp(p_o5174479301817_gradient_col[0], p_o5174479301817_gradient_col[1], ((x-p_o5174479301817_gradient_pos[0])/(p_o5174479301817_gradient_pos[1]-p_o5174479301817_gradient_pos[0])));
  } else if (x < p_o5174479301817_gradient_pos[2]) {
    return lerp(p_o5174479301817_gradient_col[1], p_o5174479301817_gradient_col[2], ((x-p_o5174479301817_gradient_pos[1])/(p_o5174479301817_gradient_pos[2]-p_o5174479301817_gradient_pos[1])));
  } else if (x < p_o5174479301817_gradient_pos[3]) {
    return lerp(p_o5174479301817_gradient_col[2], p_o5174479301817_gradient_col[3], ((x-p_o5174479301817_gradient_pos[2])/(p_o5174479301817_gradient_pos[3]-p_o5174479301817_gradient_pos[2])));
  } else if (x < p_o5174479301817_gradient_pos[4]) {
    return lerp(p_o5174479301817_gradient_col[3], p_o5174479301817_gradient_col[4], ((x-p_o5174479301817_gradient_pos[3])/(p_o5174479301817_gradient_pos[4]-p_o5174479301817_gradient_pos[3])));
  }
  return p_o5174479301817_gradient_col[4];
}
float4 o5174194089142_gradient_gradient_fct(float x) {
  if (x < p_o5174194089142_gradient_pos[0]) {
    return p_o5174194089142_gradient_col[0];
  } else if (x < p_o5174194089142_gradient_pos[1]) {
    return lerp(p_o5174194089142_gradient_col[0], p_o5174194089142_gradient_col[1], ((x-p_o5174194089142_gradient_pos[0])/(p_o5174194089142_gradient_pos[1]-p_o5174194089142_gradient_pos[0])));
  } else if (x < p_o5174194089142_gradient_pos[2]) {
    return lerp(p_o5174194089142_gradient_col[1], p_o5174194089142_gradient_col[2], ((x-p_o5174194089142_gradient_pos[1])/(p_o5174194089142_gradient_pos[2]-p_o5174194089142_gradient_pos[1])));
  } else if (x < p_o5174194089142_gradient_pos[3]) {
    return lerp(p_o5174194089142_gradient_col[2], p_o5174194089142_gradient_col[3], ((x-p_o5174194089142_gradient_pos[2])/(p_o5174194089142_gradient_pos[3]-p_o5174194089142_gradient_pos[2])));
  } else if (x < p_o5174194089142_gradient_pos[4]) {
    return lerp(p_o5174194089142_gradient_col[3], p_o5174194089142_gradient_col[4], ((x-p_o5174194089142_gradient_pos[3])/(p_o5174194089142_gradient_pos[4]-p_o5174194089142_gradient_pos[3])));
  }
  return p_o5174194089142_gradient_col[4];
}
float4 o5175385271511_gradient_gradient_fct(float x) {
  if (x < p_o5175385271511_gradient_pos[0]) {
    return p_o5175385271511_gradient_col[0];
  } else if (x < p_o5175385271511_gradient_pos[1]) {
    return lerp(p_o5175385271511_gradient_col[0], p_o5175385271511_gradient_col[1], ((x-p_o5175385271511_gradient_pos[0])/(p_o5175385271511_gradient_pos[1]-p_o5175385271511_gradient_pos[0])));
  } else if (x < p_o5175385271511_gradient_pos[2]) {
    return lerp(p_o5175385271511_gradient_col[1], p_o5175385271511_gradient_col[2], ((x-p_o5175385271511_gradient_pos[1])/(p_o5175385271511_gradient_pos[2]-p_o5175385271511_gradient_pos[1])));
  } else if (x < p_o5175385271511_gradient_pos[3]) {
    return lerp(p_o5175385271511_gradient_col[2], p_o5175385271511_gradient_col[3], ((x-p_o5175385271511_gradient_pos[2])/(p_o5175385271511_gradient_pos[3]-p_o5175385271511_gradient_pos[2])));
  } else if (x < p_o5175385271511_gradient_pos[4]) {
    return lerp(p_o5175385271511_gradient_col[3], p_o5175385271511_gradient_col[4], ((x-p_o5175385271511_gradient_pos[3])/(p_o5175385271511_gradient_pos[4]-p_o5175385271511_gradient_pos[3])));
  }
  return p_o5175385271511_gradient_col[4];
}
float o5175100058835_curve_curve_fct(float x) {
if (x <= p_o5175100058835_curve_1_x) {
float dx = x - p_o5175100058835_curve_0_x;
float d = p_o5175100058835_curve_1_x - p_o5175100058835_curve_0_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o5175100058835_curve_0_y;
float yac = p_o5175100058835_curve_0_y + d*p_o5175100058835_curve_0_rs;
float ybc = p_o5175100058835_curve_1_y - d*p_o5175100058835_curve_1_ls;
float y2 = p_o5175100058835_curve_1_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
{
float dx = x - p_o5175100058835_curve_1_x;
float d = p_o5175100058835_curve_2_x - p_o5175100058835_curve_1_x;
float t = dx/d;
float omt = (1.0 - t);
float omt2 = omt * omt;
float omt3 = omt2 * omt;
float t2 = t * t;
float t3 = t2 * t;
d /= 3.0;
float y1 = p_o5175100058835_curve_1_y;
float yac = p_o5175100058835_curve_1_y + d*p_o5175100058835_curve_1_rs;
float ybc = p_o5175100058835_curve_2_y - d*p_o5175100058835_curve_2_ls;
float y2 = p_o5175100058835_curve_2_y;
return y1*omt3 + yac*omt2*t*3.0 + ybc*omt*t2*3.0 + y2*t3;
}
}
float o5161158192115_input_depth_tex(float2 uv, float _seed_variation_) {
float4 o5174848400584_0 = textureLod(texture_1, frac((uv)), 0.0);
// #code: tones_step (o5174177311921)
float3 o5174177311921_0_false = clamp((o5174848400584_0.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_0_true = tofloat3(1.0)-o5174177311921_0_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_1_rgba = tofloat4(o5174177311921_0_false, o5174848400584_0.a);
// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_1_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_1_rgba).rgb, tofloat3(1.0))/3.0));
// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_1_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_0).rgb, tofloat3(1.0))/3.0));
// #code: blend2_3 (o5174462524596)
float4 o5174462524596_0_b = o5174194089142_0_1_rgba;
float4 o5174462524596_0_l;
float o5174462524596_0_a;
o5174462524596_0_l = o5174479301817_0_1_rgba;
o5174462524596_0_a = p_o5174462524596_amount1*1.0;
o5174462524596_0_b = tofloat4(blend_multiply((uv), o5174462524596_0_l.rgb, o5174462524596_0_b.rgb, o5174462524596_0_a*o5174462524596_0_l.a), min(1.0, o5174462524596_0_b.a+o5174462524596_0_a*o5174462524596_0_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_1_rgba = o5174462524596_0_b;
float4 o5177247542553_0 = textureLod(texture_2, frac((uv)), 0.0);
// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_1_f = o5175100058835_curve_curve_fct((dot((o5177247542553_0).rgb, tofloat3(1.0))/3.0));
// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_1_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_1_f);
// #code: blend2_6 (o5175653706991)
float4 o5175653706991_0_b = o5175385271511_0_1_rgba;
float4 o5175653706991_0_l;
float o5175653706991_0_a;
o5175653706991_0_l = o5174462524596_0_1_rgba;
o5175653706991_0_a = p_o5175653706991_amount1*1.0;
o5175653706991_0_b = tofloat4(blend_lighten((uv), o5175653706991_0_l.rgb, o5175653706991_0_b.rgb, o5175653706991_0_a*o5175653706991_0_l.a), min(1.0, o5175653706991_0_b.a+o5175653706991_0_a*o5175653706991_0_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_1_rgba = o5175653706991_0_b;
// #code: blend2_7 (o5175771147486)
float4 o5175771147486_0_b = o5174177311921_0_1_rgba;
float4 o5175771147486_0_l;
float o5175771147486_0_a;
o5175771147486_0_l = o5175653706991_0_1_rgba;
o5175771147486_0_a = p_o5175771147486_amount1*1.0;
o5175771147486_0_b = tofloat4(blend_normal((uv), o5175771147486_0_l.rgb, o5175771147486_0_b.rgb, o5175771147486_0_a*o5175771147486_0_l.a), min(1.0, o5175771147486_0_b.a+o5175771147486_0_a*o5175771147486_0_l.a));
// #output0: blend2_7 (o5175771147486)
float4 o5175771147486_0_1_rgba = o5175771147486_0_b;
// #output0: invert (o5165386050587)
float4 o5165386050587_0_1_rgba = tofloat4(tofloat3(1.0)-o5175771147486_0_1_rgba.rgb, o5175771147486_0_1_rgba.a);
return (dot((o5165386050587_0_1_rgba).rgb, tofloat3(1.0))/3.0);
}
float o5165486713930_input_in(float2 uv, float _seed_variation_) {
float o5165553822792_0 = textureLod(texture_4, frac(uv), 0.0).r;
return o5165553822792_0;
}
// #instance: normal_map2/edge_detect_1 (o5165486713930)
float3 nm_o5165486713930(float2 uv, float amount, float size, float _seed_variation_) {
	float3 e = tofloat3(1.0/size, -1.0/size, 0);
	float2 rv;
	if (0 == 0) {
		rv = tofloat2(1.0, -1.0)*o5165486713930_input_in((uv+e.xy), _seed_variation_);
		rv += tofloat2(-1.0, 1.0)*o5165486713930_input_in((uv-e.xy), _seed_variation_);
		rv += tofloat2(1.0, 1.0)*o5165486713930_input_in((uv+e.xx), _seed_variation_);
		rv += tofloat2(-1.0, -1.0)*o5165486713930_input_in((uv-e.xx), _seed_variation_);
		rv += tofloat2(2.0, 0.0)*o5165486713930_input_in((uv+e.xz), _seed_variation_);
		rv += tofloat2(-2.0, 0.0)*o5165486713930_input_in((uv-e.xz), _seed_variation_);
		rv += tofloat2(0.0, 2.0)*o5165486713930_input_in((uv+e.zx), _seed_variation_);
		rv += tofloat2(0.0, -2.0)*o5165486713930_input_in((uv-e.zx), _seed_variation_);
		rv *= size*amount/128.0;
	} else if (0 == 1) {
		rv = tofloat2(3.0, -3.0)*o5165486713930_input_in((uv+e.xy), _seed_variation_);
		rv += tofloat2(-3.0, 3.0)*o5165486713930_input_in((uv-e.xy), _seed_variation_);
		rv += tofloat2(3.0, 3.0)*o5165486713930_input_in((uv+e.xx), _seed_variation_);
		rv += tofloat2(-3.0, -3.0)*o5165486713930_input_in((uv-e.xx), _seed_variation_);
		rv += tofloat2(10.0, 0.0)*o5165486713930_input_in((uv+e.xz), _seed_variation_);
		rv += tofloat2(-10.0, 0.0)*o5165486713930_input_in((uv-e.xz), _seed_variation_);
		rv += tofloat2(0.0, 10.0)*o5165486713930_input_in((uv+e.zx), _seed_variation_);
		rv += tofloat2(0.0, -10.0)*o5165486713930_input_in((uv-e.zx), _seed_variation_);
		rv *= size*amount/512.0;
	} else if (0 == 2) {
		rv = tofloat2(2.0, 0.0)*o5165486713930_input_in((uv+e.xz), _seed_variation_);
		rv += tofloat2(-2.0, 0.0)*o5165486713930_input_in((uv-e.xz), _seed_variation_);
		rv += tofloat2(0.0, 2.0)*o5165486713930_input_in((uv+e.zx), _seed_variation_);
		rv += tofloat2(0.0, -2.0)*o5165486713930_input_in((uv-e.zx), _seed_variation_);
		rv *= size*amount/64.0;
	} else {
		rv = tofloat2(1.0, 0.0)*o5165486713930_input_in((uv+e.xz), _seed_variation_);
		rv += tofloat2(0.0, 1.0)*o5165486713930_input_in((uv+e.zx), _seed_variation_);
		rv += tofloat2(-1.0, -1.0)*o5165486713930_input_in((uv), _seed_variation_);
		rv *= size*amount/20.0;
	}
	return tofloat3(0.5)+0.5*normalize(tofloat3(rv, -1.0));
}
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
	  		float _seed_variation_ = 0.0;
			float2 uv = IN.uv_MainTex;

// #output0: normal_map2/edge_detect_1 (o5165486713930)
float3 o5165486713930_0_1_rgb = nm_o5165486713930((uv), p_o5165486713930_amount, 1024.0, _seed_variation_);
float4 o5174848400584_0 = textureLod(texture_1, frac((uv)), 0.0);

// #code: tones_step (o5174177311921)
float3 o5174177311921_0_false = clamp((o5174848400584_0.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_0_true = tofloat3(1.0)-o5174177311921_0_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_1_rgba = tofloat4(o5174177311921_0_false, o5174848400584_0.a);

// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_1_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_1_rgba).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_1_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_0).rgb, tofloat3(1.0))/3.0));

// #code: blend2_3 (o5174462524596)
float4 o5174462524596_0_b = o5174194089142_0_1_rgba;
float4 o5174462524596_0_l;
float o5174462524596_0_a;

o5174462524596_0_l = o5174479301817_0_1_rgba;
o5174462524596_0_a = p_o5174462524596_amount1*1.0;
o5174462524596_0_b = tofloat4(blend_multiply((uv), o5174462524596_0_l.rgb, o5174462524596_0_b.rgb, o5174462524596_0_a*o5174462524596_0_l.a), min(1.0, o5174462524596_0_b.a+o5174462524596_0_a*o5174462524596_0_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_1_rgba = o5174462524596_0_b;
float4 o5177247542553_0 = textureLod(texture_2, frac((uv)), 0.0);

// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_1_f = o5175100058835_curve_curve_fct((dot((o5177247542553_0).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_1_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_1_f);

// #code: blend2_6 (o5175653706991)
float4 o5175653706991_0_b = o5175385271511_0_1_rgba;
float4 o5175653706991_0_l;
float o5175653706991_0_a;

o5175653706991_0_l = o5174462524596_0_1_rgba;
o5175653706991_0_a = p_o5175653706991_amount1*1.0;
o5175653706991_0_b = tofloat4(blend_lighten((uv), o5175653706991_0_l.rgb, o5175653706991_0_b.rgb, o5175653706991_0_a*o5175653706991_0_l.a), min(1.0, o5175653706991_0_b.a+o5175653706991_0_a*o5175653706991_0_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_1_rgba = o5175653706991_0_b;

// #code: blend2_7 (o5175771147486)
float4 o5175771147486_0_b = o5174177311921_0_1_rgba;
float4 o5175771147486_0_l;
float o5175771147486_0_a;

o5175771147486_0_l = o5175653706991_0_1_rgba;
o5175771147486_0_a = p_o5175771147486_amount1*1.0;
o5175771147486_0_b = tofloat4(blend_normal((uv), o5175771147486_0_l.rgb, o5175771147486_0_b.rgb, o5175771147486_0_a*o5175771147486_0_l.a), min(1.0, o5175771147486_0_b.a+o5175771147486_0_a*o5175771147486_0_l.a));
// #output0: blend2_7 (o5175771147486)
float4 o5175771147486_0_1_rgba = o5175771147486_0_b;

// #output0: invert (o5165386050587)
float4 o5165386050587_0_1_rgba = tofloat4(tofloat3(1.0)-o5175771147486_0_1_rgba.rgb, o5175771147486_0_1_rgba.a);
float4 o5174848400584_1 = textureLod(texture_1, frac((scale((2.0*(uv)-tofloat2(1.0, 1.0)), tofloat2(0.5+p_o5177348205812_cx, 0.5+p_o5177348205812_cy), tofloat2(p_o5177348205812_scale_x, p_o5177348205812_scale_y)))), 0.0);

// #code: tones_step (o5174177311921)
float3 o5174177311921_2_false = clamp((o5174848400584_1.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_2_true = tofloat3(1.0)-o5174177311921_2_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_3_rgba = tofloat4(o5174177311921_2_false, o5174848400584_1.a);

// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_3_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_3_rgba).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_3_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_1).rgb, tofloat3(1.0))/3.0));

// #code: blend2_3 (o5174462524596)
float4 o5174462524596_2_b = o5174194089142_0_3_rgba;
float4 o5174462524596_2_l;
float o5174462524596_2_a;

o5174462524596_2_l = o5174479301817_0_3_rgba;
o5174462524596_2_a = p_o5174462524596_amount1*1.0;
o5174462524596_2_b = tofloat4(blend_multiply((scale((2.0*(uv)-tofloat2(1.0, 1.0)), tofloat2(0.5+p_o5177348205812_cx, 0.5+p_o5177348205812_cy), tofloat2(p_o5177348205812_scale_x, p_o5177348205812_scale_y))), o5174462524596_2_l.rgb, o5174462524596_2_b.rgb, o5174462524596_2_a*o5174462524596_2_l.a), min(1.0, o5174462524596_2_b.a+o5174462524596_2_a*o5174462524596_2_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_3_rgba = o5174462524596_2_b;
float4 o5177247542553_1 = textureLod(texture_2, frac((scale((2.0*(uv)-tofloat2(1.0, 1.0)), tofloat2(0.5+p_o5177348205812_cx, 0.5+p_o5177348205812_cy), tofloat2(p_o5177348205812_scale_x, p_o5177348205812_scale_y)))), 0.0);

// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_3_f = o5175100058835_curve_curve_fct((dot((o5177247542553_1).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_3_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_3_f);

// #code: blend2_6 (o5175653706991)
float4 o5175653706991_2_b = o5175385271511_0_3_rgba;
float4 o5175653706991_2_l;
float o5175653706991_2_a;

o5175653706991_2_l = o5174462524596_0_3_rgba;
o5175653706991_2_a = p_o5175653706991_amount1*1.0;
o5175653706991_2_b = tofloat4(blend_lighten((scale((2.0*(uv)-tofloat2(1.0, 1.0)), tofloat2(0.5+p_o5177348205812_cx, 0.5+p_o5177348205812_cy), tofloat2(p_o5177348205812_scale_x, p_o5177348205812_scale_y))), o5175653706991_2_l.rgb, o5175653706991_2_b.rgb, o5175653706991_2_a*o5175653706991_2_l.a), min(1.0, o5175653706991_2_b.a+o5175653706991_2_a*o5175653706991_2_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_3_rgba = o5175653706991_2_b;

// #output0: scale_2 (o5177348205812)
float4 o5177348205812_0_1_rgba = o5175653706991_0_3_rgba;
float4 o5174848400584_2 = textureLod(texture_1, frac((scale((2.0*(uv)-tofloat2(0.0, 1.0)), tofloat2(0.5+p_o5177364983029_cx, 0.5+p_o5177364983029_cy), tofloat2(p_o5177364983029_scale_x, p_o5177364983029_scale_y)))), 0.0);

// #code: tones_step (o5174177311921)
float3 o5174177311921_4_false = clamp((o5174848400584_2.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_4_true = tofloat3(1.0)-o5174177311921_4_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_5_rgba = tofloat4(o5174177311921_4_false, o5174848400584_2.a);

// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_5_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_5_rgba).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_5_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_2).rgb, tofloat3(1.0))/3.0));

// #code: blend2_3 (o5174462524596)
float4 o5174462524596_4_b = o5174194089142_0_5_rgba;
float4 o5174462524596_4_l;
float o5174462524596_4_a;

o5174462524596_4_l = o5174479301817_0_5_rgba;
o5174462524596_4_a = p_o5174462524596_amount1*1.0;
o5174462524596_4_b = tofloat4(blend_multiply((scale((2.0*(uv)-tofloat2(0.0, 1.0)), tofloat2(0.5+p_o5177364983029_cx, 0.5+p_o5177364983029_cy), tofloat2(p_o5177364983029_scale_x, p_o5177364983029_scale_y))), o5174462524596_4_l.rgb, o5174462524596_4_b.rgb, o5174462524596_4_a*o5174462524596_4_l.a), min(1.0, o5174462524596_4_b.a+o5174462524596_4_a*o5174462524596_4_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_5_rgba = o5174462524596_4_b;
float4 o5177247542553_2 = textureLod(texture_2, frac((scale((2.0*(uv)-tofloat2(0.0, 1.0)), tofloat2(0.5+p_o5177364983029_cx, 0.5+p_o5177364983029_cy), tofloat2(p_o5177364983029_scale_x, p_o5177364983029_scale_y)))), 0.0);

// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_5_f = o5175100058835_curve_curve_fct((dot((o5177247542553_2).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_5_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_5_f);

// #code: blend2_6 (o5175653706991)
float4 o5175653706991_4_b = o5175385271511_0_5_rgba;
float4 o5175653706991_4_l;
float o5175653706991_4_a;

o5175653706991_4_l = o5174462524596_0_5_rgba;
o5175653706991_4_a = p_o5175653706991_amount1*1.0;
o5175653706991_4_b = tofloat4(blend_lighten((scale((2.0*(uv)-tofloat2(0.0, 1.0)), tofloat2(0.5+p_o5177364983029_cx, 0.5+p_o5177364983029_cy), tofloat2(p_o5177364983029_scale_x, p_o5177364983029_scale_y))), o5175653706991_4_l.rgb, o5175653706991_4_b.rgb, o5175653706991_4_a*o5175653706991_4_l.a), min(1.0, o5175653706991_4_b.a+o5175653706991_4_a*o5175653706991_4_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_5_rgba = o5175653706991_4_b;

// #output0: scale_3 (o5177364983029)
float4 o5177364983029_0_1_rgba = o5175653706991_0_5_rgba;
float4 o5174848400584_3 = textureLod(texture_1, frac((scale((2.0*(uv)-tofloat2(1.0, 0.0)), tofloat2(0.5+p_o5177314651378_cx, 0.5+p_o5177314651378_cy), tofloat2(p_o5177314651378_scale_x, p_o5177314651378_scale_y)))), 0.0);

// #code: tones_step (o5174177311921)
float3 o5174177311921_6_false = clamp((o5174848400584_3.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_6_true = tofloat3(1.0)-o5174177311921_6_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_7_rgba = tofloat4(o5174177311921_6_false, o5174848400584_3.a);

// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_7_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_7_rgba).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_7_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_3).rgb, tofloat3(1.0))/3.0));

// #code: blend2_3 (o5174462524596)
float4 o5174462524596_6_b = o5174194089142_0_7_rgba;
float4 o5174462524596_6_l;
float o5174462524596_6_a;

o5174462524596_6_l = o5174479301817_0_7_rgba;
o5174462524596_6_a = p_o5174462524596_amount1*1.0;
o5174462524596_6_b = tofloat4(blend_multiply((scale((2.0*(uv)-tofloat2(1.0, 0.0)), tofloat2(0.5+p_o5177314651378_cx, 0.5+p_o5177314651378_cy), tofloat2(p_o5177314651378_scale_x, p_o5177314651378_scale_y))), o5174462524596_6_l.rgb, o5174462524596_6_b.rgb, o5174462524596_6_a*o5174462524596_6_l.a), min(1.0, o5174462524596_6_b.a+o5174462524596_6_a*o5174462524596_6_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_7_rgba = o5174462524596_6_b;
float4 o5177247542553_3 = textureLod(texture_2, frac((scale((2.0*(uv)-tofloat2(1.0, 0.0)), tofloat2(0.5+p_o5177314651378_cx, 0.5+p_o5177314651378_cy), tofloat2(p_o5177314651378_scale_x, p_o5177314651378_scale_y)))), 0.0);

// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_7_f = o5175100058835_curve_curve_fct((dot((o5177247542553_3).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_7_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_7_f);

// #code: blend2_6 (o5175653706991)
float4 o5175653706991_6_b = o5175385271511_0_7_rgba;
float4 o5175653706991_6_l;
float o5175653706991_6_a;

o5175653706991_6_l = o5174462524596_0_7_rgba;
o5175653706991_6_a = p_o5175653706991_amount1*1.0;
o5175653706991_6_b = tofloat4(blend_lighten((scale((2.0*(uv)-tofloat2(1.0, 0.0)), tofloat2(0.5+p_o5177314651378_cx, 0.5+p_o5177314651378_cy), tofloat2(p_o5177314651378_scale_x, p_o5177314651378_scale_y))), o5175653706991_6_l.rgb, o5175653706991_6_b.rgb, o5175653706991_6_a*o5175653706991_6_l.a), min(1.0, o5175653706991_6_b.a+o5175653706991_6_a*o5175653706991_6_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_7_rgba = o5175653706991_6_b;

// #output0: scale (o5177314651378)
float4 o5177314651378_0_1_rgba = o5175653706991_0_7_rgba;
float4 o5174848400584_4 = textureLod(texture_1, frac((2.0*(uv))), 0.0);

// #code: tones_step (o5174177311921)
float3 o5174177311921_8_false = clamp((o5174848400584_4.rgb-tofloat3(p_o5174177311921_value))/max(0.0001, p_o5174177311921_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o5174177311921_8_true = tofloat3(1.0)-o5174177311921_8_false;
// #output0: tones_step (o5174177311921)
float4 o5174177311921_0_9_rgba = tofloat4(o5174177311921_8_false, o5174848400584_4.a);

// #output0: colorize_3 (o5174479301817)
float4 o5174479301817_0_9_rgba = o5174479301817_gradient_gradient_fct((dot((o5174177311921_0_9_rgba).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_2 (o5174194089142)
float4 o5174194089142_0_9_rgba = o5174194089142_gradient_gradient_fct((dot((o5174848400584_4).rgb, tofloat3(1.0))/3.0));

// #code: blend2_3 (o5174462524596)
float4 o5174462524596_8_b = o5174194089142_0_9_rgba;
float4 o5174462524596_8_l;
float o5174462524596_8_a;

o5174462524596_8_l = o5174479301817_0_9_rgba;
o5174462524596_8_a = p_o5174462524596_amount1*1.0;
o5174462524596_8_b = tofloat4(blend_multiply((2.0*(uv)), o5174462524596_8_l.rgb, o5174462524596_8_b.rgb, o5174462524596_8_a*o5174462524596_8_l.a), min(1.0, o5174462524596_8_b.a+o5174462524596_8_a*o5174462524596_8_l.a));
// #output0: blend2_3 (o5174462524596)
float4 o5174462524596_0_9_rgba = o5174462524596_8_b;
float4 o5177247542553_4 = textureLod(texture_2, frac((2.0*(uv))), 0.0);

// #output0: tonality_2 (o5175100058835)
float o5175100058835_0_9_f = o5175100058835_curve_curve_fct((dot((o5177247542553_4).rgb, tofloat3(1.0))/3.0));

// #output0: colorize_4 (o5175385271511)
float4 o5175385271511_0_9_rgba = o5175385271511_gradient_gradient_fct(o5175100058835_0_9_f);

// #code: blend2_6 (o5175653706991)
float4 o5175653706991_8_b = o5175385271511_0_9_rgba;
float4 o5175653706991_8_l;
float o5175653706991_8_a;

o5175653706991_8_l = o5174462524596_0_9_rgba;
o5175653706991_8_a = p_o5175653706991_amount1*1.0;
o5175653706991_8_b = tofloat4(blend_lighten((2.0*(uv)), o5175653706991_8_l.rgb, o5175653706991_8_b.rgb, o5175653706991_8_a*o5175653706991_8_l.a), min(1.0, o5175653706991_8_b.a+o5175653706991_8_a*o5175653706991_8_l.a));
// #output0: blend2_6 (o5175653706991)
float4 o5175653706991_0_9_rgba = o5175653706991_8_b;

// #output0: tile2x2_2 (o5177331428595)
float4 o5177331428595_0_1_rgba = ((uv).y < 0.5) ? (((uv).x < 0.5) ? (o5175653706991_0_9_rgba) : (o5177314651378_0_1_rgba)) : (((uv).x < 0.5) ? (o5177364983029_0_1_rgba) : (o5177348205812_0_1_rgba));

// #output0: uniform (o5177415314709)
float4 o5177415314709_0_1_rgba = p_o5177415314709_color;

// #code: blend2_8 (o5177398537494)
float4 o5177398537494_0_b = o5177415314709_0_1_rgba;
float4 o5177398537494_0_l;
float o5177398537494_0_a;

o5177398537494_0_l = o5177331428595_0_1_rgba;
o5177398537494_0_a = p_o5177398537494_amount1*1.0;
o5177398537494_0_b = tofloat4(blend_additive((uv), o5177398537494_0_l.rgb, o5177398537494_0_b.rgb, o5177398537494_0_a*o5177398537494_0_l.a), min(1.0, o5177398537494_0_b.a+o5177398537494_0_a*o5177398537494_0_l.a));

o5177398537494_0_l = o5165386050587_0_1_rgba;
o5177398537494_0_a = p_o5177398537494_amount2*1.0;
o5177398537494_0_b = tofloat4(blend_addsub((uv), o5177398537494_0_l.rgb, o5177398537494_0_b.rgb, o5177398537494_0_a*o5177398537494_0_l.a), min(1.0, o5177398537494_0_b.a+o5177398537494_0_a*o5177398537494_0_l.a));
// #output0: blend2_8 (o5177398537494)
float4 o5177398537494_0_1_rgba = o5177398537494_0_b;

			o.Albedo = ((o5177398537494_0_1_rgba).rgb).rgb*p_o5161158192115_albedo_color.rgb;
			o.Metallic = 1.0*p_o5161158192115_metallic;
			o.Smoothness = 1.0-(dot((o5175653706991_0_1_rgba).rgb, tofloat3(1.0))/3.0)*p_o5161158192115_roughness;
			o.Alpha = 1.0;
			o.Normal = o5165486713930_0_1_rgb*tofloat3(-1.0, 1.0, -1.0)+tofloat3(1.0, 0.0, 1.0);
		}
		ENDCG
	}
	FallBack "Diffuse"
}



