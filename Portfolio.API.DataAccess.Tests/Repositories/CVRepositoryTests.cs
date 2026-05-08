using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.DataAccess.MappingConfigs;
using Portfolio.API.DataAccess.Repositories;
using Portfolio.API.DataAccess.Test.TestHelpers;
using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.DataAccess.Test.Repositories;

public class CVRepositoryTests
{
    private readonly DbContextFactory _dbContextFactory;
    private readonly CVRepository _cvRepository;

    public CVRepositoryTests()
    {
        _dbContextFactory = new DbContextFactory();

        IMapper mapper = MapperUtils.CreateMapper(new CurriculumVitaeMapConfigs());

        _cvRepository = new CVRepository(_dbContextFactory.CreateSQLiteContext(), mapper);
    }

    #region Read
    [Fact]
    public async Task Read_ShouldReturnOneDefaultCV_WhenDbIsFirstCreated()
    {
        CurriculumVitaeDTO cv = await _cvRepository.Read(TestContext.Current.CancellationToken);

        Assert.NotNull(cv);
    }
    #endregion

    #region Update
    [Fact]
    public async Task Update_ShouldUpdateSimpleProperties()
    {
        // setup
        InitializeMockCV();
        CurriculumVitaeDTO mockCV = CreateMockCVDTO();

        // execute
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using var dbContext = _dbContextFactory.CreateSQLiteContext();
        CurriculumVitae updatedCV = dbContext.CurriculumVitae.First();

        Assert.Equal(mockCV.Name, updatedCV.Name);
        Assert.Equal(mockCV.Location, updatedCV.Location);
        Assert.Equal(mockCV.Phone, updatedCV.Phone);
        Assert.Equal(mockCV.Email, updatedCV.Email);
        Assert.Equal(mockCV.About, updatedCV.About);
        Assert.Equal(mockCV.Skills.Select(s => s.Name), updatedCV.Skills);
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
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using PortfolioDbContext dbContext = _dbContextFactory.CreateSQLiteContext();
        var updatedCV = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
        int linksCount = dbContext.Links.Count();

        Assert.Equivalent(mockCV.LinkedInProfile, updatedCV.LinkedInProfile);
        Assert.Equivalent(mockCV.Website, updatedCV.Website);
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
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var updatedCV = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
            int linksCount = dbContext.Links.Count();

            Assert.Equivalent(mockCV.LinkedInProfile, updatedCV.LinkedInProfile);
            Assert.Equivalent(mockCV.Website, updatedCV.Website);
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
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var updatedCV = dbContext.CurriculumVitae.Include(p => p.LinkedInProfile).Include(p => p.Website).First();
            int linksCount = dbContext.Links.Count();

            Assert.Null(updatedCV.LinkedInProfile);
            Assert.Null(updatedCV.Website);
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
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using var dbContext = _dbContextFactory.CreateSQLiteContext();
        var updatedCV = dbContext.CurriculumVitae
            .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
            .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
            .Include(p => p.EducationHistory).First();

        int linksCount = dbContext.Links.Count();
        int workExperiencesCount = dbContext.WorkExperiences.Count();
        int personalProjectsCount = dbContext.PersonalProjects.Count();
        int educationHistoryCount = dbContext.EducationHistory.Count();

        Assert.Equal(1, workExperiencesCount);
        Assert.Equal(2, personalProjectsCount);
        Assert.Equal(1, educationHistoryCount);

        Assert.Equivalent(mockCV.WorkExperienceList, updatedCV.WorkExperienceList);
        Assert.Equivalent(mockCV.PersonalProjects, updatedCV.PersonalProjects);
        Assert.Equivalent(mockCV.EducationHistory, updatedCV.EducationHistory);
        
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
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var updatedCV = dbContext.CurriculumVitae
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory).First();
            
            int linksCount = dbContext.Links.Count();
            int workExperiencesCount = dbContext.WorkExperiences.Count();
            int personalProjectsCount = dbContext.PersonalProjects.Count();
            int educationHistoryCount = dbContext.EducationHistory.Count();

            Assert.Equal(1, workExperiencesCount);
            Assert.Equal(2, personalProjectsCount);
            Assert.Equal(1, educationHistoryCount);

            Assert.Equivalent(mockCV.WorkExperienceList, updatedCV.WorkExperienceList);
            Assert.Equivalent(mockCV.PersonalProjects, updatedCV.PersonalProjects);
            Assert.Equivalent(mockCV.EducationHistory, updatedCV.EducationHistory);
            
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
            WorkExperienceList = [],
            PersonalProjects = [new("Ttile1", "Desc1", new("Label1", "Uri1"))],
            EducationHistory = [],
        };

        // execute
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var updatedCV = dbContext.CurriculumVitae
                .Include(p => p.WorkExperienceList).ThenInclude(p => p.ExternalLink)
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .Include(p => p.EducationHistory).First();

            int linksCount = dbContext.Links.Count();
            int workExperiencesCount = dbContext.WorkExperiences.Count();
            int personalProjectsCount = dbContext.PersonalProjects.Count();
            int educationHistoryCount = dbContext.EducationHistory.Count();

            Assert.Equal(0, workExperiencesCount);
            Assert.Equal(1, personalProjectsCount);
            Assert.Equivalent(mockCV.PersonalProjects, updatedCV.PersonalProjects, false);
            Assert.Equal(0, educationHistoryCount);
            Assert.Equal(1, linksCount);
        }
    }

    [Fact]
    public async Task Update_ShouldRemoveLinkFromPersonalProject_WhenTheNewOneIsNull()
    {
        // setup
        InitializeMockCV();
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var dbCV = dbContext.CurriculumVitae.First();
            dbCV.PersonalProjects.Add(new PersonalProject()
            {
                Id = Guid.Empty,
                Description = "a",
                Title = "a",
                ExternalLink = new Link() { Id = Guid.Empty, Label = "a", Uri = "a" }
            });

            dbContext.Update(dbCV);
            dbContext.SaveChanges();
        }

        CurriculumVitaeDTO mockCV = CreateMockCVDTO() with
        {
            PersonalProjects = [new("Title1", "Desc1", null)]
        };

        // execute
        _ = await _cvRepository.Update(mockCV, TestContext.Current.CancellationToken);

        // assert
        using (var dbContext = _dbContextFactory.CreateSQLiteContext())
        {
            var updatedCV = dbContext.CurriculumVitae
                .Include(p => p.PersonalProjects).ThenInclude(p => p.ExternalLink)
                .First();

            int linksCount = dbContext.Links.Count();
            int personalProjectsCount = dbContext.PersonalProjects.Count();

            Assert.Equivalent(mockCV.PersonalProjects, updatedCV.PersonalProjects);
            Assert.Equal(1, personalProjectsCount);
            Assert.Equal(0, linksCount);
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
