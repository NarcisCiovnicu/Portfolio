using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess.Repositories
{
    internal class CVRepository(PortfolioDbContext dbContext, IMapper mapper) : ICVRepository
    {
        private readonly PortfolioDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<CurriculumVitaeDTO> Read(CancellationToken cancellationToken)
        {
            CurriculumVitae cv = await _dbContext.CurriculumVitae
                .Include(p => p.LinkedInProfile).Include(p => p.Website)
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory)
                .AsNoTracking().FirstAsync(q => q.Id == Constants.Database.DefaultCVId, cancellationToken);

            return _mapper.Map<CurriculumVitaeDTO>(cv);
        }

        public async Task<CurriculumVitaeDTO> Update(CurriculumVitaeDTO cvDTO, CancellationToken cancellationToken)
        {
            CurriculumVitae cv = await _dbContext.CurriculumVitae
                .Include(p => p.LinkedInProfile).Include(p => p.Website)
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory)
                .FirstAsync(q => q.Id == Constants.Database.DefaultCVId, cancellationToken);

            RemoveLink(cv.LinkedInProfile, cvDTO.LinkedInProfile);
            RemoveLink(cv.Website, cvDTO.Website);

            _mapper.Map(cvDTO, cv);

            RemoveOrUpdateWorkExperience(cv.WorkExperienceList, cvDTO.WorkExperienceList);
            RemoveOrUpdatePersonalProjects(cv.PersonalProjects, cvDTO.PersonalProjects);
            RemoveOrUpdateEducationHistory(cv.EducationHistory, cvDTO.EducationHistory);

            _dbContext.CurriculumVitae.Update(cv);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CurriculumVitaeDTO>(cv);
        }

        private void RemoveLink(Link? link, LinkDTO? linkDto)
        {
            if (link is not null && linkDto is null)
            {
                _dbContext.Links.Remove(link);
            }
        }

        private void RemoveOrUpdateWorkExperience(IList<WorkExperience> workExperiences, IList<WorkExperienceDTO> workExperienceDTOs)
        {
            DeleteExtraListEntities(_dbContext.WorkExperiences, workExperiences, workExperienceDTOs);
            RemoveExternalLinks(workExperiences, workExperienceDTOs);
            UpdateListEntities(workExperiences, workExperienceDTOs);
        }

        private void RemoveOrUpdatePersonalProjects(IList<PersonalProject> personalProjects, IList<PersonalProjectDTO> personalProjectDTOs)
        {
            DeleteExtraListEntities(_dbContext.PersonalProjects, personalProjects, personalProjectDTOs);
            RemoveExternalLinks(personalProjects, personalProjectDTOs);
            UpdateListEntities(personalProjects, personalProjectDTOs);
        }

        private void RemoveOrUpdateEducationHistory(IList<Education> educationHistory, IList<EducationDTO> educationDTOs)
        {
            DeleteExtraListEntities(_dbContext.EducationHistory, educationHistory, educationDTOs);
            UpdateListEntities(educationHistory, educationDTOs);
        }

        private void RemoveExternalLinks<E, D>(IList<E> entities, IList<D> DTOs)
            where E : ExternalLinkCVEntity
            where D : ExternalLinkCVDTO
        {
            int index = 0;
            while (index < entities.Count && index < DTOs.Count)
            {
                // "Update" (if necessary)
                RemoveLink(entities[index].ExternalLink, DTOs[index].ExternalLink);
                index++;
            }
            while (index < entities.Count)
            {
                // Delete (if exists)
                RemoveLink(entities[index].ExternalLink, null);
                index++;
            }
        }

        private void UpdateListEntities<E, D>(IList<E> entities, IList<D> DTOs)
        {
            int index = 0;
            foreach (D dto in DTOs)
            {
                if (index < entities.Count)
                {
                    _mapper.Map(dto, entities[index]);
                }
                else
                {
                    entities.Add(_mapper.Map<E>(dto));
                }
                index++;
            }
        }

        private static void DeleteExtraListEntities<E, D>(DbSet<E> dbSet, IList<E> entities, IList<D> DTOs) where E : BaseEntity
        {
            if (entities.Count > DTOs.Count)
            {
                dbSet.RemoveRange(entities.Skip(DTOs.Count));
            }
        }
    }
}
