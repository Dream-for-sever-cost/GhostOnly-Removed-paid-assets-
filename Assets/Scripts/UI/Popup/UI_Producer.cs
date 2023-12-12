using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Producer : UI_Popup
{
    #region Enums
    enum Buttons
    {
        CloseButton
    }
    enum GameObjects
    {
        UpGameObject
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        GetObject((int)GameObjects.UpGameObject).gameObject.transform.position += Vector3.up;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(() => { OnClickedCloseButton(); });

        return true;
    }

    void OnClickedCloseButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
