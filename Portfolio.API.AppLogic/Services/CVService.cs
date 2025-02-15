using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services
{
    internal class CVService(ICVRepository cvRepository) : ICVService
    {
        private readonly ICVRepository _cvRepository = cvRepository;

        public Task<CurriculumVitaeDTO> GetCV()
        {
            return _cvRepository.Read();
        }

        public Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO curriculumVitaeDTO)
        {
            return _cvRepository.Update(curriculumVitaeDTO);
        }
    }
}
