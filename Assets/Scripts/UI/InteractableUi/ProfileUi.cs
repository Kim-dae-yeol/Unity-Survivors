using System;
using System.Collections;
using System.Collections.Generic;
using Model.Player;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class ProfileUi : UiPopup
{
    //todo image resource 가져오는 방법 공부 
    [SerializeField] private Image profileImage;
    [SerializeField] public Text nameText;
    [SerializeField] public Slider expSlider;

    public void Initialize(Sprite profile, UserInfo info)
    {
        //todo 
    }

    public void UpdateState()
    {
        
    }
}