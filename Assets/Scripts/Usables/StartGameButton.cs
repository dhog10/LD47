using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : Button
{
    public override bool Activate()
    {
        GameManager.Instance.StartGame(true);

        return base.Activate();
    }
}
