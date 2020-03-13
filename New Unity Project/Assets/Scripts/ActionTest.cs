using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ActionTest : MonoBehaviour
{
  public SteamVR_Input_Sources handType; // 1 
  public SteamVR_Action_Boolean teleportAction; // 2
  public SteamVR_Action_Boolean grabAction; // 3 


  public bool GetTeleportDown() // 1
  {
    return teleportAction.GetStateDown(handType);
  }

  public bool GetGrab() // 2
  {
    return grabAction.GetState(handType);
  }

void Update()
  {
    if(GetTeleportDown())
    {
      print("Teleport " + handType);
    }

    if(GetGrab())
    {
      print("Grab " + handType);
    }
  }
}
