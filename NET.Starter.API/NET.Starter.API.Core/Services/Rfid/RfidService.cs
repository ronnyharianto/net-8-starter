using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.API.Core.Bases;
using NET.Starter.API.DataAccess;
using NET.Starter.API.Shared.Enums;
using NET.Starter.API.Shared.Objects.Dtos;
using NET.Starter.SDK;
using NET.Starter.SDK.Interfaces;

namespace NET.Starter.API.Core.Services.Rfid
{
    public class RfidService (ApplicationDbContext dbContext, IMapper mapper, ILogger<RfidService> logger, RfidFixedReaderFactory rfidFixedReaderFactory)
        : BaseService<RfidService>(dbContext, mapper, logger)
    {
        private readonly RfidFixedReaderFactory _rfidFixedReaderFactory = rfidFixedReaderFactory;

        public ResponseObject<string> ReadTag(string readerType)
        {
            var rfidReader = _rfidFixedReaderFactory.CreateRfidFixedReader(readerType);

            return new ResponseObject<string>(responseCode: ResponseCode.Ok)
            {
                Obj = rfidReader.ReadTag()
            };
        }
    }
}
