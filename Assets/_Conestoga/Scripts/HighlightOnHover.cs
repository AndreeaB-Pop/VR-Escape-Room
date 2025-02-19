using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightOnHover : MonoBehaviour
{
    [Tooltip("Set the material that we will highlight our objects with.")]
    [SerializeField] private Material highlightMaterial;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.sharedMaterial;

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.hoverEntered.AddListener(StartHighlight);
        grabInteractable.hoverExited.AddListener(StopHighlight);
    }

    public void StartHighlight(HoverEnterEventArgs _) => meshRenderer.sharedMaterial = highlightMaterial;

    public void StopHighlight(HoverExitEventArgs _) => meshRenderer.sharedMaterial = originalMaterial;
}
