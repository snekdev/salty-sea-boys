float3 PointPointColisionSolveImpl(float3 position1, float3 position2, float radius)
{
	float3 relPosition = position1 - position2;
	float sumRadius = radius * 2;
	float distance = length(relPosition);

	if (distance > sumRadius)
		return float3(0, 0, 0);

	float penetration = sumRadius - distance;
	float3 normal = normalize(relPosition);

	return normal*penetration*0.5f;
}

void BodyMeshColisionSolve(
	SphereCollider collider1, 
	RWStructuredBuffer<Body> bodies, 
	RWStructuredBuffer<float3> bodyMeshVertexBuffer, float dt)
{
	Body body1 = bodies[collider1.bodyId];

	for (uint i = 0; i < bodyMeshVertexBuffer.Length; i++)
	{
		float3 position2 = bodyMeshVertexBuffer[i];

		float3 correction = PointPointColisionSolveImpl(body1.position, position2, 0.02f);
		body1.position += correction*dt;
	}
	bodies[collider1.bodyId] = body1;
}

void BodyMeshBroadPhase(
	uint id,
	Body body,
	RWStructuredBuffer<int> indices,
	RWStructuredBuffer<float3> bodyMeshVertexBuffer)
{
	float distanse = 0.125f;
	
	uint j = 0;
	for (uint i = 0; i < bodyMeshVertexBuffer.Length; i++)
	{
		float3 position = bodyMeshVertexBuffer[i];

		float3 diff = body.position - position;
		if (dot(diff, diff) < distanse*distanse)
		{
			indices[id*50 + j] = i;
			j++;
		}

		if (j >= 50)
			break;
	}
}

void BodyMeshNarrowPhase(
	SphereCollider collider,
	RWStructuredBuffer<Body> bodies,
	RWStructuredBuffer<int> indices,
	RWStructuredBuffer<float3> bodyMeshVertexBuffer, 
	float dt)
{
	Body body = bodies[collider.bodyId];

	for (uint i = 0; i < 50; i++)
	{
		int index = indices[collider.bodyId*50 + i];
		if (index == 0)
			continue;

		float3 position = bodyMeshVertexBuffer[index];
	
		float3 correction = PointPointColisionSolveImpl(body.position, position, collider.radius);
		body.position += correction*dt;
	}

	bodies[collider.bodyId] = body;
}
