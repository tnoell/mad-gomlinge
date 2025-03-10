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
    private class ModuleSlot
    {
        private Module _module = null;
        public Module module
        {
            //get { return _module; }
            set { 
                if(IsOccupied())
                { throw new Exception("slot already occupied"); }
                _module = value;
             }
        }
        private AttachmentPoint _attachmentPoint = null;
        public AttachmentPoint attachmentPoint
        {
            //get { return _attachmentPoint; }
            set { 
                if(IsOccupied())
                { throw new Exception("slot already occupied"); }
                _attachmentPoint = value;
             }
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
        if (moduleSlots[iModule].IsOccupied()) return;
        Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
        AttachmentPoint ap = Ui.UiManager.GetInstance().AddAttachmentPoint(gameObject, pos3);
        moduleSlots[iModule].attachmentPoint = ap;
        ap.GetComponent<Button>().onClick.AddListener(() => 
        {
            PlaceCurrentModule(pos);
        });
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
