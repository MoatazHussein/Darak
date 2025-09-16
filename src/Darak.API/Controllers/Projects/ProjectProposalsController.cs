using Darak.Application.Features.Projects.ProjectProposals.Commands.CreateProjectProposal;
using Darak.Application.Features.Projects.ProjectProposals.Commands.DeleteProjectProposal;
using Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposal;
using Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposalStatus;
using Darak.Application.Features.Projects.ProjectProposals.Queries.GetAllProjectProposals;
using Darak.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Darak.API.Controllers.Projects;

[ApiController]
[Authorize]
[Route("api/project-proposals")]
public class ProjectProposalsController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] GetAllProjectProposalsQuery query)
    {
        var projectProposals = await mediator.Send(query);
        return Ok(projectProposals);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Contractor)]
    public async Task<IActionResult> Create([FromBody] CreateProjectProposalCommand command)
    {
        var id = await mediator.Send(command);
        return Ok("Created Successfully");
    }

    [HttpPatch()]
    [Authorize(Roles = UserRoles.Contractor)]
    public async Task<IActionResult> UpdateProjectProposal(UpdateProjectProposalCommand command)
    {
        await mediator.Send(command);

        return StatusCode(200, $"Updated successfully");
    }

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateProjectProposalStatus(UpdateProjectProposalStatusCommand command)
    {
        await mediator.Send(command);

        return StatusCode(200, $"Updated successfully");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Contractor)]
    public async Task<IActionResult> DeleteProjectProposal([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteProjectProposalCommand(id));

        return StatusCode(200, $"Deleted successfully");
    }

}

