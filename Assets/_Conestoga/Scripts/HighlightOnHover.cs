using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightOnHover : MonoBehaviour
{
    [Tooltip("Set the material that we will highlight our objects with.")]
    [SerializeField] private Material highlightMaterial;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    public bool heldInHand;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.sharedMaterial;

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.hoverEntered.AddListener(StartHighlight);
        grabInteractable.hoverExited.AddListener(StopHighlight);
    }

    void StartGlow()
    {
        if (!heldInHand)
        {
            meshRenderer.sharedMaterial = highlightMaterial;
        }
        else
        {
            meshRenderer.sharedMaterial = originalMaterial;
        }
    }

    public void HeldInHand()
    {
        heldInHand = true;
    }

    public void DroppedFromHand()
    {
        heldInHand = false;
    }

    public void StartHighlight(HoverEnterEventArgs _) => StartGlow();

    public void StopHighlight(HoverExitEventArgs _) => meshRenderer.sharedMaterial = originalMaterial;

    public void SetOriginalMaterial() => meshRenderer.sharedMaterial = originalMaterial;
}
