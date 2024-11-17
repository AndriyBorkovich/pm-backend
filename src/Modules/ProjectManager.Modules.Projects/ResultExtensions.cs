using Ardalis.Result;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using IResult = Ardalis.Result.IResult;

namespace ProjectManager.Modules.Projects;

public static class ArdalisResultsExtensions
{
    public static async Task SendResponseAsync<TResult, TResponse>(this IEndpoint ep, TResult result, Func<TResult, TResponse> mapper) where TResult : IResult
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                await ep.HttpContext.Response.SendOkAsync(mapper(result));
                break;

            case ResultStatus.Invalid:
                await ep.HttpContext.Response.SendErrorsAsync(
                    failures: result.ValidationErrors.Select(e => new ValidationFailure(e.Identifier, e.ErrorMessage)).ToList());
                break;

            case ResultStatus.NotFound:
                await ep.HttpContext.Response.SendNotFoundAsync();
                break;

            case ResultStatus.Forbidden:
                await ep.HttpContext.Response.SendForbiddenAsync();
                break;

            default:
                await ep.HttpContext.Response.SendResultAsync(Results.Problem(
                    title: "Error",
                    statusCode: 500,
                    detail: "An unexpected error occurred."));
                break;
        }
    }
}
