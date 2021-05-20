# Software development laboratory 1 exercises

![Build docs](https://github.com/bmeviauac01/laboratories-en/workflows/Build%20docs/badge.svg?branch=master)

[BMEVIAUAC09 Software development laboratory 1](https://www.aut.bme.hu/Course/ENVIAUAC09/) exercises.

The exercise documentation is build with MkDocs and published on GitHub Pages at <https://bmeviauac01.github.io/laboratories-en/>

#### Render website locally

1. Open a PowerShell console at the repository root

1. `docker run -it --rm -p 8000:8000 -v ${PWD}:/docs squidfunk/mkdocs-material:7.1.5`

1. Open <http://localhost:8000> in a browser

1. Edit the Markdown and it will trigger automatic update in the browser
