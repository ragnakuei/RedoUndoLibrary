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

        public Stack<T> UndoList { get; set; } = new Stack<T>();

        public event EventHandler<T> UndoEvent;

        public void Do(T t)
        {
            DoList.Push(t);
            AdjustDoListOverMaxSteps();
            UndoList.Clear();
        }

        public void Undo()
        {
            if (DoList.Count == 0)
            {
                return;
            }

            var undoAction = DoList.Pop();
            UndoEvent?.Invoke(this, undoAction);
            UndoList.Push(undoAction);
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
