using System.Diagnostics;
using ElderEatsAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ElderEatsAPI.Controllers;

[Route("api/v2/[controller]")]
public class DeployController : ControllerBase
{
    [HttpPost]
    public IActionResult DeployReact([FromBody] DeployRequest deployRequest)
    {
        if (deployRequest.secret == null)
        {
            ModelState.AddModelError("", "Secret is required");

            return StatusCode(500, ModelState);
        }
        
        Deploy();

        return Ok();
    }

    private static void Deploy()
    {
        const string projectPath = "/var/www/toepenreact";
        const string npmCommand = "npm";
        const string npmArguments = "run build";

        ProcessStartInfo psi = new()
        {
            FileName = npmCommand,
            Arguments = npmArguments,
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new();
        process.StartInfo = psi;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine("Output:\n" + output);
        Console.WriteLine("Error:\n" + error);
        Console.WriteLine("Exit Code: " + process.ExitCode);
    }
}