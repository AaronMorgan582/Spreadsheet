// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependents; //In this Dictionary, dependees are the key, dependents are the values.
        private Dictionary<string, HashSet<string>> dependees;  //In this Dictionary, dependents are the key, dependees are the values.

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get 
            {
                int total = 0;
                foreach (KeyValuePair<string, HashSet<string>> dependee in dependents)
                {
                    total += dependee.Value.Count;
                }
                return total;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                HashSet<string> setOfDependees = new HashSet<string>();
                //If the dependent is in the Dictionary, it'll be copied into setOfDependees.
                if(dependees.TryGetValue(s, out setOfDependees))
                {
                    return setOfDependees.Count;
                }
                //If the dependent is not in the Dictionary, then it has no dependees, and the empty setOfDependees's count can be used.
                else
                {
                    return setOfDependees.Count;
                }
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            HashSet<string> setOfDependents = new HashSet<string>();
            if(dependents.TryGetValue(s, out setOfDependents))
            {
                if(setOfDependents.Count > 0)
                {
                    return true;
                }
                //It's possible that the given dependee key is in the Dictionary, but has no dependents.
                else
                {
                    return false;
                }
            }
            //If the dependee key is not in the Dictionary, it has no dependents.
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            HashSet<string> setOfDependees = new HashSet<string>();
            if(dependees.TryGetValue(s, out setOfDependees))
            {
                if(setOfDependees.Count > 0)
                {
                    return true;
                }
                //It's possible that the given dependent key is in the Dictionary, but has no dependees.
                else
                {
                    return false;
                }
            }
            //If the given dependent key is not in the Dictionary, then it has no dependees.
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> setOfDependents = new HashSet<string>();
            if(dependents.TryGetValue(s, out setOfDependents))
            {
                return setOfDependents;
            }
            else
            {
                return setOfDependents;//This should be safe to return, since it's just an empty HashSet, and has no connection to the Dictionary.
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> setOfDependees = new HashSet<string>();
            if(dependees.TryGetValue(s, out setOfDependees))
            {
                return setOfDependees;
            }
            else
            {
                return setOfDependees;
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (!dependents.ContainsKey(s))
            {
                HashSet<string> setOfDependents = new HashSet<string>();//Empty set to add to the dependents Dictionary.
                HashSet<string> setOfDependees = new HashSet<string>();//Empty set to add to the dependees Dictionary.
                setOfDependents.Add(t);

                //This adds the dependee ("s") to both Dictionaries.
                dependents.Add(s, setOfDependents);
                dependees.Add(s, setOfDependees);
            }
            else
            {
                dependents[s].Add(t);
            }
            if (!dependees.ContainsKey(t))
            {
                HashSet<string> setOfDependees = new HashSet<string>();
                HashSet<string> setOfDependents = new HashSet<string>();
                setOfDependees.Add(s);

                //This adds the dependent ("t") to both Dictionaries.
                dependees.Add(t, setOfDependees);
                dependents.Add(t, setOfDependents);
            }
            else
            {
                dependees[t].Add(s);
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependents.ContainsKey(s))
            {
                dependents[s].Remove(t);
                dependees[t].Remove(s);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<string> oldDependents = dependents[s];//Store the old dependent list.
                dependents.Remove(s);//Remove the given key entirely from the Dictionary.
                dependents.Add(s, (HashSet<string>) newDependents);//Cast the parameter newDependents as a HashSet, and add it normally.

                foreach (string dependent in oldDependents)//Iterate over the old dependent list to ensure the dependee Dictionary gets updated.
                {
                    dependees[dependent].Remove(s);//It's not necessary to remove the HashSet entirely, just remove the given dependee.
                }

                foreach (string updatedDependent in newDependents)//For each of the given newDependents, check to see if they're in the dependees Dictionary.
                {
                    if (!dependees.ContainsKey(updatedDependent))
                    {
                        HashSet<string> setOfDependents = new HashSet<string>();
                        setOfDependents.Add(s);
                        dependees.Add(updatedDependent, setOfDependents);
                    }
                    else
                    {
                        dependees[updatedDependent].Add(s);
                    }
                }
            }
            //If the given dependee is not in the dependent Dictionary yet:
            else
            {
                HashSet<string> updatedDependents = (HashSet<string>) newDependents;
                //In the scenario where the given dependee is not in the dependent Dictionary, and the replacement list is empty,
                //only the given dependee needs to be added to both Dictionaries.
                if (updatedDependents.Count == 0)
                {
                    dependents.Add(s, updatedDependents);
                    dependees.Add(s, new HashSet<string>());
                }
                //If it's any other size, then the dependee needs to be added to the dependent Dictionary, and each
                //dependent in the given replacement list also needs to be added to (potentially) both lists, which 
                //AddDependency will take care of.
                else
                {
                    dependents.Add(s, updatedDependents);
                    foreach (string dependent in newDependents)
                    {
                        this.AddDependency(s, dependent);
                    }
                }


            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                HashSet<string> oldDependees = dependees[s];//Stores the old list of dependees.
                dependees.Remove(s);//Remove the given key from the Dictionary.
                dependees.Add(s, (HashSet<string>)newDependees);//Cast the parameter newDependees as a HashSet, and add it normally.

                foreach (string dependee in oldDependees)//Iterate over the old dependee list to ensure the dependent Dictionary gets updated.
                {
                    dependents[dependee].Remove(s);//It's not necessary to remove the HashSet entirely, just remove the given dependee.
                }

                foreach(string updatedDependee in newDependees)//For each of the given newDependees, check to see if they're in the dependents Dictionary.
                {
                    if (!dependents.ContainsKey(updatedDependee))
                    {
                        HashSet<string> setOfDependees = new HashSet<string>();
                        setOfDependees.Add(s);
                        dependents.Add(updatedDependee, setOfDependees);
                    }
                    else
                    {
                        dependents[updatedDependee].Add(s);
                    } 
                }
            }
            //If the given dependent is not in the dependee Dictionary yet:
            else
            {
                HashSet<string> updatedDependees = (HashSet<string>)newDependees;
                //In the scenario where the given dependent is not in the dependee Dictionary, and the replacement list is empty,
                //only the given dependent needs to be added to both Dictionaries.
                if(updatedDependees.Count == 0)
                {
                    dependees.Add(s, updatedDependees);
                    dependents.Add(s, new HashSet<string>());
                }
                //If it's any other size, then the dependent needs to be added to the dependee Dictionary, and each
                //dependee in the given replacement list also needs to be added to (potentially) both lists, which 
                //AddDependency will take care of.
                else
                {
                    dependees.Add(s, updatedDependees);
                    foreach (string dependee in newDependees)
                    {
                        this.AddDependency(dependee, s);
                    }
                }
            }
        }
    }
}
