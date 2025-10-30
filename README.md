# zurbit-server

## Technologies
- [.NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis)
- [FastEndpoints](https://fast-endpoints.com/)
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)

## Docker
### Building and Running with Docker

Build the Docker image:
```shell
docker build -t zurbit.server -f src/Zurbit.Server/Dockerfile .
```

Run the container:
```shell
docker run -p 8080:8080 -p 8081:8081 zurbit.server
```

### Using Docker Compose

Build and start the services:
```shell
docker compose up --build
```

Run in detached mode:
```shell
docker compose up -d
```

Stop the services:
```shell
docker compose down
```

View logs:
```shell
docker compose logs -f
```
