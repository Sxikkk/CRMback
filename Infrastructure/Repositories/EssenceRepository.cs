using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EssenceRepository: IEssenceRepository
{
    private readonly AppDbContext _context;

    public EssenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Essence?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.CreatedById == creatorId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByAssignedUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.AssignedToId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByStatusAsync(EEssenceStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Essences.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Essence essence, CancellationToken cancellationToken = default)
    {
        await _context.Essences.AddAsync(essence, cancellationToken);
    }

    public void Update(Essence essence)
    {
        _context.Essences.Update(essence);        
    }

    public void Remove(Essence essence)
    {
        _context.Essences.Remove(essence);
    }

    public async Task<IReadOnlyList<Essence>> GetUserTasksAsync(Guid userId, EEssenceStatus? status, EEssencePriority? priority,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Essences
            .Where(e => e.AssignedToId == userId)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(e => e.Priority == priority.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
    
      public async Task<IReadOnlyDictionary<EEssenceStatus, int>> GetStatusStatisticByCreatorSqlAsync(                                                                                                                                                                                                          
      Guid creatorId,                                                                                                                                                                                                                                                                                       
      CancellationToken cancellationToken = default)                                                                                                                                                                                                                                                        
  {                                                                                                                                                                                                                                                                                                         
      var grouped = await _context.Essences                                                                                                                                                                                                                                                                 
          .AsNoTracking()                                                                                                                                                                                                                                                                                   
          .Where(e => e.CreatedById == creatorId)                                                                                                                                                                                                                                                           
          .GroupJoin(                                                                                                                                                                                                                                                                                       
              _context.Set<EssenceStage>().AsNoTracking(),                                                                                                                                                                                                                                                  
              e => e.Id,                                                                                                                                                                                                                                                                                    
              s => s.EssenceId,                                                                                                                                                                                                                                                                             
              (e, stages) => new                                                                                                                                                                                                                                                                            
              {                                                                                                                                                                                                                                                                                             
                  Status =                                                                                                                                                                                                                                                                                  
                      !stages.Any() ? EEssenceStatus.Waiting :                                                                                                                                                                                                                                              
                      stages.All(s => s.Status == EEssenceStatus.Completed) ? EEssenceStatus.Completed :                                                                                                                                                                                                    
                      stages.Any(s => s.Status == EEssenceStatus.InProgress) ? EEssenceStatus.InProgress :                                                                                                                                                                                                  
                      stages.Any(s => s.Status == EEssenceStatus.Paused) ? EEssenceStatus.Paused :                                                                                                                                                                                                          
                      EEssenceStatus.Waiting                                                                                                                                                                                                                                                                
              })                                                                                                                                                                                                                                                                                            
          .GroupBy(x => x.Status)                                                                                                                                                                                                                                                                           
          .Select(g => new { Status = g.Key, Count = g.Count() })                                                                                                                                                                                                                                           
          .ToListAsync(cancellationToken);                                                                                                                                                                                                                                                                  
                                                                                                                                                                                                                                                                                                            
      var result = new Dictionary<EEssenceStatus, int>                                                                                                                                                                                                                                                      
      {                                                                                                                                                                                                                                                                                                     
          [EEssenceStatus.Waiting] = 0,                                                                                                                                                                                                                                                                     
          [EEssenceStatus.Paused] = 0,                                                                                                                                                                                                                                                                      
          [EEssenceStatus.InProgress] = 0,                                                                                                                                                                                                                                                                  
          [EEssenceStatus.Completed] = 0                                                                                                                                                                                                                                                                    
      };                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                            
      foreach (var row in grouped)                                                                                                                                                                                                                                                                          
          result[row.Status] = row.Count;                                                                                                                                                                                                                                                                   
                                                                                                                                                                                                                                                                                                            
      return result;
  } 
}