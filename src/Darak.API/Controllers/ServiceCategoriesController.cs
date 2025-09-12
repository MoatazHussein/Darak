using Darak.Application.Features.ServiceCategories.Commands.CreateServiceCategory;
using Darak.Application.Features.ServiceCategories.Commands.DeleteServiceCategory;
using Darak.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;
using Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;
using Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darak.API.Controllers
{
    [ApiController]
    [Route("api/serviceCategories")]
    //[Authorize]
    public class ServiceCategoriesController(IMediator mediator) : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            var newsItem = await mediator.Send(new GetServiceCategoryByIdQuery(id));

            return Ok(newsItem);
        }

        [AllowAnonymous]
        [HttpGet("GetAllMatching")]
        public async Task<IActionResult> GetAllMatching([FromQuery] GetAllServiceCategoriesQuery query)
        {
            var storeCategories = await mediator.Send(query);
            return Ok(storeCategories);
        }

        [HttpPost]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateServiceCategory(CreateServiceCategoryCommand command)
        {
            Guid id = await mediator.Send(command);
            return StatusCode(201, $"Added successfully with Id {id}");
        }


        [HttpPatch()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateServiceCategory(UpdateServiceCategoryCommand command)
        {
            await mediator.Send(command);
            return StatusCode(200,$"Updated successfully" );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteServiceCategory([FromRoute] Guid id)
        {
            await mediator.Send(new DeleteServiceCategoryCommand(id));

            //return NoContent();
            return StatusCode(200, $"Deleted successfully");

        }

    }


}
