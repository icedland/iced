cmake_minimum_required(VERSION 3.20)
project(iced_test)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Find the iced_x86 library
add_subdirectory(../src/cpp/iced-x86 iced_build)

add_executable(test_constexpr test_constexpr.cpp)
target_link_libraries(test_constexpr PRIVATE iced_x86)