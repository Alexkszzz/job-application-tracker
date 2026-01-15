using JobTracker.Api.DTOs.Applications;
using JobTracker.Api.Mapping;
using JobTracker.Api.Models;
using JobTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    // GET: api/applications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetApplications()
    {
        var userId = GetUserId();
        var applications = await _applicationService.GetAllApplicationsAsync(userId);
        var response = applications.Select(a => a.ToResponseDto());
        return Ok(response);
    }

    // GET: api/applications/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationResponseDto>> GetApplication(Guid id)
    {
        var userId = GetUserId();
        var application = await _applicationService.GetApplicationByIdAsync(id, userId);

        if (application == null)
        {
            return NotFound();
        }

        return Ok(application.ToResponseDto());
    }

    // POST: api/applications
    [HttpPost]
    public async Task<ActionResult<ApplicationResponseDto>> CreateApplication(CreateApplicationRequest request)
    {
        var userId = GetUserId();
        var application = request.ToEntity(userId);
        var createdApplication = await _applicationService.CreateApplicationAsync(application);

        return CreatedAtAction(
            nameof(GetApplication),
            new { id = createdApplication.Id },
            createdApplication.ToResponseDto());
    }

    // PUT: api/applications/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApplication(Guid id, ApplicationUpdateDto updateDto)
    {
        var userId = GetUserId();
        var application = await _applicationService.GetApplicationByIdAsync(id, userId);
        if (application == null)
        {
            return NotFound();
        }

        application.UpdateEntity(updateDto);
        var success = await _applicationService.UpdateApplicationAsync(id, application, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/applications/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApplication(Guid id)
    {
        var userId = GetUserId();
        var success = await _applicationService.DeleteApplicationAsync(id, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
    }
}
