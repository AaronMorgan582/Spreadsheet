Author:     Aaron Morgan
Partner:    None
Date:       2/14/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #5 - Spreadsheet 2.0
Copyright:  CS 3500 and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 15 hours.

    Time spent on Analysis: 6 hours.
    Time spent on Implementation: 3 hours.
    Time spent Debugging: 0 hours.
    Time spent Testing: 0 hours.
    Total Time: 9 hours.

3. Consulted Peers:

    1. Alejandro Serrano: Discussion of how to use the :base in the Constructors, as well as reading/writing XML. He also described how he used Evaluate.
    2. Xavier Davis: Discussion of reading/writing XML, when to use Evaluate, and the "whiteboard" section of the assignment.

4. References:
    
    1. C# Corner: Reading and Writing XML in C# https://www.c-sharpcorner.com/article/reading-and-writing-xml-in-C-Sharp/
    2. Microsoft: base(C# reference) https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base

5. My Software Practices:


Author:     Aaron Morgan
Partner:    None
Date:       2/1/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #4 - Spreadsheet
Copyright:  CS 3500 and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 15 hours.

    Time spent on Analysis: 5.5 hours.
    Time spent on Implementation: 4 hours.
    Time spent Debugging: 2.5 hours.
    Time spent Testing: 2.5 hours.
    Total Time: 14.5 hours.

3. Consulted Peers:

    1. Alejandro Serrano: Discussion of how to create SetCellContents(Formula), and how the Circular Exception should be handled. He also
    provided ideas for testing purposes (such as testing for indirect dependents). He also provided the information that methods that return 
    IEnumberables can be used in Constructors, such as HashSet<string> set = new HashSet<string>(graph.GetDependents).

4. References:

    1. Dot Net Perls: C# Property Examples (get, set) https://www.dotnetperls.com/property
    2. Microsoft: C# Dictionary<TKey,TValue>.Keys Property https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keys?view=netframework-4.8 

5. My Software Practices:

    1. I noticed that the implementation of SetCellContents was roughly the same for each input, regardless of whether it was
    a double, a string, or a Formula. For that reason I decided to implement a helper method that takes an object in order to generalize
    the procedure.

    2. Despite all of the SetCellContents methods being similar, there were specific differences with SetCellContents(Formula),
    namely the checking of circular dependencies. I decided to write comments describing the process to check for those
    dependencies.

    3. In my testing, I knew that there were special cases that the comments and instructions did not explicitly illustrate.
    Even though the header comments in AbstractSpreadsheet mentioned "direct or indirect dependents", the example it gave
    did not explicitly illustrate an indirect dependency, so I knew I had to test for it. Replacing a cell with something else
    was also something I noticed I needed to test for, because if a cell had been set to a Formula, but then replaced with
    a double, it wouldn't be a dependent anymore.
