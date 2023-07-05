#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED

float dot2( in float2 v ) { return dot(v,v); }

void SDFCircle_float(float r, float2 p, float2 uv,
    out float Out){
    
    Out = length(uv + p) - r;
}

void SDFRect_float(float2 b, float2 p, float2 uv, 
    out float Out){
    
    float2 d = abs(uv+ p) - b;
    Out = length(max(d, 0)) + min(max(d.x, d.y), 0);
}

void SDFLine_float(float2 a, float2 b, float2 uv, 
    out float Out){
    
    float2 pa = uv-a;
    float2 ba = b-a;
    float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
    Out = length(pa - ba*h);
}

void SDFTriangle_float(float2 a, float2 b, float2 c, float2 uv,
    out float Out){
    
    float2 e0 = b - a;
    float2 e1 = c - b;
    float2 e2 = a - c;
    
    float2 v0 = uv - a;
    float2 v1 = uv - b;
    float2 v2 = uv - c;
    
    float2 pq0 = v0 - e0 * clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    float2 pq1 = v1 - e1 * clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    float2 pq2 = v2 - e2 * clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );
    
    float s = sign( e0.x*e2.y - e0.y*e2.x );
    float2 d = min(min(float2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                       float2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                       float2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));
    Out = -sqrt(d.x) * sign(d.y);
}

void SDFBezier_float(float2 A, float2 B, float2 C, float2 UV,
    out float Out){
    
    float2 pos = UV;
    float2 a = B - A;
    float2 b = A - 2.0*B + C;
    float2 c = a * 2.0;
    float2 d = A - pos;
    float kk = 1.0/dot(b,b);
    float kx = kk * dot(a,b);
    float ky = kk * (2.0*dot(a,a)+dot(d,b)) / 3.0;
    float kz = kk * dot(d,a);      
    float res = 0.0;
    float p = ky - kx*kx;
    float p3 = p*p*p;
    float q = kx*(2.0*kx*kx-3.0*ky) + kz;
    float h = q*q + 4.0*p3;
    if( h >= 0.0) 
    { 
        h = sqrt(h);
        float2 x = (float2(h,-h)-q)/2.0;
        float2 uv = sign(x)*pow(abs(x), float2(1.0/3.0,1.0/3.0));
        float t = clamp( uv.x+uv.y-kx, 0.0, 1.0 );
        res = dot2(d + (c + b*t)*t);
    }
    else
    {
        float z = sqrt(-p);
        float v = acos( q/(p*z*2.0) ) / 3.0;
        float m = cos(v);
        float n = sin(v)*1.732050808;
        float3  t = clamp(float3(m+m,-n-m,n-m)*z-kx,0.0,1.0);
        
        res = min( dot2(d+(c+b*t.x)*t.x),
                   dot2(d+(c+b*t.y)*t.y) );
    }
    Out = sqrt(res);
}

///////////////////////

void smin_float(float a, float b, float k,
    out float Out){
    
    float h = max( k-abs(a-b), 0.0 )/k;
    Out =  min( a, b ) - h*h*k*(1.0/4.0);
}
    
#endif