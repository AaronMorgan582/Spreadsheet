/// <summary> 
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      1/25/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote the code for the following tests, EXCEPT: SimpleEmptyTest(), SimpleEmptyRemoveTest(),
/// EmptyEnumeratorTest(), SimpleReplaceTest(), StaticTest(), SizeTest(), EnumeratorTest(), ReplaceThenEnumerate(), and StressTest().
/// 
/// The aforementioned tests were provided by starter code provided by the University of Utah's School of Computing.
/// </summary>
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTests
    {
        /// <summary>
        /// Testing the Indexer.
        /// </summary>
        [TestMethod()]
        public void DependeeSizeTest()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("c", "b");
            graph.AddDependency("d", "b");

            Assert.AreEqual(3, graph["b"]);
        }

        /// <summary>
        /// Testing the Indexer with a value that isn't known.
        /// </summary>
        [TestMethod()]
        public void DependeeSizeWithInvalidEntryTest()
        {
            DependencyGraph graph = new DependencyGraph();

            Assert.AreEqual(0, graph["b"]);
        }

        /// <summary>
        /// Testing Indexer with a value that is known, but has no dependees.
        /// </summary>
        [TestMethod()]
        public void DependeeSizeEmptyTest()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("c", "b");
            graph.AddDependency("d", "b");

            Assert.AreEqual(0, graph["a"]);
        }

        /// <summary>
        /// Testing ReplaceDependents to add a new value, but with no given dependents.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsEmptyNewAdditionTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "z");
            t.ReplaceDependents("d", new HashSet<string>());
            Assert.AreEqual(0, t["d"]);
            Assert.IsFalse(t.HasDependees("d"));
            Assert.IsFalse(t.HasDependents("d"));//Should be false because a blank list was passed in.
            Assert.IsTrue(t.HasDependents("c"));
            Assert.IsTrue(t.HasDependees("b"));
        }

        /// <summary>
        /// Testing ReplaceDependees with a new addition, but with no given dependees.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependeesEmptyNewAdditionTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "z");
            t.ReplaceDependees("d", new HashSet<string>());
            Assert.AreEqual(0, t["d"]);
            Assert.IsFalse(t.HasDependees("d"));
            Assert.IsFalse(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependees("z"));
        }

        /// <summary>
        /// Testing ReplaceDependents with multiple values, some known to the graph, and some not known.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsAdditionWithMultiplesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("m", "g");
            t.ReplaceDependents("a", new HashSet<string>() { "g", "h" });
            t.AddDependency("c", "z");
            t.ReplaceDependents("d", new HashSet<string>() { "z", "f" });
            Assert.AreEqual(0, t["d"]);
            Assert.AreEqual(2, t["z"]);
            Assert.AreEqual(6, t.Size);
            Assert.AreEqual(2, t["g"]);

            IEnumerator<string> zEnumerator = t.GetDependees("z").GetEnumerator();
            string dependeesOfZ = "";
            zEnumerator.MoveNext();
            dependeesOfZ += zEnumerator.Current + " ";
            zEnumerator.MoveNext();
            dependeesOfZ += zEnumerator.Current;
            Console.WriteLine("The Dependees of Z are: " + dependeesOfZ);

            IEnumerator<string> aEnumerator = t.GetDependents("a").GetEnumerator();
            string dependentsOfA = "";
            aEnumerator.MoveNext();
            dependentsOfA += aEnumerator.Current + " ";
            aEnumerator.MoveNext();
            dependentsOfA += aEnumerator.Current;
            Console.WriteLine("The Dependents of A are: " + dependentsOfA); //"b" should not be part of the list.
        }

        /// <summary>
        ///With a given dependency, both variables should have dependent lists, but only one of those lists should be non-empty.
        ///</summary>
        [TestMethod()]
        public void HasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependents("z"));

            IEnumerator<string> aEnumerator = t.GetDependents("a").GetEnumerator();
            string dependentsOfA = "";
            aEnumerator.MoveNext();
            dependentsOfA += aEnumerator.Current;
            Console.WriteLine("The Dependents of A are: " + dependentsOfA);

            IEnumerator<string> bEnumerator = t.GetDependents("b").GetEnumerator();
            Assert.IsFalse(bEnumerator.MoveNext());
        }

        /// <summary>
        ///With a given dependency, both variables should have dependee lists, but only one of those lists should be non-empty.
        ///
        ///</summary>
        [TestMethod()]
        public void HasDependeeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsFalse(t.HasDependees("z"));

            IEnumerator<string> aEnumerator = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(aEnumerator.MoveNext());

            IEnumerator<string> bEnumerator = t.GetDependees("b").GetEnumerator();
            string dependeesOfB = "";
            bEnumerator.MoveNext();
            dependeesOfB += bEnumerator.Current;
            Console.WriteLine("The Dependees of B are: " + dependeesOfB);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Even if the variable is not in the graph, it should still return an IEnumerator when GetDependees is called.
        ///</summary>
        [TestMethod()]
        public void GetDependeesEnumeratorInvalidTest()
        {
            DependencyGraph t = new DependencyGraph();
            IEnumerator<string> enumerator = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        /// <summary>
        ///Even if the variable is not in the graph, it should still return an IEnumerator when GetDependents is called.
        ///</summary>
        [TestMethod()]
        public void GetDependentsEnumeratorInvalidTest()
        {
            DependencyGraph t = new DependencyGraph();
            IEnumerator<string> enumerator = t.GetDependents("a").GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }

        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            t.ReplaceDependees("s", new HashSet<string>() { "k", "u" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "k") && (s2 == "u")) || ((s1 == "u") && (s2 == "k")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
    }
}
