using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.RepositoryInterfaces
{
    public interface ICVRepository
    {
        Task<CurriculumVitaeDTO> Read();
        Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO);
    }
}
