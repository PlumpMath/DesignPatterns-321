using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace CommandPattern
{
    [TestFixture]
    internal class CommandPatternTests
    {
        private const string TitleCaseMenuText = "Title-case";
        private const string ToUpperCaseMenuText = "Uppercase";

        // WHAT:
        // The command pattern is a behavioural pattern that encapsulates a method call (request) as an object with
        // all of the state it needs to run so it can be called at a future time by something that does not need to 
        // know how to set up the request's context. 

        // HOW:
        // +--------+         +----------+       +------------------+
        // | Client |         | Invoker  |+----->| Command          |
        // +---+----+         +----------+       +------------------+
        //     |                                          ^
        //     |                                          |
        //     |              +----------+       +--------+---------+
        //     +------------->+ Receiver |<------+ Concrete command |
        //                    +----------+       +------------------+
        //
        // Command      an interface for executing an operation. The concrete commands bind a receiver to an operation
        //              or might just implement some logic depending on design.
        // Client       creates the concrete command and sets its receiver if it has one
        // Invoker      stores and then invokes the command
        // Receiver     performs the actual operation associated with the command (optional)

        // WHY:
        // The invoker can call a command without needing to knowing anything about it except the command's interface. 
        // In particular it decouples the invocation of the command from its instantation and decouples the invoker
        // from the receiver. 
        // 
        // Useful for situtations where the method will be invoked some time after it has been created
        // and it needs some state to be able to run. Example: a queue or an undo stack.

        // Alternatives:
        // Use delegates/events, anonymous functions and closures

        // Benefits:
        // - Can separate framework code from application code. 
        // - Can abstract away the implementation of very different operations. 
        // - Easy to add new commands without needing to change existing commands or the invoker. 

        // Interesting reading:
        // - Does the command pattern stand the test of time? http://ayende.com/blog/159873/design-patterns-in-the-test-of-time-command

        [Test]
        public void RunSingleCommand()
        {
            var menu = new CommandMenu();
            var document = new Document("great expectations");

            menu.AddCommand(TitleCaseMenuText, new TitleCaseCommand(document));
            menu.Run(TitleCaseMenuText);

            Assert.That(document.Text, Is.EqualTo("Great Expectations"));
        }

        [Test]
        public void RunSameCommandTwice()
        {
            var menu = new CommandMenu();
            var document = new Document("great expectations");

            menu.AddCommand(TitleCaseMenuText, new TitleCaseCommand(document));
            menu.Run(TitleCaseMenuText);
            menu.Run(TitleCaseMenuText);

            Assert.That(document.Text, Is.EqualTo("Great Expectations"));
        }

        [Test]
        public void RunTwoDifferentCommands()
        {
            var menu = new CommandMenu();
            var document = new Document("great expectations");

            menu.AddCommand(TitleCaseMenuText, new TitleCaseCommand(document));
            menu.AddCommand(ToUpperCaseMenuText, new UpperCaseCommand(document));
            menu.Run(TitleCaseMenuText);
            menu.Run(ToUpperCaseMenuText);

            Assert.That(document.Text, Is.EqualTo("GREAT EXPECTATIONS"));
        }
    }

    // invoker
    public class CommandMenu
    {
        private readonly IDictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void AddCommand(string commandName, ICommand command)
        {
            _commands.Add(commandName, command);
        }

        public void Run(string commandName)
        {
            _commands[commandName].Execute();   
        }

        public IEnumerable<string> Commands
        {
            get { return _commands.Keys; }
        } 
    }

    // receiver
    public class Document
    {
        public Document(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }

    // command
    public interface ICommand
    {
        void Execute();
    }

    // Concrete command
    public class UpperCaseCommand : ICommand
    {
        private readonly Document _document;

        public UpperCaseCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            _document.Text = _document.Text.ToUpper();
        }
    }

    // Concrete command
    public class TitleCaseCommand : ICommand
    {
        private readonly Document _document;

        public TitleCaseCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            _document.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_document.Text);
        }
    }
}