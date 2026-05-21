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

    public async Task<(IEnumerable<Interview> Items, int TotalCount)> GetFilteredAsync(InterviewFilter filter)
    {
        var query = _context.Interviews
            .Include(x => x.Company)
            .Include(x => x.Recruiter)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(x => x.Status == filter.Status);

        if (filter.CompanyId.HasValue)
            query = query.Where(x => x.CompanyId == filter.CompanyId.Value);

        if (filter.RecruiterId.HasValue)
            query = query.Where(x => x.RecruiterId == filter.RecruiterId.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, totalCount);
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

    public async Task<bool> UpdateAsync(Interview interview)
    {
        _context.Interviews.Update(interview);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Interview interview)
    {
        _context.Interviews.Remove(interview);
        return await _context.SaveChangesAsync() > 0;
    }
}