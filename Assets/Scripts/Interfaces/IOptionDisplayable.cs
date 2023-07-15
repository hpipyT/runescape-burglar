using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOptionDisplayable
{
    public Dictionary<string, System.Action> GetActionsToDisplay();

}
