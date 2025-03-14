using UnityEngine;

public class TrailInEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject trailTilePrefab;

    private GameObject lastTile;
    private Environment environment;
    private float tileDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        environment = Environment.GetInstance();
        tileDistance = trailTilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        SpawnTile(transform.position);
    }

    void SpawnTile(Vector3 pos)
    {
        lastTile = GameObject.Instantiate(trailTilePrefab, pos, Quaternion.identity);
        environment.MakeScrolling(lastTile.transform);
    }

    // Update is called once per frame
    void Update()
    {
        float nextTileY = lastTile.transform.position.y - tileDistance;
        if(transform.position.y > nextTileY) return;
        Vector3 pos = transform.position;
        pos.y = nextTileY;
        SpawnTile(pos);
    }
}
