using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void loadScene(int i){
        UnityEngine.SceneManagement.SceneManager.LoadScene(i);
    }
}
