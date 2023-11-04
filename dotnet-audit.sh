#!/usr/bin/env bash

dotnet restore

output=$(dotnet list package --vulnerable)

echo "$output"

echo "$output" | grep "following vulnerable" 
if [[ $? == 0 ]]; then
  exit 1
fi
