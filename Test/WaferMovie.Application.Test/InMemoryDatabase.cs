﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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

        dbContext.SeedMovies();

        return dbContext;
    }

    public static ApplicationDbContext SeedMovies(this ApplicationDbContext dbContext)
    {
        return dbContext;
    }
}