namespace L6.CommandProcessor.Command
{

    public interface IOldCommandHandler { }


    public interface ICommandHandler<in TCommand> // : ICommandHandler
        where TCommand: IOldCommand
    {
        void  Execute(TCommand command);
    }


}

