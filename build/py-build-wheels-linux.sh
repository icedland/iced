#!/bin/sh
set -e

root_dir=$(dirname "$0")
root_dir=$(cd "$root_dir/.." && pwd)
if [ ! -f "$root_dir/LICENSE.txt" ]; then
	echo "Couldn't find the root dir"
	exit 1
fi

container_name=iced-py-wheel
manylinux_image=quay.io/pypa/manylinux2014_x86_64

mkdir -p /tmp/py-dist
docker run --rm -itd --name $container_name $manylinux_image
docker cp "$root_dir/src/rust" $container_name:/tmp/iced-build
docker exec -w /tmp/iced-build/iced-x86-py $container_name bash build-wheels.sh
docker cp $container_name:/tmp/iced-build/iced-x86-py/dist /tmp/py-dist
mv /tmp/py-dist/dist/* /tmp/py-dist
rmdir /tmp/py-dist/dist
docker kill $container_name
