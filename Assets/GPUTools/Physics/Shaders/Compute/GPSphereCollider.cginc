struct SphereCollider
{
	int bodyId;
	float radius;
};

float3 SphereSphereColisionSolveImpl(float3 position1, float3 position2, SphereCollider collider1, SphereCollider collider2)
{
	float3 relPosition = position1 - position2;
	float sumRadius = collider1.radius + collider2.radius;
	float distance = length(relPosition);

	if (distance > sumRadius)
		return float3(0, 0, 0);

	float penetration = sumRadius - distance;
	float3 normal = normalize(relPosition);

	return normal*penetration*0.5f;
}

void SphereSphereColisionSolve(
	SphereCollider collider1, 
	RWStructuredBuffer<Body> bodies, 
	RWStructuredBuffer<Body> kinematicBodies,
	RWStructuredBuffer<SphereCollider> kinematicSphereColliders, float dt)
{
	Body body1 = bodies[collider1.bodyId];

	for (uint i = 0; i < kinematicSphereColliders.Length; i++)
	{
		SphereCollider collider2 = kinematicSphereColliders[i];

		Body body2 = kinematicBodies[collider2.bodyId];

		float3 correction = SphereSphereColisionSolveImpl(body1.position, body2.position, collider1, collider2);
		body1.position += correction*dt;
	}

	bodies[collider1.bodyId] = body1;
}
