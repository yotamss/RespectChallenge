// using System.Security.Cryptography;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Respect.Server.Data;
// using Respect.Server.Models;
//
// namespace Respect.Server.Controllers;
//
// /// <summary>
// /// Manages user accounts
// /// </summary>
// [ApiController]
// [Route("[controller]")]
// public class SeedsController : ControllerBase
// {
//     private readonly UserManager<ApplicationUser> userManager;
//     private readonly AuthenticationContext authenticationContext;
//     private readonly PetitionContext petitionContext;
//
//     List<string> firstNames = new()
//     {
//         "Emma", "Olivia", "Ava", "Isabella", "Sophia", "Mia", "Charlotte", "Amelia", "Harper", "Evelyn",
//         "Abigail", "Emily", "Elizabeth", "Avery", "Sofia", "Ella", "Madison", "Aurora", "Scarlett",
//         "Victoria", "Riley", "Aria", "Liliana", "Adalynn", "Aaliyah", "Natalie", "Camila", "Aubree", "Aurora",
//         "Avery", "Brooklyn", "Bella", "Blue Ivy", "Rylee", "Adalyn", "Adalynn", "Aria", "Arianna", "Aubree",
//         "Aubrey", "Avery", "Brooklyn", "Bella", "Cora", "Evelyn", "Hazel", "Isabelle", "Natalie", "Paige"
//     };
//
//     List<string> lastNames = new()
//     {
//         "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
//         "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Young", "Allen", "King",
//         "Wright", "Scott", "Green", "Baker", "Adams", "Nelson", "Carter", "Mitchell", "Perez", "Roberts",
//         "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez",
//         "Morris",
//         "Rogers", "Reed", "Cook", "Bailey", "Bell", "Cooper", "Richardson", "Cox", "Howard", "Ward"
//     };
//
//     Random random = new();
//
//
//     public SeedsController(UserManager<ApplicationUser> userManager, AuthenticationContext authenticationContext,
//         PetitionContext petitionContext)
//     {
//         this.userManager = userManager;
//         this.authenticationContext = authenticationContext;
//         this.petitionContext = petitionContext;
//     }
//
//     /// <summary>
//     /// Registers a new user
//     /// </summary>
//     /// <param name="request">The registration request</param>
//     [HttpGet]
//     public async Task<IActionResult> SeedAsync()
//     {
//         await CreatePetitionStuff("ryangreen@somedomain.com",
//             "Ban Plastic Straws Nationwide: Protect Our Oceans and Marine Life",
//             "Plastic straws are one of the biggest contributors to ocean pollution and harm marine life. This petition calls for a nationwide ban on single-use plastic straws to help reduce waste and protect the environment. By signing this petition, you are supporting the fight against plastic pollution and encouraging the government to take action to protect our oceans and the animals that call them home.",
//             250, 70);
//
//         await CreatePetitionStuff("joybrown@somedomain.com",
//             "Save Our Public Schools: Increase Funding and Support for Education",
//             "Public schools are the cornerstone of our communities and play a vital role in shaping the future of our children. This petition calls for increased funding and support for public schools to ensure that every student has access to quality education. By signing this petition, you are supporting the future of our children and advocating for a brighter tomorrow through education.",
//             100, 82);
//
//         await CreatePetitionStuff("aliceperr@somedomain.com",
//             "Mandatory Napping Time for Office Workers",
//             "The average office worker spends hours sitting at a desk, staring at a computer screen. It's no secret that this can be tiring. To combat this, we propose mandatory napping time in the office. A 20-minute nap can do wonders for productivity and overall well-being. Let's make this happen!",
//             400, 673);
//
//         return Ok();
//     }
//
//     private async Task CreatePetitionStuff(string email, string petitionName, string petitionDescription,
//         int targetVotes, int votesAmount)
//     {
//         var user = new ApplicationUser { Email = email, UserName = Guid.NewGuid().ToString() };
//         await userManager.CreateAsync(user, password: Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
//         var petition = new Petition(user, petitionName,
//             petitionDescription,
//             targetVotes);
//
//         await petitionContext.Petitions.AddAsync(petition);
//
//         await petitionContext.SaveChangesAsync();
//
//         var votes = new List<Vote>();
//
//         for (var i = 0; i < votesAmount; i++)
//         {
//             var vote = new Vote(petition, firstNames[random.Next(0, firstNames.Count - 1)],
//                 lastNames[random.Next(0, lastNames.Count - 1)], "12376669090" + i);
//             votes.Add(vote);
//         }
//
//         await petitionContext.Votes.AddRangeAsync(votes);
//         await petitionContext.SaveChangesAsync();
//     }
//
//     public record RegistrationRequest(string Email, string Password);
// }