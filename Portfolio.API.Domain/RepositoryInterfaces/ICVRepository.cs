using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.RepositoryInterfaces
{
    public interface ICVRepository
    {
        Task<CurriculumVitaeDTO> Read(CancellationToken cancellationToken);
        Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken);
    }
}
