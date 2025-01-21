#!/bin/bash
export DEBIAN_FRONTEND=noninteractive

# Check if the package name is provided
if [ -z "$1" ]; then
    echo "Usage: $0 <package-name>"
    exit 1
fi

PACKAGE=$1
sudo apt-get update

# Check if the package is available on the current system
if apt-cache show "$PACKAGE" > /dev/null 2>&1; then
    echo "Package '$PACKAGE' is available in the current repository. Installing..."
    sudo apt-get update
    sudo apt-get install -y "$PACKAGE"
    exit 0
else
    echo "Package '$PACKAGE' is not available in the current repository."
    echo "Adding the Ubuntu 22.04 (Jammy) repository..."
fi

# Add the Ubuntu 22.04 (Jammy) repository
echo "deb http://archive.ubuntu.com/ubuntu jammy main universe" | sudo tee /etc/apt/sources.list.d/ubuntu-jammy.list

# Update the package list
sudo apt-get update

# Try to install the package
if sudo apt-get install -y "$PACKAGE"; then
    echo "Package '$PACKAGE' installed successfully from the Ubuntu 22.04 repository."
else
    echo "Failed to install '$PACKAGE'. It might have unresolved dependencies."
fi

# Clean up: Remove the Ubuntu 22.04 repository
echo "Cleaning up the Ubuntu 22.04 repository..."
sudo rm /etc/apt/sources.list.d/ubuntu-jammy.list
sudo apt-get update

exit 0

