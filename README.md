# PoC-Hangfire

Hangfire PoC. We want to validate:

1. We can run two servers
2. Each server is in char of a sort of task
3. We can manage the number of concurrent task in each server

## Preconditions:

.NET 6

## Run locally

### Install dependencies and build

In the repositoru root directory execute
```cmd
dotnet restore
dotnet build
```

### Run server 1 (long running tasks)

TODO

### Run server 2 (short lived tasks)

TODO

### Run task queuer (client)

TODO

### Queue tasks

Long task:

POST /Api/Long?Id=x

Optionallly you can send the task duration in seconds

POST /Api/Long?Id=x&diration=60

Short task:

POST /Api/Short?Id=x

