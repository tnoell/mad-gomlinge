using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using static DirectionUtil;

public class ModuleGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector2Int startModulePos;
    [SerializeField] private Module startModule;
    [SerializeField] private GameObject wheelPrefab;
    [SerializeField] private Vector2 wheelOffset;

    private class ModuleSlot
    {
        private Module _module = null;
        public Module module
        {
            get { return _module; }
            set { 
                if(value == null)
                {
                    if(_module == null) return;
                    GameObject.Destroy(_module.gameObject);
                    _module = null;
                    return;
                }
                if(IsOccupied())
                { throw new Exception("slot already occupied"); }
                _module = value;
             }
        }
        private AttachmentPoint _attachmentPoint = null;
        public AttachmentPoint attachmentPoint
        {
            get { return _attachmentPoint; }
            set { 
                if(value == null)
                {
                    if(_attachmentPoint == null) return;
                    GameObject.Destroy(_attachmentPoint.gameObject);
                    _attachmentPoint = null;
                    return;
                }
                if(IsOccupied())
                { throw new Exception("slot already occupied"); }
                _attachmentPoint = value;
             }
        }
        private GameObject _wheel = null;
        public GameObject wheel
        {
            get { return _wheel; }
            set {
                if(value == null)
                {
                    if(_wheel == null) return;
                    GameObject.Destroy(_wheel);
                    _wheel = null;
                    return;
                }
                if(!CanAddWheel())
                { throw new Exception("slot already occupied"); }
                _wheel = value;
             }
        }

        public bool CanAddWheel()
        {
            return !_module && !_wheel; // not blocked by attachmentPoint
        }

        public bool IsOccupied()
        {
            return _attachmentPoint || _module;
        }

        public void ClearDecorations()
        {
            attachmentPoint = null;
            wheel = null;
        }

        public void Clear()
        {
            module = null;
            attachmentPoint = null;
            wheel = null;
        }
    }
    private List<ModuleSlot> moduleSlots;

    private struct ModulePos
    {
        public int index;
        public Vector2Int vec;
        public Vector3Int Vector3Int()
        {
            return (Vector3Int)vec;
        }
        public Vector3 Vector3()
        {
            return (Vector3)Vector3Int();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moduleSlots = new List<ModuleSlot>();
        for(int i = 0; i < size.x * size.y; i++)
        {
            moduleSlots.Add(new ModuleSlot());
        }
        Module module = GameObject.Instantiate(startModule, transform);
        SetModule(Pos(startModulePos).Value, module);
    }

    ModulePos? Pos(Vector2Int pos)
    {
        if(pos.x < 0 || pos.x >= size.x) return null;
        if(pos.y < 0 || pos.y >= size.y) return null;
        return new ModulePos{ index = pos.y * size.x + pos.x, vec = pos };
    }

    ModulePos? Pos(int x, int y)
    {
        return Pos(new Vector2Int(x, y));
    }

    ModulePos? Pos(int index)
    {
        if(index < 0 || index >= moduleSlots.Count) return null;
        return new ModulePos{ index = index, vec = new Vector2Int(index % size.x, index / size.x) };
    }

    void SetModule(ModulePos pos, Module moduleInstance)
    {
        moduleSlots[pos.index].Clear();
        moduleInstance.transform.parent = transform;
        moduleInstance.transform.localPosition = pos.Vector3();
        moduleSlots[pos.index].module = moduleInstance;
        moduleInstance.SetOnGround(false, moduleSlots.Count - pos.index);
        CheckAddAllNeighborDecorations(pos);
        // UpdateAttachmentPoints();
    }

    public void DestroyModule(Module module)
    {
        int iModule;
        for(iModule = 0; ; iModule++)
        {
            if(iModule == moduleSlots.Count)
            {
                Debug.LogError("Module " + module + " not found for deletion");
                return;
            }
            if(moduleSlots[iModule].module == module) break;
        }
        moduleSlots[iModule].Clear();
        ModulePos pos = Pos(iModule).Value;
        CheckAddDecorations(pos, true);
        CheckAddAllNeighborDecorations(pos, true);
    }

    private void CheckAddAllNeighborDecorations(ModulePos pos, bool refresh = false)
    {
        foreach(Direction dir in AllDirections())
        {
            Vector2Int offset = GetVector2Int(dir);
            ModulePos? neighborPos = Pos(pos.vec + offset);
            if(!neighborPos.HasValue) continue;
            CheckAddDecorations(neighborPos.Value, refresh);
        }
    }

    void CheckAddDecorations(ModulePos pos, bool refresh = false)
    {
        if(refresh) moduleSlots[pos.index].ClearDecorations();
        CheckAddWheel(pos);
        CheckAddAttachmentPoint(pos);
    }

    private bool CheckAddAttachmentPoint(ModulePos pos)
    {
        if (moduleSlots[pos.index].IsOccupied()) return false;
        if (!AnyNeighborHasModule(pos)) return false;
        AttachmentPoint ap = Ui.UiManager.GetInstance().AddAttachmentPoint(gameObject, pos.Vector3());
        moduleSlots[pos.index].attachmentPoint = ap;
        ap.GetComponent<Button>().onClick.AddListener(() => 
        {
            PlaceCurrentModule(pos);
        });
        return true;
    }

    private bool AnyNeighborHasModule(ModulePos pos)
    {
        foreach(Direction dir in AllDirections())
        {
            Vector2Int offset = GetVector2Int(dir);
            ModulePos? neighborPos = Pos(pos.vec + offset);
            if (HasModule(neighborPos)) return true;
        }
        return false;
    }

    private bool HasModule(ModulePos? pos)
    {
        if(!pos.HasValue) return false;
        return moduleSlots[pos.Value.index].module;
    }

    private void CheckAddWheel(ModulePos? pos, bool refresh = false)
    {
        if(pos.HasValue)
        {
            CheckAddWheel(pos.Value, refresh);
        }
    }

    private void CheckAddWheel(ModulePos pos, bool refresh = false)
    {
        if(refresh)
        {
            GameObject wheel = moduleSlots[pos.index].wheel;
            if(wheel) GameObject.Destroy(wheel);
        }
        if(!moduleSlots[pos.index].CanAddWheel()) return;
        CheckAddSideWheel(pos, Vector2Int.left);
        CheckAddSideWheel(pos, Vector2Int.right);
    }

    private bool CheckAddSideWheel(ModulePos emptyPos, Vector2Int neighborDir)
    {
        ModulePos? neighborPos = Pos(emptyPos.vec + neighborDir);
        if(!CanHaveWheel(neighborPos)) return false;
        GameObject wheelInstance = GameObject.Instantiate(wheelPrefab, transform);
        Vector2 sideWheelLocalPos = wheelOffset;
        if(sideWheelLocalPos.x > 0 != neighborDir.x > 0)
        {
            sideWheelLocalPos.x *= -1;
        }
        sideWheelLocalPos += emptyPos.vec;
        wheelInstance.transform.localPosition = sideWheelLocalPos;
        moduleSlots[emptyPos.index].wheel = wheelInstance;
        return true;
    }

    private bool CanHaveWheel(ModulePos? pos)
    {
        // if(pos.x == startModulePos.x)
        // {
        //     int yDist = pos.y - startModulePos.y;
        //     if(yDist == 0 || yDist == 1) return false;
        // }
        if(!pos.HasValue) return false;
        return moduleSlots[pos.Value.index].module != null;
    }

    // void UpdateAttachmentPoints()
    // {
    //     foreach(ModuleSlot slot in moduleSlots)
    //     {
    //         GameObject.Destroy(slot.attachmentPoint.gameObject);
    //     }
    //     for (int iModule = 0; iModule < moduleSlots.Count; iModule++)
    //     {
    //         Vector2Int pos = Pos(iModule).Value;
    //         if(moduleSlots[iModule] != null) continue;
    //         Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
    //         AttachmentPoint ap = Ui.UiManager.GetInstance().AddAttachmentPoint(gameObject, pos3);
    //         ap.GetComponent<Button>().onClick.AddListener(() => 
    //         {
    //             PlaceCurrentModule(pos);
    //             GameObject.Destroy(ap.gameObject);
    //         });
    //     }
    // }

    void PlaceCurrentModule(ModulePos pos)
    {
        ModuleHolder moduleHolder = GameObject.FindWithTag("GameManager").GetComponent<ModuleHolder>();
        Module module = moduleHolder.PopModule();
        SetModule(pos, module);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnDrawGizmosSelected()
    {
        Rect gridArea = new Rect(
            transform.position.x - 0.5f,
            transform.position.y - 0.5f,
            size.x,
            size.y
        );
        Util.DrawRect(gridArea, Color.blue);

        Vector3 globalStartModulePos = transform.position + (Vector3)(Vector2)startModulePos;
        Rect startModuleArea = new Rect(
            globalStartModulePos.x - 0.5f,
            globalStartModulePos.y - 0.5f,
            1, 1);
        Util.DrawRect(startModuleArea, Color.red);
    }
}
