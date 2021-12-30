

[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/tterb/atomic-design-ui/blob/master/LICENSEs)

# Hotel Reservation With TDD

I prepared this project to teach my students writing test with the TDD method in my private classes. Although to learn TDD you have to be in the class to get a deep undersanding of TDD, nevertheless, this project gives you many techniques for writing test.
## What You Will Learn
- Writing test for Razor Pages
- Writing test for Repositories
- Writing test for Services
- Naming Conventions
- Mocking Services

## Prerequisites
- .Net 6
- .VS 2022
## Acknowledgements

 - [The Art of Unit Testing ](https://www.amazon.com/Art-Unit-Testing-Examples-NET/dp/1933988274)
 - [Unit Testing for C# Developers](https://codewithmosh.com/p/unit-testing-for-csharp-developers)
 
## Tech Stack

**Client:** Razor, Html, Css, Bootstrap

**Server:**.Net 6, ASP.Net Core (Razor Page),Ef Core, SQLLite, XUnit


## Run Locally

Clone the project

```bash
  git clone https://github.com/mohammad-niazmand/TDD-Course-HotelReservation.git
```

Go to the project directory

```bash
  cd TDD-Course-HotelReservation
```

Install dependencies

```bash
  dotnet build
```

Start the server

```bash
  dotnet run --project Hotel.Web
```


## Running Tests

To run tests, run the following command

```bash
  dotnet test
```

