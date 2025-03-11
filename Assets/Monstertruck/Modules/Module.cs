using Ui;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Module : MonoBehaviour
{
    public UnityEvent onAttachChange;
    [SerializeField] private UiTrackObject clickRegistrarPrefab;
    private bool onGround = false;
    [SerializeField] private GameObject visualWhenOnGround;
    [SerializeField] private GameObject visualWhenAttached;
    protected bool needsMaintenance = false;

    public void SetOnGround(bool onGround, int spriteOrderInLayer = 0)
    {
        this.onGround = onGround;
        GameObject[] visuals = new GameObject[]{visualWhenOnGround, visualWhenAttached};
        foreach(GameObject visual in visuals)
        {
            visual.SetActive(false);
        }
        GameObject activeVisual = visuals[onGround ? 0 : 1];
        activeVisual.SetActive(true);
        SpriteRenderer[] renderers = activeVisual.GetComponentsInChildren<SpriteRenderer>(false);
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = spriteOrderInLayer;
        }
        onAttachChange.Invoke();
    }

    void OnClicked()
    {
        if(onGround)
        {
            ModuleHolder moduleHolder = GameObject.FindWithTag("GameManager").GetComponent<ModuleHolder>();
            moduleHolder.PushModule(this);
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
        GameObject clickRegistrar = UiManager.GetInstance().AddTracking(clickRegistrarPrefab, gameObject);
        clickRegistrar.GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool IsAttached()
    {
        return !onGround;
    }
}
