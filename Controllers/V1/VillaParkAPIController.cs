using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using System.Text.Json;
using VilllaParks.Core.IRepository;
using VilllaParks.Model;
using VilllaParks.Model.Dto;


namespace VilllaParks.Controllers.V1
{
    //Api attribute
    [Route("api/v{version:apiVersion}/VillaPark")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaParkAPIController : ControllerBase
    {
        private readonly IVillaParkRepository _villaPark;
        private readonly IMapper _mapper;
        private readonly ILogger<VillaParkAPIController> _logger;
        protected APIResponse _response;

        public VillaParkAPIController(IVillaParkRepository villaPark, IMapper mapper, ILogger<VillaParkAPIController> logger)
        {
            _villaPark = villaPark;
            _mapper = mapper;
            _response = new();
            _logger = logger;
        }
        [HttpGet("GetVillaParks")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetVillaParks([FromQuery(Name="filterOccupancy")]int? occupancy, [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<VillaPark> villaList;
                if(occupancy > 0)
                {
                    villaList = await _villaPark.GetAllAsync(u => u.Occupancy == occupancy , pageSize:pageSize,
                        pageNumber:pageNumber);
                }
                else
                {
                    villaList = await _villaPark.GetAllAsync(pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(u => u.Name.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize};
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                //Get All VillaPArks
                _logger.LogInformation("Getting all ParkVillas");
              //  IEnumerable<VillaPark> ParkList = await _villaPark.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaParkDto>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch the villas");
                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaParks"),]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetVillaParks(int id)
        {
            try
            {

                if (id == 0)
                {
                    _logger.LogError("Get Villa Error with Id " + id, "Error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaParkDto = await _villaPark.GetAsync(v => v.Id == id);
                if (villaParkDto == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                _response.Result = _mapper.Map<VillaParkDto>(villaParkDto);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("CreateVillaPark")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVillaPark(VillaParkCreatedDTO createDTO)
        {
            try
            {
                if (await _villaPark.GetAsync(v => v.Name.ToLower() ==
            createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaPark  Already Exits!");
                    return BadRequest(ModelState);
                }
                //Check to see if the VillaPark is null
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                VillaPark villaPark = _mapper.Map<VillaPark>(createDTO);
                await _villaPark.CreateAsync(villaPark);
                _response.Result = _mapper.Map<VillaPark>(villaPark);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaParks", new { id = villaPark.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaPark(int id,  VillaParkUpdatedDTO updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //}
                VillaPark model = _mapper.Map<VillaPark>(updateDto);

                await _villaPark.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}", Name = "UpdateParkVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParkVilla(int id, JsonPatchDocument<VillaParkUpdatedDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villaPark = await _villaPark.GetAsync(n => n.Id == id, tracked: false);

            VillaParkUpdatedDTO villaParkUpdateDTO = _mapper.Map<VillaParkUpdatedDTO>(villaPark);

            if (villaPark == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaParkUpdateDTO, ModelState);
            VillaPark model = _mapper.Map<VillaPark>(villaParkUpdateDTO);
            await _villaPark.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaPark = await _villaPark.GetAsync(x => x.Id == id);
                if (villaPark == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                await _villaPark.RemoveAsync(villaPark);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }



    }

}

