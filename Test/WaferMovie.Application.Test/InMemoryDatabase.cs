using Microsoft.Data.Sqlite;
using WaferMovie.Domain.Entities;
using WaferMovie.Infrastructure.Persistence;

namespace WaferMovie.Application.Test;

public static class InMemoryDatabase
{
    public static IApplicationDbContext Create()
    {
        string connectionString = $"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared"; // In memory database
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new ApplicationDbContext(options);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.SeedMovies()
            .SeedSeries()
            .SaveChanges();

        return dbContext;
    }

    public static ApplicationDbContext SeedMovies(this ApplicationDbContext dbContext)
    {
        dbContext.Movies.AddRange(new List<Movie> {
            new Movie {
                Title = "The Batman",
                AgeRestriction = MovieAgeRestriction.PG13,
                Description = "When a sadistic serial killer begins murdering key political figures in Gotham, Batman is forced to investigate the city's hidden corruption and question his family's involvement.",
                IMDB = "tt1877830",
                IsFree = true,
                Length = 176,
                OutYear = 2022,
                Unavailable = true,
            },
            new Movie {
                Title = "No Time to Die",
                AgeRestriction = MovieAgeRestriction.PG13,
                Description = "James Bond has left active service. His peace is short-lived when Felix Leiter, an old friend from the CIA, turns up asking for help, leading Bond onto the trail of a mysterious villain armed with dangerous new technology.",
                IMDB = "tt2382320",
                IsFree = false,
                Length = 163,
                OutYear = 2021,
                Unavailable = false,
            },
            new Movie {
                Title = "Aquaman",
                AgeRestriction = MovieAgeRestriction.PG13,
                Description = "Arthur Curry, the human-born heir to the underwater kingdom of Atlantis, goes on a quest to prevent a war between the worlds of ocean and land.",
                IMDB = "tt1477834",
                IsFree = false,
                Length = 153,
                OutYear = 2018,
                Unavailable = true,
            },
            new Movie {
                Title = "Hitman's Wife's Bodyguard",
                AgeRestriction = MovieAgeRestriction.R,
                Description = "The bodyguard, Michael Bryce, continues his friendship with assassin, Darius Kincaid, as they try to save Darius' wife Sonia.",
                IMDB = "tt8385148",
                IsFree = true,
                Length = 100,
                OutYear = 2021,
                Unavailable = false,
            }
        });

        return dbContext;
    }

    public static ApplicationDbContext SeedSeries(this ApplicationDbContext dbContext)
    {
        dbContext.Series.AddRange(new List<Serie>
        {
            new Serie {
                Title="Lucifer",
                AgeRestriction = SerieAgeRestriction.TV14,
                Description = "Lucifer Morningstar has decided he's had enough of being the dutiful servant in Hell and decides to spend some time on Earth to better understand humanity. He settles in Los Angeles - the City of Angels.",
                IMDB = "tt4052886",
                IsFree = true,
                FirstSeasonYear = 2016,
                LastSeasonYear = 2021,
                StreamNetwork = "Netflix",
                Length = 42,
                Unavailable = true
            },
            new Serie {
                Title="Gravity Falls",
                AgeRestriction = SerieAgeRestriction.TVY7,
                Description = "Twin siblings Dipper and Mabel Pines spend the summer at their great-uncle's tourist trap in the enigmatic Gravity Falls, Oregon.",
                IMDB = "tt1865718",
                IsFree = false,
                FirstSeasonYear = 2012,
                LastSeasonYear = 2016,
                StreamNetwork = "Disney XD",
                Length = 23,
                Unavailable = false
            },
            new Serie {
                Title="Obi-Wan Kenobi",
                AgeRestriction = SerieAgeRestriction.TV14,
                Description = "Jedi Master Obi-Wan Kenobi has to save young Leia after she is kidnapped, all the while being pursued by Imperial Inquisitors and his former Padawan, now known as Darth Vader.",
                IMDB = "tt8466564",
                IsFree = true,
                FirstSeasonYear = 2022,
                LastSeasonYear = 2022,
                StreamNetwork = "Disney+",
                Length = 42,
                Unavailable = false
            },
            new Serie {
                Title="How I Met Your Mother",
                AgeRestriction = SerieAgeRestriction.TV14,
                Description = "A father recounts to his children - through a series of flashbacks - the journey he and his four best friends took leading up to him meeting their mother.",
                IMDB = "tt0460649",
                IsFree = false,
                FirstSeasonYear = 2005,
                LastSeasonYear = 2014,
                StreamNetwork = "CBS",
                Length = 22,
                Unavailable = true
            }
        });

        return dbContext;
    }
}