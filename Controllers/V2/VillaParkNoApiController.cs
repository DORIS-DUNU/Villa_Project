using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using VilllaParks.Core.IRepository;
using VilllaParks.Model;
using VilllaParks.Model.Dto;

namespace VilllaParks.Controllers.V2
{
    [Route("api/v{version:apiVersion}/VillaParkNumberApi")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaParkNoApiController : ControllerBase
    {
        private readonly IVillaNumberRepository _villaParkNo;
        private readonly IVillaParkRepository _dbvillaPark;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly ILogger<VillaParkNoApiController> _logger;

        public VillaParkNoApiController(IVillaNumberRepository villaParkNo, IMapper mapper, ILogger<VillaParkNoApiController> logger, IVillaParkRepository dbvillaPark)
        {
            _villaParkNo = villaParkNo;
            _mapper = mapper;
            _response = new();
            _logger = logger;
            _dbvillaPark = dbvillaPark;

        }
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "string2" };
        }
    }
}