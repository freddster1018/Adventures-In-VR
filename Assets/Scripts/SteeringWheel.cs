using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteeringWheel : MonoBehaviour
{
  public GameObject h1;
  public GameObject h2;

  public SteamVR_Action_Boolean grabAction;

  private void OnTriggerEnter(Collider other)
  {
    //Set h1 to look at hand
    h1.transform.LookAt(other.transform);
  }

  private void OnTriggerStay(Collider other)
  {
    SteamVR_Input_Sources handtype = other.GetComponent<SteamVR_Input_Sources>();
    if (grabAction.GetLastStateDown(handtype))
    {
      h2.transform.LookAt(other.transform);
    }
    else
    {
      h1.transform.LookAt(other.transform);
    }
    //if(other.isTriggerDown??)
    //If !trigger down 
    //    h1 looks at hand
    //else 
    //    h2 looks at hand
    //    Calculate angle diff between h1 and h2
    //    update Wheel angle
  }
}
