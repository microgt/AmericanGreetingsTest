using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*///////////////////////////////////////////////////////////////////////////////////

The Shape highlighter script is responsible for putting a highlight around the
selected shape within the menu, initially the highlight is placed on the very
first element of the shapes group and when in use the script will just remove
all highlights and then apply one to only the button that was pressed.

//////////////////////////////////////////////////////////////////////////////////*/
public class ShapeHighligherScript : MonoBehaviour
{
    void Start(){
        buttonHighlighter(transform.GetChild(0).gameObject);
    }
    public void buttonHighlighter(GameObject button){
        foreach(Outline o in transform.GetComponentsInChildren<Outline>()){
            o.enabled = false;
        }
        button.GetComponent<Outline>().enabled = true;
    }
}
