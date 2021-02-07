# Unisave usage - facet calls

> This also applies to scheduler and HTTP endpoints.

A client requests Unisave to call a facet method. Unisave validates all
the parameters and goes on to execute the facet.

Backend ID `ABCDEF` is mapped onto snack ID `backend-ABCDEF`.

    1. try executing the appropriate snack
    2. if it fails due to 404, create the snack image and retry


## Snack execution

Unisave sends the following HTTP request at `POST
/snacks/backend-ABCDEF/execute`:

```json
{
    "snackParameters": "[parameters for Unisave framework]"
}
```

In case of 200, the response is sent to the client. In case of 418 a
fake exception is created and sent to the client. In case the service is
unreachable a 503 response is sent to the client (same as downtime or
maintenance mode) and the occurence is logged. In other cases a special
fake exception is sent to the client and the occurrence is logged.


## Snack image creation

Unisave sends the following HTTP request at `PUT /snacks/backend-ABCDEF`:

```json
{
    "image": {
        "dockerfile": "[Dockerfile]",
        "context": [
            "[context files here]"
        ]
    },
    "resources": {
        "...": "[resource limits to which Unisave is configured]"
    },
    "...": "[restart conditions for a snack container]"
}
```

The dockerfile:

```Dockerfile
FROM mono:6.4.0

WORKDIR /app

# download compiled game assemblies
ADD "https://.../Gme.dll?=.." Game.dll

# download framework assemblies
ADD "https://.../Fwk.dll?=.." UnisaveFramework.dll

# ADD the snack container manager process

# TODO TODO TODO ... missing details

EXPOSE 80

ENTRYPOINT ["mono", "--debug", "/app/Sandbox.exe"]
CMD []
```
