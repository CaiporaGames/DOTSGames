using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(PickupSystem))]
public class DeleteEntitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        Entities
            .WithAll<DeleteTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
        {
            GameManager._intance.IncreaseScore();
            commandBuffer.DestroyEntity(entity);
        }).Run();
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }   
}
