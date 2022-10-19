using Grpc.Core;
using serverInfo;

using System;
using System.IO;


namespace serverInfo.Services;

public class ServerStatusService : ServerStatus.ServerStatusBase
{
    private readonly ILogger<ServerStatusService> _logger;
    public ServerStatusService(ILogger<ServerStatusService> logger)
    {
        _logger = logger;
    }

    public override Task<GetStatusResponse> GetStatus(GetStatusRequest request, ServerCallContext context)
    {

        DriveInfo[] drives = DriveInfo.GetDrives();
        string diskSpace = "";
        List<Disk> disks = new List<Disk>();
        foreach (DriveInfo drive in drives) {
            //There are more attributes you can use.
            //Check the MSDN link for a complete example.
            diskSpace = diskSpace + "Disco: " + drive.Name;
            // tenes AvaibvlableFreeSpace, TotalFreeSpace, TotalSize, Name

            if (drive.IsReady)
                disks.Add(new Disk {
                    Name = drive.Name,
                    AvailableFreeSpace = drive.AvailableFreeSpace,
                    TotalFreeSpace = drive.TotalFreeSpace,
                    TotalSize = drive.TotalSize
                }
            );
        }


        return Task.FromResult(new GetStatusResponse
        {
            Message = "saludos:" + request.Name + " El servicio " + context.Host + " está funcionando correctamente",
            Disks = { disks }
        });
    }


}
