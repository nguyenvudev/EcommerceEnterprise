using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EcommerceEnterprise.API.Hubs;

[Authorize]
public class OrderTrackingHub : Hub
{
    // Client gọi khi mở trang theo dõi đơn hàng
    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"order-{orderId}");
    }

    // Client gọi khi rời trang
    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            $"order-{orderId}");
    }
}