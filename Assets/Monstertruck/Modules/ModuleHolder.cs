using Ui;
using UnityEngine;

public class ModuleHolder : MonoBehaviour
{
    private Module currentModule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentModule = null;
        UpdateAttachmentPointVisibility();
    }

    void UpdateAttachmentPointVisibility()
    {
        UiManager.GetInstance().EnableAttachmentPoints(currentModule != null);
    }

    // Update is called once per frame
    void Update()
    {
        if(!currentModule) return;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        currentModule.transform.position = pos;
    }

    public Module PopModule()
    {
        Module module = currentModule;
        currentModule = null;
        UpdateAttachmentPointVisibility();
        return module;
    }

    public void PushModule(Module module)
    {
        currentModule = module;
        UpdateAttachmentPointVisibility();
    }
}
