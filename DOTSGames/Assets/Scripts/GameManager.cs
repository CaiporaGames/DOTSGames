using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AlwaysSynchronizeSystem]

public class GameManager : MonoBehaviour
{
    public static GameManager _intance;

    public GameObject ballPrefab;
    public TextMeshProUGUI scoreText;

    int currentScore;
    Entity ballEntityPrefab;
    EntityManager manager;//We use it to instantiate the prefab on the world
    BlobAssetStore blobAssetStore;

    private void Awake()
    {
        if (_intance != this && _intance != null)
        {
            Destroy(gameObject);
            return;
        }else
        {
            _intance = this;
        }

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab,settings);//Transform our prefab in an entity 
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    private void Start()
    {
        currentScore = 0;
        DisplayScore();
        SpawnBall();
    }

    public void IncreaseScore()
    {
        currentScore++;
        DisplayScore();
    }

    void DisplayScore()
    {
        scoreText.text = "Score: " + currentScore;
    }

    void SpawnBall()
    {
        Entity newBallEntity = manager.Instantiate(ballEntityPrefab);

        Translation ballTranslation = new Translation
        {
            Value = new float3(0f, 0.5f, 0f)
        };

        manager.AddComponentData(newBallEntity, ballTranslation);
        CameraFollow._instance.ballEntity = newBallEntity;
    }
}
