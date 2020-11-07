#!/bin/sh
set -e

root_dir=$(dirname "$0")
root_dir=$(cd "$root_dir/.." && pwd)
if [ ! -f "$root_dir/LICENSE.txt" ]; then
	echo "Couldn't find the root dir"
	exit 1
fi

if [ "$#" -gt 0 ]; then
	echo "No command line args are supported"
	exit 1
fi

$root_dir/build/build-rust.sh
$root_dir/build/build-js.sh
$root_dir/build/build-dotnet.sh
