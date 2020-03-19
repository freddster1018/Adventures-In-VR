using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{

  public SteamVR_Input_Sources handType;
  public SteamVR_Behaviour_Pose controllerPose;
  public SteamVR_Action_Boolean grabAction;
  public SteamVR_Action_Boolean triggerAction;

  private GameObject collidingObject;
  private GameObject objectInHand;
  private IPickupActionable heldItem;

  //interactable object
  //Set this when you enter a collider
  //private GameObject interactionZoneObject;
  //private IPickupActionable interactionZone;

  private void SetCollidingObject(Collider col)
  {
    // 1
    if (collidingObject || !col.GetComponent<Rigidbody>())
    {
      return;
    }
    // 2
    collidingObject = col.gameObject;
  }

  // 1
  public void OnTriggerEnter(Collider other)
  {
    SetCollidingObject(other);
  }

  // 2
  public void OnTriggerStay(Collider other)
  {
    SetCollidingObject(other);
  }

  // 3
  public void OnTriggerExit(Collider other)
  {
    if (!collidingObject)
    {
      return;
    }

    collidingObject = null;
  }


  private void GrabObject()
  {
    // 1
    objectInHand = collidingObject;
    collidingObject = null;
    heldItem = objectInHand.GetComponent<IPickupActionable>();
    // 2

    //Align object

    //if(objectInHand.CompareTag("Pickup")) objectInHand.transform.SetPositionAndRotation(controllerPose.transform.position, controllerPose.transform.rotation);

    var joint = AddFixedJoint();
    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
  }

  // 3
  private FixedJoint AddFixedJoint()
  {
    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
    fx.breakForce = 20000;
    fx.breakTorque = 20000;
    return fx;
  }

  private void ReleaseObject()
  {
    // 1
    if (GetComponent<FixedJoint>())
    {
      // 2
      GetComponent<FixedJoint>().connectedBody = null;
      Destroy(GetComponent<FixedJoint>());
      // 3
      objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
      objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

    }
    // 4
    objectInHand = null;
    heldItem = null;
  }



  // Update is called once per frame
  void Update()
    {
    // 1
    if (grabAction.GetLastStateDown(handType))
    {
      if (collidingObject)
      {
        GrabObject();
      }

      //Do a thing with interactions yknow!
    }

    // 2
    if (grabAction.GetLastStateUp(handType))
    {
      if (objectInHand)
      {
        ReleaseObject();
      }
    }

    if (triggerAction.GetLastStateUp(handType))
    {
      if (objectInHand)
      {
        //invoke action
        if (heldItem != null)
        {
          //import as an interface?
          heldItem.Action();
        }

        //if interaction object isn't done
        //invoke action
      }
    }
  }
}
