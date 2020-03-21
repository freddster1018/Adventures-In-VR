using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteeringWheel : MonoBehaviour, IPickupActionable
{
  public GameObject h1;
  public GameObject h2;

  public float wheelAngle = 0;

  public float maxWheelAngle = 360;
  public float minWheelAngle = 0;

  bool gripDown = false;

  private void Start()
  {
    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,(maxWheelAngle-minWheelAngle)/2, transform.localEulerAngles.z);
  }
  public void OnEnter() { }

  public void GrabDown()
  {
    gripDown = true;
  }

  public void GrabUp()
  {
    gripDown = false;
  }

  public void OnExit()
  {
    gripDown = false;
  }


  private void OnTriggerEnter(Collider other)
  {
    h1.transform.LookAt(other.transform);  
  }

  private void OnTriggerStay(Collider other)
  {
    if (!gripDown)
    {

      h1.transform.LookAt(other.transform);
    }
    else
    {
      h2.transform.LookAt(other.transform);
      //calc diff in angles?
      //update steering wheel?

      float h2_float = h2.transform.eulerAngles.y;
      float h1_float = h1.transform.eulerAngles.y;
      
      //Acounts for going over the limit
      if(Mathf.Abs(h2_float - h1_float) >= 180.0)
      {
        if (h2_float < h1_float) h2_float += 360;
        else h1_float += 360;
      }
      wheelAngle += h2_float - h1_float;
      Debug.Log(wheelAngle);
      if (wheelAngle > maxWheelAngle) wheelAngle = maxWheelAngle;
      else if (wheelAngle < minWheelAngle) wheelAngle = minWheelAngle;
      else
      {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, wheelAngle, transform.localEulerAngles.z);
        h1.transform.LookAt(other.transform);
      }
      
    }
  }

  public void TriggerUp() { }

  public void TriggerDown() { }
}
