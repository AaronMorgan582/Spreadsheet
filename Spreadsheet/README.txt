Author:     Aaron Morgan and Xavier Davis
Partner:    None
Date:       2/22/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #6 - Spreadsheet GUI
Copyright:  CS 3500, Xavier Davis and Aaron Morgan - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 15 hours.

    Time spent on Analysis: 7 hours.
    Time spent on Implementation: 4.5 hours.
    Time spent Debugging: 2.0 hours.
    Time spent Testing: 2 hours.

    Total Time: 14 hours.


3. Consulted Peers:

    1.

4. References:
    
    1. Microsoft: KeyPressEventArgs.KeyChar Property https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keypresseventargs.keychar?view=netframework-4.8#System_Windows_Forms_KeyPressEventArgs_KeyChar
    2. C Sharp Corner: Dialog Boxes In C# https://www.c-sharpcorner.com/article/dialog-boxes-in-c-sharp/
    3. Stack Overflow: SaveFileDialog setting default path and file type? https://stackoverflow.com/questions/5136254/savefiledialog-setting-default-path-and-file-type
    4. Stack Overflow: How to create an array of tuples? https://stackoverflow.com/questions/20490884/how-to-create-an-array-of-tuples
    5. C Sharp Corner: C# Message Box https://www.c-sharpcorner.com/uploadfile/mahesh/understanding-message-box-in-windows-forms-using-C-Sharp/
    6. Stack Overflow: Checking if a string array contains a value, and if so, getting its position https://stackoverflow.com/questions/7867377/checking-if-a-string-array-contains-a-value-and-if-so-getting-its-position
    7. Microsoft: Form.Closing Event https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.closing?view=netframework-4.8

5. My Software Practices:

    1.

    2.

    3. 

=======Version 5.0=======
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

=======Version 4.0=======
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
