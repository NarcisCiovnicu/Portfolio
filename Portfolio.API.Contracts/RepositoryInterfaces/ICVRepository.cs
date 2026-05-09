using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.RepositoryInterfaces;

public interface ICVRepository
{
    Task<CurriculumVitaeDTO> Read(CancellationToken cancellationToken);
    Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken);
}
