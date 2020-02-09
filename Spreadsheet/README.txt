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

=======Version 3.0=======
Author:     Aaron Morgan
Partner:    None
Date:       1/27/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #3 - Formula
Copyright:  CS 3500 and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

   None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 12 hours.

    Time spent on Analysis/Design: 6.0 hours.
    Time spent on Implementation: 7.0 hours.
    Time spent Debugging: 1.0 hours.
    Time spent Testing: 3.0 hours.
    Total Time: 17 hours.

3. Consulted Peers:

    1. Alejandro Serrano: Discussion on both constructors, what the Evaluate method should do, and how the parsing rules should be implemented.
    He also gave suggestions on how to condense the code in the Evaluate method.

4. References:

    1. StackOverflow: Casting IEnumerable<T> to List<T> https://stackoverflow.com/questions/961375/casting-ienumerablet-to-listt
    2. Microsoft: Type.IsInstanceOfType(Object) Method https://docs.microsoft.com/en-us/dotnet/api/system.type.isinstanceoftype?view=netframework-4.8

=======Version 2.0=======
Author:     Aaron Morgan
Partner:    None
Date:       1/20/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #2 - Dependency Graph
Copyright:  CS 3500 and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 15 hours.

    Time spent on Analysis: 3 hours.
    Time spent on Implementation: 3.5 hours.
    Time spent Debugging: 1 hour.
    Time spent Testing: 5 hours.
    Total Time: 12.5 hours.

3. Consulted Peers:

    1. Alejandro Serrano: Discussion of what data structure to use, as well as how to determine the size of the Dependency Graph. Provided
    the idea to test a new addition to each Dictionary via Replace, but with an empty list.

4. References:

    1. C# Dictionary API - https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.8
    2. C# HashSet API - https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=netframework-4.8
    3. C# Casting - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions


=======Version 1.0=======
Author:     Aaron Morgan
Partner:    None
Date:       1/10/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #1 - Formula Evaluator
Copyright:  CS 3500 and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 10-15 hours.
    Total Time: 12.0

3. Consulted Peers:

    1. Alejandro Serrano: Provided a test case expression of 2 + 5*(2 + 3), and (2 * 3) / 2 * 10 + (10 - 1). We also discussed how 
    the variables in the Evaluator would be used and looked up.
    2. Xavier Davis: Discussion of the Exception Handling, recommended the usage of Regular Expressions to identify variable Strings.

4. References:

    1. StackOverflow - Identify if a string is a number https://stackoverflow.com/questions/894263/identify-if-a-string-is-a-number
    2. StackOverflow - C# Regex: Checking for “a-z” and “A-Z” https://stackoverflow.com/questions/6017778/c-sharp-regex-checking-for-a-z-and-a-z
