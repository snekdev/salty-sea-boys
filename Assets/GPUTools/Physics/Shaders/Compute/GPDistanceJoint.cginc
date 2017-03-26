struct DistanceJoint
{
	int body1Id;
	int body2Id;
	float distance;
	float elasticity;
};

float3 DistanceJointSolveImpl(float3 position1, float3 position2, float distance)
{
	float3 relPosition = position1 - position2;
	float actualDistance = length(relPosition);

	float penetration = (distance - actualDistance) / actualDistance;
	return relPosition*penetration;
}

void DistanceJointsSolve(DistanceJoint joint, RWStructuredBuffer<Body> bodies, float dt)
{
	Body body1 = bodies[joint.body1Id];
	Body body2 = bodies[joint.body2Id];

	float3 correction = DistanceJointSolveImpl(body1.position, body2.position, joint.distance)*joint.elasticity*dt;

	body1.position += correction;
	body2.position -= correction;

	bodies[joint.body1Id] = body1;
	bodies[joint.body2Id] = body2;
}
