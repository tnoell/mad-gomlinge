using Ui;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Combat;

public class Module : MonoBehaviour
{
    [FormerlySerializedAs("onAttachChange")] public UnityEvent onAttached;
    public UnityEvent onUnattached;
    public UnityEvent onDoMaintenance;
    [SerializeField] private UiTrackObject clickRegistrarPrefab;
    [SerializeField] private GameObject visualWhenOnGround;
    [SerializeField] private GameObject visualWhenAttached;
    [SerializeField] private float damageWhenBroken = 10;
    [SerializeField] private GameObject explosionPrefab;
    private bool onGround = false;
    private bool broken;

    public void SetOnGround(bool onGround, int spriteOrderInLayer)
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
        InvokeAttachEvent();
    }


    public void SetOnGround(bool onGround) // used in Event in Tutorial (Unity events don't like default parameters)
    {
        SetOnGround(onGround, 0);
    }

    private void InvokeAttachEvent()
    {
        if (onGround)
        {
            onUnattached.Invoke();
        }
        else
        {
            onAttached.Invoke();
        }
    }

    void OnClicked()
    {
        if(onGround)
        {
            ModuleHolder moduleHolder = GameObject.FindWithTag("GameManager").GetComponent<ModuleHolder>();
            moduleHolder.GroundModuleClicked(this);
        }
        else
        {
            onDoMaintenance.Invoke();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        broken = false;
        MaintenanceTimer maintenance = GetComponent<MaintenanceTimer>();
        if(maintenance)
        {
            maintenance.onExpired.AddListener(() => SetBroken(true));
            maintenance.onRestored.AddListener(() => SetBroken(false));
        }
    }

    void Start()
    {
        GameObject clickRegistrar = UiManager.GetInstance().AddTracking(clickRegistrarPrefab, gameObject);
        clickRegistrar.GetComponent<Button>().onClick.AddListener(OnClicked);
        // InvokeAttachEvent();
    }

    public void SetBroken(bool broken)
    {
        if(broken == this.broken) return;
        this.broken = broken;
        if(broken)
        {
            GetComponentInParent<Combatant>().DealDamage(damageWhenBroken, "Broken Module");
        }
        Animator[] animators = visualWhenAttached.GetComponentsInChildren<Animator>();
        foreach(Animator anim in animators)
        {
            anim.enabled = !broken;
        }
    }

    public bool IsAttached()
    {
        return !onGround;
    }

    [ContextMenu("Explode")]
    public void Explode(bool destroy)
    {
        GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform.parent);
        if(destroy)
        {
            SetOnGround(true);
            GetComponentInParent<ModuleGrid>().DestroyModule(this);
        }
    }
}
