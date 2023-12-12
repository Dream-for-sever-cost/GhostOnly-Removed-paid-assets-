using UnityEngine;
using static SoundManager;

public class UI_Popup : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, true);
        /*RectTransform myRectTransform = this.gameObject.GetComponent<RectTransform>();
        myRectTransform = transform as RectTransform;
        myRectTransform.SetAnchor(AnchorPresets.StretchAll);
        myRectTransform.offsetMin = new Vector2(0, 0);
        myRectTransform.offsetMax = new Vector2(-0,-0);*/

        return true;
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);   
    }

    public virtual void HidePopupUI()
    {
        Managers.UI.HidePopupUI(this);
    }
}