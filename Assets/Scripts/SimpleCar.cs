using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCar : MonoBehaviour
{
    [SerializeField]
    public AxleInfo[] axleInfos;

    [SerializeField]
    public List<GearInfo> gears = new List<GearInfo>();

    private GearInfo currentGear;

    [SerializeField]
    private float brakespeed = 80.0f;

    private Rigidbody rb;

    private Vector3 visualWheelOffset = Vector3.zero;

    //[SerializeField]
    //private Text speedText;

    //[SerializeField]
    //private Text gearText;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        visualWheelOffset = axleInfos[0].LeftWheel.transform.GetChild(0).eulerAngles;

        currentGear = gears[0];
        //speedText.text = "Speed: 0";
        //gearText.text = "Gear: 1";

        for (int i = 0; i < axleInfos.Length; i++)
        {
            AxleInfo axle = axleInfos[i];

            axle.RightWheel.ConfigureVehicleSubsteps(20, 12, 16);
            axle.LeftWheel.ConfigureVehicleSubsteps(20, 12, 16);


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            ChangeGear();
        }
    }

    private void FixedUpdate()
    {
        ControlCar();

    }

    public void ChangeGear()
    {
        int currentIndex = gears.IndexOf(currentGear);

        if (Input.GetKeyDown(KeyCode.Q) && currentIndex != 0) currentGear = gears[currentIndex - 1];
        if (Input.GetKeyDown(KeyCode.E) && currentIndex != gears.Count - 1) currentGear = gears[currentIndex + 1];

        //gearText.text = "Gear: " + (gears.IndexOf(currentGear) + 1).ToString();
    }

    public void ControlCar()
    {
        float motor = currentGear.MaxMotorTorque * Input.GetAxis("Vertical");
        float steering = currentGear.MaxSteeringAngle * Input.GetAxis("Horizontal");


        //speedText.text = "Speed: " + rb.velocity.magnitude.ToString();

        if (rb.velocity.magnitude > currentGear.MinimumSpeed) motor *= 2.0f;
        else motor *= 0.25f;

        if (Input.GetKey(KeyCode.Space))
        { //if braking you cannot stear?!
            Brake();
            return;
        }

        for (int i = 0; i < axleInfos.Length; i++)
        {
            AxleInfo axle = axleInfos[i];

            //Adjust steering
            if (axle.Steering)
            {
                axle.LeftWheel.steerAngle = steering;
                axle.RightWheel.steerAngle = steering;
            }

            //Adjust motor power
            if (axle.Motor)
            {
                axle.LeftWheel.motorTorque = motor;
                axle.RightWheel.motorTorque = motor;
                axle.LeftWheel.brakeTorque = 0.0f;
                axle.RightWheel.brakeTorque = 0.0f;
            }

            ApplyLocalPositionsToVisuals(axle.LeftWheel);
            ApplyLocalPositionsToVisuals(axle.RightWheel);
        }
    }

    private void Brake()
    {
        for (int i = 0; i < axleInfos.Length; i++)
        {
            AxleInfo axle = axleInfos[i];

            if (axle.Motor)
            {
                axle.LeftWheel.motorTorque = 0.0f;
                axle.RightWheel.motorTorque = 0.0f;

                axle.LeftWheel.brakeTorque = brakespeed;
                axle.RightWheel.brakeTorque = brakespeed;
            }

            ApplyLocalPositionsToVisuals(axle.LeftWheel); //Don't think this should be here!
            ApplyLocalPositionsToVisuals(axle.RightWheel);
        }
    }

    private void ApplyLocalPositionsToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) return;

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        rotation.eulerAngles += visualWheelOffset;

        visualWheel.position = position;
        visualWheel.rotation = rotation;
    }
}

[System.Serializable]
public struct AxleInfo
{
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public bool Motor;
    public bool Steering;
}

[System.Serializable]
public struct GearInfo
{
    public float MaxMotorTorque;
    public float MaxSteeringAngle;
    public float MinimumSpeed;
}