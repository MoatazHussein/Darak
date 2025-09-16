using Darak.Application.Features.Projects.ProjectRequests.Commands.CreateProjectRequest;
using Darak.Application.Features.Projects.ProjectRequests.Commands.DeleteProjectRequest;
using Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequest;
using Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequestStatus;
using Darak.Application.Features.Projects.ProjectRequests.Queries.GetAllProjectRequests;
using Darak.Application.Features.Projects.ProjectRequests.Queries.GetProjectRequestById;
using Darak.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darak.API.Controllers.Projects;

[ApiController]
[Authorize]
[Route("api/project-requests")]
public class ProjectRequestsController (IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetAllProjectRequestsQuery query)
    {
        var projectRequests = await mediator.Send(query);
        return Ok(projectRequests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var projectRequest = await mediator.Send(new GetProjectRequestByIdQuery(id));
        return Ok(projectRequest);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Client)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequestCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }


    [HttpPatch()]
    [Authorize(Roles = UserRoles.Client)]
    public async Task<IActionResult> UpdateProjectRequest(UpdateProjectRequestCommand command)
    {
        await mediator.Send(command);

        return StatusCode(200, $"Updated successfully");
    }

    [HttpPatch("status")]
    [Authorize(Roles = UserRoles.Client)]
    public async Task<IActionResult> UpdateProjectRequestStatus(UpdateProjectRequestStatusCommand command)
    {
        await mediator.Send(command);

        return StatusCode(200, $"Updated successfully");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Client)]
    public async Task<IActionResult> DeleteProjectRequest([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteProjectRequestCommand(id));

        return StatusCode(200, $"Deleted successfully");
    }

}

