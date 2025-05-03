using System.Collections.Generic;
using UnityEngine;

public class NeighborModifier : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> neighborOffsets = new List<Vector2Int>{
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };
    [SerializeField] float amount;
    [SerializeField] private ModuleStat stat;

    private Module module;
    private ModuleGrid grid;
    private ModuleGrid.ModulePos pos;
    private List<ModuleStats.Modifier> appliedModifiers; // can contain modifiers for modules that have been destroyed

    void Awake()
    {
        module = GetComponent<Module>();
        module.onAttached.AddListener(HandleAttach);
        module.onUnattached.AddListener(HandleUnattach);
        grid = null;
        pos = new ModuleGrid.ModulePos(-1, -Vector2Int.one);
        appliedModifiers = new List<ModuleStats.Modifier>();
    }

    private void HandleAttach()
    {
        grid = GetComponentInParent<ModuleGrid>();
        pos = grid.FindModule(module);
        List<Module> neighbors = grid.GetModulesAtOffsets(pos, neighborOffsets);
        foreach(Module neighbor in neighbors)
        {
            AddModifier(neighbor);
        }

        grid.onModuleChanged += ModifyIfNeighbor;
    }

    private void AddModifier(Module neighbor)
    {   
        ModuleStats neighborStats = neighbor.GetComponent<ModuleStats>();
        ModuleStats.Modifier modifier = new ModuleStats.Modifier(stat, amount, gameObject);
        neighborStats.AddModifier(modifier);
        appliedModifiers.Add(modifier);
    }

    private void ModifyIfNeighbor(ModuleGrid.ModulePos pos, Module module)
    {
        if(module == null) return; // We don't remove the modifiers from appliedModifiers because
                                    // we would need to track which modifiers belong where.
        foreach(Vector2Int offset in neighborOffsets)
        {
            if(this.pos.vec + offset == pos.vec)
            {
                AddModifier(module);
                break;
            }
        }
    }

    private void HandleUnattach()
    {
        if(!grid) return;
        foreach(ModuleStats.Modifier modifier in appliedModifiers)
        {
            modifier.Destroy();
        }
        appliedModifiers.Clear();
        grid.onModuleChanged -= ModifyIfNeighbor;
        grid = null;
        pos = new ModuleGrid.ModulePos(-1, -Vector2Int.one);
    }
}
