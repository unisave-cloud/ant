#!/usr/bin/env bash

TARGET_SERVER="http://localhost:5000"

#python3 build_request.py | cat
#exit

curl -v \
    -X POST \
    -H "Content-Type: application/json" \
    -d "$(python3 build_request.py)" \
    "${TARGET_SERVER}/execute-job"
