# DotNetCoreAngularSpa

Created using dotnet new angular

Used to analyse the operation of the Spa middleware

Removed "ASPNETCORE_ENVIRONMENT": "Development" entry from Properties/launchSettings.json to allow running in production mode by using env vars.

To run in Development mode

```sh
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

To run in Production mode first build the Angular app in prod mode:

```sh
cd ClientApp
npm run build -- --prod
cd ..
ASPNETCORE_ENVIRONMENT=Production dotnet run
```
