using UnityEngine;

public class Gun : MonoBehaviour, IPickupActionable
{

    public float damage = 10.0f;
    public float range = 100.0f;
    public float impactForce = 30.0f;
    public float fireRate = 15f;

    public GameObject end;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    private bool triggerDown = false;


    // Update is called once per frame
    private void Update()
    {
       if (Time.time >= nextTimeToFire && triggerDown) //Input.GetButton("Fire1")
       {
          nextTimeToFire = Time.time + 1f / fireRate;
          Shoot();
       }
    }

  public void Shoot()
  {
    muzzleFlash.Play();

    RaycastHit hit;
    if (Physics.Raycast(end.transform.position, end.transform.forward, out hit))
    {
      Debug.Log(hit.transform.name);
      
      Target target = hit.transform.GetComponent<Target>();
      if (target != null)
      {
          target.TakeDamage(damage);
      }
      
      if (hit.rigidbody != null)
      {
          hit.rigidbody.AddForce(-hit.normal*impactForce);
      }
      
      GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
      Destroy(impact, 2f);
    }

  }

  public void GrabUp() { }
  public void GrabDown() { }

  public void OnEnter() { }

  public void OnExit() 
  {
    triggerDown = false;
  }

  public void TriggerUp() 
  {
    triggerDown = false;
  }

  public void TriggerDown()
  {
    triggerDown = true;
  }
}
