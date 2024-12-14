using UnityEngine;

public class BtnQuit : BaseButton
{   
    protected override void OnClick()
    {
        Application.Quit();
    }
}