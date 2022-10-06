using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Milestone.Models
{
    [HubName("postHub")]
    public class PostHub : Hub
    {
        
    }
}
