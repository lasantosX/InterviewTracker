using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using InterviewTracker.Data.Repositories;

namespace InterviewTracker.Data.Repositories;

public class InterviewRepository : IInterviewRepository
{
    private readonly InterviewTrackerDbContext _context;

    public InterviewRepository(InterviewTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interview>> GetAllAsync()
    {
        return await _context.Interviews
            .Include(x => x.Company)
            .Include(x => x.Recruiter)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Interview?> GetByIdAsync(int id)
    {
        return await _context.Interviews
            .Include(x => x.Company)
            .Include(x => x.Recruiter)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Interview> AddAsync(Interview interview)
    {
        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return interview;
    }

    public async Task<bool> CompanyExistsAsync(int companyId)
    {
        return await _context.Companies.AnyAsync(x => x.Id == companyId);
    }

    public async Task<bool> RecruiterExistsAsync(int recruiterId)
    {
        return await _context.Recruiters.AnyAsync(x => x.Id == recruiterId);
    }
}