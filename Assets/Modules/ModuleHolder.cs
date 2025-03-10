using UnityEngine;

public class ModuleHolder : MonoBehaviour
{
    [SerializeField] private Module currentModule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Module GetCurrentModule()
    {
        // placeholder: always give same Module
        Module moduleInstance = GameObject.Instantiate(currentModule);
        return moduleInstance;
    }

    public void HoldModule(Module module)
    {
        currentModule = module;
    }
}
