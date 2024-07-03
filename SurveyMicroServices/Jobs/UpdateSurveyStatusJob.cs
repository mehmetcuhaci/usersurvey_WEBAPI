using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices;

public class UpdateSurveyStatusJob : IJob
{
    private readonly ApplicationDbContext _context;

    public UpdateSurveyStatusJob(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var expiredSurveys = await _context.Surveys
            .Where(s => s.ExpiresAt < DateTime.UtcNow && s.Status == true)
            .ToListAsync();

        foreach (var survey in expiredSurveys)
        {
            survey.Status = false;
        }

        await _context.SaveChangesAsync();
    }
}
