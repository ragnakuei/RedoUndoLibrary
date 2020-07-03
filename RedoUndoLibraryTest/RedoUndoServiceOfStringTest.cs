using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RedoUndoLibrary;

namespace RedoUndoLibraryTest
{
    public class RedoUndoServiceOfStringTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Do3Times_Then_DoList()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            var actual = target.DoList;

            var expected = new Stack<string>(new[] { "A", "B", "C" });

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Do2TimesAndUndo2Times_Then_RedoList()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Undo();
            target.Undo();
            var actual = target.RedoList;

            var expected = new Stack<string>(new[] { "B", "A" });

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Do3TimesAndUndo2Times_Then_DoList()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            target.Undo();
            target.Undo();
            var actual = target.DoList;

            var expected = new Stack<string>(new[] { "A" });

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Do3TimesAndUndo2Times_Then_RedoList()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            target.Undo();
            target.Undo();
            var actual = target.RedoList;

            var expected = new Stack<string>(new[] { "C", "B" });

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Do3TimesAndUndo1TimesAndDo1Time_Then_RedoList()
        {
            var target = new RedoUndoService<string>(5);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            target.Undo();
            target.Do("D");
            var actual = target.RedoList;

            var expected = new Stack<string>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void MaxStep2Do3Times_Then_DoList()
        {
            var target = new RedoUndoService<string>(2);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            var actual = target.DoList;

            var expected = new Stack<string>(new[] { "B", "C" });

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UndoRaiseEvent()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Do("C");

            string actualUndoDto = string.Empty;
            EventHandler<string> handler = (sender, e) =>
                                           {
                                               actualUndoDto = e;
                                           };

            target.UndoEvent += handler;
            target.Undo();
            target.UndoEvent -= handler;

            var expectedUndoDto = "C";
            Assert.AreEqual(expectedUndoDto, actualUndoDto);

            var actualDoList   = target.DoList;
            var expectedDoList = new Stack<string>(new[] { "A", "B" });
            Assert.AreEqual(expectedDoList, actualDoList);

            var actualRedoList   = target.RedoList;
            var expectedRedoList = new Stack<string>(new[] { "C" });
            Assert.AreEqual(expectedRedoList, actualRedoList);
        }

        [Test]
        public void RedoRaiseEvent()
        {
            var target = new RedoUndoService<string>(3);
            target.Do("A");
            target.Do("B");
            target.Do("C");
            target.Undo();
            target.Undo();

            string actualRedoDto = string.Empty;
            EventHandler<string> handler = (sender, e) =>
                                           {
                                               actualRedoDto = e;
                                           };

            target.RedoEvent += handler;
            target.Redo();
            target.RedoEvent -= handler;

            var expectedUndoDto = "B";
            Assert.AreEqual(expectedUndoDto, actualRedoDto);

            var actualDoList   = target.DoList;
            var expectedDoList = new Stack<string>(new[] { "A", "B" });
            Assert.AreEqual(expectedDoList, actualDoList);

            var actualRedoList   = target.RedoList;
            var expectedRedoList = new Stack<string>(new[] { "C" });
            Assert.AreEqual(expectedRedoList, actualRedoList);
        }


        [Test]
        public void MaxStep2Do2TimesAndUndo3Times()
        {
            var target = new RedoUndoService<string>(2);
            target.Do("A");
            target.Do("B");
            target.Undo();
            target.Undo();
            target.Undo();

            var actualDoList   = target.DoList;
            var expectedDoList = new Stack<string>();
            actualDoList.Should().BeEquivalentTo(expectedDoList);

            var actualRedoList   = target.RedoList;
            var expectedRedoList = new Stack<string>(new[] { "B", "A" });
            actualRedoList.Should().BeEquivalentTo(expectedRedoList);
        }
    }
}
