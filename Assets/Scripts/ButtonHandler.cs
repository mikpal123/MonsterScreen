using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// up to two buttons can be pressed at the same time
/// if another button is pressed, the oldest button is unpressed
/// if a unit greater than 1 is spawned, only the first slot can be selected
/// </summary>

public class ButtonHandler : MonoBehaviour
{
    //values that don't need to be set in Unity
    public Button firstButtton;
    public Button secondButtton;
    public bool canSwap = false;
    private MonsterBase monster1;
    private MonsterBase monster2;
    private List<Button> buttonsFromMonster = new List<Button>();


    public void SelectButton(Button selectedButton)
    {                      
        _SelectButton(selectedButton);
        CanSwap();
    }
         
    private void _SelectButton(Button selectedButton)
    {
        if (selectedButton.GetComponent<ColorChange>().selectedColor == selectedButton.colors.selectedColor)
        {
            if (firstButtton == null && secondButtton == null)                              //none are pressd
            {
                firstButtton = selectedButton;
            }
            else if (secondButtton == null && secondButtton != firstButtton)                //first button pressd
            {
                secondButtton = selectedButton;
            }

            else if (firstButtton != selectedButton && secondButtton != selectedButton)     //other button pressd
            {
                if(firstButtton == null)
                {
                    firstButtton = selectedButton;
                }
                else
                {
                    firstButtton.GetComponent<ColorChange>().ChangeButtonColor(firstButtton);
                    firstButtton = secondButtton;
                    secondButtton = selectedButton;
                }
                
            }
        }
        else                                                                                //unpress button
        {
            if (selectedButton == firstButtton)
            {
                firstButtton = secondButtton;
                secondButtton = null;
            }
            else if (secondButtton == selectedButton)
            {
                secondButtton = null;
            }

        }
    }

    //check if under button is MonserBase type component
    public bool IsThereMonster(Button button)
    {
        if(button.GetComponent<MonsterBase>() == null)
        {
            return false;
        }
        return true;
    }

    //get next child (button + val) from button parent
    public Transform NextChild(Button button, int val)
    {
        if (button == null) return null;
        // Check where we are
        int thisIndex = button.transform.GetSiblingIndex();


        if (button.transform.parent == null)
            return null;
        if (button.transform.parent.childCount <= thisIndex + val)
            return null;

        // Then return whatever was next, now that we're sure it's there
        try
        {
            return button.transform.parent.GetChild(thisIndex + val);
        }
        catch(System.Exception)
        {
            return null;
        }
        
    }

    
    //get next button (button +val) from parent
    public Button NextButton(Button button, int val)
    {
        if (NextChild(button, val) != null)
        {
            return NextChild(button, val).gameObject.GetComponent<Button>();
        }
        return null;
    }

    public void CanSwap()
    {
        if (firstButtton != null && secondButtton != null)
        {
            if (firstButtton.gameObject.GetComponent<MonsterBase>() != null && secondButtton.gameObject.GetComponent<MonsterBase>() != null) //two monsters are selected
            {
                monster1 = firstButtton.gameObject.GetComponent<MonsterBase>();
                monster2 = secondButtton.gameObject.GetComponent<MonsterBase>();

                //same size monsters
                if (monster1.place.Count == monster2.place.Count)
                {
                    canSwap = true;                   
                }
                // when second monster is bigger than first
                else if (monster1.place.Count < monster2.place.Count)
                {
                    _CanSwapBiggerMonster(secondButtton,firstButtton);
                }
                //when first monster is bigger than first
                else if(monster2.place.Count < monster1.place.Count)
                {
                    _CanSwapBiggerMonster(firstButtton, secondButtton);
                }
            }
            else
            {
                if (firstButtton.gameObject.GetComponent<MonsterBase>() != null)
                {
                    _CanSwapEmptyMonster(firstButtton, secondButtton);
                }
                else if (secondButtton.gameObject.GetComponent<MonsterBase>() != null)
                {
                    _CanSwapEmptyMonster(secondButtton, firstButtton);
                }
            }
            buttonsFromMonster.Clear();
        }
        else
        {
            canSwap = false;
            return;
        }
    }

    private void _CanSwapBiggerMonster(Button biggerMonsterButton, Button smallerMonsterButton)
    {
        MonsterBase biggerMonster = biggerMonsterButton.gameObject.GetComponent<MonsterBase>();                //setup references
        MonsterBase smallerMonster = smallerMonsterButton.gameObject.GetComponent<MonsterBase>();
        int neededSize = biggerMonster.place.Count - smallerMonster.place.Count;
        canSwap = true;

        foreach(Button monster in biggerMonster.place)                                                    //save values of Bigger monster
        {
            buttonsFromMonster.Add(monster);
        }

        for (int i = 1; i <= neededSize; i++)                                                                  //check if bigger monster can be created forward ---------
        {
            if (NextChild(smallerMonsterButton, i) == null)
            {
                canSwap = false;
                break;
            }            

            Button nextButton = NextButton(smallerMonsterButton, i);
            if (nextButton.gameObject.GetComponent<MonsterBase>() != null && !buttonsFromMonster.Contains(nextButton))
            {
                canSwap = false;
                break;
            }
        }
        if (canSwap)
        {
            return;
        }    
        
        //check if bigger monster can be created backward       
        for (int i = 1; i <= neededSize; i++)
        {
            if (NextChild(smallerMonsterButton, -i) == null)
            {
                canSwap = false;
                break;
            }

            Button nextButton = NextButton(smallerMonsterButton, -i);
            if (nextButton.gameObject.GetComponent<MonsterBase>() != null && buttonsFromMonster.Contains(nextButton))
            {
                canSwap = true;
            }
            else if (nextButton.gameObject.GetComponent<MonsterBase>() == null)
            {
                canSwap = true;
            }
            else
            {
                canSwap = false;
                break;
            }                      
        }
    }

    private void _CanSwapEmptyMonster(Button monsterButton, Button emptyButton)
    {
        MonsterBase monster = monsterButton.gameObject.GetComponent<MonsterBase>();                             //setup references
        canSwap = true;

        foreach(Button button in monster.place)                                                                  //save values of Bigger monster
        {
            buttonsFromMonster.Add(button);
        }
      
        for (int i = 1; i < monster.place.Count; i++)                                                               //check if bigger monster can be created forward
        {
            if (NextChild(emptyButton, i) == null)
            {
                canSwap = false;
                break;
            }
            
            Button nextButton = NextButton(emptyButton, i);
            if (nextButton.gameObject.GetComponent<MonsterBase>() != null && !buttonsFromMonster.Contains(nextButton))
            {
                canSwap = false;
                break;
            }

        }
        if(!canSwap)                                                                                                    //check if bigger monster can be created backward
        {
            for (int i = 1; i < monster.place.Count; i++)
            {
                if (NextChild(emptyButton, -i) != null)
                {
                    Button nextButton = NextButton(emptyButton, -i);

                    if (nextButton.gameObject.GetComponent<MonsterBase>() != null && buttonsFromMonster.Contains(nextButton))
                    {
                        canSwap = true;
                    }
                    else if (nextButton.gameObject.GetComponent<MonsterBase>() == null)
                    {
                        canSwap = true;
                    }
                    else
                    {
                        canSwap = false;
                        break;
                    }
                }
                else
                {
                    canSwap = false;
                    break;
                }
            }           
        }
    }
}
