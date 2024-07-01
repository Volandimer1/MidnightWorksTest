using System.Threading.Tasks;

public interface IGameState
{
    Task EnterState();
    Task ExitState();
}