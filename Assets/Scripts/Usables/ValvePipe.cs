using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValvePipe : Button
{
    private ValveHandle m_ValveHandle;
    private bool m_Sealed;

    public bool Sealed
        => m_Sealed;

    public bool HasHandle
        => m_ValveHandle != null;

    public override void Update()
    {
        base.Update();

        var player = TankCharacterController.Instance;
        if (m_ValveHandle != null && m_ValveHandle == player.Holdable)
        {
            m_ValveHandle = null;
        }
    }

    public override bool Activate()
    {
        var player = TankCharacterController.Instance;
        if (player == null)
        {
            return false;
        }

        if (m_ValveHandle != null)
        {
            base.Activate();
            return true;
        }

        var heldValve = player.Holdable;
        if (heldValve == null || !(heldValve is ValveHandle))
        {
            return false;
        }

        player.Drop();

        m_ValveHandle = (ValveHandle)heldValve;
        m_ValveHandle.Freeze();
        m_ValveHandle.transform.position = transform.position;
        m_ValveHandle.transform.rotation = transform.rotation;
        m_ValveHandle.transform.SetParent(m_ButtonPressObject.transform);

        return true;
    }

    public override void Deactivate()
    {
        var player = TankCharacterController.Instance;
        if (player == null)
        {
            return;
        }

        if (m_ValveHandle == null)
        {
            return;
        }

        base.Deactivate();
    }
}
