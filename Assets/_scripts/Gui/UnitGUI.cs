using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitGUI : EventPubSub
{
    
    [SerializeField]
    GameObject gotext = null;
    public void Attk()
    {
        Publish("GUI: Attack");
    }
    public void End()
    {
        Publish("GUI: End");
    }

    void GameOver()
    {
        UnSubscribe("GameOver", GameOver);
        ShowText();
        gameObject.SetActive(false);
    }
    void ShowText()
    {
        gotext.SetActive(true);
    }
    void OnEnable()
    {
        Subscribe("GameOver", GameOver);

    }

}
