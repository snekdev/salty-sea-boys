struct PointJoint
{
	int bodyId;
	int matrixId;
	float3 position;
	float elasticity;
};

float3 PointJointSolveImpl(float3 position1, float3 position2, float elasticity)
{
	float3 correction = position1 - position2;
	return correction*elasticity;
}

void PointJointSolve(PointJoint joint, RWStructuredBuffer<Body> bodies, RWStructuredBuffer<float4x4> matrices, float dt)
{
	Body body = bodies[joint.bodyId];
	float4x4 m4 = matrices[joint.matrixId];
	float3 guidePosition = mul(m4, float4(joint.position, 1.0)).xyz;
	body.position -= PointJointSolveImpl(body.position, guidePosition, joint.elasticity)*dt;

	bodies[joint.bodyId] = body;
}
