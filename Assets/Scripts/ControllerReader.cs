using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerReader : MonoBehaviour
{
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxis("Vertical"));

        Debug.Log(Input.GetAxis("JoyStickR_X"));
        Debug.Log(Input.GetAxis("JoyStickR_Y"));

        if (Input.GetAxis("JoyStickR_X") > 0)
        {
            spr.color = Color.red;
        }
        else if (Input.GetAxis("JoyStickR_X") < 0)
        {
            spr.color = Color.green;
        }
        else if (Input.GetAxis("JoyStickR_Y") > 0)
        {
            spr.color = Color.blue;
        }
        else if (Input.GetAxis("JoyStickR_Y") < 0)
        {
            spr.color = Color.white;
        }


        //if (Input.GetKeyDown("joystick 1 button 0"))
        //{
        //    Debug.Log("A");
        //}
        //if (Input.GetKeyDown("joystick 1 button 1"))
        //{
        //    Debug.Log("B");
        //}
        //if (Input.GetKeyDown("joystick 1 button 2"))
        //{
        //    Debug.Log("C");
        //}
        //if (Input.GetKeyDown("joystick 1 button 3"))
        //{
        //    Debug.Log("D");
        //}
        //if (Input.GetKeyDown("joystick 1 button 4"))
        //{
        //    Debug.Log("E");
        //}
        //if (Input.GetKeyDown("joystick 1 button 5"))
        //{
        //    Debug.Log("F");
        //}
    }
}
