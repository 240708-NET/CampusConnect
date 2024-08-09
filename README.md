# CampusConnect
CampusConnect is a platform designed to facilitate blog creation, user interaction, and profile management. The platform balances ease of use with comprehensive administrative tools, providing a smooth experience for both casual bloggers and site administrators.

### Key functionalities
+ Post Viewing: The front page showcases all posts, with options to filter by categories and tags.
+ Post Management: Users can create, edit, and delete their posts. Admins have full control to manage all posts.
+ Comment System: Users can comment on posts, reply to comments, and manage their own comments. Post owners and admins have additional moderation capabilities.
+ User Profiles: Profiles include personal details and a history of posts and comments.
+ User Management: Users can create accounts and log in to access features.

### Prerequisites
* .NET SDK 8.0
* Node.js v20.16
* Microsoft SQL Server 2022
    + To create the server with Docker:
        ```sh
        docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<yourPassword>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
        ```

### Getting Started
* Server
    1. Enter the `BlogAPI` directory
        ```sh
        cd src/BlogAPI
        ```
    2. Set up the `ConnectionString` user secret:
        ```sh
        dotnet user-secrets init
        dotnet user-secrets set "ConnectionString" "Server=localhost; Database=<yourDatabase>; User Id=sa; Password=<yourPassword>; TrustServerCertificate=True"
        ```
    3. Run the migrations to initialize the database: 
        ```sh
        dotnet ef database update
        ```
    4. Start the API server:
        ```sh
        dotnet run
        ```
    5. View the Sagger UI at `https://localhost:<port>/swagger`.

* Client
    1. Enter the `frontend` directory:
        ```sh
        cd frontend
        ```
    2. Install frontend dependencies:
        ```sh
        npm install
        ```
    3. Start the frontend server:
        ```
        npm run start
        ```
