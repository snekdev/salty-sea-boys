struct Body
{
	float3 position;
	float3 lastPosition;
};

Body IntegrateImpl(Body body, float3 gravity, float drag, float dt)
{
	float3 difference = body.position - body.lastPosition;
	float3 velocity = difference*drag + gravity*dt;
	float3 nextPosition = body.position + velocity;

	body.lastPosition = body.position;
	body.position = nextPosition;

	return body;
}

void Integrate(RWStructuredBuffer<Body> bodies, uint3 id, float3 gravity, float drag, float dt)
{
	Body body = bodies[id.x];
	body = IntegrateImpl(body, gravity, drag, dt);
	bodies[id.x] = body;
}

void Reset(RWStructuredBuffer<Body> bodies, uint3 id)
{
	Body body = bodies[id.x];
	body.lastPosition = body.position;
	bodies[id.x] = body;
}

void Wind(RWStructuredBuffer<Body> bodies, uint3 id, float3 wind, float dt)
{
	Body body = bodies[id.x];
	body.position += wind*dt;
	bodies[id.x] = body;
}


