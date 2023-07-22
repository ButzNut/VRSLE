using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneTo()
    {
        StartCoroutine(ChangeSceneSlow());
    }

    private IEnumerator ChangeSceneSlow()
    {
        //fade to black
        OVRScreenFade.instance.FadeOut();
        yield return new WaitForSeconds(OVRScreenFade.instance.fadeTime + 1f);
        SceneManager.LoadScene(1);
    }
}