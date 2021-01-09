using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

/*///////////////////////////////////////////////////////////////////////////////////

The Hexa script is the script that is responsible for all the shape behaviours, it's
also in charge of listening to complex input events that get calculated inside of
Game Manager Script.

//////////////////////////////////////////////////////////////////////////////////*/
public class HexaScript : MonoBehaviour
{
    Image image;
    Animator anim;
    bool rotate = false;
    float defaultRotationTime = 0.5f;
    float rotationTime;
    float turnDirection = 1;
    AudioSource aSource;
    public AudioClip colorChangeSFX;
    public AudioClip shapeChangeSFX;
    public AudioClip swipeSFX;
    void Start()
    {
        //Initializing variables
        image = GetComponent<Image>();
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        rotationTime = defaultRotationTime;
        aSource.playOnAwake = false;
        //Subscribing to input events
        GameManagerScript.instance.onDoubleClick += OnDoubleClickTap;
        GameManagerScript.instance.onSwipeLeft += onSwipeLeft;
        GameManagerScript.instance.onSwipeRight += onSwipeRight;   
    }

/*A helper method that generates and returns a random color, once generated, that color
is sent to the Game Manager Script which inverts it and applies the inverted color to
the background object*/
    Color generateRandomColor(){
        byte red = (byte)Random.Range(0, 255);
        byte green = (byte)Random.Range(0, 255);
        byte blue = (byte)Random.Range(0, 255);
        byte alpha = 255;

        Color32 newColor = new Color32(red, green, blue, alpha);
        GameManagerScript.instance.calculateBackGroundColor(newColor);
        return newColor;
    }
/*The double click listener method which changes the color of the shape, plays an
animation and a sound*/
    public void OnDoubleClickTap(){
        image.color = generateRandomColor();
        anim.Play("ColorChange", 0);
        playSFX(colorChangeSFX);
  }
/*When a shape is selected, its sprite is passed to this method which applies it to
the main shape object and plays an animation as well as a sound effect confirming
the change*/
  public void setShape(Sprite image){
      anim.Play("ShapeChange", 0);
      this.image.sprite = image;
      playSFX(shapeChangeSFX);
  }
/*In the update, the shape is constantly tweening its rotation between two fixed points in 
a fixed speed, also if a swipe ticks the rotate flag the object rotates 180 degrees around
the Y axis in the direction of that swipe*/
  void Update(){
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.PingPong(Time.time * 20, 25));
      if(rotate){
          transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.Lerp(transform.localEulerAngles.y, transform.localEulerAngles.y - 180*turnDirection, Time.deltaTime * 2f), transform.localEulerAngles.z);
          rotationTime -= Time.deltaTime;
          if(rotationTime <= 0){
              rotate = false;
              rotationTime = defaultRotationTime;
          }
      }

  }
//The left swipe listener, it sets the swipe direction, allows the object to rotate left and plays a sound
  void onSwipeLeft(){
      rotate = true;
      turnDirection = -1;
      playSFX(swipeSFX);
  }
  //The left swipe listener, it sets the swipe direction, allows the object to rotate right and plays a sound
  void onSwipeRight(){
      rotate = true;
      turnDirection = 1;
      playSFX(swipeSFX);
  }

  public void playSFX(AudioClip clip){
        aSource.pitch = Random.Range(0.9f, 1.1f);
        aSource.clip = clip;
        aSource.Play();
  }
}
