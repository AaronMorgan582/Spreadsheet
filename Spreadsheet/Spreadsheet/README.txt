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

    Time spent on Analysis: 8 hours.
    Time spent on Implementation: 5 hours.
    Time spent Debugging: 3 hours.
    Time spent Testing: 3 hours.

    Total Time: 19 hours.

    I think that the estimation of my time has been mostly what I expected for each Assignment; although my actual time and expected
    time have not matched up on occasion (such as this Assignment), that is mostly because I get stuck on one particular aspect of the
    Assignment that I don't know how to solve. For this Assignment, it took me significantly longer than expected to figure out how
    to use the XML Read/Write. It also wasn't very clear how Evaluate was supposed to work with this Assignment (instructions never
    mentioned it, thus expected time didn't account for it), so it took me a while to figure out how to correctly evaluate a Formula.

3. Consulted Peers:

    1. Alejandro Serrano: Discussion of how to use the :base in the Constructors, as well as reading/writing XML. He also described how 
    he used Evaluate.
    2. Xavier Davis: Discussion of reading/writing XML, when to use Evaluate, and the "whiteboard" section of the assignment.

4. References:
    
    1. C# Corner: Reading and Writing XML in C# https://www.c-sharpcorner.com/article/reading-and-writing-xml-in-C-Sharp/
    2. Microsoft: base(C# reference) https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base

5. My Software Practices:

    1. I used Regression Testing to ensure that all the previous tests that I had from Assignment 4 were still working correctly.
    I thought it was important to do that with this Assignment, because the methods themselves haven't changed at all, but how
    the Spreadsheet class uses them has changed.

    2. I decided to break up setting a Cell's Value (if it's a Formula) into a private method called Recalculate. I think it's
    a pretty small method that is easy to understand, and I felt it made sense because it has specific functionality (setting
    the Value), which might be utilized in some other scenario.

    3. I'm not sure what this would be considered (Abstraction maybe), but prior to this Assignment, it never occurred to me
    the reason why we needed a delegate for Formula's Evaluate. It was kind of hinted at that "other developers may have different a
    variable", which I understood, but I didn't know how that was applied. When I wrote the LookUp method in Spreadsheet, I 
    realized that variables for a Spreadsheet specifically are the Cell names; we don't really have any functionality that
    defines a Formula with "normal" Mathematical terms, such as "x+y", but there may be some other Class that may want to use
    Formula, and that Class will internally define its own LookUp to be able to evaluate "x+y".

=======Assignment #4=======
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
