using UnityEngine;

public class TestRocketSteer : MonoBehaviour
{

    [SerializeField] float torque = 1.0f;
    private Rigidbody rb;
    private bool stabilityAssistOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            stabilityAssistOn = !stabilityAssistOn;
        }
    }


    void FixedUpdate()
    {
        ApplySteerInputs();
        ApplyStabilityAssist();
    }

    void ApplyStabilityAssist()
    {
        if(stabilityAssistOn)
        {
            rb.AddTorque((-rb.angularVelocity).normalized * Mathf.Min(torque, rb.angularVelocity.magnitude));
        }
    }

    void ApplySteerInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeTorque(Vector3.right * torque);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeTorque(Vector3.right * -torque);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeTorque(Vector3.forward * torque);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeTorque(Vector3.forward * -torque);
        }
        if (Input.GetKey(KeyCode.E))
        {
            rb.AddRelativeTorque(Vector3.up * torque);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddRelativeTorque(Vector3.up * -torque);
        }
    }
}
