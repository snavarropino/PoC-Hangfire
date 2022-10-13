# PoC-Hangfire

Hangfire PoC. We want to validate:

1. We can run two servers
2. Each server is in charge of a sort of tasks
3. We can manage the number of concurrent task in each server

## Preconditions

- .NET 6
- Sql Server

## Run locally

### Create storage

We need an empty database named **PoC-Hangfire** in your default instance of your sql server

If case you wany to use another sql server, please modify the connection string in the three hosts (appsettings.json)

### Install dependencies and build

In the repository root directory execute

```cmd
dotnet restore
dotnet build
```

### Run server 1 (long running tasks)

```cmd
cd src\Server1
dotnet run
```

### Run server 2 (short lived tasks and default tasks)

```cmd
cd src\Server2
dotnet run
```

### Run task queuer (client api)

```cmd
cd src\ClientApi
dotnet run
```

This api listens at port 7291 and offers Swagger in following url

```url
https://localhost:7291/swagger/index.html
```

## Queueing tasks

### Long tasks

```cmd
curl -X POST "https://localhost:7291/Long?Id=1&Duration=60"
```

Duration is in seconds

### Short tasks

```cmd
curl -X POST "https://localhost:7291/Short?Id=1"
```

For short tasks the duration is randomly set by the server.

### Recurrent task

```cmd
curl -X POST "https://localhost:7291/Easy"
```

This start a recurrent task that will be executed minutelly. This task has no custom queue so it is queued to "Default" queue (executed by server 2)

## DEMO

When we enqueue a long task with 60 seconds of duration:

- If we enqueue another long task, the new task is not started until the first one is finished
- If we enqueue short tasks, they are axecuted inmediatly even if the long task is not finished.

When we enqueue several short tasks

- They always are executed

## Considerations

This sample use three different hosts, one that enqueue tasks, and two background processing hosts. This requires following considerations:

- All of your Client/Servers use the same job storage
- All of your Client/Servers have the same code base

Official documentation:
<https://docs.hangfire.io/en/latest/background-processing/placing-processing-into-another-process.html>
