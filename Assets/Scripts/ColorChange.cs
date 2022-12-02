using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Change color of available buttons
/// </summary>
public class ColorChange : MonoBehaviour
{
    //values that must be set in Unity
    public Button button;
    public Canvas canvas;
    public Color selectedColor; //the color of the selected button will be changed to this color

    //values that don't need to be set in Unity
    private Color currentColor;
    private Color oldColor;    
    private ButtonHandler buttonHandler;




    void Start()
    {
        //setup some values
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        oldColor = button.colors.selectedColor;
        buttonHandler = canvas.GetComponent<ButtonHandler>();
    }

    void TaskOnClick()
    {
        this.ChangeButtonColor(button);
        buttonHandler.SelectButton(button);
    }
   

   
    public void ChangeButtonColor(Button bt)
    {
        if(bt.gameObject.GetComponent<MonsterBase>() != null)                                                  
        {
            if (bt.gameObject.GetComponent<MonsterBase>().canBeSelected)
            {
                if (currentColor != selectedColor)
                {
                    _ChangeButtonPropery(selectedColor, bt);
                }
                else
                {
                    _ChangeButtonPropery(oldColor, bt);
                }
            }
            else
            {
                if (bt.gameObject.GetComponent<ColorChange>().selectedColor == bt.colors.selectedColor)
                {
                    _ChangeButtonPropery(oldColor, bt);
                }
            }
        }
        else
        {
            if (currentColor == selectedColor)
            {
                _ChangeButtonPropery(oldColor, bt);
            }
            else
            {
                _ChangeButtonPropery(selectedColor, bt);
            }
        }
    }

    //change the color of the selected button
    private void _ChangeButtonPropery(Color cl, Button bt)
    {
        ColorBlock cb = bt.colors;
        cb.selectedColor = cl;
        cb.highlightedColor = cl;
        cb.normalColor = cl;
        bt.colors = cb;
        currentColor = cl;
    }

}
