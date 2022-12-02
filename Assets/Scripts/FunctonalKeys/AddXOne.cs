using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// script for button that wil add one slot monster to selected buttons
/// </summary>
public class AddXOne : MonoBehaviour
{
    //values that must be set in Unity
    public Button addOne;
    public Canvas canvas;

    //values that don't need to be set in Unity
    private ButtonHandler handler;

    void Start()
    {
        //setup some values
        Button btn = addOne.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        handler = canvas.GetComponent<ButtonHandler>();
    }

    void TaskOnClick()
    {
        AddXOneUnit();
        handler.CanSwap();
    }

    private void AddXOneUnit()
    {
        SetXOneUnit(handler.firstButtton);
        SetXOneUnit(handler.secondButtton);
    }

    private void SetXOneUnit(Button button)
    {
        if(button == null) return; 
        
        if (!handler.IsThereMonster(button))
        {
            GameObject buttonObject = button.gameObject;
            buttonObject.AddComponent<MonsterXOne>();
            buttonObject.GetComponent<MonsterXOne>().place.Add(button);
        }       
    }
}
