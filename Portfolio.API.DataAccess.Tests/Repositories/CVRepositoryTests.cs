using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.DataAccess.Repositories;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess.Tests.Repositories;

public class CVRepositoryTests
{
    private readonly DbContextFactory _dbContextFactory;
    private readonly ICVRepository _cvRepository;
    private readonly IMapper _mapper;

    public CVRepositoryTests()
    {
        _dbContextFactory = new DbContextFactory();
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new DataAccessMappingProfile());
        });
        _mapper = config.CreateMapper();
        _cvRepository = new CVRepository(_dbContextFactory.CreateSQLiteContext(), _mapper);
    }

    #region Read
    [Fact]
    public async Task Read_ShouldReturnOneDefaultCV_WhenDbIsFirstCreated()
    {
        CurriculumVitaeDTO cv = await _cvRepository.Read();

        Assert.NotNull(cv);
    }
    #endregion

    #region Update
    [Fact]
    public async Task Update_ShouldUpdateStringProperties()
    {
        // setup
        InitializeMockCV();
        CurriculumVitaeDTO mockCV = CreateMockCVDTO();

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using var dbContext = _dbContextFactory.CreateSQLiteContext();
        CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(dbContext.CurriculumVitae.First());

        Assert.Equivalent(mockCV, updatedCV, true);
    }

    [Fact]
    public async Task Update_ShouldAddLinksInDB()
    {
        // setup
        InitializeMockCV();
        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            LinkedInProfile = new("LinkedIn", "www.linkedin-url"),
            Website = new("my website", "www.mywebsite.com"),
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using PortfolioDbContext dbContext = _dbContextFactory.CreateSQLiteContext();
        var cv = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
        CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
        int linksCount = dbContext.Links.Count();

        Assert.Equivalent(mockCV, updatedCV, true);
        Assert.Equal(2, linksCount);
    }

    [Fact]
    public async Task Update_ShouldOnlyUpdateLinksInDB_WhenTheyAlreadyExists()
    {
        // setup
        InitializeMockCV();
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var dbCV = dbContext.CurriculumVitae.First();
            dbCV.LinkedInProfile = new() { Id = Guid.Empty, Label = "a", Uri = "a" };
            dbCV.Website = new() { Id = Guid.Empty, Label = "b", Uri = "b" };
            dbContext.Update(dbCV);
            dbContext.SaveChanges();
        }

        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            LinkedInProfile = new("LinkedIn", "www.linkedin-url"),
            Website = new("my website", "www.mywebsite.com"),
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var cv = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
            CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
            int linksCount = dbContext.Links.Count();

            Assert.Equivalent(mockCV, updatedCV, true);
            Assert.Equal(2, linksCount);
        }
    }

    [Fact]
    public async Task Update_ShouldDeleteLinksFromDB_WhenTheNewOnesAreSetToNull()
    {
        // setup
        InitializeMockCV();
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var dbCV = dbContext.CurriculumVitae.First();
            dbCV.LinkedInProfile = new() { Id = Guid.Empty, Label = "a", Uri = "a" };
            dbCV.Website = new() { Id = Guid.Empty, Label = "b", Uri = "b" };
            dbContext.Update(dbCV);
            dbContext.SaveChanges();
        }

        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            LinkedInProfile = null,
            Website = null,
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var cv = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
            CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
            int linksCount = dbContext.Links.Count();
            Assert.Equivalent(mockCV, updatedCV, true);
            Assert.Equal(0, linksCount);
        }
    }

    [Fact]
    public async Task Update_ShouldAddNewForeignRelationsInDB()
    {
        // setup
        InitializeMockCV();
        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            WorkExperienceList = [new("a", "a", DateOnly.MinValue, null, null, "a", null, "a", new("a", "a"))],
            PersonalProjects = [
                new("a", "a", new("a", "a")),
                new("b", "b", new("b", "b")),
            ],
            EducationHistory = [new("a", "a", DateOnly.MinValue, DateOnly.MaxValue)],
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using var dbContext = _dbContextFactory.CreateSQLiteContext();
        var cv = dbContext.CurriculumVitae
            .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
            .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
            .Include(p => p.EducationHistory).First();
        CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
        int linksCount = dbContext.Links.Count();
        int workExperiencesCount = dbContext.WorkExperiences.Count();
        int personalProjectsCount = dbContext.PersonalProjects.Count();
        int educationHistoryCount = dbContext.EducationHistory.Count();

        Assert.Equivalent(mockCV, updatedCV, true);
        Assert.Equal(1, workExperiencesCount);
        Assert.Equal(2, personalProjectsCount);
        Assert.Equal(1, educationHistoryCount);
        Assert.Equal(3, linksCount);
    }

    [Fact]
    public async Task Update_ShouldUpdateExistingForeignRelationsInDB_WhenTheyAlreadyExists()
    {
        // setup
        InitializeMockCV();
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var dbCV = dbContext.CurriculumVitae.First();
            dbCV.WorkExperienceList.Add(new WorkExperience()
            {
                Id = Guid.Empty,
                Title = "a",
                CompanyName = "a",
                StartDate = DateOnly.MinValue,
                ExternalLink = new Link() { Id = Guid.Empty, Label = "a", Uri = "a" }
            });
            dbCV.PersonalProjects.Add(new PersonalProject()
            {
                Id = Guid.Empty,
                Description = "a",
                Title = "a",
                ExternalLink = new Link() { Id = Guid.Empty, Label = "a", Uri = "a" }
            });
            dbCV.EducationHistory.Add(new Education()
            {
                Id = Guid.Empty,
                DegreeName = "a",
                SchoolName = "a",
                StartDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue
            });

            dbContext.Update(dbCV);
            dbContext.SaveChanges();
        }

        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            WorkExperienceList = [new("x", "x", DateOnly.MinValue, null, null, "x", null, "x", new("x", "x"))],
            PersonalProjects = [
                new("x", "x", new("x", "x")),
                new("y", "y", new("y", "y")),
            ],
            EducationHistory = [new("x", "x", DateOnly.MinValue, DateOnly.MaxValue)],
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var cv = dbContext.CurriculumVitae
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory).First();
            CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
            int linksCount = dbContext.Links.Count();
            int workExperiencesCount = dbContext.WorkExperiences.Count();
            int personalProjectsCount = dbContext.PersonalProjects.Count();
            int educationHistoryCount = dbContext.EducationHistory.Count();

            Assert.Equivalent(mockCV, updatedCV, true);
            Assert.Equal(1, workExperiencesCount);
            Assert.Equal(2, personalProjectsCount);
            Assert.Equal(1, educationHistoryCount);
            Assert.Equal(3, linksCount);
        }
    }

    [Fact]
    public async Task Update_ShouldDeleteForeignRelationsFromDB_WhenRecievedOnesAreFewerThanExistingOnes()
    {
        // setup
        InitializeMockCV();
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var dbCV = dbContext.CurriculumVitae.First();
            dbCV.WorkExperienceList.Add(new WorkExperience()
            {
                Id = Guid.Empty,
                Title = "a",
                CompanyName = "a",
                StartDate = DateOnly.MinValue,
                ExternalLink = new Link() { Id = Guid.Empty, Label = "a", Uri = "a" }
            });
            dbCV.PersonalProjects.Add(new PersonalProject()
            {
                Id = Guid.Empty,
                Description = "a",
                Title = "a",
                ExternalLink = new Link() { Id = Guid.Empty, Label = "a", Uri = "a" }
            });
            dbCV.EducationHistory.Add(new Education()
            {
                Id = Guid.Empty,
                DegreeName = "a",
                SchoolName = "a",
                StartDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue
            });

            dbContext.Update(dbCV);
            dbContext.SaveChanges();
        }

        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            Website = new("a", "a"),
            WorkExperienceList = [],
            PersonalProjects = [],
            EducationHistory = [],
        };

        // execute
        await _cvRepository.Update(mockCV);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var cv = dbContext.CurriculumVitae.Include(p => p.Website)
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory).First();
            CurriculumVitaeDTO updatedCV = _mapper.Map<CurriculumVitaeDTO>(cv);
            int linksCount = dbContext.Links.Count();
            int workExperiencesCount = dbContext.WorkExperiences.Count();
            int personalProjectsCount = dbContext.PersonalProjects.Count();
            int educationHistoryCount = dbContext.EducationHistory.Count();

            Assert.Equivalent(mockCV, updatedCV, true);
            Assert.Equal(0, workExperiencesCount);
            Assert.Equal(0, personalProjectsCount);
            Assert.Equal(0, educationHistoryCount);
            Assert.Equal(1, linksCount);
        }
    }
    #endregion

    private void InitializeMockCV()
    {
        using var dbContext = _dbContextFactory.CreateSQLiteContext();
        CurriculumVitae cv = dbContext.CurriculumVitae.First();
        cv.Name = "";
        cv.Location = "";
        cv.Phone = "";
        cv.Email = "";
        cv.About = "";

        dbContext.Update(cv);

        dbContext.SaveChanges();
    }

    private static CurriculumVitaeDTO CreateMockCVDTO()
    {
        return new CurriculumVitaeDTO("dto-name", "dto-location", "dto-phone", "dto-email",
            null, null, "dto-about", [], [], [], [new("dto-skill-1"), new("dto-skill-2")]);
    }
}
