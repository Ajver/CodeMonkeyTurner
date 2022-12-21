using System;

public static class AIBrain
{

    // Calculates AI action value for the `unit` opening the `door` 
    public static int GetOpenDoorActionValue(Door door, Unit unit)
    {
        if (door.IsOpen())
        {
            const int CLOSING_DOOR_VALUE = 0;
            return CLOSING_DOOR_VALUE;
        }
        
        int otherTeamUnitsInOtherArea = 0;
        
        foreach (LevelArea area in door.GetLevelAreasConnected())
        {
            if (unit.IsOccupyingLevelArea(area))
            {
                // Unit already is in this area. We don't care about it
                continue;
            }

            foreach (Unit otherUnit in area.GetUnitsList())
            {
                if (otherUnit.GetGameTeam() != unit.GetGameTeam())
                {
                    otherTeamUnitsInOtherArea++;
                }
            }
        }

        const int VALUE_PER_UNIT = 30;
        const int MAX_ACTION_VALUE = 90;

        int actionValue = otherTeamUnitsInOtherArea * VALUE_PER_UNIT;
        actionValue = Math.Min(actionValue, MAX_ACTION_VALUE);
        
        return actionValue;
    }
    
}
