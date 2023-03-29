using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WaferMovie.Application.Common.Models;
using WaferMovie.Application.Movies.Commands.CreateMovieCommand;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Application.Test.Movies.Commands;

public class CreateMovieCommandTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public CreateMovieCommandTest()
    {
    }

    [Fact]
    public async Task ShouldCreateMovie()
    {
        var command = new CreateMovieCommand
        {
            Description = "T'Challa, heir to the hidden but advanced kingdom of Wakanda, must step forward to lead his people into a new future and must confront a challenger from his country's past.",
            IMDB = "tt1825683",
            IsFree = true,
            Length = 134,
            OutYear = 2018,
            Title = "Black Panther",
            Unavailable = false
        };

        var result = await mediator.Send(command);
        result.Succeeded.ShouldBeTrue();
    }
}