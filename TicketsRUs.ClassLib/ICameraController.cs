
namespace TicketsRUs.ClassLib;

public interface ICameraController
{
    Task<string> GetScanResultsAsync();
}