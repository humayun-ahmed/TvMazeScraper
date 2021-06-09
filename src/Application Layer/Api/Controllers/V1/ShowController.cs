using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repository.Contracts.Filter;
using Rtl.TvMaze.Scraper.Service.Contracts;

namespace Rtl.TvMaze.Api.Controllers.V1
{
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowQueryService m_showQueryService;

        public ShowController(IShowQueryService showQueryService)
        {
            m_showQueryService = showQueryService;
        }
        [HttpGet]
        [Route(ApiRoutes.Shows)]
        public async Task<IActionResult> Shows([FromQuery] FilterRequest filter)
        {
            var result = await m_showQueryService.GetShows(filter);
            return this.Ok(result);
        }
    }
}
