using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Respect.Server.Data;
using Respect.Server.Models;

namespace Respect.Server.Controllers;

/// <summary>
/// Phone emulator
/// </summary>
[ApiController]
[Route("[controller]")]
public class MyPhoneEmulatorController : ControllerBase
{
    private readonly PetitionContext petitionContext;

    const string PhoneNumber = "3155854634"; // hardcoded!

    public MyPhoneEmulatorController(PetitionContext petitionContext)
    {
        this.petitionContext = petitionContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sms>>> GetSmsMessages()
    {
        return Ok(await petitionContext.SmsSet
            .Where(s => s.PhoneNumber == PhoneNumber)
            .OrderBy(s => s.Id)
            .ToListAsync());
    }
}