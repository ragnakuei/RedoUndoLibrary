using System;
using System.Collections.Generic;
using System.Linq;

namespace RedoUndoLibrary
{
    public class RedoUndoService<T>
    {
        public RedoUndoService(int maxStep)
        {
            _maxStep = maxStep;
        }

        public Stack<T> DoList { get; set; } = new Stack<T>();

        public Stack<T> RedoList { get; set; } = new Stack<T>();

        public event EventHandler<T> UndoEvent;
        public event EventHandler<T> RedoEvent;

        public void Do(T t)
        {
            DoList.Push(t);
            AdjustDoListOverMaxSteps();
            RedoList.Clear();
        }

        public void Undo()
        {
            if (DoList.Count == 0)
            {
                return;
            }

            var undoAction = DoList.Pop();
            UndoEvent?.Invoke(this, undoAction);
            RedoList.Push(undoAction);
        }

        public void Redo()
        {
            if (RedoList.Count == 0)
            {
                return;
            }

            var redoAction = RedoList.Pop();
            RedoEvent?.Invoke(this, redoAction);

            DoList.Push(redoAction);
        }

        private readonly int _maxStep;

        private void AdjustDoListOverMaxSteps()
        {
            if (DoList.Count > _maxStep)
            {
                DoList = new Stack<T>(DoList.Take(_maxStep).Reverse());
            }
        }
    }
}
