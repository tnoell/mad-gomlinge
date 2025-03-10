using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Transform scrollingTransform;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Rect spawnArea;

    private static Environment instance = null;

    public static Environment GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("No Environment instance exists");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = scrollingTransform.position;
        pos.y += Time.deltaTime * scrollSpeed;
        scrollingTransform.position = pos;
    }

    public GameObject Spawn(GameObject prefab)
    {
        Vector3 position = new Vector3(
            Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax),
            0);
        return GameObject.Instantiate(prefab, position, Quaternion.identity, scrollingTransform);
    }
    
    void OnDrawGizmosSelected()
    {
        Util.DrawRect(spawnArea, Color.blue);
    }
}
