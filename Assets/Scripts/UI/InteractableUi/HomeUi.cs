using System;
using System.Collections;
using System.Collections.Generic;
using Model.UI;
using UI;
using UI.InteractableUi;
using UnityEngine;

public class HomeUi : UiPopup
{
    private ProfileUi _profile;
    [SerializeField] private GameObject buttons;
    [SerializeField] private List<IconButtonInfo> iconInfos;
    
    private void OnEnable()
    {
        _profile = UiManager.ShowPopupByName(nameof(ProfileUi)).GetComponent<ProfileUi>();
        _profile.transform.SetParent(transform, false);
        foreach (var iconButtonInfo in iconInfos)
        {
            IconButton iconButton = UiManager.ShowPopupByName("IconButton").GetComponent<IconButton>();
            iconButton.SetInfo(iconButtonInfo);
            iconButton.transform.SetParent(buttons.transform);
        }
    }
}