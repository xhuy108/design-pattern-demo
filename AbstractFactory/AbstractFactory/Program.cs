// See https://aka.ms/new-console-template for more information
using System;

namespace AbstractFactoryPattern
{
    // The abstract factory interface
    public interface IGUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    // Concrete factories
    public class WinFactory : IGUIFactory
    {
        public IButton CreateButton() => new WinButton();
        public ICheckbox CreateCheckbox() => new WinCheckbox();
    }

    public class MacFactory : IGUIFactory
    {
        public IButton CreateButton() => new MacButton();
        public ICheckbox CreateCheckbox() => new MacCheckbox();
    }

    // Product interfaces
    public interface IButton
    {
        void Paint();
    }

    public interface ICheckbox
    {
        void Paint();
    }

    // Concrete products
    public class WinButton : IButton
    {
        public void Paint()
        {
            Console.WriteLine("Rendering a button in Windows style.");
        }
    }

    public class MacButton : IButton
    {
        public void Paint()
        {
            Console.WriteLine("Rendering a button in macOS style.");
        }
    }

    public class WinCheckbox : ICheckbox
    {
        public void Paint()
        {
            Console.WriteLine("Rendering a checkbox in Windows style.");
        }
    }

    public class MacCheckbox : ICheckbox
    {
        public void Paint()
        {
            Console.WriteLine("Rendering a checkbox in macOS style.");
        }
    }

    // The application class
    public class Application
    {
        private IGUIFactory _factory;
        private IButton _button;

        private ICheckbox _checkbox;

        public Application(IGUIFactory factory)
        {
            _factory = factory;
        }

        public void CreateUI()
        {
            _button = _factory.CreateButton();
            _checkbox = _factory.CreateCheckbox();
        }

        public void Paint()
        {
            _button.Paint();
            _checkbox.Paint();
        }
    }

    // Application configurator
    public static class ApplicationConfigurator
    {
        public static void Client()
        {
            string os = ReadApplicationConfigFile();

            IGUIFactory factory;

            if (os == "Windows")
            {
                factory = new WinFactory();
            }
            else if (os == "Mac")
            {
                factory = new MacFactory();
            }
            else
            {
                throw new Exception("Error! Unknown operating system.");
            }

            Application app = new Application(factory);
            app.CreateUI();
            app.Paint();
        }

        private static string ReadApplicationConfigFile()
        {
            // Simulating reading the OS from a config file
            // In a real application, this would read from an actual configuration source
            return "Windows"; // Change to "Mac" to test the other factory
        }
    }

    // Entry point
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationConfigurator.Client();
        }
    }
}
