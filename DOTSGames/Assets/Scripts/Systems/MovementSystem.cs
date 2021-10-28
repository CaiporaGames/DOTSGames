using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
//JobComponentSystem
[AlwaysSynchronizeSystem]//When running code on main thread we sync properly with this.
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float2 currentInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entities.ForEach((ref PhysicsVelocity velocity, in SpeedData speedData) =>
        {
            float2 newVelocity = velocity.Linear.xz;

            newVelocity += currentInput * speedData.speed * deltaTime;

            velocity.Linear.xz = newVelocity;
        }).Run();
    }
}
