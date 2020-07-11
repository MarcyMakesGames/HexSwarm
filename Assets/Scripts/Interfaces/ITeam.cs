﻿using UnityEngine;
using System.Collections.Generic;

public interface ITeam
{
    string Name { get; }
    string Discription { get; }
    Sprite Icon { get; }
    Color Color { get; }
    Teams Type { get; }
    int RemainingUnits { get; }

    IGameManager Game { get; }
    IEnumerable<IUnit> Units { get;}
    
    bool HasUnitsAfterLosses(IEnumerable<IUnit> losses);
    bool hasMove { get; }
    void StartTurn();
    bool Undo();
    bool EndTurn();

    IEnumerable<Vector3Int> HighlightMove { get; }
    IEnumerable<Vector3Int> HighlightAttack { get; }
    IEnumerable<Vector3Int> HighlightOverlap { get; }

    bool DoMove(IUnit unit, IPosAbilityDefault ablity, Vector3Int target);
    void ResolveHighlight(IUnit unit, IPosAbilityDefault ablity, Vector3Int target);
}

