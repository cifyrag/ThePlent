using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Abstractions;

namespace ThePlant.EF.Utils;

public sealed record Error
{
	public required ErrorType Type { get; init; }

	public required string Detail { get; init; }

	public Dictionary<string, object?>? Extensions { get; init; }

	public string Code { get; init; } = string.Empty;

	public static Error Unauthorized(
		string description = "Operation was not authorized.",
		string code = "UNAUTHORIZED",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.Unauthorized,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

	public static Error Validation(
		string description = "A validation error has occurred.",
		string code = "VALIDATION_ERROR",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.Validation,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

	public static Error NotFound(
		string description = "Entity was not found.",
		string code = "NOT_FOUND",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.NotFound,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

	public static Error Forbidden(
		string description = "Operation was forbidden.",
		string code = "FORBIDDEN",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.Forbidden,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

	public static Error Failure(
		string description = "A failure has occurred.",
		string code = "FAILURE",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.Failure,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

	public static Error Unexpected(
		string description = "An unexpected error has occurred.",
		string code = "UNEXPECTED_ERROR",
		Dictionary<string, object?>? extensions = null) => new()
		{
			Type = ErrorType.Unexpected,
			Detail = description,
			Extensions = extensions,
			Code = code
		};

}
