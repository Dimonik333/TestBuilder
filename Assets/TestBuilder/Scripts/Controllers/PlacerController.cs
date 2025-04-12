using UnityEngine;

public sealed class PlacerController : MonoBehaviour
{
    [SerializeField] private ItemBlueprint _cubeBlueprint;
    [SerializeField] private ItemBlueprint _sphereBlueprint;
    [SerializeField] private ItemPlacer _placer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _placer.SetBlueprint(_cubeBlueprint);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _placer.SetBlueprint(_sphereBlueprint);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _placer.RemoveBlueprint();
        if (Input.GetMouseButtonDown(0))
            _placer.Build();
        _placer.SetScroll(Input.mouseScrollDelta);
    }
}
