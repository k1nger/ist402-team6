sampler meterSample : register(s0);
float meterValue = 1;
float4 Color;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{    
	Color /= 255;//Divide by 255 to make the Color values between 0 and 1 for the shader
	float3 tex = tex2D(meterSample, texCoord);
	float4 meter = ceil((meterValue - tex.r));
	meter.a = min(tex.g, meter.a);
	meter.rgb = Color;
	return meter;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.        
        PixelShader = compile ps_2_b PixelShaderFunction();
    }
}
