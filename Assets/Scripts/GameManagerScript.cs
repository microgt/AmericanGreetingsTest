using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
/*///////////////////////////////////////////////////////////////////////////////////

The game manager script is a singleton responsible for handling complex user input
and firing events based on those inputs, it is also responsible for handling other
scene elements like changing the background color and showing the menu.

//////////////////////////////////////////////////////////////////////////////////*/
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance; 
    int numberOfClicks = 0;
    float defaultClickDelay = 0.5f;
    float clickDelay = 0;
    bool canRegisterSwipe = true;
    float defaultSwipeTimer = 0.2f;
    float swipeTimer;
    Vector2 swipeStartPos;
    Image backGround; 
    void Awake()
    {
        //initializing the singleton
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
        //initializing variables
        clickDelay = defaultClickDelay;
        swipeTimer = defaultSwipeTimer;
        backGround = GameObject.FindWithTag("Background").GetComponent<Image>();
    }

    void Update(){
        /*when a click is registered, the user has a set amount of time
        to click again in order to fire the double click event, otherwise
        the double click listener resets*/
        if(numberOfClicks > 0){
            clickDelay -= Time.deltaTime;
        }
        if(clickDelay <= 0){
            resetDoubleClick();
        }

        /*Swipe detection section, when the mouse is pressed, mouse position is registered*/
        if(Input.GetMouseButtonDown(0)){
            swipeStartPos = Input.mousePosition;
        }
        /*If the mouse is held down long enough, the current mouse position is compared to
        the original mouse poistion that was saved when the mouse was first clicked, if
        it's lager the swipeRight event is triggered while the swipeLeft even is triggered
        if it is smaller.
        When an event is fired the swipe variables reset.
        The canRegisterSwipe flag is here to prevent a new swipe operation from executing
        until the user takes their finger off the screen/mouse*/
        if(canRegisterSwipe && Input.GetMouseButton(0)){
            swipeTimer -= Time.deltaTime;
            if(swipeTimer < 0){
                if(Input.mousePosition.x > swipeStartPos.x){
                    swipeRight();
                }else{
                    swipeLeft();
                }

                swipeTimer = defaultSwipeTimer;
                canRegisterSwipe = false;
            }
        }
        /*When the user takes their finger off the screen/mouse, the swipe timer is reset
        and the user regains the ability to initiate a new swipe*/
        if(Input.GetMouseButtonUp(0)){
            swipeTimer = defaultSwipeTimer;
            canRegisterSwipe = true;
        }
        //If the user presses the escape key or the back key on a phone the app closes
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    /*When this event is called from outside, a click is registered prompting the double click
    timer to activate, failure to activate double click before that timer reaches zero will
    reset all double click variables, success will still reset these variables, however it will
    also fire the doubleClick event*/
    public void registerClick(){
            numberOfClicks++;

            if(numberOfClicks == 2){
                resetDoubleClick();
                doubleClick();
            }
    }
    //Helper method that resets the timer and number of click for the double click system
    void resetDoubleClick(){
            numberOfClicks = 0;
            clickDelay = defaultClickDelay;
    }
    //The double click event
    public event Action onDoubleClick;
    public void doubleClick()
    {
        if(onDoubleClick != null)
        {
            onDoubleClick();
        }
    }
    //The left swipe event
    public event Action onSwipeLeft;
    public void swipeLeft(){
        if(onSwipeLeft != null){
            onSwipeLeft();
        }
    }
    //The right swipe event
    public event Action onSwipeRight;
    public void swipeRight(){
        if(onSwipeLeft != null){
            onSwipeRight();
        }
    }
//A method that toggles menu visibility
    public void ToggleMenu (GameObject menu){
        if(menu.activeSelf){
            menu.SetActive(false);
        }else{
            menu.SetActive(true);
        }
    }
    /*When a random color is chosen for the shape, that color is then passed here and a calculation
    takes place in order to find the most complementary color possible to it and apply that to the
    background element, the calculation is done by inverting the color which is best achieved by
    converting the RGB based color into an HSV based color, manipulating the hue and the converting
    the color back to RGB*/
    public void calculateBackGroundColor(Color32 color){
        Color.RGBToHSV((Color32)color, out float h, out float s , out float v);
        float negativeH = (h + 0.5f) % 1;
        Color32 bgColor = Color.HSVToRGB(negativeH, s, v);
        backGround.color = bgColor;
    }
}
 