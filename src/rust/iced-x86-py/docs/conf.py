# Configuration file for the Sphinx documentation builder.
#
# This file only contains a selection of the most common options. For a full
# list see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Path setup --------------------------------------------------------------

# If extensions (or modules to document with autodoc) are in another directory,
# add these directories to sys.path here. If the directory is relative to the
# documentation root, use os.path.abspath to make it absolute, like shown here.
#
import os
import sys
import sysconfig
lib_dir = f"lib.{sysconfig.get_platform()}-cpython-{sys.version_info.major}{sys.version_info.minor}"
sys.path.insert(0, os.path.abspath(os.path.join("..", "build", lib_dir)))


# -- Project information -----------------------------------------------------

project = "iced-x86"
copyright = "Copyright (C) 2018-present iced project and contributors"
author = "iced project and contributors <https://github.com/icedland>"
# The full version, including alpha/beta/rc tags
release = "1.18.0"


# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = [
	"sphinx.ext.doctest",
	"sphinx.ext.autodoc",
	"sphinx.ext.intersphinx",
	"sphinx.ext.napoleon",
]

autodoc_member_order = "bysource"

intersphinx_mapping = {"python": ("https://docs.python.org/3", None)}

napoleon_google_docstring = True
napoleon_numpy_docstring = False

# Add any paths that contain templates here, relative to this directory.
#templates_path = ["_templates"]

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = ["_build", "Thumbs.db", ".DS_Store"]


# -- Options for HTML output -------------------------------------------------

# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
#
html_theme = "alabaster"

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
#html_static_path = ["_static"]
