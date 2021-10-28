using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow _instance;

    public Entity ballEntity;
    public float3 offset;

    EntityManager manager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (ballEntity == null)
        {
            return;
        }

        Translation ballPosition = manager.GetComponentData<Translation>(ballEntity);
        transform.position = ballPosition.Value + offset;
    }
}
