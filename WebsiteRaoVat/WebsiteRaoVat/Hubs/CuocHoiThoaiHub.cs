using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteRaoVat.Hubs
{
    public class CuocHoiThoaiHub:Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CuocHoiThoaiHub>();
            context.Clients.All.displayCuocHoiThoai();
        }
    }
}