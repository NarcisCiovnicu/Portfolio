using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.RepositoryInterfaces;
using Portfolio.API.Contracts.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services;

internal class CVService(ICVRepository cvRepository) : ICVService
{
    private readonly ICVRepository _cvRepository = cvRepository;

    public Task<CurriculumVitaeDTO> GetCV(CancellationToken cancellationToken)
    {
        return _cvRepository.Read(cancellationToken);
    }

    public Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken)
    {
        return _cvRepository.Update(curriculumVitaeDTO, cancellationToken);
    }
}
