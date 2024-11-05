using System;

namespace MementoPattern
{
    // The originator class
    public class Editor
    {
        private string _text;
        private int _curX, _curY, _selectionWidth;

        public void SetText(string text)
        {
            _text = text;
        }

        public void SetCursor(int x, int y)
        {
            _curX = x;
            _curY = y;
        }

        public void SetSelectionWidth(int width)
        {
            _selectionWidth = width;
        }

        // Saves the current state inside a memento.
        public Snapshot CreateSnapshot()
        {
            return new Snapshot(this, _text, _curX, _curY, _selectionWidth);
        }

        // For restoring state from the memento
        public void Restore(string text, int curX, int curY, int selectionWidth)
        {
            SetText(text);
            SetCursor(curX, curY);
            SetSelectionWidth(selectionWidth);
        }

        public override string ToString() // To print current state
        {
            return $"Text: {_text}, Cursor: ({_curX}, {_curY}), Selection Width: {_selectionWidth}";
        }
    }

    // The memento class
    public class Snapshot
    {
        private readonly Editor _editor;
        private readonly string _text;
        private readonly int _curX, _curY, _selectionWidth;

        public Snapshot(Editor editor, string text, int curX, int curY, int selectionWidth)
        {
            _editor = editor;
            _text = text;
            _curX = curX;
            _curY = curY;
            _selectionWidth = selectionWidth;
        }

        // Restores the state of the editor
        public void Restore()
        {
            _editor.Restore(_text, _curX, _curY, _selectionWidth);
        }
    }

    // The caretaker class
    public class Command
    {
        private Snapshot _backup;
        private readonly Editor _editor;

        public Command(Editor editor)
        {
            _editor = editor;
        }

        public void MakeBackup()
        {
            _backup = _editor.CreateSnapshot();
        }

        public void Undo()
        {
            _backup?.Restore();
        }
    }

    // Client code
    class Program
    {
        static void Main(string[] args)
        {
            var editor = new Editor();
            var command = new Command(editor);

            // Set initial state
            editor.SetText("Hello, World!");
            editor.SetCursor(0, 0);
            editor.SetSelectionWidth(5);

            // Save state
            command.MakeBackup();

            // Change state
            editor.SetText("Hello, Memento Pattern!");
            editor.SetCursor(5, 5);
            editor.SetSelectionWidth(10);

            // Print current state
            Console.WriteLine("Current State: " + editor);

            // Undo to previous state
            command.Undo();
            Console.WriteLine("After Undo: " + editor);
        }
    }
}
