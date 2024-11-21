using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.SDK;
using NET.Starter.SDK.Enums;

namespace NET.Starter.Core.Services.Rfid
{
    public class RfidService (ApplicationDbContext dbContext, IMapper mapper, ILogger<RfidService> logger, RfidFixedReaderFactory rfidFixedReaderFactory)
        : BaseService<RfidService>(dbContext, mapper, logger)
    {
        private readonly RfidFixedReaderFactory _rfidFixedReaderFactory = rfidFixedReaderFactory;

        public ResponseObject<string> ReadTag(ReaderType readerType)
        {
            var rfidReader = _rfidFixedReaderFactory.CreateRfidFixedReader(readerType);

            return new ResponseObject<string>(responseCode: ResponseCode.Ok)
            {
                Obj = rfidReader.ReadTag()
            };
        }
    }
}
