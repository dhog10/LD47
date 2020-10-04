using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door")]
    [HideInInspector]
    public GameObject m_DoorObject;
    public float m_DoorSpeed = 10f;
    public Vector3 m_DoorMovement;
    public float m_DoorAngleSpeed = 150f;
    public Vector3 m_DoorMoveAngle;

    private Vector3 m_DoorPressStartMovement;
    private Vector3 m_DoorPressStartAngle;

    private bool m_IsOpen;

    public bool IsOpen
        => m_IsOpen;

    public void Start()
    {
        m_DoorObject = gameObject;

        if (m_DoorObject != null)
        {
            m_DoorPressStartMovement = m_DoorObject.transform.localPosition;
            m_DoorPressStartAngle = m_DoorObject.transform.localRotation.eulerAngles;
        }
    }

    public void Update()
    {
        this.UpdateButtonObject();
    }

    public void Open()
    {
        m_IsOpen = true;
    }

    public void Close()
    {
        m_IsOpen = false;
    }

    private void UpdateButtonObject()
    {
        if (m_DoorObject == null)
        {
            return;
        }

        if (m_DoorMovement != Vector3.zero)
        {
            var targetPos = this.IsOpen ? m_DoorMovement : Vector3.zero;
            targetPos = m_DoorPressStartMovement + targetPos;
            m_DoorObject.transform.localPosition += (targetPos - m_DoorObject.transform.localPosition) * Time.deltaTime * m_DoorSpeed;
        }

        if (m_DoorMoveAngle != Vector3.zero)
        {
            var targetAng = this.IsOpen ? m_DoorMoveAngle : Vector3.zero;
            targetAng = m_DoorPressStartAngle + targetAng;

            var localAng = m_DoorObject.transform.localRotation.eulerAngles;
            var targetAngQ = Quaternion.Euler(targetAng.x, targetAng.y, targetAng.z);

            var newAng = Quaternion.RotateTowards(m_DoorObject.transform.localRotation, targetAngQ, Time.deltaTime * m_DoorAngleSpeed);

            m_DoorObject.transform.localRotation = newAng;
        }
    }
}
