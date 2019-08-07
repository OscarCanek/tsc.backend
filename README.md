# TSC Backend

This API allow you handle countries it's subdivisions. The database used was SQL Server on Linux, you can change the connection string in the `appsettings.json` file of the tsc.backend project.

```json
"ConnectionStrings": {
        "tsc": "Server=<server>;Database=<database name>;User Id=<username>;Password=<password>"
}
```

The database script can be found at the `db.sql` file.


### Run the API
To run the API you need to go to the tsc.backend and run in a terminal the following command:

```
dotnet run
```

The API will start listening at:

- http://localhost:5000
- https://localhost:5001

The two services can be found at:

- api/countries
- api/subdivisions

Examples of how to consume those API's can be found at the `TSC.postman_collection.json` file.

## Extras

You can find the Swagger page at `https://localhost:5001/swagger/index.html`