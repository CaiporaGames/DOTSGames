using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation rotation, in RotationSpeedData rotationSpeedData) =>
        {
            rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(rotationSpeedData.speed * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(rotationSpeedData.speed * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(rotationSpeedData.speed * deltaTime)));
        }).Run();
    }   
}
