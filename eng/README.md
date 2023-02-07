# Software development laboratory 1 exercises

![Build docs](https://github.com/bmeviauac01/laboratories-en/workflows/Build%20docs/badge.svg?branch=master)

[BMEVIAUAC09 Software development laboratory 1](https://www.aut.bme.hu/Course/ENVIAUAC09/) exercises.

The exercise documentation is build with MkDocs and published on GitHub Pages at <https://bmeviauac01.github.io/laboratories-en/>

## Render website (with Docker)

You need Docker in order to build and run the documentation. On a local machine with Windows [Docker Desktop](https://www.docker.com/products/docker-desktop/) could be the right tooling or you could use any cloud based development environment like GitHub Codespaces.

This repository contains a Dockerfile which need to be built and run.

1. Open a terminal on the repository's root.
2. Run the following commands on Windows (PowerShell), Linux or MacOS:

   ```cmd
   docker build -t mkdocs .
   docker run -it --rm -p 8000:8000 -v ${PWD}:/docs mkdocs
   ```

3. Open <http://localhost:8000> or codespace's port forwarded address in a browser.
4. Edit Markdown files. After saving any file the webpage should refresh automatically.