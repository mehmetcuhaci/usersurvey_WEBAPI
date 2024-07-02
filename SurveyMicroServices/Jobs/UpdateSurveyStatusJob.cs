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
            .Where(s => s.ExpiresAt < DateTime.UtcNow && s.Status == 1)
            .ToListAsync();

        foreach (var survey in expiredSurveys)
        {
            survey.Status = 0;
        }

        await _context.SaveChangesAsync();
    }
}
