using UnityEngine;

public class Module : MonoBehaviour
{
    private bool onGround = false;
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
    }

    // Update is called once per frame
    void Update()
    {
    }
}
