
namespace Inventory.Model
{
    public record class PopupDateModel(long From, long To, bool MoreData, Guid[] DriverId)
    {
    }
}
