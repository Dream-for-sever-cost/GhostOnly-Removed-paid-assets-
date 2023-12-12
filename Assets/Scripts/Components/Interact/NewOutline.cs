using UnityEngine;

public enum InteractType
{
    Slave = 0,
    Equip = 1,
    NPC = 2,
}

[RequireComponent(typeof(SpriteRenderer))]
public class NewOutline : MonoBehaviour
{
    private const int OUTLINE_SIZE = 5;

    [field: SerializeField] public InteractType Type { get; private set; }

    [SerializeField] private SpriteRenderer originRenderer;
    [SerializeField] private SpriteRenderer outlineRenderer;

    [SerializeField] private GameObject interactObject;

    private void Start()
    {
        outlineRenderer.enabled = false;

        SetOutline();
        outlineRenderer.sortingLayerID = originRenderer.sortingLayerID;
        outlineRenderer.sortingOrder = originRenderer.sortingOrder - 1;
    }

    public void ShowOutline(bool onoff)
    {
        outlineRenderer.sprite = originRenderer.sprite;

        outlineRenderer.enabled = onoff;
        interactObject?.SetActive(onoff);
    }

    private void SetOutline()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        outlineRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", 1);
        mpb.SetColor("_OutlineColor", SwitchColor(Type));
        mpb.SetFloat("_OutlineSize", OUTLINE_SIZE);
        outlineRenderer.SetPropertyBlock(mpb);
    }

    private Color SwitchColor(InteractType type)
    {
        return type switch
        {
            InteractType.Slave => Color.white,
            InteractType.Equip => Color.blue,
            InteractType.NPC   => Color.yellow,
            _ => Color.white,
        };
    }
}
