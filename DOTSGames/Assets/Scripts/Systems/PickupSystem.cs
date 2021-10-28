using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

[AlwaysSynchronizeSystem]

public class PickupSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem bufferSystem;
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetExistingSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob triggerJob = new TriggerJob
        {
            speedEntities = GetComponentDataFromEntity<SpeedData>(), //return an array with all the entities that has the SpeedData.
            entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(),
            commandBuffer = bufferSystem.CreateCommandBuffer()
        };

       return triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }


    struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<SpeedData> speedEntities;
        [ReadOnly] public ComponentDataFromEntity<DeleteTag> entitiesToDelete;
        public EntityCommandBuffer commandBuffer;//Allows to put the deleteTag on the entities

        public void Execute(TriggerEvent triggerEvent)
        {
            TestEntityTrigger(triggerEvent.EntityA, triggerEvent.EntityB);
            TestEntityTrigger(triggerEvent.EntityB, triggerEvent.EntityA);
        }      

        void TestEntityTrigger(Entity entityA, Entity entityB)
        {
            if (speedEntities.HasComponent(entityA))//if the entityA has the speedEntites then do...
            {
                if (entitiesToDelete.HasComponent(entityB))//if the entityB has the entitiesToDelete then return
                {
                    return;
                }
                commandBuffer.AddComponent(entityB, new DeleteTag());
            }
        }
    }
}
