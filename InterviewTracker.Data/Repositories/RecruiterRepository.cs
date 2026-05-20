using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Data.Repositories;

public class RecruiterRepository : IRecruiterRepository
{
    private readonly InterviewTrackerDbContext _context;

    public RecruiterRepository(InterviewTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recruiter>> GetAllAsync()
    {
        return await _context.Recruiters
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<Recruiter?> GetByIdAsync(int id)
    {
        return await _context.Recruiters.FindAsync(id);
    }

    public async Task<Recruiter> AddAsync(Recruiter recruiter)
    {
        _context.Recruiters.Add(recruiter);
        await _context.SaveChangesAsync();

        return recruiter;
    }
}