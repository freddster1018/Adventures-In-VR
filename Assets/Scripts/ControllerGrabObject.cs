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
  private GameObject interactionZoneObject;
  private IPickupActionable interactionZone;

  private void SetCollidingObject(Collider col)
  {
    if (collidingObject) return;

    if(col.GetComponent<Rigidbody>() || col.CompareTag("InteractionZone")) collidingObject = col.gameObject;

    if (col.CompareTag("InteractionZone"))
    {
      interactionZoneObject = collidingObject;
      interactionZone = interactionZoneObject.GetComponent<IPickupActionable>();
      interactionZone.OnEnter();
    }
  }

  private void RemoveCollidingObject(Collider col)
  {
    if (!collidingObject)
    {
      return;
    }

    if (col.CompareTag("InteractionZone"))
    {
      interactionZone.OnExit();
      interactionZoneObject = null;
      interactionZone = null;
    }

    collidingObject = null;
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
    RemoveCollidingObject(other);
  }


  private void GrabObject()
  {
    objectInHand = collidingObject;
    collidingObject = null;
    heldItem = objectInHand.GetComponent<IPickupActionable>();

    //Align object
    //if(objectInHand.CompareTag("Align")) objectInHand.transform.SetPositionAndRotation(controllerPose.transform.position, controllerPose.transform.rotation);

    var joint = AddFixedJoint();
    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
  }

  private FixedJoint AddFixedJoint()
  {
    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
    fx.breakForce = 20000;
    fx.breakTorque = 20000;
    return fx;
  }

  private void ReleaseObject()
  {
    if (GetComponent<FixedJoint>())
    {
      GetComponent<FixedJoint>().connectedBody = null;
      Destroy(GetComponent<FixedJoint>());
      objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
      objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

    }
    objectInHand = null;
    heldItem = null;
  }

  // Update is called once per frame
  void Update()
    {
    if (grabAction.GetLastStateDown(handType))
    {
      if (collidingObject)
      {
        if(collidingObject.CompareTag("Pickup"))
        {
          GrabObject();
        }

        else if (collidingObject.CompareTag("InteractionZone"))
        {
          interactionZone.GrabDown();
        }
      }
    }

    if (grabAction.GetLastStateUp(handType))
    {
      if (objectInHand)
      {
        ReleaseObject();
      }

      if (interactionZone != null)
      {
        interactionZone.GrabUp();
      }
    }

    if (triggerAction.GetLastStateDown(handType))
    {
      if (objectInHand)
      {
        //invoke action
        if (heldItem != null)
        {
          //import as an interface?
          heldItem.TriggerDown();
        }
        //if interaction object isn't done
        //invoke action
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
          heldItem.TriggerUp();
        }
        //if interaction object isn't done
        //invoke action
      }
    }
  }
}
