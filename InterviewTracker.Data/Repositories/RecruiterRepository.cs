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

    public async Task<(IEnumerable<Recruiter> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Recruiters
            .OrderBy(x => x.FullName);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
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

    public async Task<bool> UpdateAsync(Recruiter recruiter)
    {
        _context.Recruiters.Update(recruiter);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Recruiter recruiter)
    {
        _context.Recruiters.Remove(recruiter);
        return await _context.SaveChangesAsync() > 0;
    }
}