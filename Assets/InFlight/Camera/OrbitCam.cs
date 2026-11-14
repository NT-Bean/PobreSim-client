using System.Threading;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{
    [SerializeField] private GameObject leaderObj;
    
    private Transform leaderTransform;

    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private float arrowSensitivity = 100.0f;

    [SerializeField] private float orbitRadius = 5.0f;

    private float pitch;
    private float yaw;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leaderTransform = leaderObj.transform;
        transform.position = leaderTransform.position + new Vector3(0.0f, 0.0f, -orbitRadius);

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch += -Input.GetAxis("Mouse Y") * sensitivity;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            yaw += arrowSensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            yaw -= arrowSensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            pitch += arrowSensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            pitch -= arrowSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        orbitRadius = Mathf.Max(orbitRadius - (Input.mouseScrollDelta.y * (orbitRadius / 10.0f)), 0.1f);

        transform.position = leaderTransform.position - transform.forward * orbitRadius;
    }
}
