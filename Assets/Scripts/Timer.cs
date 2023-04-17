using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    float gameTimer;
    public TMPro.TMP_Text tText;
    public TMPro.TMP_Text oneLifeText;
    bool spacedown = false;
    public GameObject FinishUI;
    public bool notDied = true;
    public string notDiedString;

    [ContextMenu("Do Something")]
    public void DoSomething()
    {
        notDiedString = "You Finished without dying";
    }

    void Start(){
        FinishUI = GameObject.Find("FinishUI");
        FinishUI.SetActive(false);
    }

    void Update()
    {
        tText.text = gameTimer.ToString();

        if(Input.GetKeyDown(KeyCode.Space)){
            spacedown = !spacedown;
        }

        if(!spacedown){
            gameTimer += Time.deltaTime;
        }
    }

    void Finish(){
        FinishUI.SetActive(true);
        Time.timeScale = 0.2f;

        if(notDied){
            oneLifeText.text = notDiedString;
        }

        Destroy(this);
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            Finish();
        }
    }
}
