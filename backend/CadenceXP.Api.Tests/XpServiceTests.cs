using CadenceXP.Api.Services.Xp;

public class XpServiceTests
{
    [Fact]
    public void CalculateLevel_WithZeroXp_ReturnsLevelOne()
    {
        var xpService = new XpService();
        int level = xpService.CalculateLevel(0);
        Assert.Equal(1, level);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(99, 1)]
    [InlineData(100, 2)]
    [InlineData(249, 2)]
    [InlineData(250, 3)]
    [InlineData(63700, 50)]
    [InlineData(64000, 50)]
    public void CalculateLevel_WithDifferentXp_ReturnsExpectedLevel(
        int totalXp,
        int expectedLevel)
    {
        var xpService = new XpService();
        int level = xpService.CalculateLevel(totalXp);
        Assert.Equal(expectedLevel, level);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(99, 99)]
    [InlineData(100, 0)]
    [InlineData(150, 50)]
    [InlineData(249, 149)]
    [InlineData(250, 0)]
    [InlineData(252, 2)]
    [InlineData(63700, 0)]
    [InlineData(64000, 300)]
    public void CalculateXpIntoLevel_WithDifferentXp_ReturnsExpectedXp(
        int totalXp,
        int expectedXpIntoLevel)
    {
        var xpService = new XpService();
        int xpIntoLevel = xpService.CalculateXpIntoLevel(totalXp);
        Assert.Equal(expectedXpIntoLevel, xpIntoLevel);
    }

    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 150)]
    [InlineData(3, 200)]
    [InlineData(49, 2500)]
    [InlineData(50, 0)]
    public void CalculateXpForNextLevel_WithDifferentLevels_ReturnsExpectedXp(
        int level,
        int expectedXp)
    {
        var xpService = new XpService();
        int xpForNextLevel = xpService.CalculateXpForNextLevel(level);
        Assert.Equal(expectedXp, xpForNextLevel);
    }

    [Fact]
    public void CalculateRideXp_ZeroDistanceAndElevation_ReturnsBaseXp()
    {
        var xpService = new XpService();
        int rideXp = xpService.CalculateRideXp(0, 0);
        Assert.Equal(25, rideXp);
    }

    [Fact]
    public void CalculateRideXp_WithDistanceAndElevation_ReturnsExpectedXp()
    {
        var xpService = new XpService();
        int rideXp = xpService.CalculateRideXp(
            107.745852,
            5
        );
        Assert.Equal(26, rideXp);
    }





}