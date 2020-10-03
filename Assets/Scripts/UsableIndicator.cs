using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableIndicator : Indicator
{
    public Usable m_Usable;

    public override bool IsOn()
    {
        return m_Usable.IsActive;
    }

    public override bool IsDisabled()
    {
        return m_Usable.IsDisabled();
    }
}
