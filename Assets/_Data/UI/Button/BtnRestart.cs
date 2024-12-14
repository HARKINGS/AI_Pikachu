using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnRestart : BaseButton
{
    protected override void OnClick()
    {
        SceneManager.LoadScene("level1");
    }
}
