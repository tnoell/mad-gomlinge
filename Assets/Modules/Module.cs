using Ui;
using UnityEngine;
using UnityEngine.UI;

public class Module : MonoBehaviour
{
    private bool onGround = false;
    [SerializeField] private UiTrackObject clickRegistrarPrefab;
    [SerializeField] private Sprite spriteWhenOnGround;
    [SerializeField] private Sprite spriteWhenAttached;
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected bool needsMaintenance = false;

    public void SetOnGround(bool onGround)
    {
        if(this.onGround == onGround) return;
        this.onGround = onGround;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Sprite sprite = onGround ? spriteWhenOnGround : spriteWhenAttached;
        spriteRenderer.sprite = sprite;
    }

    void OnClicked()
    {
        Debug.Log(gameObject.name + " clicked");
        if(onGround)
        {
            ModuleHolder moduleHolder = GameObject.FindWithTag("GameManager").GetComponent<ModuleHolder>();
            moduleHolder.HoldModule(this);
        }
        else
        {
            DoMaintenance();
        }
    }

    protected virtual void DoMaintenance()
    {
        needsMaintenance = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSprite();
        GameObject clickRegistrar = UiManager.GetInstance().AddTracking(clickRegistrarPrefab, gameObject);
        clickRegistrar.GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
