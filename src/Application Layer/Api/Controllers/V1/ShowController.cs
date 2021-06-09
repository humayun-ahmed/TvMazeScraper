using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Validator.Contract;
using Rtl.TvMaze.Api.Dto;
using Rtl.TvMaze.Scraper.Service.Contracts;

namespace Rtl.TvMaze.Api.Controllers.V1
{
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowQueryService m_showQueryService;
        private readonly IValidator<ShowRequest> m_validator;

        public ShowController(IShowQueryService showQueryService, IValidator<ShowRequest> validator)
        {
            m_showQueryService = showQueryService;
            m_validator = validator;
        }
        [HttpGet]
        [Route(ApiRoutes.Shows)]
        public async Task<IActionResult> Shows([FromQuery] ShowRequest request)
        {
            var validationResult = m_validator.PerformValidation(request);

            if (validationResult.IsValid)
            {
                var showRequest = new Scraper.Service.Contracts.DTO.ShowRequest { PageNo = request.PageNo, PageSize = request.PageSize };
                var result = await m_showQueryService.GetShows(showRequest);
                return this.Ok(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }
    }
}
