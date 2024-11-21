using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.SDK;
using NET.Starter.SDK.Enums;
using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;

namespace NET.Starter.Core.Services.Rfid
{
    public class RfidService (ApplicationDbContext dbContext, IMapper mapper, ILogger<RfidService> logger, RfidFixedReaderFactory rfidFixedReaderFactory)
        : BaseService<RfidService>(dbContext, mapper, logger)
    {
        private readonly RfidFixedReaderFactory _rfidFixedReaderFactory = rfidFixedReaderFactory;

        public ResponseObject<ConnectedInfoDto> Connect(ConnectInput input)
        {
            // TODO: Change it to real implementation
            var readerType = ReaderType.Zebra;
            var rfidReader = _rfidFixedReaderFactory.CreateRfidFixedReader(readerType);

            rfidReader.Listen(new ListenInput
            {
                BackLog = 2,
                HostName = "127.0.0.1",
                Port = 5084
            });

            return new ResponseObject<ConnectedInfoDto>(responseCode: ResponseCode.Ok)
            {
                Obj = rfidReader.Connect(input)
            };
        }
    }
}
