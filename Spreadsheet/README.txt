Author:     Aaron Morgan and Xavier Davis
Partner:    None
Date:       2/22/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #6 - Spreadsheet GUI
Copyright:  CS 3500, Xavier Davis and Aaron Morgan - This work may not be copied for use in Academic Coursework.
Github Repository: https://github.com/uofu-cs3500-spring20/assignment-six-completed-spreadsheet-see-sharp-run

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    Expected Time to Complete: 15 hours.

    Time spent on Analysis: 7 hours.
    Time spent on Implementation: 5.5 hours.
    Time spent Debugging: 2.5 hours.
    Time spent Testing: 2.5 hours.

    Aaron's time spent on comments/variable names/organization: 1.5

    Total Time: 19 hours.

    Our estimate was too optimistic, but our longer time was primarily due to a single bug that we were encountering when opening an existing spreadsheet. It
    took us a significant amount of time to figure out what the bug was, but it was very easy to change once we identified it.

    We think our abilities in estimating are about the same as previous work (not really getting better or worse), but we noticed we struggle the most identifying how 
    to properly utilize the tools that we were expected to use (specifically, in this assignment, how to work with events). Once we figured out how the tools worked, 
    we mostly didn't have a problem with implementation.


3. Consulted Peers:

    None.

4. References:
    
    1. Microsoft: KeyPressEventArgs.KeyChar Property https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keypresseventargs.keychar?view=netframework-4.8#System_Windows_Forms_KeyPressEventArgs_KeyChar
    2. C Sharp Corner: Dialog Boxes In C# https://www.c-sharpcorner.com/article/dialog-boxes-in-c-sharp/
    3. Stack Overflow: SaveFileDialog setting default path and file type? https://stackoverflow.com/questions/5136254/savefiledialog-setting-default-path-and-file-type
    4. Stack Overflow: How to create an array of tuples? https://stackoverflow.com/questions/20490884/how-to-create-an-array-of-tuples
    5. C Sharp Corner: C# Message Box https://www.c-sharpcorner.com/uploadfile/mahesh/understanding-message-box-in-windows-forms-using-C-Sharp/
    6. Stack Overflow: Checking if a string array contains a value, and if so, getting its position https://stackoverflow.com/questions/7867377/checking-if-a-string-array-contains-a-value-and-if-so-getting-its-position
    7. Microsoft: Form.Closing Event https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.closing?view=netframework-4.8
    8. Stack Overflow: Show label text as warning message and hide it after a few seconds? https://stackoverflow.com/questions/15951689/show-label-text-as-warning-message-and-hide-it-after-a-few-seconds

5. My Software Practices:

    1. Versioning: We have multiples commits (around 20 we believe), with expressive descriptions that detail important changes. This was facilitated by how
       our Pair Programming strategy worked; we wanted to switch off writing code every hour.

    2. Interface: We think the interface for the spreadsheet is simple and intuitive, because we thought about the layout of the buttons and the textbox; we
       wanted to ensure that the spreadsheet button layout (in the Menu buttons) were similar to Excel or Google Sheets. When we were implementing the "Autosave"
       label that's drawn on the grid when the program autosaves, we thought it would be best to leave it to the right of the textbox, rather than putting it
       in front of the textbox or the Save Button.

    3. Code Complexity/Separation of Concerns: We knew that the saving functionality was going to occur in multiple places (using the Save Button, clicking Save As...,
       upon closing the application), so we decided to make a private method called "Save" to display the SaveDialogBox, and call the method whenever it was 
       necessary.

6. Partnership:

    All code was created via Pair Programming. Aaron did work on his own once or twice for a little less than an hour, simply writing comments, renaming a couple
    of variables, and organizing some of the code.

7. Additional Features and Design Decisions:

    We decided to not use the "TextChanged" event for the textbox because it was making the display of the contents/values difficult. We originally thought it would
    be ideal to keep the grid updated as the user typed along the textbox, but then thought it looked better if it didn't; it forces the user to remember to press
    "Enter" to enter the value on the grid.

    We chose pressing "Enter" to be the event to enter values to the grid because we were both accustomed to pressing Enter when using Excel or Google Sheets. We
    thought about using a Click event, but considering the user may not have access to a mouse, we thought Enter would be a safer bet.

    We organized some of the Menu Buttons to be the exact same way as other applications (File -> Open, Save As..., Close) because that order is what we were
    both most familiar with.

    When we implemented the "Help" Menu Button, we chose not to have a drop-down menu associated with it, because it seemed a little pointless to only have a 
    drop-down with a single item. Instead, we chose to implement a simple message box displaying the necessary information.

    Additional Feature:

    We implemented an autosave feature in our spreadsheet. When saved, a timer will begin so that every 30 seconds the file is automatically saved. There is
    a label that displays "Autosaving..." when the spreadsheet has been autosaved.
    
8. Best Team Practices:

    The partnership was most effective when we were working on one computer instead of trying to work separately. One person was giving advice and checking code
    (for consistency and errors), while the other was writing the code, which resulted in cleaner code. We switched off roughly once an hour, which proved effective
    because it allowed us to keep focus for longer periods of time.

    We didn't assign specific tasks to either of us individually (since we worked together throughout the majority of it), but the "Issue" system in Github was
    useful because it allowed us to see what we still needed to accomplish for the assignment.

    We have been working together as partners ever since CS 2420, and at this time, we can't think of any way to improve our partnership. We're both familiar
    with each other's working habits, and there hasn't been any problems that have arisen in any of the assignments that we've worked on
    together (including this one).

9. Testing:

    When initially setting to the grid, we knew that the GetSelection method was returning a (x, y) coordinate that started at (0,0). To ensure that values
    were being set in the appropriate cells, we made sure to test that the cells at the edges (A1, A99, Z1, and Z99) were displaying correctly.

    We considered dependent formulas, and wanted to make sure that multiple dependencies were correctly being set on the grid, and that the values were being updated
    immediately whenever possible. For example, if the A1 cell had "=B1" in it, and B1 had "=C1" and C1 had "=D1", if "5" was entered in D1, we wanted to ensure
    that A1, B1, and C1 were immediately set to the value of 5.

    There were errors that we needed to account for, and we wanted to display a message when they occured. For example, we ensured there was a message box when
    trying to enter a formula with a circular dependency; we display a message when the formula has an improper format (such as 5@2); we also a display a message
    when the user tries to open an improper file type.

    We tested Open, all the Save functionality (via the Save Button, Save As..., and upon close) multiple times. For Open, we also made sure that the "edge case" cells
    (A1, Z99, etc.) were all getting set correctly when opened. We tested to ensure that when opening a new window, closing the original window did not close all
    the other windows.

    We tested to see if there was a warning when trying to overwrite a previously saved file, as well as trying to open a file with an invalid file path.

    We tested to ensure that the "Autosave" label was appearing correctly after saving, every 30 seconds. We also made sure that it wasn't appearing if the spreadsheet
    hasn't been saved.

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
