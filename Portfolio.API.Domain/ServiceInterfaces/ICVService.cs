using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface ICVService
    {
        Task<CurriculumVitaeDTO> GetCV(CancellationToken cancellationToken);
        Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken);
    }
}
