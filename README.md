# Development Exercise Overview
In our development process, we hold in high regard the use of SOLID design principles, design patterns, and _pragmatic_ unit and integration testing practices. To ensure that you are a good fit for our development style and requirements, we would like you to demonstrate your ability to put these concepts into practice as you develop and _evolve_ software.

Since we also use git for source code control, we would like for you to fork our repository at GitHub and then submit your code for review through commits on _Pull Requests_ back to our repository (details below). Additionally, we may use the online code review features of GitHub to ask questions and discuss your approach with you.

## Source Control Guidelines
You should start by creating a fork of our repository at GitHub, create a branch for your changes (using the format of `{yourInitials}-{featureName}`), and when your work is ready for review then open a Pull Request back to our repository.  Generally try to work with source control as you would on real project, providing concise (but helpful) comments with your commits, and trying to keep commits fairly focused (avoiding commits with multiple unrelated types of changes included).

## General Development Guidelines
The focus of this exercise is primarily on software design and evolution, and not about writing fully optimized code.  For example, the `CountryCodeRepository` class that is the subject of this exercise should be persisted to disk in CSV form and you should not be concerned with the performance implications of using a simple text file as your backing storage.

## General Testing Guidelines
Writing unit and/or integration tests is very important to us.  However, sometimes writing these tests can actually take more time than developing the code under test (which is why many people don't bother).  In the interest of both your time and ours, we would like you to demonstrate just a representative sampling of unit and integration tests, and then simply "stub" out the other scenarios and test methods that you feel should be added to adequately test the requirements (i.e. create the classes and methods on the test fixtures following the style we have provided in _SampleTests.cs_, but don't actually feel compelled to implement them).

## Development Assignment
In this exercise, you will build a simple `CountryCodeRepository` class capable of reading and writing country codes and names to and from persistent storage.

### Feature Name: CountryRepository

The requirements are as follows:
- The `Country` class should contain `Code` and `Name` properties, both typed as `string`.
    - Codes should be unique across all Countries in the repository.
    - Codes should be handled in a case-insensitive fashion, but be stored and return as upper-case.
    - Codes should be a 3-character string of letters (e.g. `USA`, `CAN`, `MEX`).
    - User-assigned codes cannot be added to the repository.  User-assigned codes are defined by the following ranges: AAA to AAZ, QMA to QZZ, XAA to XZZ, and ZZA to ZZZ
    - For the purpose of this exercise, you can assume that all Names will be simple string values (not multi-line strings).
- The repository should immediately save changes to disk in a text file using a CSV (Comma Separated Values) format.
- The physical storage should be partitioned into separate CSV files based on the first character of the Code (e.g. _A.csv_, _B.csv_, etc.).
- The files should be stored in the directory returned by the following code:
```csharp
string dataPath = Path.Combine(
    System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
    "Interview-Exercise"); // i.e. C:\ProgramData\Interview-Exercise
```
- The following repository methods should be supported:
```csharp
void Add(Country country);
void Update(Country country);
void Delete(string countryCode);
Country Get(string countryCode);
void Clear();
```
- `Add` should create new entries, or throw an exception if the Country already exists.
- `Update` should update an existing entry, or throw an exception if the Country doesn't exist.
- `Delete` should delete the existing entry, or throw an exception if the Country doesn't exist.
- `Get` should retrieve an existing entry, or throw an exception if the Country doesn't exist.
- `Clear` should remove all entries from the repository.
