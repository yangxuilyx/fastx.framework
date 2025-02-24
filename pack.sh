#!/bin/bash

API_KEY="$NUGET_API_KEY"
NUGET_SOURCE="https://api.nuget.org/v3/index.json"
OUTPUT_DIR="./nupkgs" 
VERSION="$NUGET_VERSION"

dotnet pack \
    -c Release \
    -o $OUTPUT_DIR \
    -p:Version=$VERSION
    --include-symbols \
    -p:SymbolPackageFormat=snupkg

find $OUTPUT_DIR -name "*.nupkg" | while read package; do
  dotnet nuget push "$package" \
    -k $API_KEY \
    -s $NUGET_SOURCE \
    --skip-duplicate              # 避免重复包错误[1,6]
done