using UnityEngine;

public class EngineMGProgressIndicator : MonoBehaviour
{
    [SerializeField] private EngineMinigame minigame;
    [SerializeField] private float startRotation;
    [SerializeField] private float endRotation;

    void Awake()
    {
        if(startRotation < 0) startRotation += 360;
        if(endRotation < 0) endRotation += 360;
    }

    void Update()
    {
        float rotation = Mathf.Lerp(startRotation, endRotation, minigame.GetProgress());
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
