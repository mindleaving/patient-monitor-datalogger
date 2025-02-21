using Microsoft.AspNetCore.Mvc;

namespace PatientMonitorDataLogger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
}