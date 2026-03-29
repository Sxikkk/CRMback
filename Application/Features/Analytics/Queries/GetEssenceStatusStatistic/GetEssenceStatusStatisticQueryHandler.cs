using Application.Common.Interfaces;
using Contracts.Analytics;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Analytics.Queries.GetEssenceStatusStatistic;

public class GetEssenceStatusStatisticQueryHandler: IRequestHandler<GetEssenceStatusStatisticQuery, EssenceStatusStatisticDto>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IRequestContext _requestContext;

    public GetEssenceStatusStatisticQueryHandler(IEssenceRepository essenceRepository, IRequestContext requestContext)
    {
        _essenceRepository = essenceRepository;
        _requestContext = requestContext;
    }

    public async Task<EssenceStatusStatisticDto> Handle(GetEssenceStatusStatisticQuery request, CancellationToken cancellationToken)
    {
         var userId =  _requestContext.UserId;                                                                                                                                                                                                                                            
                                                                                                                                                                                                                                                                                                            
          if (userId is null)                                                                                                                                                                                                                                                                               
              throw new ApplicationException("UserId is required");                                                                                                                                                                                                                                         
                                                                                                                                                                                                                                                                                                            
          var stats = await _essenceRepository.GetStatusStatisticByCreatorSqlAsync(                                                                                                                                                                                                                         
              userId.Value,                                                                                                                                                                                                                                                                                 
              cancellationToken);                                                                                                                                                                                                                                                                           
                                                                                                                                                                                                                                                                                                            
          return new EssenceStatusStatisticDto{                                                                                                                                                                                                                                                              
              Waiting = stats.GetValueOrDefault(EEssenceStatus.Waiting, 0),                                                                                                                                                                                                                                  
              Paused = stats.GetValueOrDefault(EEssenceStatus.Paused, 0),                                                                                                                                                                                                                                    
              InProgress = stats.GetValueOrDefault(EEssenceStatus.InProgress, 0),                                                                                                                                                                                                                            
              Completed = stats.GetValueOrDefault(EEssenceStatus.Completed, 0)                                                                                                                                                                                                                               
          };     
    }
}