using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using static DirectionUtil;

public class ModuleGrid : MonoBehaviour
{
    public delegate void OnModuleChanged(ModulePos pos, Module module);
    public OnModuleChanged onModuleChanged;

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
            set
            {
                if (value == null)
                {
                    if (_module == null) return;
                    GameObject.Destroy(_module.gameObject);
                    _module = null;
                    return;
                }
                if (IsOccupied())
                { throw new Exception("slot already occupied"); }
                _module = value;
            }
        }
        private AttachmentPoint _attachmentPoint = null;
        public AttachmentPoint attachmentPoint
        {
            get { return _attachmentPoint; }
            set
            {
                if (value == null)
                {
                    if (_attachmentPoint == null) return;
                    GameObject.Destroy(_attachmentPoint.gameObject);
                    _attachmentPoint = null;
                    return;
                }
                if (IsOccupied())
                { throw new Exception("slot already occupied"); }
                _attachmentPoint = value;
            }
        }
        private GameObject _leftWheel = null;
        public GameObject leftWheel
        {
            get { return _leftWheel; }
            set
            {
                if (value == null)
                {
                    if (_leftWheel == null) return;
                    GameObject.Destroy(_leftWheel);
                    _leftWheel = null;
                    return;
                }
                if (!CanAddLeftWheel())
                { throw new Exception("slot already occupied"); }
                _leftWheel = value;
            }
        }
        public bool CanAddLeftWheel()
        {
            return !_module && !_leftWheel;
        }

        private GameObject _rightWheel = null;
        public GameObject rightWheel
        {
            get { return _rightWheel; }
            set
            {
                if (value == null)
                {
                    if (_rightWheel == null) return;
                    GameObject.Destroy(_rightWheel);
                    _rightWheel = null;
                    return;
                }
                if (!CanAddRightWheel())
                { throw new Exception("slot already occupied"); }
                _rightWheel = value;
            }
        }

        public bool CanAddRightWheel()
        {
            return !_module && !_rightWheel;
        }

        public bool IsOccupied()
        {
            return _attachmentPoint || _module;
        }

        public void ClearDecorations()
        {
            attachmentPoint = null;
            leftWheel = null;
            rightWheel = null;
        }

        public void Clear()
        {
            module = null;
            ClearDecorations();
        }
    }
    private List<ModuleSlot> moduleSlots;

    public struct ModulePos
    {
        public readonly int index;
        public readonly Vector2Int vec;

        public ModulePos(int index, Vector2Int vec)
        {
            this.index = index;
            this.vec = vec;
        }
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

    public ModulePos? Pos(Vector2Int pos)
    {
        if(pos.x < 0 || pos.x >= size.x) return null;
        if(pos.y < 0 || pos.y >= size.y) return null;
        return new ModulePos(pos.y * size.x + pos.x, pos);
    }

    ModulePos? Pos(int x, int y)
    {
        return Pos(new Vector2Int(x, y));
    }

    ModulePos? Pos(int index)
    {
        if(index < 0 || index >= moduleSlots.Count) return null;
        return new ModulePos(index, new Vector2Int(index % size.x, index / size.x));
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
        onModuleChanged?.Invoke(pos, moduleInstance);
    }

    public void DestroyModule(Module module)
    {
        ModulePos pos = FindModule(module);
        moduleSlots[pos.index].Clear();
        CheckAddDecorations(pos, true);
        CheckAddAllNeighborDecorations(pos, true);
        onModuleChanged?.Invoke(pos, null);
    }

    public Module GetModule(ModulePos pos)
    {
        return moduleSlots[pos.index].module;
    }

    public ModulePos FindModule(Module module)
    {
        for(int iModule = 0; iModule < moduleSlots.Count; iModule++)
        {
            if(moduleSlots[iModule].module == module) return Pos(iModule).Value;
        }
        throw new Exception("Module " + module + " not found in grid");
    }


    public List<Module> GetModulesAtOffsets(ModulePos basePos, List<Vector2Int> offsets)
    {
        List<Module> modules = new List<Module>();
        for(int i = 0; i < offsets.Count; i++)
        {
            ModulePos? pos = Pos(basePos.vec + offsets[i]);
            if(!pos.HasValue) continue;
            Module module = GetModule(pos.Value);
            if(!module) continue;
            modules.Add(module);
        }
        return modules;
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
            moduleSlots[pos.index].leftWheel = null;
            moduleSlots[pos.index].rightWheel = null;
        }
        if(moduleSlots[pos.index].CanAddLeftWheel())
        { CheckAddSideWheel(pos, Direction.left); }
        if(moduleSlots[pos.index].CanAddRightWheel())
        { CheckAddSideWheel(pos, Direction.right); }
    }

    private bool CheckAddSideWheel(ModulePos emptyPos, Direction neighborDir)
    {
        Vector2Int neighborDirVec = GetVector2Int(neighborDir);
        ModulePos? neighborPos = Pos(emptyPos.vec + neighborDirVec);
        if(!CanHaveWheel(neighborPos)) return false;
        GameObject wheelInstance = GameObject.Instantiate(wheelPrefab, transform);
        Vector2 sideWheelLocalPos = wheelOffset;
        if(sideWheelLocalPos.x > 0 != neighborDirVec.x > 0)
        {
            sideWheelLocalPos.x *= -1;
        }
        sideWheelLocalPos += emptyPos.vec;
        wheelInstance.transform.localPosition = sideWheelLocalPos;
        switch(neighborDir)
        {
        case Direction.left:
            moduleSlots[emptyPos.index].leftWheel = wheelInstance;
            break;
        case Direction.right:
            moduleSlots[emptyPos.index].rightWheel = wheelInstance;
            break;
        default:
            throw new Exception("unhandled direction");
        }
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
