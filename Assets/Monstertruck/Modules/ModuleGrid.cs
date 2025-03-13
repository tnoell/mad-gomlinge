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
                if(!CanAddWheel())
                { throw new Exception("slot already occupied"); }
                _wheel = value;
             }
        }

        public bool CanAddWheel()
        {
            return _module == null && _wheel == null; // not blocked by attachmentPoint
        }

        public bool IsOccupied()
        {
            return _attachmentPoint != null || _module != null;
        }
        public void Clear()
        {
            if(_module != null) GameObject.Destroy(_module.gameObject);
            _module = null;
            if(_attachmentPoint != null) GameObject.Destroy(_attachmentPoint.gameObject);
            _attachmentPoint = null;
            if(_wheel != null) GameObject.Destroy(wheel);
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

        foreach(Direction dir in AllDirections())
        {
            Vector2Int offset = GetVector2Int(dir);
            Vector2Int neighborPos = pos + offset;
            AddAttachmentPoint(neighborPos);
        }
        // UpdateAttachmentPoints();
    }

    void AddAttachmentPoint(Vector2Int pos)
    {
        int? iModuleNullable = Index(pos);
        if(!iModuleNullable.HasValue) return;
        int iModule = iModuleNullable.Value;
        CheckAddWheel(iModule);
        if (moduleSlots[iModule].IsOccupied()) return;
        Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
        AttachmentPoint ap = Ui.UiManager.GetInstance().AddAttachmentPoint(gameObject, pos3);
        moduleSlots[iModule].attachmentPoint = ap;
        ap.GetComponent<Button>().onClick.AddListener(() => 
        {
            PlaceCurrentModule(pos);
        });
    }

    private void CheckAddWheel(int iModule)
    {
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
        return true;
    }

    private bool CanHaveWheel(Vector2Int pos)
    {
        if(pos.x == startModulePos.x)
        {
            int yDist = pos.y - startModulePos.y;
            if(yDist == 0 || yDist == 1) return false;
        }
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
