# SuperHeroAPI

### A simple Dotnet Restful API that allows a user to search for a Superhero, returning information on the Superhero from https://superheroapi.com/ and allowing a user to store favourite Superhero's. The user should be able to get a list of favourites back from the Restful API.

- Reading information from https://superheroapi.com/
- Simple user management using Identity
- Using SQLite
- Favourite superheroes stored and read per user

#

How to run:

1. `git clone https://github.com/magedragheb/SuperHeroAPI`
2. `dotnet ef database update`
3. `dotnet user-secrets` to add your API token and name it `superheroapitoken`
4. `dotnet run`

#

1. Hosting:
   This app runs on dotnet 8 and can be hosted on any supported windows or linux enviroment.
2. Scalability:
   From infrastructure point, it can be deployed to a serverless function to scale on demand, from design perspective it is already structured and ready to improvements like adding layers and tests.
3. Design Approach:
4. Explain the code:
5. Documentation:
