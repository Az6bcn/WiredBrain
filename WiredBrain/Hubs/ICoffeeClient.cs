using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiredBrain.Models;

namespace WiredBrain.Hubs
{
    // methods callable on theclient. The clients must implements this methods.
    public interface ICoffeeClient
    {
        Task NewOrder(Order order);
        Task ReceiveOrderUpdate(UpdateInfo updateInfo);
        Task Finished(Order order);
    }
}
