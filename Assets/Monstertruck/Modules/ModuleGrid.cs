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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moduleSlots = new List<ModuleSlot>();
        for(int i = 0; i < size.x * size.y; i++)
        {
            moduleSlots.Add(new ModuleSlot());
        }
        Module module = GameObject.Instantiate(startModule, transform);
        SetModule(startModulePos, module);
    }

    int? Index(int x, int y)
    {
        if(x < 0 || x >= size.x) return null;
        if(y < 0 || y >= size.y) return null;
        return y * size.x + x;
    }

    int? Index(Vector2Int pos)
    {
        return Index(pos.x, pos.y);
    }

    Vector2Int? Pos(int index)
    {
        if(index < 0 || index >= moduleSlots.Count) return null;
        return new Vector2Int(index % size.x, index / size.x);
    }

    void SetModule(Vector2Int pos, Module moduleInstance)
    {
        int? indexNullable = Index(pos);
        if(!indexNullable.HasValue) throw new Exception("Invalid pos: " + pos);
        int index = indexNullable.Value;
        moduleSlots[index].Clear();
        moduleInstance.transform.parent = transform;
        moduleInstance.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        moduleSlots[index].module = moduleInstance;
        moduleInstance.SetOnGround(false, moduleSlots.Count - index);
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
        Vector2Int pos = Pos(iModule).Value;
        CheckAddDecorations(pos, true);
        CheckAddAllNeighborDecorations(pos, true);
    }

    private void CheckAddAllNeighborDecorations(Vector2Int pos, bool refresh = false)
    {
        foreach(Direction dir in AllDirections())
        {
            Vector2Int offset = GetVector2Int(dir);
            Vector2Int neighborPos = pos + offset;
            CheckAddDecorations(neighborPos, refresh);
        }
    }

    void CheckAddDecorations(Vector2Int pos, bool refresh = false)
    {
        int? iModuleNullable = Index(pos);
        if(!iModuleNullable.HasValue) return;
        int iModule = iModuleNullable.Value;
        if(refresh) moduleSlots[iModule].ClearDecorations();
        CheckAddWheel(iModule);
        CheckAddAttachmentPoint(iModule);
    }

    private bool CheckAddAttachmentPoint(int iModule)
    {
        Vector2Int pos = Pos(iModule).Value;
        if (moduleSlots[iModule].IsOccupied()) return false;
        if (!AnyNeighborHasModule(pos)) return false;
        Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
        AttachmentPoint ap = Ui.UiManager.GetInstance().AddAttachmentPoint(gameObject, pos3);
        moduleSlots[iModule].attachmentPoint = ap;
        ap.GetComponent<DragReceiver>().onReceive.AddListener(() => 
        {
            PlaceCurrentModule(pos);
        });
        return true;
    }

    private bool AnyNeighborHasModule(Vector2Int pos)
    {
        foreach(Direction dir in AllDirections())
        {
            Vector2Int offset = GetVector2Int(dir);
            Vector2Int neighborPos = pos + offset;
            if (HasModule(Index(neighborPos))) return true;
        }
        return false;
    }

    private bool HasModule(int? iModule)
    {
        if(!iModule.HasValue) return false;
        return moduleSlots[iModule.Value].module;
    }

    private void CheckAddWheel(int? iModule, bool refresh = false)
    {
        if(iModule.HasValue)
        {
            CheckAddWheel(iModule.Value, refresh);
        }
    }

    private void CheckAddWheel(int iModule, bool refresh = false)
    {
        if(refresh)
        {
            GameObject wheel = moduleSlots[iModule].wheel;
            if(wheel) GameObject.Destroy(wheel);
        }
        if(!moduleSlots[iModule].CanAddWheel()) return;
        Vector2Int pos = Pos(iModule).Value;
        CheckAddSideWheel(pos, Vector2Int.left);
        CheckAddSideWheel(pos, Vector2Int.right);
    }

    private bool CheckAddSideWheel(Vector2Int emptyPos, Vector2Int neighborDir)
    {
        Vector2Int neighborPos = emptyPos + neighborDir;
        if(!CanHaveWheel(neighborPos)) return false;
        GameObject wheelInstance = GameObject.Instantiate(wheelPrefab, transform);
        Vector2 sideWheelPos = wheelOffset;
        if(sideWheelPos.x > 0 != neighborDir.x > 0)
        {
            sideWheelPos.x *= -1;
        }
        sideWheelPos += emptyPos;
        wheelInstance.transform.localPosition = sideWheelPos;
        moduleSlots[Index(emptyPos).Value].wheel = wheelInstance;
        return true;
    }

    private bool CanHaveWheel(Vector2Int pos)
    {
        // if(pos.x == startModulePos.x)
        // {
        //     int yDist = pos.y - startModulePos.y;
        //     if(yDist == 0 || yDist == 1) return false;
        // }
        int? iModuleNullable = Index(pos);
        if(!iModuleNullable.HasValue) return false;
        int iModule = iModuleNullable.Value;
        return moduleSlots[iModule].module != null;
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

    void PlaceCurrentModule(Vector2Int pos)
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
