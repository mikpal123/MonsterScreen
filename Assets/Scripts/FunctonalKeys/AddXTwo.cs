using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// script for button that wil add two slot monster to selected buttons
/// </summary>
public class AddXTwo : MonoBehaviour
{
    //values ​​that must be set in Unity
    [SerializeReference] private Canvas canvas;
    public Button addTwo;

    //values that don't need to be set in Unity
    private Button firstButtonRef;
    private Button secondButtonRef;    
    private ButtonHandler handler;
    private int childCount;


    void Start()
    {
        //setup some values
        Button btn = addTwo.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        handler = canvas.GetComponent<ButtonHandler>();
    }

    void TaskOnClick()
    {       
        AddXTwoUnit();
        handler.CanSwap();
    }
  
    //check if monster can be spawned, true = spawn
    private void AddXTwoUnit()
    {
        //setup this every click (simply for reference)
        firstButtonRef = handler.firstButtton;
        secondButtonRef = handler.secondButtton;
        
        if(firstButtonRef == null && secondButtonRef ==null) return;

        //when someone click slot in order - slot[1],slot[0]       
        if ((handler.NextButton(secondButtonRef,1)!=null) && (handler.NextButton(firstButtonRef,1) != null))  
        {
            if (handler.NextButton(secondButtonRef,1) == firstButtonRef &&
                CheckParentNames(firstButtonRef, secondButtonRef) &&
                !handler.IsThereMonster(firstButtonRef))
            {
                SetXTwoUnit(secondButtonRef);                      
                return;
            }
        }               
                    
        //first button
        if (firstButtonRef != null) FillWithXTwoMonster(firstButtonRef);                

        //second button
        if (secondButtonRef != null) FillWithXTwoMonster(secondButtonRef);
    }

    //fill with two slot monsters
    private void FillWithXTwoMonster(Button button)
    {
        childCount = button.transform.parent.childCount - 1;

        if (handler.NextChild(button, 1) == null)                                                       //setting for last slot
        {
            if (!handler.IsThereMonster(button) && !handler.IsThereMonster(handler.NextButton(button, -1)))
            {
                if (button.transform.parent.GetChild(childCount).GetComponent<Button>() == button.GetComponent<Button>())
                {
                    SetXTwoUnit(handler.NextButton(button, -1));
                }
            }
        }        
        else
        {            
            if (!handler.IsThereMonster(button) && !handler.IsThereMonster(handler.NextButton(button, 1)))  //normal setting
            {
                SetXTwoUnit(button);                                                                 
            }
            else if (handler.NextChild(button, -1) != null)
            {
                if (!handler.IsThereMonster(button) && !handler.IsThereMonster(handler.NextButton(button, -1)))
                {
                    SetXTwoUnit(handler.NextButton(button, -1));                     //backward setting                      
                }
            }
        }
    }

    //spawn two slot monster
    private void SetXTwoUnit(Button button)
    {
        button.gameObject.AddComponent<MonsterXTwo>();
        handler.NextButton(button,1).gameObject.AddComponent<MonsterXTwo>();

        button.gameObject.GetComponent<MonsterXTwo>().place.Add(button);
        button.gameObject.GetComponent<MonsterXTwo>().place.Add(handler.NextButton(button,1));

        handler.NextButton(button,1).gameObject.GetComponent<MonsterXTwo>().place.Add(button);
        handler.NextButton(button,1).gameObject.GetComponent<MonsterXTwo>().place.Add(handler.NextButton(button,1));
    }

    //check if buttons have the same parents
    private bool CheckParentNames(Button button1, Button button2)
    {
        if (button1.transform.parent.name == button2.transform.parent.name) 
        {
            return true;
        }
       
        return false;
    }    
   
    
}
