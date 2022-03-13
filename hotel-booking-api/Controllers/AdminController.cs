using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet("{managerId}/transaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> GetHotelManagerTransactions([FromRoute] string managerId, [FromQuery] TransactionFilter filter)
        {
            _logger.LogInformation($"Retrieveing Getting Hotel Manager Transaction for {managerId}");

            var result = await _adminService.GetManagerTransactionsAsync(managerId, filter);

            _logger.LogInformation($"Retrieved Hotel Manager Transaction for {managerId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("transactions")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Response<PageResult<IEnumerable<TransactionResponseDto>>>>> GetAllTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await _adminService.GetAllTransactions(filter);
            return Ok(response);
        }

    }
}


