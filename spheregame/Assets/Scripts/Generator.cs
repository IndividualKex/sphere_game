using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour
{
    public static Generator Instance { get; private set; }

    public static Transform CurrentBlock {
        get {
            if(Instance.blockQueue.Count > 0)
                return Instance.blockQueue[Instance.blockQueue.Count - 1].transform;
            return null;
        }
    }

    public GameObject platformPrefab;
    public Enemy enemyPrefab;
    public EnemyData[] enemies;
    public GeneratorTrigger generatorTriggerPrefab;
    public int bufferSize = 3;
    public float blockHeight = 20f;

    List<GameObject> blockQueue = new List<GameObject>();
    int idx;

    void Awake() {
        Instance = this;
    }

    void Start() {
        Generate();
    }

    void HandleTrigger(GeneratorTrigger trigger) {
        trigger.OnTrigger -= HandleTrigger;
        Generate();
    }

    void Generate() {
        GenerateNextBlock();
        while(blockQueue.Count < bufferSize)
            GenerateNextBlock();
    }

    void GenerateNextBlock() {
        float yTop = idx * -blockHeight;
        GameObject block = new GameObject("block" + idx);
        block.transform.position = Vector3.up * (yTop - blockHeight / 2f);

        GameObject leftWall = new GameObject("LeftWall");
        GameObject rightWall = new GameObject("RightWall");
        leftWall.layer = LayerMask.NameToLayer("Wall");
        rightWall.layer = LayerMask.NameToLayer("Wall");
        leftWall.transform.SetParent(block.transform);
        rightWall.transform.SetParent(block.transform);
        leftWall.transform.localPosition = new Vector3(-10f, 0);
        rightWall.transform.localPosition = new Vector3(10f, 0);
        leftWall.gameObject.AddComponent<BoxCollider>().size = new Vector3(1f, blockHeight, 1f);
        rightWall.gameObject.AddComponent<BoxCollider>().size = new Vector3(1f, blockHeight, 1f);

        float platformX = Mathf.Round(Mathf.Clamp(Random.Range(-10f, 10f), -7f, 7f));
        if(idx == 0)
            platformX = 0;
        GameObject platform = Instantiate(platformPrefab, block.transform);
        platform.transform.localPosition = Vector3.right * platformX;

        GeneratorTrigger trigger = Instantiate(generatorTriggerPrefab, block.transform);
        trigger.transform.localPosition = Vector3.down * blockHeight * 2;
        trigger.OnTrigger += HandleTrigger;

        GenerateEnemies(block);

        blockQueue.Add(block);
        while(blockQueue.Count > bufferSize) {
            GameObject prev = blockQueue[0];
            blockQueue.RemoveAt(0);
            Destroy(prev);
            Destroy(prev);
        }

        idx++;
    }

    void GenerateEnemies(GameObject block) {
        if(GameTime.Value > enemies.Length * 30f) {
            int enemyCount = 10;
            EnemyData enemyData = enemies[enemies.Length - 1];
            for(int j = 0; j < enemyCount; j++)
                SpawnEnemy(enemyData, block);
        } else {
            for(int i = enemies.Length - 1; i >= 0; i--) {
                int enemyCount = Mathf.FloorToInt(Mathf.Max(0, 10 - Mathf.Pow(((GameTime.Value - (i + 1) * 30f) / 10f), 2f)));
                for(int j = 0; j < enemyCount; j++)
                    SpawnEnemy(enemies[i], block);
            }
        }
    }

    void SpawnEnemy(EnemyData enemyData, GameObject block) {
        float xPos = Random.Range(-6.5f, 6.5f);
        float yPos = Random.Range(-blockHeight + 4f, -4f); 
        Vector3 pos = block.transform.TransformPoint(new Vector3(xPos, yPos, 0));
        Collider[] cols = Physics.OverlapSphere(pos, 1f, LayerMask.GetMask("Ground", "Enemy"));
        if(!Physics.CheckSphere(pos, 1f, LayerMask.GetMask("Ground", "Enemy"))) {
            Enemy enemy = Instantiate(enemyPrefab, block.transform);
            enemy.Initialize(enemyData);
            enemy.transform.position = pos;
            Physics.SyncTransforms();
        }
    }
}
