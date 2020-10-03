using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Usable
{
    [Header("Button")]
    public GameObject m_ButtonPressObject;
    public float m_ButtonPressMovementSpeed = 10f;
    public Vector3 m_ButtonPressMovement;
    public float m_ButtonPressAngleSpeed = 150f;
    public Vector3 m_ButtonPressAngle;
    
    private Vector3 m_ButtonPressStartMovement;
    private Vector3 m_ButtonPressStartAngle;

    public override void Start()
    {
        base.Start();

        if (m_ButtonPressObject != null)
        {
            m_ButtonPressStartMovement = m_ButtonPressObject.transform.localPosition;
            m_ButtonPressStartAngle = m_ButtonPressObject.transform.localRotation.eulerAngles;
        }
    }

    public override void Update()
    {
        base.Update();

        this.UpdateButtonObject();
    }

    public override bool Activate()
    {
        base.Activate();

        return true;
    }

    private void UpdateButtonObject()
    {
        if (m_ButtonPressObject == null)
        {
            return;
        }

        if (m_ButtonPressMovement != Vector3.zero)
        {
            var targetPos = this.IsActive ? m_ButtonPressMovement : Vector3.zero;
            targetPos = m_ButtonPressStartMovement + targetPos;
            m_ButtonPressObject.transform.localPosition += (targetPos - m_ButtonPressObject.transform.localPosition) * Time.deltaTime * m_ButtonPressMovementSpeed;
        }

        if (m_ButtonPressAngle != Vector3.zero)
        {
            var targetAng = this.IsActive ? m_ButtonPressAngle : Vector3.zero;
            targetAng = m_ButtonPressStartAngle + targetAng;

            var localAng = m_ButtonPressObject.transform.localRotation.eulerAngles;
            var targetAngQ = Quaternion.Euler(targetAng.x, targetAng.y, targetAng.z);

            var newAng = Quaternion.RotateTowards(m_ButtonPressObject.transform.localRotation, targetAngQ, Time.deltaTime * m_ButtonPressAngleSpeed);

            m_ButtonPressObject.transform.localRotation = newAng;
        }
    }
}
