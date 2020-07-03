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

        public void Do(T t)
        {
            DoList.Push(t);
            AdjustDoListOverMaxSteps();
            RedoList.Clear();
        }

        public T Undo()
        {
            if (DoList.Count == 0)
            {
                return default(T);
            }

            var undoAction = DoList.Pop();
            RedoList.Push(undoAction);

            return undoAction;
        }

        public T Redo()
        {
            if (RedoList.Count == 0)
            {
                return default(T);
            }

            var redoAction = RedoList.Pop();
            DoList.Push(redoAction);

            return redoAction;
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
