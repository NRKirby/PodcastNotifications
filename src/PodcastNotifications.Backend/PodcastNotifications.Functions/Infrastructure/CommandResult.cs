using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PodcastNotifications.Functions.Infrastructure
{
    public class CommandResult : ActionResult
    {
        public readonly IActionResult ActionResult;
        public object Data { get; }
        public IEnumerable<string> Errors { get; }

        private CommandResult(IActionResult actionResult, object data, IEnumerable<string> errors = null)
        {
            ActionResult = actionResult;
            Data = data;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static CommandResult Ok()
        {
            return new CommandResult(new OkResult(), new object());
        }

        public static CommandResult Ok(object data)
        {
            return new CommandResult(new OkObjectResult(data), data);
        }

        public static CommandResult Created(Guid identifier, object data)
        {
            return new CommandResult(new CreatedResult(identifier.ToString(), data), data);
        }

        public static CommandResult Created(int identifier, object data)
        {
            return new CommandResult(new CreatedResult(identifier.ToString(), data), data);
        }

        public static CommandResult NoContent()
        {
            return new CommandResult(new NoContentResult(), null);
        }

        public static CommandResult BadRequest(IEnumerable<string> errors)
        {
            return new CommandResult(new BadRequestObjectResult(errors), new object(), errors);
        }

        public static CommandResult BadRequest(params string[] errors)
        {
            return new CommandResult(new BadRequestObjectResult(errors), null, errors);
        }

        public static CommandResult BadRequest(string error)
        {
            return new CommandResult(new BadRequestObjectResult(error), null, new List<string>{error});
        }

        public static CommandResult NotFound(params string[] errors)
        {
            return new CommandResult(new NotFoundResult(), null, errors);
        }

        public static CommandResult NotFound()
        {
            return new CommandResult(new NotFoundResult(), null);
        }

        public static CommandResult Forbidden(params string[] errors)
        {
            return new CommandResult(new ForbidResult(), null, errors);
        }

        public static CommandResult InternalServerError(IEnumerable<string> errors)
        {
            return new CommandResult(new StatusCodeResult(500), null, errors);
        }
        
        public override Task ExecuteResultAsync(ActionContext context)
        {
            return ActionResult.ExecuteResultAsync(context);
        }
    }

    public class CommandResult<T> : ActionResult
        where T : class
    {
        private readonly IActionResult _result;
        public T Data { get; }
        public IEnumerable<string> Errors { get; }

        private CommandResult(IActionResult result, T data, IEnumerable<string> errors = null)
        {
            _result = result;
            Data = data;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static CommandResult<T> Ok()
        {
            return new CommandResult<T>(new OkResult(), null);
        }

        public static CommandResult<T> Ok(T data)
        {
            return new CommandResult<T>(new OkObjectResult(data), data);
        }

        public static CommandResult<T> Created(Guid identifier, T data)
        {
            return new CommandResult<T>(new CreatedResult(identifier.ToString(), data), data);
        }

        public static CommandResult<T> BadRequest(IEnumerable<string> errors)
        {
            return new CommandResult<T>(new BadRequestObjectResult(errors), null, errors);
        }

        public static CommandResult<T> BadRequest(params string[] errors)
        {
            return new CommandResult<T>(new BadRequestObjectResult(errors), null, errors);
        }

        public static CommandResult<T> NotFound()
        {
            return new CommandResult<T>(new NotFoundResult(), null);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return _result.ExecuteResultAsync(context);
        }
    }
}
