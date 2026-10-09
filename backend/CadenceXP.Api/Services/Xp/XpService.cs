namespace CadenceXP.Api.Services.Xp;

public class XpService
{
    public int CalculateRideXp(
        double distanceMeters,
        double elevationGainMeters)
    {
        double distanceMiles = distanceMeters / 1609.344;

        int baseXp = 25;
        double distanceXp = distanceMiles * 10;
        double elevationXp = elevationGainMeters / 10;

        int rideTotalXp = (int)Math.Floor(baseXp + distanceXp + elevationXp);

        return rideTotalXp;
    }

    public int CalculateLevel(int totalXp)
    {
        int level = 1;
        int xpRemaining = totalXp;

        while (level < 50)
        {
            int xpRequired = 100 + ((level - 1) * 50);
            if (xpRemaining < xpRequired)
            {
                return level;
            }
            xpRemaining -= xpRequired;
            level++;
        }
        return level;
    }
}