using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiredBrain.Helpers;
using WiredBrain.Models;

namespace WiredBrain.Hubs
{
    // methods in the hub are meant for clients to call
    public class CoffeeHubs: Hub<ICoffeeClient>
    {
        // to get order updates
        private static readonly OrderChecker _orderChecker = new OrderChecker(new Random());

        public async Task GetUpdateForOrder(Order order)
        {
            // call function on client i.e on all clients except for who originated the call to this hub method
            await Clients.Others.NewOrder(order);
            UpdateInfo result;

            // checks if there's a new update every 700ms
            do
            {
                // checks if there's an update on the order
                result = _orderChecker.GetUpdate(order);
                await Task.Delay(700);
                if (!result.New) continue;

                // if there's an update calls the callers client method ReceiveOrderUpdate.
                await Clients.Caller.ReceiveOrderUpdate(result);

                // notify all member of the group of updates
                await Clients.Group("allUpdatesReceivers").ReceiveOrderUpdate(result);

            } while (!result.Finished);
            // when is finished calls the callers client method finish.
            await Clients.Caller.Finished(order);
        }

        public override Task OnConnectedAsync()
        {
            if(Context.GetHttpContext().Request.QueryString.HasValue.Equals("allUpdates"))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "allUpdatesReceivers");
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
