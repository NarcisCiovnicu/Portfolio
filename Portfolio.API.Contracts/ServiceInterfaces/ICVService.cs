using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ServiceInterfaces;

public interface ICVService
{
    Task<CurriculumVitaeDTO> GetCV(CancellationToken cancellationToken);
    Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken);
}
