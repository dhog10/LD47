using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Usable
{

    public override bool Activate()
    {
        var player = TankCharacterController.Instance;
        if (player.Holdable != null && player.Holdable is Key)
        {
            GameObject.Destroy(gameObject);
            GameObject.Destroy(player.Holdable.gameObject);

            return base.Activate();
        }

        return false;
    }
}
