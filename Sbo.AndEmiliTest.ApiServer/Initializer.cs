using Sbo.AndEmiliTest.Core;

namespace Sbo.AndEmiliTest.ApiServer;

public class Initializer : BackgroundService
{
    private readonly BallDontLieServices ballDontLieServices;

    public Initializer(BallDontLieServices ballDontLieServices)
    {
        this.ballDontLieServices = ballDontLieServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ballDontLieServices.InitializePlayersAndStats(stoppingToken);
    }
}
