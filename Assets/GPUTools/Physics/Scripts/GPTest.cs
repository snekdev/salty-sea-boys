using UnityEngine;

#pragma warning disable 649

namespace GPUTools.Physics.Scripts
{
    public class GPTest : MonoBehaviour
    {
        /*[SerializeField] private ComputeShader shader;
        [SerializeField] private Transform target;
       private GPWorld world;
        private GPWorldBuilder worldData;

        private void Start()
        {
            worldData = new GPWorldBuilder();
            var totalX = 20;
            var totalY = 20;

            for (int y = 0; y < totalY; y++)
            {
                for (int x = 0; x < totalX; x++)
                {
                    var body = new GPBody(new Vector3(5 + x*0.5f, -y*0.5f));
                    worldData.AddSphere(body, 0.1f, 0);
                }
  
            }

            ExecuteTimer.Start();


            for (int y = 0; y < totalY; y++)
            {
                for (int x = 0; x < totalX; x++)
                {
                    if (x > 0)
                    {
                        var jx = x + y*totalY;
                        DJoint(jx - 1, jx);
                    }

                    if (y > 0)
                    {
                        var jy1 = x + y*totalY;
                        var jy2 = x + (y - 1)*totalY;
                        DJoint(jy1, jy2);
                    }

                    if (y > 0 && x > 0)
                    {
                        var jy1 = x + y*totalY;
                        var jy2 = x - 1 + (y - 1)*totalY;
                        DJoint(jy1, jy2);
                    }

                    if (y > 0 && x > 0)
                    {
                        var jy1 = x - 1 + y*totalY;
                        var jy2 = x + (y - 1)*totalY;
                        DJoint(jy1, jy2);
                    }
                }
            }

            ExecuteTimer.Log();

            for (var i = 0; i < totalX; i++)
            {
                var body = worldData.Data.Bodies[i];
                worldData.AddPointJoint(body, body.Position, transform.localToWorldMatrix, 1);
            }

            var sphere = new GPBody(new Vector3(10,-10,0.2f));
            worldData.AddKinematicsSphere(sphere, 4, 1);

            world = new GPWorld(shader, worldData.Data);
        }

        private void DJoint(int i1, int i2)
        {
            var body1 = worldData.Data.Bodies[i1];
            var body2 = worldData.Data.Bodies[i2];

            worldData.AddDitanceJoint(i1, i2, Vector3.Distance(body1.Position, body2.Position));
        }

        private void Update()
        {
            world.Update(10, true);

            worldData.Data.KinematicsBodies[0].Position = target.position;
            worldData.Data.KinematicsBodies[0].LastPosition = target.position;
        }

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying)
                return;
           
            GPDebugDraw.Draw(worldData.Data);
        }

        private void OnDestroy()
        {
            world.Dispose();
        }*/
    }
}
