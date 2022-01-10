#!/bin/sh
set -e

root_dir=$(dirname "$0")
root_dir=$(cd "$root_dir/.." && pwd)
if [ ! -f "$root_dir/LICENSE.txt" ]; then
	echo "Couldn't find the root dir"
	exit 1
fi

manylinux_image="$1"
shift
if [ -z "$manylinux_image" ]; then
	echo "Missing docker image"
	exit 1
fi

extra_args=
while [ "$#" -gt 0 ]; do
	case $1 in
	--pypy) extra_args="$extra_args --pypy" ;;
	*) echo "Unknown arg: $1"; exit 1 ;;
	esac
	shift
done

linux32=
if echo "$manylinux_image" | grep i686; then
	linux32=linux32
fi

mkdir -p /tmp/py-dist
container_name=$(docker run --rm -itd "$manylinux_image")
docker cp "$root_dir/src/rust" "$container_name:/tmp/iced-build"
docker exec -w /tmp/iced-build/iced-x86-py "$container_name" $linux32 bash build-wheels.sh "$manylinux_image" $extra_args
docker cp "$container_name:/tmp/iced-build/iced-x86-py/dist" /tmp/py-dist
mv /tmp/py-dist/dist/* /tmp/py-dist
rmdir /tmp/py-dist/dist
docker kill "$container_name"
