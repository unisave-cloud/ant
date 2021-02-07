# Ant

Ant is a code execution system built on Docker.

There are couple of request types the system can serve:
- "compile this dockerfile, run the container and return some files"
  (compilation) = **JOB CONTAINER**
- run this request on any reusable image of this type (facet request) =
  **SNACK CONTAINER**
- create a container and keep it running as long as I ping you
  (long-running-commands) = **SERVICE CONTAINER** (and give me the IP
  address and port mapping)

> **Note for future:** For services and commands check out
> https://github.com/shellinabox/shellinabox


## Running jobs

> Job takes couple of seconds to run, has a dedicated container and
> higher latency and computes some output, given an input.

- specify an image (by name or dockerfile)
- create a container from the image and run to completion
- returned requested filed from the resulting container
- destroy the container


## Running snacks

> A snack is like a job, but it reuses one container to decrease latency
> and overhead. Can be parallelized by having multiple containers.

- prepare an image for the snack execution
- obtain a container running the snack image, unpause it
- run the snack
- free up the snack container and pause it


## Running services

> A service is a standalone container with a process inside that runs
> for as long as the client wants. The client communicates with the
> service running inside the container directly.

- prepare an image for the service container
- start the service container
- keep it running for as long as the client pings us


## Security

Docker image building is not resource limited, so it shouldn't contain
untrusted code. However a job, a snack or a service are all resource
limited and so can execute untrusted code.
