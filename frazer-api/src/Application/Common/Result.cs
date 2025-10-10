namespace FrazerDealer.Application.Common;

public class Result
{
    private Result(bool succeeded, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public bool Succeeded { get; }

    public IReadOnlyCollection<string> Errors { get; }

    public static Result Success() => new(true);

    public static Result Failure(params string[] errors) => new(false, errors);
}

public class Result<T> : Result
{
    private Result(bool succeeded, T? value, IEnumerable<string>? errors = null)
        : base(succeeded, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value);

    public new static Result<T> Failure(params string[] errors) => new(false, default, errors);
}
