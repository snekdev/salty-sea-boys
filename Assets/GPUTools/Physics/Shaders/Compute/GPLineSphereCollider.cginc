struct LineSphereCollider
{
	int bodyId;
	float3 a;
	float3 b;
	float radiusA;
	float radiusB;
};

float3 SphereLineSphereColisionSolveImpl(float3 spherePosition, SphereCollider sphereCollider, LineSphereCollider lineSphereCollider)
{
	float3 v = lineSphereCollider.a - lineSphereCollider.b;
	float3 w = lineSphereCollider.a - spherePosition;
	float projWv = saturate(dot(v, w) / dot(v, v));
	
	float3 p = lerp(lineSphereCollider.a, lineSphereCollider.b, projWv);
	float rP = lerp(lineSphereCollider.radiusA, lineSphereCollider.radiusB, projWv);
	
	float3 relPosition = spherePosition - p;
	float sumRadius = sphereCollider.radius + rP;

	if(dot(relPosition, relPosition) > sumRadius*sumRadius)
		return float3(0, 0, 0);

	float penetration = sumRadius - length(relPosition);
	float3 normal = normalize(relPosition);

	return normal*penetration*0.5;
}

void SphereLineSphereColisionSolve(
	SphereCollider sphereCollider,
	RWStructuredBuffer<Body> bodies, 
	RWStructuredBuffer<Body> kinematicBodies,
	RWStructuredBuffer<LineSphereCollider> kinematicLineSphereColliders, float dt)
{
	Body body1 = bodies[sphereCollider.bodyId];

	for (uint i = 0; i < kinematicLineSphereColliders.Length; i++)
	{
		LineSphereCollider lineSphereCollider = kinematicLineSphereColliders[i];
		Body body2 = kinematicBodies[lineSphereCollider.bodyId];
		
		lineSphereCollider.a += body2.position;
		lineSphereCollider.b += body2.position;

		float3 correction = SphereLineSphereColisionSolveImpl(body1.position, sphereCollider, lineSphereCollider)*dt*0.5;
		body1.position += correction;
	}

	bodies[sphereCollider.bodyId] = body1;
}
