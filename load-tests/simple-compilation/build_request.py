import json

with open("Dockerfile", "r") as f:
    dockerfile = f.read()

json_string = json.dumps({
    "image": {
        "dockerfile": dockerfile,
        "context": [
            {
                "path": "job.sh",
                "content": "echo hello!"
            }
        ]
    },
    "results": [],
    "resources": {}
}, indent=2)

print(json_string)
