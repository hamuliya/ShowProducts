The backend of this website is built with ASP.NET Core, while the frontend uses React. The backend is structured in three layers: Database, DataAccess, and WebAPI. The Database layer manages connections to the SQL Server database and defines tables, relationships, and stored procedures. The DataAccess layer uses Dapper to efficiently perform data search, insertion, and updates. The WebAPI layer acts as an interface for the frontend to access data and supports the read and write operations for the website. To enhance security, the WebAPI layer uses slow hashing with a salt value to store passwords .And it also implements authentication and authorization to improve the website's security.

The frontend, built with React, uses the React Connect API to upload and display  photos.