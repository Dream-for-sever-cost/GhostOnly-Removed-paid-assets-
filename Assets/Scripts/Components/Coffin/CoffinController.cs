using UnityEngine;

public class CoffinController : MonoBehaviour
{
    private InteractNPC interact;

    private void Awake()
    {
        interact = GetComponent<InteractNPC>();
    }

    private void Start()
    {
        interact.EventInteract.AddListener(ShowCoffinPopup);
    }

    public void ShowCoffinPopup()
    {
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
        Managers.UI.ShowPopupUI<UI_Coffin>();
    }
}
