namespace FileCabinetApp.Validation.Input
{
    public interface IInputPackValidator<T>
    {
        public bool IsValid(T data);
    }
}
