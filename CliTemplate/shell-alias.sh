#!/bin/bash

# Simplified version of the PowerShell wrapper script. It automatically rebuilds the CLI binary if the git version
# has changed, but does not have a manual re-build switch (we can still just delete the build.version file).
# Fortunately, argument handling and STDIN forwarding is much easier in Bash.

echo "CliTemplate" > /dev/tty
CLI_PATH="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"
echo "  PATH: $CLI_PATH" > /dev/tty

[ -e "$CLI_PATH/build.version" ] && BUILD_VERSION=$(< "$CLI_PATH/build.version")
echo "  VERSION: $BUILD_VERSION" > /dev/tty

CLI_TAG=$(git -C $CLI_PATH describe --tags --dirty --always)
echo "  TAG: $CLI_TAG" > /dev/tty

if [ "$BUILD_VERSION" != "$CLI_TAG" ]; then
    echo "Building binary with version $CLI_TAG" > /dev/tty
    dotnet publish "$CLI_PATH/CliTemplate.csproj" > /dev/tty
    echo $CLI_TAG > "$CLI_PATH/build.version"
fi

echo "---" > /dev/tty

if [ -t 0 ]; then
    "$CLI_PATH/bin/Release/net9.0/publish/CliTemplate" "$@"
else
    "$CLI_PATH/bin/Release/net9.0/publish/CliTemplate" "$@" <&0
fi
