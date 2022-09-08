using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SixMinApi.AppData;
using SixMinApi.DTO;
using SixMinApi.Models;

namespace SixMinApi.Endpoints
{
    public static class CommandsEndpoint
    {
        public static void MapCommandsEndpoint(this WebApplication app)
        {
            app.MapGet("api/v1/command", async (ICommandRepo repo, IMapper mapper) =>
            {
                var commands = await repo.GetAllCommands();
                return Results.Ok(mapper.Map<IEnumerable<CommandReadDTO>>(commands));
            });

            app.MapGet("api/v1/comands/{id}", GetOneCommand);
            app.MapPost("api/v1/commands", PostCommand);
            app.MapPut("api/v1/commands/{id}", UpdateCommand);
            app.MapDelete("api/v1/commands/{id}", DeleteCommand);

        }
        public static async Task<IResult> GetOneCommand(ICommandRepo repo, [FromRoute]int id, IMapper mapper)
        {
            var command = await repo.GetCommandById(id);
            if(command != null)
                return Results.Ok(mapper.Map<CommandReadDTO>(command));
            return Results.NotFound();
        }
        public static async Task<IResult> PostCommand(ICommandRepo repo, IMapper mapper, CommandCreateDTO command)
        {
            if(command == null)
            {
                return Results.NotFound();
            }

            var createcommand = mapper.Map<Command>(command);
            await repo.CreateCommand(createcommand);
            await repo.SaveChanges();

            var readDto = mapper.Map<CommandReadDTO>(createcommand);
            return Results.Created("api/v1/commands/{ readDto.Id}", readDto);
        }
        public static async Task<IResult> UpdateCommand(ICommandRepo repo, IMapper mapper, [FromRoute]int id, CommandUpdateDTO cmd)
        {
            var command = await repo.GetCommandById(id);
            if(cmd == null)
            {
                return Results.NotFound();
            }
            mapper.Map(cmd, command);
            await repo.SaveChanges();

            return Results.NoContent();
        }
        public static async Task<IResult> DeleteCommand(ICommandRepo repo, [FromRoute]int id)
        {
            var cmd = await repo.GetCommandById(id);
            repo.DeleteCommand(cmd);
            await repo.SaveChanges();
            return Results.NoContent();
        }
    }
}
