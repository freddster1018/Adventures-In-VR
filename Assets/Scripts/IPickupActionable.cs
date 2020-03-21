using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupActionable
{
  void OnEnter();
  void OnExit();
  void GrabUp();
  void GrabDown();
  void TriggerUp();
  void TriggerDown();
}
