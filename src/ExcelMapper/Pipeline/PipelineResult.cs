namespace ExcelMapper.Pipeline
{
    public struct PipelineResult<T>
    {
        public PipelineStatus Status { get; }
        public string StringValue { get; }
        public T Result { get; }

        public PipelineResult<T>? PreviousResult { get; }

        public PipelineResult(PipelineStatus status, string stringValue, T result)
        {
            Status = status;
            StringValue = stringValue;
            Result = result;

            PreviousResult = null;
        }

        public PipelineResult(PipelineStatus status, string stringValue, T result, PipelineResult<T> previousResult) : this(status, stringValue, result)
        {
            PreviousResult = previousResult;
        }

        public PipelineResult<T> MakeEmpty() => new PipelineResult<T>(PipelineStatus.Empty, StringValue, Result, this);

        public PipelineResult<T> MakeInvalid() => new PipelineResult<T>(PipelineStatus.Invalid, StringValue, Result, this);

        public PipelineResult<T> MakeSuccess(string stringValue) => new PipelineResult<T>(PipelineStatus.Success, stringValue, Result, this);

        public PipelineResult<T> MakeCompleted(T result) => new PipelineResult<T>(PipelineStatus.Completed, StringValue, result, this);
    }
}
