# Prerequisites for Local Environment Setup & Running the Project  

Before building and running this project locally, ensure you have the following prerequisites installed and configured:  

---

## 1. .NET SDK  

- **.NET 9.0 SDK** or later    
  [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- **Verify installation:**  
  dotnet --version  

---  

## 2. Node.js  

- **Node.js v22.x**    
  [Download Node.js 22](https://nodejs.org/en/download/)  
- **Verify installation:**  
  node --version  
  The output should be v22.x.x.  

---

## 3. Database  

- **SQL Server** (Express, Developer, or Docker container)    
  [Download SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- **Or run via Docker:**  
  docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest  

- **Connection String:**  
  Update your appsettings.json with your local SQL Server connection string.  

---

### Entity Framework Core Tools  

For running migrations and updating the database:  

dotnet tool install --global dotnet-ef  

---

## Local Setup & Running the Project  

1. **Clone the repository:**  
   git clone <your-repo-url>  
   cd <your-project-folder>  

2. **Restore dependencies:**  
   dotnet restore  

3. **Apply database migrations:**  
   dotnet ef database update  

4. **Update appsettings.json:**    
   Set your SQL Server connection string under ConnectionStrings.  

5. **Build and run the project:**  
   dotnet build  
   dotnet run  

6. **Access the application:**    
   The default URL and port will be shown in the console output.  

---  