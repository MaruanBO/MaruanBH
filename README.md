# MaruanBH

This project is built using C# 8.0 and .NET Web API, following enterprise-level architecture patterns, including:

- **Github CI Pipeline** A CD pipeline was not provided as the application is intended for local or development environments and does not require deployment to production or staging. The focus was on setting up a CI pipeline to ensure automated testing and integration of code changes
- **Domain-Driven Design (DDD)**
- **Test-Driven Development (TDD)**
- **Command Query Responsibility Segregation (CQRS)** with command/query validation
- **S.O.L.I.D principles**
- **Mediator Pattern**
- **Functional Programming principles** Applied in Services, Repositories, Entities (chained functions style) and CQRS (Only on GetCustomerDetailsQueryHandler as showcase ).
- **Logging**: We utilizes file-based logging to track application behavior and issues, facilitating easier debugging and monitoring for **showcase purposes** (Usually we should use **Event sourcing** with **Kafka** data pipeline).
- **Error Handling** We implements general error handling strategies to log system messages and errors for **showcase purposes**.
- **Testing coverage**  In business, repositories core layers.
- **Api Documentation (Swagger)**

The data is stored in memory, allowing for easier testing and evaluation, and the application uses a multi-layered architecture with proper abstractions for testability.

---

## API Endpoints

All data is exposed through the Web API to facilitate testing purposes, including the following endpoints:

- **Create Account**: Endpoint to create a new account associated with a customer.
- **Get Account Details**: Endpoint to retrieve account details, including information about transactions, balance and associated customer.
- **Create Customer**: Endpoint to create a new customer in the system.
- **Get Customer**: Endpoint to retrieve customer details. This is provided for demonstration and user experience purposes.


## Setup and Running the Application

### Prerequisites

Ensure you have the following installed on your machine:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/MaruanBH.git
cd MaruanBH
```

### 2. Build the project

To build the solution, run the following command:

```bash
dotnet build MaruanBH.sln --configuration Release
```

### 3. Run the application

You can run the API by using:
```bash
dotnet run --project MaruanBH.Api
```

The Swagger API should now be running at http://localhost:5118/swagger/index.html.

### 4. Testing

The test are located at **MaruanBH.Tests directory**, to execute the tests, run the following command:

```bash
dotnet test MaruanBH.Tests/MaruanBH.Tests.csproj
```

### License

This project is licensed under the MIT License.

> This project was created by Marouane Boukhriss Ouchab, including logic, directory structure, architecture, coding, testing, and software engineering decisions.
