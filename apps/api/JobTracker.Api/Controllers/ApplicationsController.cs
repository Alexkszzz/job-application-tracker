using JobTracker.Api.DTOs.Applications;
using JobTracker.Api.Mapping;
using JobTracker.Api.Models;
using JobTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var applications = await _applicationService.GetAllApplicationsAsync();
        var response = applications.Select(a => a.ToResponseDto());
        return Ok(response);
    }

    // GET: api/applications/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationResponseDto>> GetApplication(Guid id)
    {
        var application = await _applicationService.GetApplicationByIdAsync(id);

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
        var application = request.ToEntity();
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
        var application = await _applicationService.GetApplicationByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        application.UpdateEntity(updateDto);
        var success = await _applicationService.UpdateApplicationAsync(id, application);

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
        var success = await _applicationService.DeleteApplicationAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
